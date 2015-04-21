(function (TaskManagement) {
    TaskManagement.Comment = Comment = {};

    TaskManagement.Comment = function (container, popup, task) {
        this.popup = popup;
        this.container = container;
        this.txtCreateComment = popup.find('#txtComment');
        this.createButton = popup.find('#btnCreateComment');
        this.task = task;

        this.emitter = async.emitter();
        this.on = $.proxy(this.emitter, "on");

        this.createButton.on("click", $.proxy(this, "create"));

        this.urls = {
            load: '/Comments/Index',
            create: '/Comments/Create'
        }
    };

    TaskManagement.Comment.prototype = function () {
        var loadComments = function () {
            var that = this;
            var dataToSend = { projectId: this.task.projectId, taskId: this.task.taskId };
            makeAjaxCall({
                url: this.urls.load,
                data: dataToSend,
                contentType: 'application/html; charset=utf-8',
                type: 'GET',
                dataType: 'html'
            }, error, false).done(function (result) {
                that.container.html(result);
            });
        },
        togglePopup = function () {
            this.popup.modal('toggle');
        },
        create = function (event) {
            var dataToSend = { projectId: this.task.projectId, taskId: this.task.taskId, content: this.txtCreateComment.val() };
            makeAjaxCall({
                url: this.urls.create,
                data: JSON.stringify(dataToSend),
                contentType: 'application/json; charset=utf-8',
                type: 'POST',
                dataType: 'json',
                context: this
            }, error, true, this.popup).done(function (result) {
                togglePopup.apply(this);
                this.txtCreateComment.val('');
                this.loadComments(this.task.projectId, this.task.taskId);
                
                //LoadTimeline();
                $.growlUI('Criação de comentário', 'Operação realizada com sucesso...');
                this.emitter.emit("created", result);
            });
        },
        error = function error(jqXHR, textStatus, error) {
            alert('erro status: ' + textStatus);
        };
        return {
            loadComments: loadComments,
            create: create
        };
    }();
})(TaskManagement);