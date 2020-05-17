

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("ReceiveNotification", function () {

    var notificationParagraph = document.getElementById("notifications");
    var value = parseInt(notificationParagraph.textContent, 10);

    value = isNaN(value) ? 0 : value;
    value++;
    notificationParagraph.className = "badge badge-danger";

    document.getElementById('notifications').textContent = value;
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

var likedPost = {};

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