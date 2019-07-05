const appUrl = 'https://localhost:44339/api/';
let currentUsername = null;

function renderMessages(data) {
    $('#messages').empty();

    for(let message of data) {
        $('#messages')
            .append('<div class="message d-flex justify-content-start"><strong>'
                + message.user
                + '</strong>: '
                + message.content
                +'</div>')

    }
}

function loadMessages() {
    $.get({
        url: appUrl + 'messages/all',
        success: function success(data) {
            renderMessages(data);
        },
        error: function error(error) {
            console.log(error);
        }
    });
}

function createMessage() {
    let username = currentUsername;
    let message = $('#message').val();

    if(username == null) {
        alert('You cannot send a message before choosing an username!');
        return;
    }

    if(message.length === 0) {
        alert('You cannot send empty messages!');
        return;
    }

    $.post({
        url: appUrl + 'messages/create',
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify({content: message, user: username}),
        success: function success(data) {
            loadMessages();
        },
        error: function error(error) {
            console.log(error);
        }
    });
}

function chooseUsername() {
    let username = $('#username').val();

    if(username.length === 0) {
        alert('You cannot choose an empty username!');
        return;
    }

    currentUsername = username;
    $('#username-choice').text(currentUsername);
    $('#choose-data').hide();
    $('#reset-data').show();
}

function resetUsername() {
    currentUsername = null;

    $('#choose-data').show();
    $('#reset-data').hide();
}

$('#reset-data').hide();
loadMessages();

setInterval(loadMessages, 100);