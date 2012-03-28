using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using MySql.Data.MySqlClient;
using Templater.Misc;

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

        public DataTable GetUserByCredentials(String email, String password)
        {
            String query = "SELECT * FROM users WHERE email = @email AND password = @password";
            Array parameters = new[] {
                new DBParam ("@email", MySqlDbType.VarChar, email),
                new DBParam ("@password", MySqlDbType.VarChar, password)
            };

            DataTable result = this.ExecuteQuery(query, parameters);

            return result;
        }

        /// <summary>
        /// Для запроса query и параметров parameters выполняет запрос и заполняет его в таблицу
        /// </summary>
        /// <param name="query">Запрос с плейсхолдерами для данных</param>
        /// <param name="parameters">Объект с данными к запросу</param>
        /// <returns>Таблица с результатом</returns>
        private DataTable ExecuteQuery(String query, Array parameters = null)
        {
            //Сюда будем записывать результат
            DataTable result = new DataTable();
            //Инициализируем команду
            MySqlCommand command = new MySqlCommand(query, connection);
            //Извлекаем свойства из объекта и заполняем ими запрос
            if (parameters != null)
            {
                foreach (DBParam field in parameters)
                {
                    command.Parameters.Add(field.Name, field.Type);
                    command.Parameters[field.Name].Value = field.Value;
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