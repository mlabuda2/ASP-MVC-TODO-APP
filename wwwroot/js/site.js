// Write your JavaScript code.
$(document).ready(function() {

    // Wire up all of the cehckboxes to run markCompleted()
    $('.done-checkbox').click(function(event) {
        markCompleted(event.target);
    });

    // Wire up the Add button to send the new item to the server
    $('#add-item-button').click(addItem);

    $("#add-item-title").keydown(function(event) {
        if (event.keyCode == 13) {
            $("#add-item-button").click();
        }
    });

})

    
function addItem() {
    $('#add-item-error').hide();
    var newTitle = $('#add-item-title').val();
    var newDate = $('#add-item-date').val();

    $.post('/Todo/AddItem', {title : newTitle, date : newDate}, function() {
        window.location = '/Todo';
    })
    .fail(function(data) {
        if (data && data.responseJSON) {
            var firstError = data.responseJSON[Object.keys(data.responseJSON)[0]];
            $('#add-item-error').text(firstError);
            $('#add-item-error').show();
        }
    });
}

function markCompleted(checkbox) {
    checkbox.disabled = true;

    $.post('/Todo/MarkDone', { id: checkbox.name }, function() {
        var row = checkbox.parentElement.parentElement;
        $(row).addClass('done');
    });
}