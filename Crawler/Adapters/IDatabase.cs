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
        /// <param name="taskID">ID задачи</param>
        /// <param name="status">Текст статуса</param>
        /// <param name="result">Сериализованный объект результата</param>
        /// <returns>Успех/Неуспех</returns>
        bool AddNewResult(int taskID, string status, string result);

        /// <summary>
        /// Обновить прогресс задачи
        /// </summary>
        /// <param name="taskID">ID задачи</param>
        /// <param name="results">Количество результатов</param>
        /// <param name="status">Статус задачи</param>
        /// <param name="progress">Процент выполнения</param>
        /// <returns>Успех/Неуспех</returns>
        bool UpdateProgress(int taskID, int results, string status, int progress);

        /// <summary>
        /// Сбросить результаты предыдущего прохода
        /// </summary>
        /// <param name="taskID">ID задачи</param>
        /// <returns>Успех/Неуспех</returns>
        bool ResetResults(int taskID);

        /// <summary>
        /// Указать, какой процесс занимается задачей
        /// </summary>
        /// <param name="taskID">ID задачи</param>
        /// <param name="processID">ID процесса</param>
        /// <returns></returns>
        bool MonopolizeTask(int taskID, int processID);

        /// <summary>
        /// Закрыть соединение
        /// </summary>
        void Close();
    }
}
