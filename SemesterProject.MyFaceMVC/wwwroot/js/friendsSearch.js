function updateResult(query, userId) {
    let resultList = document.querySelector(".result");
    resultList.innerHTML = "";


    var baseLink = 'https://localhost:44349/api/users/with/';
    var fullLink = baseLink + query;

    var request = new XMLHttpRequest()

    request.open('GET', fullLink, true)
    request.onload = function () {
        var data = JSON.parse(this.response)

        if (request.status >= 200 && request.status < 400) {
            data.forEach(user => {
                var userData = user['firstName'] + ' ' + user['lastName'];

                var path = user['profileImagePath'];

                var pic = '';
                if (path!=null) {
                    var imagePath = "/Image/" + path;
                    pic += '<div class="float-left mt-1 pr-4"><img class="rounded-circle" src=';
                    pic += imagePath;
                    pic += '/></div>';
                }

                resultList.innerHTML += `<li class="list-group-item d-flex justify-content-between"> <a href="/Friends/ViewProfile?friendId=${user['id']}"> ${pic} ${userData} </a>
                   <button onclick="sendNotification('${userId}', '${user['id']}', null, 'friendRequest')" class="btn btn-light btn-sm offset-sm-6"><i class="fas fa-user-plus"></i></button>
                    </li>`;
            })
        } 
    }

    request.send();
}