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

        public Template CreateNewTemplate(int owner, string name, string website)
        {
            String query = "INSERT INTO templates VALUES (null, @owner, @website, @name)";
            DBParam[] parameters = new[] {
                new DBParam ("@owner", MySqlDbType.Int32, owner),
                new DBParam ("@name", MySqlDbType.VarChar, name),
                new DBParam ("@website", MySqlDbType.VarChar, website)
            };
            List<Object[]> result = this.ExecuteQuery(query, parameters);

            query = "SELECT a.id, b.email, a.website, a.name, b.workgroup FROM templates AS a" +
                    " INNER JOIN users AS b ON a.owner = b.id" +
                    " WHERE a.owner = @owner AND a.name = @name ORDER BY a.id DESC LIMIT 0,1";
            result = this.ExecuteQuery(query, parameters);
            if (result.Count != 0)
            {
                return new Template((int)result[0][0], (int)result[0][4], (string)result[0][1], (string)result[0][2], (string)result[0][3]);
            }
            else
                throw new Exception("Can't create new template");
        }

        public Template[] GetTemplates(int UserID)
        {
            String query = "SELECT a.id, b.email, a.website, a.name, b.workgroup FROM templates AS a"+ 
                    " INNER JOIN users AS b ON a.owner = b.id"+
                    " WHERE b.workgroup = (SELECT workgroup FROM users WHERE id = @UserID) ORDER BY a.id DESC";
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

        public Task[] GetTasks(int UserID)
        {
            throw new NotImplementedException();
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

        public Task CreateNewTask(int templateID, int depth)
        {
            throw new NotImplementedException();
        }

        public bool UpdateTask(int taskID, int depth, int templateID, int status, int results, int progress)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTask(int taskID)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTemplate(int templateID)
        {
            String query = "DELETE FROM templates WHERE id = @templateID";
            DBParam[] parameters = new[] {
                new DBParam ("@templateID", MySqlDbType.VarChar, templateID)
            };

            List<Object[]> result = this.ExecuteQuery(query, parameters);

            return true;
        }

        public bool CheckRightsForTemplate(int templateID, int userID)
        {
            String query = "SELECT count(*) FROM templates AS a" +
                    " INNER JOIN users AS b ON a.owner = b.id" +
                    " WHERE b.workgroup = (SELECT workgroup FROM users WHERE id = @userID) AND a.id = @templateID";
            DBParam[] parameters = new[] {
                new DBParam ("@templateID", MySqlDbType.Int32, templateID),
                new DBParam ("@userID", MySqlDbType.Int32, userID)
            };
            List<Object[]> result = this.ExecuteQuery(query, parameters);

            return (long)result[0][0] == 1 ? true : false;
        }

        public bool CheckRightsForTask(int taskID, int userID)
        {
            String query = "SELECT count(*) FROM templates AS a" +
                    " INNER JOIN users AS b ON a.owner = b.id" +
                    " WHERE b.workgroup = (SELECT workgroup FROM users WHERE id = @userID)" +
                    " AND a.id = (SELECT templateid FROM tasks WHERE id = @taskID)";
            DBParam[] parameters = new[] {
                new DBParam ("@templateID", MySqlDbType.Int32, taskID),
                new DBParam ("@userID", MySqlDbType.Int32, userID)
            };
            List<Object[]> result = this.ExecuteQuery(query, parameters);

            return (long)result[0][0] == 1 ? true : false;
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