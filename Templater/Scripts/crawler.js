/**
* Управляющий скрипт для кроулера
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
    allowed: ["index"],
    current: {
        state: null,
        page: null
    },

    indexPage: function (params) {
        Kurs.Util.sendRequest("Crawler", "List", {}, Controller.loadSucc, Controller.loadFail);
        this.pageLoading("index");
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

        errorPage: function (params) {
            alert("Fail");
        }
    }
}