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
    let message = $('#message').val();

    if(isLoggedIn()) {
        alert('You cannot send a message before logging in!');
        return;
    }

    if(message.length === 0) {
        alert('You cannot send empty messages!');
        return;
    }

    let username = getUser();

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