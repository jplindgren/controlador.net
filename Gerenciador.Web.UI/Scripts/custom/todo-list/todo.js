$(function () {
    var TodoModule;
    window.TodoModule = TodoModule = {};

    TodoModule.init = function (items) {
        var Todo = function (id, title, done, order, callback) {
            var self = this;
            self.title = ko.observable(title);
            self.done = ko.observable(done);
            self.order = order;
            self.id = id;

            self.done.subscribe(function (selected) {
                var posting = $.post('/Todo/SetDone', { id: self.id, done: selected }, 'json');
            });

            self.updateCallback = ko.computed(function () {
                callback(self);
                return true;
            });
        }

        var viewModel = function (todosData) {
            var self = this;

            self.inputTitle = ko.observable("");
            self.doneTodos = ko.observable(0);
            self.markAll = ko.observable(false);
            self.todos = ko.observableArray([]);

            self.countUpdate = function (item) {
                var doneArray = ko.utils.arrayFilter(self.todos(), function (it) {
                    return it.done();
                });
                self.doneTodos(doneArray.length);
                return true;
            };

            $.each(todosData, function (index, todoItem) {
                self.todos.push(new Todo(todoItem.Id, todoItem.Content, todoItem.Done, todoItem.Order, self.countUpdate))
            });

            self.addOne = function () {
                var order = self.todos().length;
                var posting = $.post('/Todo/CreateItem', { Content: self.inputTitle(), Order: order }, 'json');
                posting.done(function (result) {
                    var t = new Todo(result.Id, result.Content, false, result.Order, self.countUpdate);
                    self.todos.push(t);
                    $(".todo-list").trigger('itemAdded');
                });
            };

            self.createOnEnter = function (item, event) {
                if (event.keyCode == 13 && self.inputTitle()) {
                    self.addOne();
                    self.inputTitle("");
                } else {
                    return true;
                };
            }

            self.toggleEditMode = function (item, event) {
                $(event.target).closest('li').toggleClass('editing');
            }

            self.editOnEnter = function (item, event) {
                if (event.keyCode == 13 && item.title) {
                    var posting = $.post('/Todo/EditItem', { id: item.id, content: item.title }, 'json');
                    posting.done(function (result) {
                        item.updateCallback();
                        self.toggleEditMode(item, event);
                    });                    
                } else {
                    return true;
                };
            }

            self.markAll.subscribe(function (newValue) {
                ko.utils.arrayForEach(self.todos(), function (item) {
                    return item.done(newValue);
                });
            });

            self.clearItem = function (item) {
                var posting = $.post('/Todo/DeleteItem', { id: item.id }, 'json');
                posting.done(function (result) {
                    self.todos.remove(item);
                });                
            };

            self.countDoneText = function (bool) {
                var cntAll = self.todos().length;
                var cnt = (bool ? self.doneTodos() : cntAll - self.doneTodos());
                var text = "<span class='count'>" + cnt.toString() + "</span>";
                var numero = (self.doneTodos() > 1 ? "s" : "")
                text += " item" + numero;
                text += (bool ? " completo" + numero : " restante" + numero);
                return text;
            }

            self.clear = function () {
                var doneArray = ko.utils.arrayFilter(self.todos(), function (it) {
                    return it.done();
                });
                var selectedIds = doneArray.map(function (item) {
                    return item.id;
                });

                $.ajax({
                    type: "POST",
                    url: "/Todo/DeleteItems",
                    dataType: "json",
                    traditional: true,
                    data: { ids: selectedIds }
                }).done(function (result) {
                    self.todos.remove(function (item) { return item.done(); });
                    self.markAll(false);
                });
                //self.todos.remove(function (item) { return item.done(); });
            }
        };

        ko.applyBindings(new viewModel(items));
    }
});
