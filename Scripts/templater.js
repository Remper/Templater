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
            $(".temp-delete").click(function () {
                var id = this.id.split("-")[1];
                $("#temp-deletion").remove();
                $(document.body).append("<div class=\"modal\" id=\"temp-deletion\">" +
                    "   <div class=\"modal-header\">" +
                    "       <a class=\"close\" data-dismiss=\"modal\">×</a>" +
                    "       <h3>Удаление шаблона</h3>" +
                    "   </div>" +
                    "   <div class=\"modal-body\">" +
                    "       <p>Вы уверены, что хотите удалить шаблон #" + id + "?</p>" +
                    "   </div>" +
                    "   <div class=\"modal-footer\">" +
                    "       <a data-dismiss=\"modal\" class=\"btn\">Отмена</a>" +
                    "       <a id=\"temp-really-delete-" + id + "\" class=\"btn btn-primary\">Удалить</a>" +
                    "   </div>" +
                    "</div>");
                $('#temp-deletion').modal();
                $("#temp-really-delete-" + id).click(function () {
                    $('#temp-deletion').modal('hide');
                    Kurs.Util.sendRequest("Template", "Delete", { templateid: this.id.split("-")[3] }, Callback.indexPage.deleteTempSucc, Callback.indexPage.deleteTempFail);
                });
            });
        },

        newPage: function (params) {
            $('input[name="temp-source-radios"]').change(function () {
                var wrapper = $("#temp-source-wrapper");
                if (this.value == "on")
                    wrapper.show();
                else
                    wrapper.hide();
            });

            $("#temp-save").click(function () {
                var name = $("#temp-name").val();
                var website = $("#temp-website").val();
                var sitedata = $('input[name="temp-source-radios"]:checked').val() == "on" && $("#temp-source").val() != "" ? $("#temp-source").val() : "null";
                var data = {
                    name: name,
                    website: website,
                    source: sitedata
                }

                Kurs.Util.sendRequest("Template", "New", data, Callback.newPage.createNewSucc, Callback.newPage.createNewFail, true);
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

/**
 * Неймспейс колбеков
 */
Callback = {
    newPage: {
        createNewSucc: function (data) {
            if (data.result)
                window.location.hash = "";
            else
                Callback.newPage.createNewFail();
        },

        createNewFail: function () {

        }
    },
    indexPage: {
        deleteTempSucc: function (data) {
            if (data.result)
                Logic.indexPage();
            else
                Callback.indexPage.deleteTempFail();
        },

        deleteTempFail: function () {

        }
    }
}