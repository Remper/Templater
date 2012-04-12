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
        Task GetTaskInfo(int taskID);

        /// <summary>
        /// Записать в базу результат
        /// </summary>
        /// <param name="templateID">ID шаблона</param>
        /// <param name="status">Текст статуса</param>
        /// <param name="result">Сериализованный объект результата</param>
        /// <returns>Успех/Неуспех</returns>
        bool AddNewResult(int templateID, string status, string result);

        /// <summary>
        /// Закрыть соединение
        /// </summary>
        void Close();
    }
}
