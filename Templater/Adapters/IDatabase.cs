using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Templater.Models;

namespace Templater.Adapters
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
        /// <returns>Задача с заданным ID</returns>
        Task GetTask(int taskID);

        /// <summary>
        /// Проверить корректность данных пользователя
        /// Проверяет на существование и соответствие
        /// </summary>
        /// <param name="email">E-mail адрес пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns>Результат проверки</returns>
        List<Object[]> GetUserByCredentials(String email, String password);

        /// <summary>
        /// Получить информацию о всех шаблонах пользователя и его рабочей группы
        /// </summary>
        /// <param name="UserID">ID пользователя</param>
        /// <returns>Информация о шаблонах</returns>
        Template[] GetTemplates(int UserID);

        /// <summary>
        /// Получить информацию о всех задачах рабочей группы
        /// </summary>
        /// <param name="UserID">ID текущего пользователя</param>
        /// <returns>Список задач</returns>
        Task[] GetTasks(int UserID);

        /// <summary>
        /// Создать новый шаблон в базе данных и вернуть объект с ним
        /// </summary>
        /// <param name="owner">ID пользователя, создающего шаблон</param>
        /// <param name="name">Имя шаблона</param>
        /// <param name="website">Вебсайт к которому применяется шаблон</param>
        /// <returns>Созданный шаблон</returns>
        Template CreateNewTemplate(int owner, string name, string website);

        /// <summary>
        /// Создать новую задачу и вернуть объект с ней
        /// </summary>
        /// <param name="templateID">ID шаблона</param>
        /// <param name="depth">Глубина поиска</param>
        /// <returns>Созданная задача</returns>
        Task CreateNewTask(int templateID, int depth);

        /// <summary>
        /// Обновить задачу
        /// </summary>
        /// <param name="taskID">ID задачи</param>
        /// <param name="depth">Глубина поиска</param>
        /// <param name="templateID">ID шаблона</param>
        /// <param name="status">Статус вычисления</param>
        /// <param name="results">Количество результатов</param>
        /// <param name="progress">Прогресс вычисления</param>
        /// <returns>Успешно ли обновление</returns>
        bool UpdateTask(int taskID, int depth, int templateID, string status, int results, int progress);

        /// <summary>
        /// Удалить задачу из базы данных
        /// </summary>
        /// <param name="taskID">ID задачи</param>
        /// <returns>Успешно ли удаление</returns>
        bool DeleteTask(int taskID);

        /// <summary>
        /// Удалить шаблон из базы данных
        /// </summary>
        /// <param name="templateID">ID шаблона</param>
        /// <returns>Успешно ли удаление</returns>
        bool DeleteTemplate(int templateID);

        /// <summary>
        /// Проверить права пользователя по отношению к шаблону
        /// </summary>
        /// <param name="templateID">ID шаблона</param>
        /// <param name="userID">ID пользователя</param>
        /// <returns>Может ли пользователь надругаться над шаблоном</returns>
        bool CheckRightsForTemplate(int templateID, int userID);

        /// <summary>
        /// Проверить права пользователя по отношению к задаче
        /// </summary>
        /// <param name="taskID">ID задачи</param>
        /// <param name="userID">ID пользователя</param>
        /// <returns>Может ли пользователь надругаться над задачей</returns>
        bool CheckRightsForTask(int taskID, int userID);
    }
}
