using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Diagnostics;

namespace Templater.Models
{
    public enum Statuses { Open, Stopped, Inprogress, Started, Closed }

    /// <summary>
    /// Класс "Задача"
    /// </summary>
    public class Task
    {
        private int _Id;
        private int _TemplateId;
        private string _TemplateName;
        private string _Website;
        private DateTime _Timestamp;
        private int _Depth;
        private int _Progress;
        private int _Results;
        private string _Status;
        private Statuses status;
        private int _Process;

        /// <summary>
        /// Конструктор задачи
        /// </summary>
        /// <param name="id">ID задачи</param>
        /// <param name="templateId">ID шаблона</param>
        /// <param name="templateName">Имя шаблона</param>
        /// <param name="website">Имя вебсайта для поиска</param>
        /// <param name="status">Статус просчёта</param>
        /// <param name="timestamp">Дата постановки задачи</param>
        /// <param name="depth">Глубина поиска</param>
        /// <param name="progress">Прогресс</param>
        /// <param name="results">Количество результатов</param>
        /// <param name="process">Процесс, который монополизировал таск</param>
        public Task(int id, int templateId, string templateName, string website, string status, 
            DateTime timestamp, int depth, int progress, int results, int process)
        {
            //Инициализируем переменные
            this._Id = id;
            this._TemplateId = templateId;
            this._TemplateName = templateName;
            this._Website = website;
            this._Timestamp = timestamp;
            this._Depth = depth;
            this._Progress = progress;
            this._Results = results;
            this._Process = process;
            //Парсим статус
            switch (status)
            {
                case "open":
                    if (process == 0)
                        this._Status = "Задача пока не запущена на исполнение";
                    else
                        this._Status = "Загрузка данных для просчёта";
                    this.status = Statuses.Open;
                    break;
                case "started":
                    this._Status = "Просчёт запущен";
                    this.status = Statuses.Started;
                    break;
                case "closed":
                    this._Status = "Процесс просчёта завершён";
                    this.status = Statuses.Closed;
                    break;
                case "stopped":
                    this._Status = "Процесс прерван";
                    this.status = Statuses.Stopped;
                    break;
                default:
                    this._Status = status;
                    this.status = Statuses.Inprogress;
                    break;
            }
        }

        //Readonly
        public int Id { get { return this._Id; } }
        public string Website { get { return this._Website; } }
        public string TemplateName { get { return this._TemplateName; } }
        public int Results { get { return this._Results; } }
        public int Progress { get { return this._Progress; } }
        public int Depth { get { return this._Depth; } }
        public int TemplateId { get { return this._TemplateId; } }
        public string GetStatusName()
        {
            return this._Status;
        }
        public Statuses GetStatus()
        {
            return this.status;
        }

        /// <summary>
        /// Поставить задачу на просчёт
        /// </summary>
        /// <returns>Успех/Неуспех</returns>
        public bool Schedule()
        {
            Process proc = Process.Start(WebConfigurationManager.AppSettings["Crawler"], this._Id.ToString());

            return proc != null;
        }

        /// <summary>
        /// Создать задачу
        /// </summary>
        /// <param name="templateID">ID шаблона для задачи</param>
        /// <param name="depth">Глубина поиска</param>
        /// <returns>Созданная задача</returns>
        public static Task CreateTask(int templateID, int depth)
        {
            return Database.Instance.CreateNewTask(templateID, depth);
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        /// <param name="taskID">ID задачи для удаления</param>
        /// <returns>Успех/Неуспех</returns>
        public static bool DeleteTask(int taskID)
        {
            return Database.Instance.DeleteTask(taskID);
        }

        /// <summary>
        /// Перезапустить задачу
        /// </summary>
        /// <param name="taskID">ID задачи</param>
        /// <returns>Успех/Неуспех</returns>
        public static bool RestartTask(int taskID)
        {
            //Получаем задачу из базы данных
            Task curTask = Database.Instance.GetTask(taskID);
            //Пытаемся убить процесс, если он запущен
            if (!curTask.KillTask())
                return false;

            //Обновить информацию в базе данных
            Database.Instance.UpdateTask(taskID, curTask.Depth, curTask.TemplateId, "open", 0, 0);

            //Запустить задачу заново на обработку
            return curTask.Schedule();
        }

        /// <summary>
        /// Обновить задачу
        /// </summary>
        /// <param name="taskID">ID задачи</param>
        /// <param name="depth">Глубина поиска</param>
        /// <param name="templateId">ID шаблона</param>
        /// <returns></returns>
        public static bool UpdateTask(int taskID, int depth, int templateId)
        {
            //Получаем задачу из базы данных
            Task curTask = Database.Instance.GetTask(taskID);
            //Пытаемся убить процесс, если он запущен
            if (!curTask.KillTask())
                return false;

            //Обновить информацию в базе данных
            return Database.Instance.UpdateTask(taskID, depth, templateId, "open", 0, 0);
        }

        /// <summary>
        /// Убить просчёт задачи
        /// </summary>
        /// <returns>Успех/Неуспех</returns>
        private bool KillTask()
        {
            if (this._Process != 0)
                try
                {
                    Process proc = Process.GetProcessById(this._Process);
                    proc.Kill();
                    //Даём 10 секунд на выход из процесса
                    bool result = proc.WaitForExit(10000);
                    //А если процесс не вышел - ну и хрен с ним впринципе
                    return true;
                }
                //Означает что где-то адский факап
                catch (System.ComponentModel.Win32Exception)
                {
                    return false;
                }
                //Означает что в базе старые данные, либо процесс завершается прямо сейчас
                catch (System.InvalidOperationException)
                {
                    return true;
                }

            return true;
        }
    }
}