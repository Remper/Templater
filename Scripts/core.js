/*

    Ядро курсача

*/
var Kurs = {
    //Функция инициализации системы
    initialize: function () {
        //Если пользователь не авторизирован, показываем ему окно входа во весь экран
        if (!User.AuthState)
            $('#authDialog').modal({
                keyboard: false
            });
    }
}