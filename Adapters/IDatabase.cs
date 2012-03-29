using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Templater.Adapters
{
    /// <summary>
    /// Интерфейс определяющий общие правила создания адаптеров баз данных для курсача
    /// Во всех методах считается, что данные безопасны
    /// </summary>
    interface IDatabase
    {
        /// <summary>
        /// Проверить корректность данных пользователя
        /// Проверяет на существование и соответствие
        /// </summary>
        /// <param name="email">E-mail адрес пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns>Результат проверки</returns>
        DataTable GetUserByCredentials(String email, String password);

        /// <summary>
        /// Получить информацию о всех шаблонах пользователя и его рабочей группы
        /// </summary>
        /// <param name="UserID">ID пользователя</param>
        /// <returns>Информация о шаблонах</returns>
        DataTable GetTemplates(int UserID);
    }
}
