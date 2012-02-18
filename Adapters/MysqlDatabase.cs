using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using MySql.Data.MySqlClient;

//TODO: А исключения кто ловить будет?
namespace Templater.Adapters
{
    /// <summary>
    /// Адаптер для использования СУБД MySQL
    /// </summary>
    public class MysqlDatabase : IDatabase
    {
        private MySqlConnection connection;

        public MysqlDatabase(String connectionString)
        {
            this.connection = new MySqlConnection(connectionString);
            this.connection.Open();
        }

        public bool CheckUserCredentials(String email, String password)
        {
            return true;
        }
        /// <summary>
        /// Для запроса query и параметров parameters выполняет запрос и заполняет его в таблицу
        /// </summary>
        /// <param name="query">Запрос с плейсхолдерами для данных</param>
        /// <param name="parameters">Объект с данными к запросу</param>
        /// <returns>Таблица с результатом</returns>
        private DataTable ExecuteQuery(String query, Object parameters = null)
        {
            //Сюда будем записывать результат
            DataTable result = new DataTable();
            //Инициализируем команду
            MySqlCommand command = new MySqlCommand(query, connection);
            //Извлекаем свойства из объекта и заполняем ими запрос
            if (parameters != null)
            {
                FieldInfo[] finfo = parameters.GetType().GetFields();
                foreach (FieldInfo field in finfo)
                {
                    command.Parameters.Add(String.Concat(@"@", field.Name), field.GetValue(parameters));
                }
            }
            //Выполняем запрос
            MySqlDataReader reader = command.ExecuteReader();
            //Заполняем таблицу данными
            result.Load(reader);

            return result;
        }
    }
}