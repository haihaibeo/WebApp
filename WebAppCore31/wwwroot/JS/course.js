const courseId = window.location.href.split("#")[1];
IsUserAuthenticated(SetUiBasedOnRole);

function GetUserById(userId, callback){
    var xhr = new XMLHttpRequest();
    xhr.open("GET", "Account/GetUser/" + userId, true);
    xhr.onload = function(){
        var res = JSON.parse(this.response);
        if(typeof res.error === "undefined"){
            callback(res);
        }
        else console.log(res);
    }
    xhr.send();
}

function DisplayCourse(course, role)
{
    //#region button
    console.log(course);
    console.log("Role in this Dom is" + role);
    var DomApp = document.getElementById("root");
    const container = document.createElement("div");
    container.setAttribute("class", "container");
    container.style.paddingLeft = "0px";
    container.style.paddingRight = "0px";
    DomApp.appendChild(container);

    let card = document.createElement("div");
    card.style.padding = "10px";
    card.style.marginBottom = "10px";
    card.style.background = "white";
    card.style.boxShadow = "5px 5px 20px rgba(0,0,0,0.2)";
    card.style.borderRadius = "8px";
    card.style.overflow = "hidden";

    const editDiv = document.createElement("div");
    editDiv.setAttribute("class", "editDiv d-flex flex-column flex-md-row align-items-center bg-white");

    const editbtn = document.createElement("button");
    editbtn.setAttribute("class", "author btn btn-outline-secondary d-flex mr-2");
    editbtn.setAttribute("data-toggle", "modal");
    editbtn.setAttribute("data-target", "#modal-edit");
    editbtn.textContent = "Edit";
    editbtn.onclick = (event) =>
    {
        ModalEdit(course.id, course.title, course.subject, course.contentCourse, course.authorID);
    };
    
    const delbtn = document.createElement("button");
    delbtn.setAttribute("class", "author btn btn-outline-danger d-flex mr-2");
    delbtn.textContent = "Delete";
    delbtn.style.margin = "2";
    delbtn.onclick = (event) =>
    {
        var cnfm = confirm("Confirm delete?");
        if(cnfm == true)
        {
            DeleteCourse(function(){

            });
        }
    };  

    const subscribebtn = document.createElement("button");
    subscribebtn.setAttribute("class", "student btn btn-outline-primary d-flex justify-content-end student");
    subscribebtn.textContent = "Subscribe";
    subscribebtn.style.margin = "2";
    IsCourseSubscribed_ByCurrentUser(function(isSub){
        if(isSub === true){
            subscribebtn.setAttribute("class", "student btn btn-outline-secondary d-flex justify-content-end student");
            subscribebtn.textContent = "Subscribed";
            subscribebtn.onmouseenter = function(){
                subscribebtn.setAttribute("class", "student btn btn-outline-danger d-flex justify-content-end student");
                subscribebtn.textContent = "Unsubscribe";
                subscribebtn.onclick = function(){
                    Unsubscribe(function(message){
                        alert(message);
                        window.location.reload();
                    });
                }
            }
            subscribebtn.onmouseout = function(){
                subscribebtn.setAttribute("class", "student btn btn-outline-secondary d-flex justify-content-end student");
                subscribebtn.textContent = "Subscribed";
            }
        }
        else if(isSub === false){

        }
        else if(typeof isSub === undefined){

        }
    })
    subscribebtn.onclick = (event) =>{
        var cnfm = confirm("Subscribe?");
        if(cnfm == true)
        {
            SubscribeCourse();
            location.reload();
        }
    }
//#endregion
    const title_h2 = document.createElement("h2");
    title_h2.setAttribute("class", "mb-0 mr-md-auto");
    title_h2.textContent = course.title;
    title_h2.style.cursor = "pointer";
    title_h2.onclick = (event) => {
        window.location.href = "/course.html#" + course.id;
    }

    const subject_h5 = document.createElement("h5");
    subject_h5.setAttribute("class", " d-block mb-3 text-muted");
    subject_h5.textContent = course.subject;

    const content_p = document.createElement("legend");
    content_p.textContent = course.contentCourse; 
    
    const author_p = document.createElement("legend");
    author_p.textContent = "by ";
    const author_a = document.createElement("a");
    GetAuthorByCourseId(function(author){
        author_a.textContent = author.name;
    })
    author_a.href = "user/"+course.authorId;
    author_p.appendChild(author_a);
    subject_h5.appendChild(author_p);

    card.appendChild(editDiv);
    editDiv.appendChild(title_h2);
    if(role == "Author")
    {
        editDiv.appendChild(editbtn);
        editDiv.appendChild(delbtn);
    }
    else if(role == "Student")
    {
        editDiv.appendChild(subscribebtn);
    }
    card.appendChild(subject_h5);
    card.appendChild(document.createElement("hr"));
    card.appendChild(content_p);
    container.appendChild(card);
};

function GetCourseById(callback, role)
{
    let DomApp = document.getElementById("root");
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "Course/"+courseId, true); // true : asynchronous, false : deprecated
    
    xhr.onload = function()
    {
        var course = JSON.parse(this.response);
        if(xhr.readyState === 4)
        {
            if(xhr.status === 200)
            {
                callback(course, role);
            }
           else console.log("error!");
        }
    };
    xhr.send();
};    

function Unsubscribe(callback){
    let xhr = new XMLHttpRequest();
    xhr.open("DELETE", "course/unsubscribe/"+courseId);
    xhr.onload = function(){
        var res = JSON.parse(this.response);
        if(typeof res.error !== "undefined"){
            console.log(res);
            callback(res.message);
        }
    }
    xhr.send();
}

function SetUiBasedOnRole(role)
{
    let btnLog = document.getElementById("btnLog");
    let btnPublish = document.getElementById("btnPublish");
    btnLog.style.cursor = "pointer";
    btnPublish.style.cursor = "pointer";

    GetCourseById(DisplayCourse, role);
    GetCommentByCourseId(courseId, Display);

    if(role != ""){
        btnLog.setAttribute("class", "btn btn-outline-secondary");
        btnLog.setAttribute("data-target", "#");
        GetCurrentUser(function(user){
            btnLog.textContent = user.name;
            btnLog.onmouseover = function(){
                btnLog.textContent = "Log out";
            }
            btnLog.onmouseleave = function(){
                btnLog.textContent = user.name;
            }
        });

        btnLog.onclick = () => {
            Logout(function (res) {
                if (res === true) {
                    location.reload();
                }
            })
        }
        if (role == "Student") {

        }
        else if (role == "Author") {
            btnPublish.setAttribute("Class", "btn btn-outline-success mr-2");
            btnPublish.setAttribute("data-target", "#publish");
            btnPublish.setAttribute("data-toggle", "modal");
            btnPublish.textContent = "Publish";
            CanAuthorEditThisCourse(DisplayEditCourse);
        }
    }
    else {
        btnLog.setAttribute("class", "btn btn-outline-primary");
        btnLog.setAttribute("data-toggle", "modal")
        btnLog.setAttribute("data-target", "#exampleModal");
        btnLog.textContent = "Log in";
    }
}

function IsCourseSubscribed_ByCurrentUser(callback){
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "course/IsSubscribed/" + courseId);
    xhr.onload = function(){
        var msg = JSON.parse(this.responseText);
        if(typeof msg.error !== "undefined"){
            console.log(msg.message);
            callback(msg.message);
        }
        else callback(undefined);
    }
    xhr.send();
}

function DisplayEditCourse(canEdit){
    if(canEdit === true){
        var btns = document.querySelectorAll(".author");
        btns.forEach(btn =>{
            btn.disabled = false;
        })
    }
    else if(canEdit === false){
        var btns = document.querySelectorAll(".author");
        btns.forEach(btn =>{
            btn.disabled = true;
        })
    }
}

function ModalEdit(id, title, subject, info)
{
    let editTitle = document.querySelector("#edited-title");
    let editSubject = document.getElementById("edited-subject");
    let editInfo = document.getElementById("edited-content");
    const btnSubmitEdit = document.getElementById("submit-edit");

    editTitle.value = title;
    editSubject.value = subject;
    editInfo.value = info;

    btnSubmitEdit.onclick = (event) =>
    {
        let cnf = confirm("Confirm edit?");
        if(cnf == true)
        {
            x += "<img class=\"round\" src=\"images/" + Products[i].product_Name + ".JPG\">";;
            let xhr = new XMLHttpRequest();
            xhr.open("PUT", "/course/" + id, true);
            xhr.onload = function () {
                var res = JSON.parse(this.response);
                if (res.error === null) {
                    alert(res.message);
                    window.location.reload();
                } else alert(res.error);
            }
            xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
            xhr.send(JSON.stringify({
                "Subject" : editSubject.value,
                "Title" : editTitle.value,
                "ContentCourse" : editInfo.value
            }));
        }
    };
}

function GetAuthorByCourseId(callback){
    let xhr = new XMLHttpRequest();
    xhr.open("GET","course/GetAuthor/" + courseId)
    xhr.onload = function(){
        var result = JSON.parse(this.response);
        if(xhr.status === 200){
            console.log(result);
            callback(result);
        }
    }
    xhr.send();
}

function CanAuthorEditThisCourse(callback){
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "course/CanAuthorEditCourse/" + courseId);
    xhr.onload = function(){
        var result = JSON.parse(this.responseText);
        if(typeof result !== "undefined")
        {
            console.log("Can author edit this course : " + result.message);
            callback(result.message);
        }
    }
    xhr.send();
}

function Login()
{
    let login = document.getElementById("email").value;
    let password = document.getElementById("password").value;
    let rememberMe = document.getElementById("checkboxRemember");
    
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Account/Login");
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onreadystatechange = function(){
        if(xhr.responseText !== "")
        {
            let msg = JSON.parse(xhr.responseText);
            console.log(msg);
            if(msg.error !== null){
                console.log(msg.error);
                let errorDiv = document.getElementById("errorDisplayDiv");
                errorDiv.innerHTML = " ";
                msg.error.forEach(er => {
                    let erDiv = document.createElement("div");
                    erDiv.setAttribute("class", "form-group text-danger");
                    erDiv.textContent = er;
                    errorDiv.appendChild(erDiv);
                })
            }
            else{
                window.location.reload();
            }
        }
    }
    xhr.send(JSON.stringify({
        "Email": login,
        "Password": password,
        "RememberMe": rememberMe.checked,
    }));
}

function Logout(callback) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "Account/Logout", true);
    xhr.onload = function () {
        var res = JSON.parse(xhr.response);
        if (res.error === null) {
            callback(true);
        }
    }
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.send();
}

function Register() {
    let name = document.getElementById("name").value;
    let login = document.getElementById("email").value;
    let password = document.getElementById("password").value;
    let cnfPassword = document.getElementById("confirmpassword").value;
    let rememberMe = document.getElementById("checkboxRemember").checked;
    var role = document.querySelector('input[name="radio-stacked"]:checked').value;

    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Account/Register", true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onreadystatechange = function () {
        if (xhr.responseText !== "") {
            let msg = JSON.parse(this.responseText);
            if (msg.error !== null) {
                let errorDiv = document.getElementById("errorDisplayDiv");
                errorDiv.innerHTML = "";
                msg.error.forEach(er => {
                    let erDiv = document.createElement("div");
                    erDiv.setAttribute("class", "form-group text-danger");
                    erDiv.textContent = er;
                    errorDiv.appendChild(erDiv);
                })
            }
            else {
                alert(msg.message);
                window.location.reload();
            }
        }
    }
    xhr.send(JSON.stringify({
        "FullName": name,
        "Email": login,
        "Password": password,
        "ConfirmPassword": cnfPassword,
        "RememberMe": rememberMe,
        "Role": role
    }));
}

function IsUserAuthenticated(callback)
{
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "Account/IsAuthenticated", true);
    xhr.onload = function(){
        let msg = JSON.parse(this.responseText);
        console.log(msg);
        if(msg.message !== null){
            console.log(msg.message[0]);
            callback(msg.message[0]);
        }
        else {
            console.log(msg.error);
            callback("");
        }
    }
    xhr.send();
}

function GetUserName(id, callback)
{
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "/Account/GetUser", true);
    xhr.onload = function(){
        callback(JSON.parse(this.responseText));
    }
    xhr.send(JSON.stringify(id));
}

function GetCommentByCourseId(cId, callback)
{
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "Comment/GetComment/" + cId, true);
    xhr.onload = function()
    {
        let comments = JSON.parse(this.response)
        if(xhr.readyState === 4)
        {
            if(xhr.status === 200)
            {
               callback(comments);
            }
           else console.log("error!");
        }
    }
    xhr.send();
}

function Display(comments)
{
    let Dom = document.getElementById("ulComment");
    comments.forEach(cmt => {
        DisplayComments(cmt ,Dom);
    });
}

function DisplayComments(comment, Dom)
{
    //console.log(comment);
    const li_media = document.createElement("li");
    li_media.setAttribute("class", "media");

    const a = document.createElement("a");
    a.setAttribute("class", "float-left");
    a.href = "#";
    const img = document.createElement("img");
    img.src = "https://bootdey.com/img/Content/user_1.jpg"                                       
    img.setAttribute("class", "img-circle");
    a.appendChild(img);
    li_media.appendChild(a);

    const div_media = document.createElement("div");
    div_media.setAttribute("class", "media-body");
    const time_span = document.createElement("span");
    time_span.setAttribute("class", "text-muted float-right");
    div_media.appendChild(time_span);
    const small = document.createElement("small");
    small.setAttribute("class", "text-muted");
    small.textContent = comment.dateTime;
    time_span.appendChild(small);

    const strong = document.createElement("strong");
    strong.setAttribute("class", "text-success");
    GetUserById(comment.studentId, function(user){
        strong.textContent = "@"+user.name;
    })
    div_media.appendChild(strong);

    const comm_string_p = document.createElement("p");
    comm_string_p.textContent = comment.commentString;
    div_media.appendChild(comm_string_p);
    li_media.appendChild(div_media);
    Dom.appendChild(li_media);
}

function DisplayErrorPostingComment(status, message)
{
    let div = document.getElementById("div-comment-error");
    switch (status) {
        case 401:
            
            break;
    
        default:
            break;
    }
}

function PostComment(){
    let comment = document.getElementById("text-comment").value;
    let errorDiv = document.getElementById("error-div-comment");
    if (comment.replace(/\s+/g, '').length > 0) {
        let xhr = new XMLHttpRequest();
        xhr.open("POST", "/Comment/AddComment", true);
        xhr.onload = function(){
            if(xhr.status === 401){
                errorDiv.textContent = "Please login to add comments!";
            }
            else if(xhr.status === 200){
                window.location.reload();
            }
        }
        xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        xhr.send(JSON.stringify({
            "CourseId" : courseId,
            "UserId" : "a",
            "CommentString": comment
        }));
    } else {
        errorDiv.textContent = "Comment cannot be blank space!";
    }
}

function SubscribeCourse(callback)
{
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "course/subscribe/" + courseId, true);
    xhr.onload = function(){
        let msg = JSON.parse(this.response);
        if(typeof msg.error !== "undefined"){
            console.log(msg.error);
            callback(msg.error);
        }
        else{
            console.log(msg.message);
            callback(msg.message);
            window.location.reload();
        }
        callback(msg);
    }
    xhr.send();
}

function DeleteCourse(callback){
    var xhr = new XMLHttpRequest();
    xhr.open("DELETE", "course/" + courseId, true);
    xhr.onload = function(){
        var res = JSON.parse(this.response);
        console.log(res);
        if(xhr.status === 401){
            callback(res);
        }else if(xhr.status === 200){
            if(typeof res !== "undefined"){
                alert(res.message);
                window.location.href = "index.html";
            }
        }
    }
    xhr.send();
}

function GetCurrentUser(callback){
    var xhr = new XMLHttpRequest();
    xhr.open("GET", "Account/GetCurrentUser", true);
    xhr.onload = function(){
        if(xhr.status === 200){
            callback(JSON.parse(this.response));
        }
    }
    xhr.send();
}