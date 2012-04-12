using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using System.Linq;
using HtmlAgilityPack;

namespace Crawler.Model
{
    enum Statuses { Open, Stopped, Inprogress, Started, Closed }
    enum StepType { Select, Criteria, Result }

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

        public Task(int id, int templateId, string website, string timestamp, int depth)
        {
            this._TemplateID = templateId;
            this._Website = website;
            this._Timestamp = DateTime.Parse(timestamp);
            this._Depth = depth;
            this._ID = id;
            _Status = Statuses.Open;
            _Progress = 0;
            _curDepth = 0;
            _Results = 0;
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
                this.UpdateStatus();
                this.ParseHTML(doc, template);
            }
        }

        //Readonly
        public int ID { get { return this._ID; } }
        public int TemplateID { get { return this._TemplateID; } }
        public string Website { get { return this._Website; } }


        private void FinalizeTask()
        {

        }

        private void UpdateStatus()
        {

        }

        private List<string> ParseHTML(HtmlDocument doc, XmlDocument template)
        {
            HtmlNode curNode = doc.DocumentNode;
            XmlElement curComp = template.DocumentElement;
            //Получаем список xpath-критериев
            List<string> results = ParseTemplate(curComp);
            List<string> targets = new List<string>();
            List<string> criterias = new List<string>();
            foreach (string result in results)
            {
                if (result.IndexOf("{") != -1 && result.IndexOf("}") != -1)
                    criterias.Add(result);
                else
                    targets.Add(result);
            }
            //Получаем стартовую ноду
            string startnode = "//"+results[0].Split('/')[0];
            //Получаем изначальный список совпадений
            List<HtmlNode> col = doc.DocumentNode.SelectNodes(startnode).ToList();
            //Отсеиваем по критериям
            List<HtmlNode> passedNodes = new List<HtmlNode>();
            foreach (HtmlNode node in col)
            {
                foreach (string criteria in criterias)
                {
                    if (criteria.IndexOf("{{") != -1)
                    {
                        string match = criteria.Split('{')[0].Substring(criteria.IndexOf("/"));
                        match = "."+match.Substring(0, match.Length - 1);
                        IEnumerable<HtmlNode> toCheck = node.SelectNodes(match);
                        if (toCheck != null)
                        {
                            bool flag = false;
                            foreach (HtmlNode checker in toCheck)
                            {
                                if (checker.InnerText + "}}" == criteria.Split('{')[2])
                                    flag = true;
                            }
                            if (flag)
                                passedNodes.Add(node);
                        }
                    }
                    else
                    {
                        string match = criteria.Split('{')[0].Substring(criteria.IndexOf("/"));
                        match = "."+match.Substring(0, match.Length - 1);
                        IEnumerable<HtmlNode> toCheck = node.SelectNodes(match);
                        if (toCheck != null)
                            passedNodes.Add(node);
                    }

                }
            }
            //Отсеиваем
            //Проверяем каждый из критериев и записываем результаты

            return new List<string>();
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
