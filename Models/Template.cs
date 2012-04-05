using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using MySql.Data.MySqlClient;
using Templater.Adapters;

namespace Templater.Models
{
    /// <summary>
    /// Клас "Шаблон", модель шаблона
    /// </summary>
    public class Template
    {
        private int _Id;
        private string _OwnerEmail;
        private int _WorkGroupId;
        private string _Website;
        private string _Name;

        //Регулярки для проверки входных данных шаблона
        public static Regex NameReg = new Regex(@"^[a-z]+$", RegexOptions.IgnoreCase);
        public static Regex WebsiteReg = new Regex(@"^([a-z\d]+\.)+[a-z\d]+$", RegexOptions.IgnoreCase);
        public static Regex TemplateDataReg = new Regex(@"^[^']+$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Конструктор для шаблона
        /// </summary>
        /// <param name="id">ID Шаблона</param>
        /// <param name="workgroupId">ID Рабочей группы</param>
        /// <param name="ownerEmail">Email владельца</param>
        /// <param name="website">Вебсайт</param>
        /// <param name="name">Имя шаблона</param>
        public Template(int id, int workgroupId, string ownerEmail, string website, string name)
        {
            this._Id = id;
            this._OwnerEmail = ownerEmail;
            this._WorkGroupId = workgroupId;
            this._Website = website;
            this._Name = name;
        }

        /// <summary>
        /// Создать новый шаблон, вернуть свежесозданную сущность
        /// </summary>
        /// <param name="ownerId">ID пользователя, создающего шаблон</param>
        /// <param name="name">Имя шаблона</param>
        /// <param name="website">Вебсайт к которому применяется шаблон</param>
        /// <param name="templateData">Исходник шаблона</param>
        public static Template CreateTemplate(int ownerId, string name, string website, string templateData) 
        {
            //Инициализируем базу данных
            MysqlDatabase database = new MysqlDatabase(WebConfigurationManager.AppSettings["ConnectionString"]);

            //Создаём шаблон в базе данных
            Template result = database.CreateNewTemplate(ownerId, name, "http://"+website);

            //Если есть чего записывать в шаблон, записываем
            if (templateData != null)
            {
                FileInfo template = new FileInfo(WebConfigurationManager.AppSettings["TemplateFolder"] + "\\" + result.Id + ".xml");
                using (StreamWriter writer = template.CreateText()) {
                    writer.Write(templateData);
                }
            }

            return result;
        }

        public string Website { 
            get { return this._Website; }
            set { this._Website = value; } 
        }
        public string Name { 
            get { return this._Name; }
            set { this._Name = value; } 
        }
        public string OwnerEmail { get { return this._OwnerEmail; } }
        public int WorkGroupId { get { return this._WorkGroupId; } }
        public int Id { get { return this._Id; } }

        public bool HasTemplateData
        {
            get
            {
                return File.Exists(WebConfigurationManager.AppSettings["TemplateFolder"] + "\\" + this._Id + ".xml");
            }
        }
    }
}