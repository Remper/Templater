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
        Logic.indexPage();
    },

    loadSucc: function (data) {
        if (data.result) {
            $("#content").html(data.data);
            Logic.pageLoaded();
        } else {
            Controller.loadFail("result");
        }
    },

    loadFail: function (result) {
        Logic.pageError();
    },

    stateTracker: function () {
        if (Kurs.Context.subpage != "") {
            var params = Kurs.Context.subpage.split("/");
            var page = params[0];

            if (Logic.allowed.indexOf(page) != -1) {
                if (page != Logic.current.page)
                    Logic[page + "Page"]();
            } else
                window.location.hash = "";
        } else
            if ("index" != Logic.current.page)
                Logic.indexPage();
    }
}

/**
 * Логика работы подстраниц и подзапросов
 */
Logic = {
    allowed: ["index", "new", "edit", "learn"],
    current: {
        state: null,
        page: null
    },

    indexPage: function (params) {
        Kurs.Util.sendRequest("Main", "List", {}, Controller.loadSucc, Controller.loadFail);
        this.pageLoading("index");
    },

    newPage: function (params) {
        Kurs.Util.sendRequest("Main", "New", {}, Controller.loadSucc, Controller.loadFail);
        this.pageLoading("new");
    },

    editPage: function (params) {

    },

    learnPage: function (params) {

    },

    errorPage: function (params) {

    },

    pageLoading: function (page) {
        this.current.state = "loading";
        Kurs.Util.appendLoader($("#content"));
        this.current.page = page;
    },

    pageLoaded: function () {
        this.current.state = "loaded";
        this.rebind[this.current.page + "Page"]();
    },

    pageError: function () {
        this.current.state = "error";
        this.rebind.errorPage();
    },

    rebind: {
        indexPage: function (params) {

        },

        newPage: function (params) {
            $('input[name="temp-source-radios"]').change(function () {
                var wrapper = $("#temp-source-wrapper");
                if (this.value == "on")
                    wrapper.show();
                else
                    wrapper.hide();
            });
        },

        editPage: function (params) {

        },

        learnPage: function (params) {

        },

        errorPage: function (params) {
            alert("Fail");
        }
    }
}