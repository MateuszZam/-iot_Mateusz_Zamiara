(function() {
    var btnSend = $('#btnSend');
    var tBoxMsg = $('#tBoxMsg');
    var chatList = $('#chatList');
    var userName = $('#user-name').val();

    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    $(btnSend).click(function() {
        var msg = $(tBoxMsg).val();

        connection.invoke('SendMessage',
            {
                userName: userName,
                message: msg
            });
    });

    connection.on('RecievedMessage',
        function (obj) {
            var htmlElement = '<li>'
                + '<span class="font-weight-bold">['
                + obj.formattedTimeStamp + '] '
                + obj.userName
                + '</span>: '
                + obj.message
                + '</li>';

            $(tBoxMsg).val('');

            $(chatList).prepend(htmlElement);
        });

    connection.on('UserSignedIn',
        function (obj) {
            var htmlElement = '<li>'
                + '<span class="font-weight-bold text-success">'
                + obj + ' joined chat!'
                + '</li>';

            $(tBoxMsg).val('');

            $(chatList).prepend(htmlElement);
        });

    connection.start().then(function() {
        console.log("connected as " + userName);
        connection.invoke('SignInUser', userName);
    });
})();