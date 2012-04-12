using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using HtmlAgilityPack;
using Crawler.Adapters;

namespace Crawler.Model
{
    enum Statuses { Open, Stopped, Inprogress, Started, Closed }

    /// <summary>
    /// Класс "Задача"
    /// </summary>
    public class Task
    {
        private int _ID;
        private int _TemplateID;
        private string _Website;
        private DateTime _Timestamp;
        private int _Depth;
        private Statuses _Status;
        private int _Results;
        private int _Progress;
        private int _curDepth;
        private string templateData;
        private MysqlDatabase connection;

        public Task(int id, int templateId, string website, string timestamp, int depth)
        {
            this._TemplateID = templateId;
            this._Website = website;
            this._Timestamp = DateTime.Parse(timestamp);
            this._Depth = depth;
            this._ID = id;
            this._Status = Statuses.Open;
            this._Progress = 0;
            this._curDepth = 0;
            this._Results = 0;
            this.connection = new MysqlDatabase(Properties.Settings.Default.ConnectionString);
            this.connection.ResetResults(this._ID);
        }

        public void StartCrawling() 
        {
            //Пытаемся найти данные шаблона
            string templatePath = Properties.Settings.Default.TemplateFolder + "\\" + this._TemplateID + ".xml";
            if (File.Exists(templatePath))
            {
                //Загружаем шаблон в XML документ
                XmlDocument template = new XmlDocument();
                template.Load(templatePath);

                //Загружаем сайт в HTML документ
                WebClient wclient = new WebClient();
                Stream downloadStream = wclient.OpenRead(this._Website);
                HtmlDocument doc = new HtmlDocument();
                doc.Load(downloadStream, System.Text.Encoding.UTF8);

                //Обозначаем наше присутствие и начинаем парсить
                this._Status = Statuses.Started;
                this.UpdateProgress();
                List<string[]> results = this.ParseHTML(doc, template);
                this.PushResults(results);
                this._Status = Statuses.Closed;
                this.UpdateProgress();
            }
        }

        //Readonly
        public int ID { get { return this._ID; } }
        public int TemplateID { get { return this._TemplateID; } }
        public string Website { get { return this._Website; } }
        public string Status
        {
            get
            {
                switch (this._Status)
                {
                    case Statuses.Open:
                    default:
                        return "open";
                    case Statuses.Closed:
                        return "closed";
                    case Statuses.Started:
                        return "started";
                    case Statuses.Stopped:
                        return "stopped";
                    case Statuses.Inprogress:
                        return "Сопоставление на глубине " + this._curDepth;
                }
            }
        }

        private void UpdateProgress()
        {
            this.connection.UpdateProgress(this._ID, this._Results, this.Status, this._Progress);
        }

        private void PushResults(List<string[]> results)
        {
            for (int i = 1; i < results.Count; i++)
            {
                MysqlDatabase database = new MysqlDatabase(Properties.Settings.Default.ConnectionString);
                string accumulator = "{" + '\n';
                for (int j = 0; j < results[0].Length; j++)
                    accumulator += results[0][j] + ": \"" + results[i][j] + (j == results[0].Length-1 ? "\"" : "\",") + '\n';
                accumulator += "}";
                this.connection.AddNewResult(this._ID, "new", accumulator);
                this._Results++;
                UpdateProgress();
            }
        }

        private List<string[]> ParseHTML(HtmlDocument doc, XmlDocument template)
        {
            this._Status = Statuses.Started;
            UpdateProgress();
            HtmlNode curNode = doc.DocumentNode;
            XmlElement curComp = template.DocumentElement;
            //Получаем список xpath-критериев
            List<string> results = ParseTemplate(curComp);
            List<string> targets = new List<string>();
            List<string> criterias = new List<string>();
            foreach (string result in results)
            {
                if (result.IndexOf("{{") != -1 && result.IndexOf("}}") != -1)
                    criterias.Add(result);
                else if (result.IndexOf("{") != -1 && result.IndexOf("}") != -1)
                    targets.Add(result);
                else
                    criterias.Add(result);
            }
            //Получаем стартовую ноду
            string startnode = "//"+results[0].Split('/')[0];
            //Запоминаем прогресс
            this._Status = Statuses.Inprogress;
            //Получаем изначальный список совпадений
            List<HtmlNode> col = doc.DocumentNode.SelectNodes(startnode).ToList();
            //Отсеиваем по критериям
            List<HtmlNode> passedNodes = new List<HtmlNode>();
            foreach (HtmlNode node in col)
            {
                bool flag = false;
                foreach (string criteria in criterias)
                {
                    flag = false;
                    if (criteria.IndexOf("{{") != -1)
                    {
                        string match = criteria.Split('{')[0].Substring(criteria.IndexOf("/"));
                        match = "."+match.Substring(0, match.Length - 1);
                        IEnumerable<HtmlNode> toCheck = node.SelectNodes(match);
                        if (toCheck != null)
                        {
                            foreach (HtmlNode checker in toCheck)
                            {
                                if (checker.InnerText + "}}" == criteria.Split('{')[2])
                                    flag = true;
                            }
                        }
                    }
                    else
                    {
                        string match = "." + criteria;
                        IEnumerable<HtmlNode> toCheck = node.SelectNodes(match);
                        if (toCheck != null)
                            flag = true;
                    }
                    if (!flag)
                        break;
                }
                if (flag)
                    passedNodes.Add(node);
            }
            col = passedNodes;
            passedNodes = new List<HtmlNode>();
            //Отсеиваем по значениям
            foreach (HtmlNode node in col)
            {
                bool flag = false;
                foreach (string target in targets)
                {
                    flag = false;
                    string match = target.Split('{')[0].Substring(target.IndexOf("/"));
                    match = "." + match.Substring(0, match.Length - 1);
                    IEnumerable<HtmlNode> toCheck = node.SelectNodes(match);
                    if (toCheck == null)
                        break;
                    flag = true;
                }
                if (flag)
                    passedNodes.Add(node);
            }
            //Формируем списки нод со значениями
            List<List<HtmlNode>> resnodes = new List<List<HtmlNode>>();
            //Списки имён значений
            List<string> names = new List<string>();
            foreach (string target in targets)
            {
                List<HtmlNode> targetnodes = new List<HtmlNode>();
                string targetname = target.Split('{')[1];
                targetname = targetname.Substring(0, targetname.Length - 1);
                names.Add(targetname);
                foreach (HtmlNode node in passedNodes)
                {
                    string match = target.Split('{')[0].Substring(target.IndexOf("/"));
                    match = "." + match.Substring(0, match.Length - 1);
                    targetnodes.AddRange(node.SelectNodes(match));
                }
                resnodes.Add(targetnodes);
            }

            //Формируем результаты
            int num = resnodes.Min(x => x.Count);
            int size = resnodes.Count;
            List<string[]> finalresults = new List<string[]>();
            finalresults.Add(names.ToArray());
            for (int i = 0; i < num; i++)
            {
                string[] temp = new string[size];
                for (int j = 0; j < size; j++)
                {
                    HtmlNode cur = resnodes[j][i];
                    foreach (HtmlNode node in cur.ChildNodes)
                        if (node.NodeType == HtmlNodeType.Text)
                            temp[j] = node.InnerText;
                }
                finalresults.Add(temp);
            }
            return finalresults;
        }

        private List<string> ParseTemplate(XmlElement curComp)
        {
            List<string> result = new List<string>();

            if (curComp.NodeType == XmlNodeType.Element)
            {
                switch (curComp.Name)
                {
                    //Могут иметь потомков
                    case "node":
                        //Формируем xpath
                        string type = curComp.GetAttribute("type");
                        switch (type)
                        {
                            case "block":
                                //not implemented yet
                                break;
                            case "table":
                                //not implemented yet
                                break;
                            case "any":
                                //not implemented yet
                                break;
                            default:
                                if (type.Substring(0, 4) == "tag-")
                                {
                                    type = type.Substring(4);
                                    string classD = "";
                                    string idD = "";
                                    if (curComp.GetAttribute("id") != "")
                                        idD = String.Concat("@id = \"", curComp.GetAttribute("id"), "\"");
                                    if (curComp.GetAttribute("class") != "")
                                        classD = String.Concat("@class = \"", curComp.GetAttribute("class"), "\"");
                                    if (classD != "" || idD != "")
                                    {
                                        type = String.Concat(type, "[", classD != "" ? classD : ""
                                                                        + idD != "" && classD != "" ? " and " + idD : idD, "]");
                                    }
                                }
                                else
                                {
                                    //Обработка ошибки
                                }
                                break;
                        }
                        if (curComp.ChildNodes.Count == 0)
                            result.Add(type);
                        else
                            foreach (XmlElement element in curComp.ChildNodes)
                            {
                                List<string> localresult = ParseTemplate(element);
                                foreach (String piece in localresult)
                                {
                                    result.Add(type + "/" + piece);
                                }
                            }

                        break;
                    case "nullnode":
                        //not implemented yet
                        break;
                    //Конечные ноды
                    case "target":
                        result.Add("{"+curComp.GetAttribute("name")+"}");
                        break;
                    case "text":
                        result.Add("{{"+curComp.GetAttribute("value")+"}}");
                        break;
                }
                return result;
            }
            else
                return null;
        }
    }
}
