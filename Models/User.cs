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
    /// Клас "Пользователь", хранит информацию о текущем пользователе
    /// </summary>
    public class User
    {
        private string _Email;
        private int _UserId;
        private string _WorkGroup;
        private int _WorkGroupId;
        private bool _AuthState;
        
        /// <summary>
        /// Конструктор для нового пользователя
        /// </summary>
        /// <param name="Email">Email пользователя</param>
        public User(String Email)
        {
            this._Email = Email;
            this._AuthState = false;
            this._WorkGroupId = 0;
            this._UserId = 0;
        }

        public string Email { get { return this._Email; } }
        public string WorkGroup { get { return this._WorkGroupId == 0 ? "none" : this._WorkGroup; } }
        public bool AuthState { get { return this._AuthState; } }

        /// <summary>
        /// Функция авторизации пользователя
        /// При успешной проверке пароля, авторизирует пользователя в системе
        /// </summary>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>Результат авторизации</returns>
        public bool Authorize(String password)
        {
            if (this._AuthState)
                return true;

            MysqlDatabase database = new MysqlDatabase(WebConfigurationManager.AppSettings["ConnectionString"]);
            DataTable result = database.GetUserByCredentials(this.Email, password);
            if (result.Rows.Count != 1)
                return false;

            this._UserId = (int)result.Rows[0].ItemArray[0];
            this._WorkGroupId = (int)result.Rows[0].ItemArray[3];
            this._AuthState = true;

            return true;
        }
    }
}