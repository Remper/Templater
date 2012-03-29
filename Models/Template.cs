using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
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
    }
}