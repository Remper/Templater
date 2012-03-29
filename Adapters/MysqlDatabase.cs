using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using MySql.Data.MySqlClient;
using Templater.Misc;
using Templater.Models;

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

        public Template[] GetTemplates(int UserID)
        {
            String query = "SELECT a.id, b.email, a.website, a.name, b.workgroup FROM templates AS a"+ 
                    " INNER JOIN users AS b ON a.owner = b.id"+
                    " WHERE b.workgroup = (SELECT workgroup FROM users WHERE id = @UserID);";
            DBParam[] parameters = new[] {
                new DBParam ("@UserID", MySqlDbType.Int32, UserID)
            };

            List<Object[]> result = this.ExecuteQuery(query, parameters);
            Template[] rows = new Template[result.Count];
            for (int i = 0; i < result.Count; i++)
            {
                Object[] row = result[i];
                rows[i] = new Template((int)row[0], (int)row[4], (string)row[1], (string)row[2], (string)row[3]);
            }

            return rows;
        }

        public List<Object[]> GetUserByCredentials(String email, String password)
        {
            String query = "SELECT * FROM users WHERE email = @email AND password = @password";
            DBParam[] parameters = new[] {
                new DBParam ("@email", MySqlDbType.VarChar, email),
                new DBParam ("@password", MySqlDbType.VarChar, password)
            };

            List<Object[]> result = this.ExecuteQuery(query, parameters);

            return result;
        }

        /// <summary>
        /// Для запроса query и параметров parameters выполняет запрос и заполняет его в таблицу
        /// </summary>
        /// <param name="query">Запрос с плейсхолдерами для данных</param>
        /// <param name="parameters">Объект с данными к запросу</param>
        /// <returns>Таблица с результатом</returns>
        private List<Object[]> ExecuteQuery(String query, DBParam[] parameters = null)
        {
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
            List<Object[]> result = new List<Object[]>();
            while (reader.Read())
            {
                Object[] temp = new Object[reader.FieldCount];
                for(int i = 0; i < reader.FieldCount; i++)
                    temp[i] = reader[i];
                result.Add(temp);
            }

            return result;
        }
    }
}