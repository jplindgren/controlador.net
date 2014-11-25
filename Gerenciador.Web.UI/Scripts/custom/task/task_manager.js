; (function () {
    var TaskManagement;
    window.TaskManagement = TaskManagement = {};

    TaskManagement.taskInit = function (data, _projectId, _taskId) {
        var subTaskItem = function (options) {
            return {
                Id: ko.observable(options.Id),
                Name: ko.observable(options.Name),
                CreatedAt: ko.observable(options.CreatedAt),
                StartDate: ko.observable(options.StartDate),
                ExpectedEndDate: ko.observable(options.ExpectedEndDate),
                Status: ko.computed(function () {
                    var status = "desconhecida";
                    var cssLabel = "label label-default";
                    switch (options.Status) {
                        case 1:
                            status = "Aberta";
                            cssLabel = "label label-warning";
                            break;
                        case 3:
                            status = "Em andamento";
                            cssLabel = "label label-info";
                            break;
                        case 4:
                            status = "Completa";
                            cssLabel = "label label-success";
                            break;
                        case 5:
                            status = "Cancelada";
                            cssLabel = "label label-danger";
                            break;
                    }
                    return "<span class='" + cssLabel + "'" + ">" + status + "</span>";
                }),
                Actions: ko.computed(function () {
                    if (options.Status == 1) {
                        return '<li><a href="#" data-action="setDone">Pronta</a></li>' +
                        '<li><a href="#" data-action="setCancelled">Cancelar</a>' +
                        '</li><li class="divider"></li>' +
                        '<li><a href="#" data-action="edit">Editar</a></li>';
                    } else {
                        return '<li><a href="#" data-action="setOpen">Reabrir</a></li>'
                    }
                })
            };
        }
        viewModel = {
            RawDeadline: ko.observable(data.Deadline),
            RawEndDate: ko.observable(data.EndDate),
            Deadline: ko.computed(function () {
                var deadLine = moment(data.Deadline);
                if (deadLine.isValid())
                    return deadLine.format('L');
            }),
            EndDate: ko.computed(function () {
                var endDate = moment(data.EndDate);
                console.log(endDate);
                if (endDate.isValid())
                    return endDate.format('L');
            }),
            Description: ko.observable(data.Description),

            //subtasks
            SubTasks: ko.observableArray(ko.utils.arrayMap(data.SubTasks, function (item) {
                return subTaskItem(item);
            })),

            addSubtask: function () {
                var dataToSend = { Name: this.subtaskNameToAdd, StartDate: this.subtaskStartDateToAdd, endDate: this.subtaskEndDateToAdd };

                dataToSend.projectId = _projectId;
                dataToSend.taskId = _taskId;

                $.blockUI({ message: 'Carregando...' });
                var posting = $.post('/Task/CreateSubTaskV2', dataToSend, 'json');
                posting.done(function (result) {

                    $('#modalCreateSubTask').modal('hide');
                    $('#txtSubTaskName').val('');
                    $('#txtStartDate').val('');
                    $('#txtEndDate').val('');

                    var newSubTask = new subTaskItem(result);
                    viewModel.SubTasks.unshift(newSubTask);

                    LoadTimeline();
                    LoadCalendar();
                });
                posting.always(function () {
                    $.unblockUI();
                });
            },
            subtaskNameToAdd: ko.observable(""),
            subtaskStartDateToAdd: ko.observable(""),
            subtaskEndDateToAdd: ko.observable("")
        };
        viewModel.Variation = ko.computed(function () {
            var end = moment(viewModel.EndDate());
            var deadline = moment(viewModel.Deadline());

            return moment.duration(end.diff(deadline)).asDays() + ' dias';
        });

        // Activates knockout.js
        ko.applyBindings(viewModel);

        $('.slider').slider();

        LoadComments(_projectId, _taskId);
        LoadTimeline();
        LoadCalendar();

        //async load calendar dates
        function LoadCalendar() {
            $('#calendar').datepicker("remove");
            var gettingLimitDates = $.get('/Task/GetLimitDates', { projectId: _projectId, taskId: _taskId }, 'json');
            gettingLimitDates.done(function (result) {
                var fixed = [];
                result.forEach(function (entry) {
                    fixed.push({ start: new Date(entry.Start), end: new Date(entry.End), description: entry.Description });
                });
                $("#calendar").datepicker({
                    language: "pt-BR",
                    format: 'dd/mm/yyyy',
                    multidate: true,
                    todayHighlight: true,
                    beforeShowDay: function (date) {
                        var description = '';
                        var cssClass = '';

                        fixed.forEach(function (entry) {
                            if (entry.end.getFullYear() == date.getFullYear() && entry.end.getMonth() == date.getMonth() && entry.end.getUTCDate() == date.getUTCDate()) {
                                var today = new Date();
                                var limitDate = new Date(entry.end.getFullYear(), entry.end.getMonth(), entry.end.getUTCDate());

                                cssClass = limitDate >= today ? 'active' : 'whaoo';
                                description += entry.description + "; ";
                            }
                        });

                        description = description || 'Não há eventos nessa data';
                        var dayValue = {
                            tooltip: description,
                            classes: cssClass
                        };

                        return dayValue;
                    }
                });
            });
        }

        //Async Populates Comments 
        function LoadComments() {
            this.dataToSend = { projectId: _projectId, taskId: _taskId };
            $.ajax({
                url: '/Comments/Index',
                data: this.dataToSend,
                contentType: 'application/html; charset=utf-8',
                type: 'GET',
                dataType: 'html'
            })
            .success(function (result) {
                $('#taskComments').html(result);
            })
            .error(function (xhr, status) {
                alert('erro status: ' + status);
            });
        }

        //async populates timeline
        function LoadTimeline() {
            $.ajax({
                url: '/Task/GetTimeline', 
                data: { taskId: _taskId },
                contentType: 'application/html; charset=utf-8',
                type: 'GET',
                dataType: 'html'
            })
            .success(function (result) {
                $('#taskTimeline').html(result);
            })
            .error(function (xhr, status) {
                alert('erro status: ' + status);
            });
        }

        //Update Progress
        $(".modal-footer").on("click", "#btnProgressSave", function () {
            var _valueToUpdate = $('#progressUpdaterSlider')[0].value
            var posting = $.post('/Task/UpdateProgress', { projectId: _projectId, id: _taskId, newValue: _valueToUpdate });
            posting.done(function (data) {
                var dataToUpdate = data + '%';
                $('#modalUpdateProgress').modal('hide')
                $('#taskUpdateBar').width(dataToUpdate);
                $("#progressLegend").html($('<strong/>').append(dataToUpdate + ' Completo'));
                $('#taskUpdateBar').attr('aria-valuenow', data);
                if (data == 100) {
                    $('#taskStatus').attr("class", "pull-right label label-success").html('completa');
                } else {
                    $('#taskStatus').attr("class", "pull-right label label-warning").html('aberta');
                }
                LoadTimeline();
            });
        });

        //subtasks actions
        $("#subtasksContainer").on("click", "a", function (event) {
            event.preventDefault();
            var subtaskId = $(this).closest('tr').data("id");
            var action = $(this).data("action");

            if (action == "setDone") {
                ChangeTaskStatus(_taskId, subtaskId, 4);
            } else if (action == "setCancelled") {
                ChangeTaskStatus(_taskId, subtaskId, 5);
            } else if (action == "setOpen") {
                ChangeTaskStatus(_taskId, subtaskId, 1);
            } else if (action == "edit") {
                OpenEditScreen(_taskId, subtaskId);
            }
        });

        //open edit screen
        function OpenEditScreen(_taskId, _subTaskId) {
            var getting = $.get('/Task/EditSubTask', { taskId: _taskId, subTaskId: _subTaskId }, 'html');
            getting.done(function (result) {
                $('#modalEditSubTask').modal('show');
                $('#editSubTask').html('');
                $('#editSubTask').html(result);
            });
        }

        //set subktask done
        function ChangeTaskStatus(_taskId, _subTaskId, _status) {
            var posting = $.post('/Task/ChangeTaskStatus', { taskId: _taskId, subTaskId: _subTaskId, subkTaskStatus: _status }, 'html');
            posting.done(function (result) {
                $('#subtasksContainer').html('');
                $('#subtasksContainer').html(result);
                LoadTimeline();
                LoadCalendar();
            });
        }

        //Create Comments
        $(".modal-footer").on("click", "#btnCreateComment", function () {

            var posting = $.post('/Comments/Create', {
                projectId: _projectId,
                taskId: _taskId,
                content: $('#txtComment').val()
            });
            posting.done(function (result) {
                $('#modalCreateComment').modal('hide');
                $('#txtComment').val('');
                LoadComments(_projectId, _taskId);
                LoadTimeline();
                $.growlUI('operação realizada com sucesso...');
            });
        });

        //Edit SubTask
        $(".modal-footer").on("click", "#btnEditSubTask", function () {
            $.blockUI({ message: 'Carregando...' });
            var posting = $.post('/Task/EditSubTask', $('#editSubtaskForm').serialize(), 'html');
            posting.done(function (result) {
                $('#modalEditSubTask').modal('hide');
                $('#subtasksContainer').html('');
                $('#subtasksContainer').html(result);
                LoadTimeline();
                LoadCalendar();
            });
            posting.always(function () {
                $.unblockUI();
            });
        });

        //open edit task
        $('#actionsPanel').on('click', '#btnEditTaskAction', function (event) {
            event.preventDefault();
            var getting = $.get('/Task/EditTask', { taskId: _taskId }, 'html');
            getting.done(function (result) {
                $('#modalEditTask').modal('show');
                $('#editTask').html('');
                $('#editTask').html(result);
            });
        });

        //Edit task
        $(".modal-footer").on("click", "#btnEditTask", function () {
            $.blockUI({ message: 'Carregando...' });
            var posting = $.post('/Task/EditTask', $('#editTaskForm').serialize(), 'json');
            posting.done(function (result) {
                $('#modalEditTask').modal('hide');

                var data = $.parseJSON(result);

                viewModel.RawDeadline(data.Deadline);
                viewModel.RawEndDate(data.EndDate);
                viewModel.Description(data.Description);

                LoadCalendar();
            });
            posting.always(function () {
                $.unblockUI();
            });
        });
    };
})();