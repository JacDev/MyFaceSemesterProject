var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

var likedPost = {};


connection.start().then(function () {
    var button = document.getElementById("submitButton");
    if (button != null) {
        document.getElementById("submitButton").disabled = false;
    }
   

}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveNotification", function () {
    var notificationParagraph = document.getElementById("notifications");
    var value = parseInt(notificationParagraph.textContent, 10);

    value = isNaN(value) ? 0 : value;
    value++;
    notificationParagraph.className = "badge badge-danger";

    document.getElementById('notifications').textContent = value;
});

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    addMessage(message, "col-md-6 offset-md-0");
});

document.getElementById("submitButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageText").value;
    connection.invoke("SendPrivateMessage", friendId, message, userId).catch(function (err) {
        return console.error(err.toString());
    });

    addMessage(message, "col-md-6 offset-md-6");

    document.getElementById("messageText").value = "";
    event.preventDefault();
});

function sendLike(postId, postNumber, isLiked) {
    var message = "like";

    var postNumberAsString = postNumber.toString();

    var likeCounter = document.getElementById(postNumberAsString);

    var value = parseInt(likeCounter.textContent, 10);
    value = isNaN(value) ? 0 : value;

    likeCounter.textContent = '';

    var icon = document.createElement("i");

    if (postNumberAsString in likedPost) {
        likedPost[postNumberAsString] = !likedPost[postNumberAsString];
       
    }
    else {
        likedPost[postNumberAsString] = !isLiked;
    }
    if (!likedPost[postNumberAsString]) {
        icon.className = "far fa-thumbs-up fa-2x";
        value--;
    }
    else {
        value++;
        icon.className = "fas fa-thumbs-up fa-2x";
    }
    icon.textContent = value

    likeCounter.appendChild(icon);

    connection.invoke("SendPrivateNotificaion", friendId, message, userId, postId, !likedPost[postNumberAsString]).catch(function (err) {
        return console.error(err.toString());
    });
}

function addMessage(message, offset) {
    var li = document.createElement("li");
    li.className = 'row';

    var div = document.createElement('div');
    div.className = offset;

    var div2 = document.createElement('div');
    div2.className = "container darker bg-primary";

    var currentdate = new Date();
    var datetime = currentdate.getHours() + ":"
        + currentdate.getMinutes() + ":"
        + currentdate.getSeconds();

    var span = document.createElement('span');
    span.className = "time-left text-light";
    span.textContent = datetime;

    var p = document.createElement("p");
    p.className = "text-left text-white m-1";
    p.textContent = message;

    div2.appendChild(p);
    div2.appendChild(span);
    div.appendChild(div2);
    li.appendChild(div);

    document.getElementById("messagesList").appendChild(li);
}


function sendNotification(userId, friendId, eventId, notificationType) {
    connection.invoke("SendPrivateNotificaion", friendId, notificationType, userId, eventId, false).catch(function (err) {
        return console.error(err.toString());
    });
}