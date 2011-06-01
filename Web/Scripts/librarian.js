/// <reference path="jquery-1.5.1.js" />
/// <reference path="jquery.contextMenu.js" />

$(function () {

    $("ol.backlog")
        .each(function () {

            var backlogTable = $(this);

            var contextMenu = $("<ul id='backlog-contextMenu' class='contextMenu'></ul>");
            contextMenu.append($("<li><a href='#white'>White</a></li>"));
            contextMenu.append($("<li><a href='#yellow'>Yellow</a></li>"));
            contextMenu.append($("<li><a href='#orange'>Orange</a></li>"));
            contextMenu.append($("<li><a href='#red'>Red</a></li>"));
            contextMenu.append($("<li><a href='#green'>Green</a></li>"));
            contextMenu.append($("<li><a href='#blue'>Blue</a></li>"));
            contextMenu.insertAfter(backlogTable);

            backlogTable
                .find("li")
                .contextMenu(
                    { menu: 'backlog-contextMenu' },
                    function (action, element) {
                        alert(action);
                    }
                );

        });

});