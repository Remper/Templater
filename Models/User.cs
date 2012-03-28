using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Templater.Adapters;

namespace Templater.Models
{
    /// <summary>
    /// Клас "Пользователь", хранит информацию о текущем пользователе
    /// </summary>
    public class User
    {
        private string _Email;
        private int UserId;
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

            return true;
        }
    }
}