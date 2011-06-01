/// <reference path="jquery-1.5.1.js" />
/// <reference path="jquery.contextMenu.js" />

$(function () {

    $('ol.backlog')
        .each(function () {

            var backlogTable = $(this);

            var contextMenu = $('<ul id="backlog-contextMenu" class="contextMenu"></ul>');
            contextMenu.append($('<li><a href="#color-white">White</a></li>'));
            contextMenu.append($('<li><a href="#color-yellow">Yellow</a></li>'));
            contextMenu.append($('<li><a href="#color-orange">Orange</a></li>'));
            contextMenu.append($('<li><a href="#color-red">Red</a></li>'));
            contextMenu.append($('<li><a href="#color-green">Green</a></li>'));
            contextMenu.append($('<li><a href="#color-blue">Blue</a></li>'));
            contextMenu.insertAfter(backlogTable);

            backlogTable
                .find('li')
                .contextMenu(
                    { menu: 'backlog-contextMenu' },
                    function (action, element) {
                        var storyId = $(element).data('story-id');

                        var colorAction = /^color-(\w*)/i.exec(action);
                        if (colorAction) {
                            var newColor = colorAction[1];
                            setStoryColor(element, storyId, newColor);
                        }
                    }
                );

        });

});

function setStoryColor(storyElement, storyId, newColor) {
    storyElement = $(storyElement);
    $.post(
        'backlog/' + storyId + '/set-color',
        'color=' + newColor,
        function () {
            var existingColorClass = /story-color-\w*/i.exec(storyElement.attr('class'));
            if (existingColorClass) storyElement.removeClass(existingColorClass[0]);
            storyElement.addClass('story-color-' + newColor);
        }
    );
}