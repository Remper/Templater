using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using MySql.Data.MySqlClient;
using Crawler.Misc;
using Crawler.Model;

//TODO: А исключения кто ловить будет?
namespace Crawler.Adapters
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

        public Task GetTaskInfo(int taskID)
        {
            String query = "SELECT a.id, a.templateid, b.website, a.timestamp, a.depth" +
                " FROM tasks AS a INNER JOIN templates AS b ON a.templateid = b.id WHERE a.id = @taskID";
            DBParam[] parameters = new[] {
                new DBParam ("@taskID", MySqlDbType.Int32, taskID)
            };

            List<Object[]> row = this.ExecuteQuery(query, parameters);
            Task result = new Task((int)row[0][0], (int)row[0][1], (string)row[0][2], (string)row[0][3], (int)row[0][4]);

            return result;
        }

        public bool AddNewResult(int taskID, string status, string result)
        {
            String query = "INSERT INTO results VALUES (null, @taskID, @status, @result)";
            DBParam[] parameters = new[] {
                new DBParam ("@taskID", MySqlDbType.Int32, taskID),
                new DBParam ("@status", MySqlDbType.VarChar, status),
                new DBParam ("@result", MySqlDbType.Text, result)
            };
            List<Object[]> row = this.ExecuteQuery(query, parameters);

            return true;
        }

        public bool UpdateProgress(int taskID, int results, string status, int progress)
        {
            String query = "UPDATE tasks SET results = @results, status = @status, progress = @progress WHERE id = @taskID";
            DBParam[] parameters = new[] {
                new DBParam ("@taskID", MySqlDbType.Int32, taskID),
                new DBParam ("@results", MySqlDbType.Int32, results),
                new DBParam ("@status", MySqlDbType.VarChar, status),
                new DBParam ("@progress", MySqlDbType.Int32, progress)
            };
            List<Object[]> row = this.ExecuteQuery(query, parameters);

            return true;
        }

        public bool ResetResults(int taskID)
        {
            String query = "DELETE FROM results WHERE taskid = @taskID";
            DBParam[] parameters = new[] {
                new DBParam ("@taskID", MySqlDbType.Int32, taskID)
            };
            List<Object[]> row = this.ExecuteQuery(query, parameters);

            return true;
        }

        public void Close()
        {
            this.connection.Close();
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

            reader.Close();
            return result;
        }
    }
}