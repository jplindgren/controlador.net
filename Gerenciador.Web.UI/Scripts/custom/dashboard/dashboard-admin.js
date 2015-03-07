//depends on TodoModulo, Mansory and jqueryui
; (function () {
    var DashboardAdminManager;
    window.DashboardAdminManager = DashboardAdminManager = {};

    DashboardAdminManager.init = function (html, todoItems) {
        TodoModule.init(todoItems, $('.todo-list'));

        initMasonry($('#widgets'),
            [
                { selector: $(".todo-list"), action: 'itemAdded' },
                { selector: $('.navbar-btn'), action: 'click' },
            ]
        );
            
        //Make the dashboard widgets sortable Using jquery UI
        $(".connectedSortable").sortable({
            placeholder: "sort-highlight",
            connectWith: ".connectedSortable",
            handle: ".box-header, .nav-tabs",
            forcePlaceholderSize: true,
            zIndex: 999999
        }).disableSelection();
        $(".connectedSortable .box-header, .connectedSortable .nav-tabs-custom").css("cursor", "move");
        //jQuery UI sortable for the todo list
        $(".todo-list").sortable({
            placeholder: "sort-highlight",
            handle: ".handle",
            forcePlaceholderSize: true,
            zIndex: 999999
        }).disableSelection();

        function initMasonry($mansoryContainer, refreshItems) {
            $mansoryContainer.masonry({
                itemSelector: '.connectedSortable',
                isAnimated: true,
                transitionDuration: 40
            });

            $.each(refreshItems, function (index, item) {
                item.selector.on(item.action, function () {
                    $mansoryContainer.masonry();
                });
            });                

        }
    }
})();