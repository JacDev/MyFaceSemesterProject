"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("submitButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    addMessage(message, "col-md-6 offset-md-0");
});

connection.start().then(function () {
    document.getElementById("submitButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
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

