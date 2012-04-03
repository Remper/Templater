/**
 * Управляющий скрипт для шаблонизатора
 */
Controller = {
    //Вызывается при загрузке страницы
    init: function () {
        //Очищаем хеш и ресетим трекер
        window.location.hash = "";
        Kurs.Context.tracker = this.stateTracker;

        //Загружаем базовый контент для страницы
        Kurs.Util.sendRequest("Main", "List", {}, Controller.loadSucc, Controller.loadFail);
    },

    loadSucc: function (data) {
        if (data.result) {
            $("#content").html(data.data);
            Controller.stateTracker();
        } else {
            Controller.loadFail("result");
        }
    },

    loadFail: function (result) {
        alert("Fail");
    },

    stateTracker: function () {
        if (Kurs.Context.subpage != "") {
            var params = Kurs.Context.subpage.split("/");
            var page = params[0];

            if (Logic.allowed.indexOf(page) != -1)
                Logic[page + "Page"]();
            else
                window.location.hash = "";
        } else
            Logic.indexPage();
    }
}

/**
 * Логика работы подстраниц
 */
Logic = {
    allowed: ["index", "new", "edit", "learn"],

    indexPage: function (params) {

    },

    newPage: function (params) {
        
    },

    editPage: function (params) {

    },

    learnPage: function (params) {

    }
}