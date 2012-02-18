using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// <returns>Результат проверки: true в случае наличия в базе и соответствия пароля, false в противном случае</returns>
        bool CheckUserCredentials(String email, String password);
    }
}
