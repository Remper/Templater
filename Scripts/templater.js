/**
 * Управляющий скрипт для шаблонизатора
 */
Controller = {
    //Вызывается при загрузке страницы
    init: function () {
        //Загружаем базовый контент для страницы
        
        Kurs.Util.sendRequest("Main", "List", {}, Controller.loadSucc, Controller.loadFail);
    },

    loadSucc: function (data) {
        if (data.result)
            $("#content").html(data.data);
        else {
            Controller.loadFail("result");
        }
    },

    loadFail: function (result) {
        alert("Fail");
    }
}