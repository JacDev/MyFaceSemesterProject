var config = {
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
    authority: "https://localhost:44387/",
    client_id: "MyFaceJsClient",
    redirect_uri: "https://localhost:44393/message/jslogin",
    post_logout_redirect_uri: "https://localhost:44393/Test/Index",
    response_type: "code",
    scope: "openid profile MyFaceApi userinfo"
};

var takenMessages = 10;

var userManager = new Oidc.UserManager(config);

function signIn() {
    userManager.signinRedirect();
};

function signOut() {
    userManager.signoutRedirect();
};

userManager.getUser().then(user => {
    console.log("user:", user);
    if (user) {
        axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
        console.log("user is logged");
    }
    else {
        console.log("User not log");
    }
});


var callApi = function () {
    var link = 'https://localhost:44349/api/users/6f63eccb-8929-41e2-834b-d32dcc9231bf/messages/43f007fe-2a6e-44ed-9f12-bc77e624f586/?PageSize=10&Skip=';
    var fulllink = link + takenMessages;
    axios.get(fulllink)
        .then(res => {
            reply = res;
            console.log(res);
            var data = reply.data;
            console.log(data);

            var list = document.getElementById("messagesList");
           
            data.forEach(function(item){
                wypisz(list, item)
            });
        });
};


function wypisz(list, item) {
    console.log(item['text']);
    if (userId == item['fromWho']) {
        addOldMessage(item['text'], "col-md-6 offset-md-6", list, item['when']);
    }
    else {
        addOldMessage(item['text'], "col-md-6 offset-md-0", list, item['when']);
    }
    takenMessages += 1;
}

var refreshing = false;

function addOldMessage(message, offset, list, when) {
    var li = document.createElement("li");
    li.className = 'row';

    var div = document.createElement('div');
    div.className = offset;

    var div2 = document.createElement('div');
    div2.className = "container darker bg-primary";

    var span = document.createElement('span');
    span.className = "time-left text-light";

    var currentdate = new Date(when);
    var datetime = currentdate.getHours() + ":"
        + currentdate.getMinutes() + ":"
        + currentdate.getSeconds();

    span.textContent = datetime;

    var p = document.createElement("p");
    p.className = "text-left text-white m-1";
    p.textContent = message;

    div2.appendChild(p);
    div2.appendChild(span);
    div.appendChild(div2);
    li.appendChild(div);

    list.insertBefore(li, list.childNodes[0]);
    
}


axios.interceptors.response.use(
    function (response) { return response; },
    function (error) {
        console.log("axios error:", error.response);

        var axiosConfig = error.response.config;

        //if error response is 401 try to refresh token
        if (error.response.status === 401) {
            console.log("axios error 401");

            // if already refreshing don't make another request
            if (!refreshing) {
                console.log("starting token refresh");
                refreshing = true;

                // do the refresh
                return userManager.signinSilent().then(user => {
                    console.log("new user:", user);
                    //update the http request and client
                    axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
                    axiosConfig.headers["Authorization"] = "Bearer " + user.access_token;
                    //retry the http request
                    return axios(axiosConfig);
                });
            }
        }

        return Promise.reject(error);
    });