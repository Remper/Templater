using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Crawler.Model;

namespace Crawler.Adapters
{
    /// <summary>
    /// Интерфейс определяющий общие правила создания адаптеров баз данных для курсача
    /// Во всех методах считается, что данные безопасны
    /// </summary>
    interface IDatabase
    {
        /// <summary>
        /// Получить информацию о конкретной задаче
        /// </summary>
        /// <param name="taskID">ID задачи</param>
        /// <returns>Задача</returns>
        public Task GetTaskInfo(int taskID);
    }
}
