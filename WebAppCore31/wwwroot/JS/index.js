﻿//import * as module from "/JS/resource.js";
const uri = "/course/";

GetAllCourses(DisplayAllCourses);

function DisplayCourse(course)
{
    DomApp = document.getElementById("root");
    const container = document.createElement("div");
    container.setAttribute("class", "container");
    container.style.paddingLeft = "0px";
    container.style.paddingRight = "0px";
    DomApp.appendChild(container);

    let card = document.createElement("div");
    card.style.padding = "10px";
    card.style.marginBottom = "20px";
    card.style.background = "white";
    card.style.boxShadow = "5px 5px 20px rgba(0,0,0,0.2)";
    card.style.borderRadius = "8px";
    card.style.overflow = "hidden";
    
    const editDiv = document.createElement("div");
    editDiv.setAttribute("class", "d-flex flex-column flex-md-row align-items-center bg-white");
    // const editbtn = document.createElement("button");
    // editbtn.setAttribute("class", "btn btn-outline-secondary d-flex mr-2 author");
    // editbtn.setAttribute("data-toggle", "modal");
    // editbtn.setAttribute("data-target", "#modal-edit");
    // editbtn.textContent = "Edit";
    // editbtn.onclick = (event) =>
    // {
    //     ModalEdit(course.id, course.title, course.subject, course.contentCourse, course.authorID);
    // };
    
    // const delbtn = document.createElement("button");
    // delbtn.setAttribute("class", " btn btn-outline-danger d-flex mr-2 author");
    // delbtn.textContent = "Delete";
    // delbtn.style.margin = "2";
    // delbtn.onclick = (event) =>
    // {
    //     var cnfm = confirm("Confirm delete?");
    //     if(cnfm == true)
    //     {
    //         DeleteCourse(course.id);
    //         alert("Course deleted!");
    //     }
    //     else console.log("Canceled");
    // };  

    // const subscribebtn = document.createElement("button");
    // subscribebtn.setAttribute("class", " btn btn-outline-primary d-flex justify-content-end student");
    // subscribebtn.textContent = "Subscribe";
    // subscribebtn.style.margin = "2";
    // subscribebtn.onclick = (event) =>{
    //     var cnfm = confirm("Subscribe?");
    //     if(cnfm == true)
    //     {
    //         SubscribeCourse(course.id);
    //     }
    // }
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

    const content_p = document.createElement("p");
    content_p.textContent = course.contentCourse; 
    
    const author_p = document.createElement("p");
    author_p.textContent = "by ";
    const author_a = document.createElement("a");
    GetAuthorByCourseId(course.id, function(author){
        author_a.textContent = author.name;
        author_a.href = "user/"+author.id;
    })
    author_p.appendChild(author_a);
    subject_h5.appendChild(author_p);

    card.appendChild(editDiv);
    editDiv.appendChild(title_h2);
    card.appendChild(subject_h5);
    card.appendChild(document.createElement("hr"));
    card.appendChild(content_p);
    container.appendChild(card);
}

function DisplayAllCourses(courses){
    courses.forEach(c => DisplayCourse(c));
}

function GetAuthorByCourseId(courseId, callback){
    let xhr = new XMLHttpRequest();
    xhr.open("GET","course/GetAuthor/" + courseId)
    xhr.onload = function(){
        var result = JSON.parse(this.response);
        if(xhr.status === 200){
            //console.log(result);
            callback(JSON.parse(this.response));
        }
    }
    xhr.send();
}

function GetAllCourses(callback)
{
    var DomApp = document.getElementById("root");
    let xhr = new XMLHttpRequest();
    xhr.open("GET", uri, true); // true : asynchronous, false : deprecated
    
    xhr.onload = function()
    {
        var data = JSON.parse(this.response);
        if(xhr.readyState === 4)
        {
            if(xhr.status === 200)
            {
                callback(data);
            }
           else console.log("error!");
        }
    };
    xhr.send();
};

function ModalEdit(id, title, subject, info, autID)
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
            let xhr = new XMLHttpRequest();
            xhr.open("PUT", uri+id, true);
            xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
            xhr.send(JSON.stringify({
                "subject" : editSubject.value,
                "title" : editTitle.value,
                "info" : editInfo.value,
                "authorID" : autID
            }));
        }
    };
}

function AddNewCourse()
{
    let title = document.getElementById("new-title").value;
    let subject = document.getElementById("new-subject").value;
    let info = document.getElementById("content-course").value;

    let xhr = new XMLHttpRequest();
    xhr.open("POST", uri, true);
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.send(JSON.stringify({
        "Subject" : subject,
        "Title" : title,
        "ContentCourse" : info
    }));

    location.reload();
}

function SubscribeCourse(id)
{
    let xhr = new XMLHttpRequest();
    xhr.open("POST", uri + "subscribe/" + id, true);
    xhr.send();
    location.reload(true);
    console.log("Course with id = ", id, " deleted!");
}

function GetCurrentUser(callback){
    var xhr = new XMLHttpRequest();
    xhr.open("GET", "Account/GetCurrentUser", true);
    xhr.onload = function(){
        if(xhr.status === 200){
            console.log(JSON.parse(this.response));
            callback(JSON.parse(this.response));
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
    console.log(role);
    if(role != ""){
        btnLog.setAttribute("class", "btn btn-outline-secondary");
        btnLog.setAttribute("data-target", "#");
        GetCurrentUser(function(user){
            btnLog.textContent = user.name;
            btnLog.onmouseenter = function(){
                btnLog.textContent = "Log out";
            }
            btnLog.onmouseleave = function(){
                btnLog.textContent = user.name;
            }
        })
        btnLog.onclick = () => {
            Logout();
            location.reload();
        }
        if(role == "Student"){
            
        }
        else if(role == "Author"){
            btnPublish.setAttribute("Class", "btn btn-outline-success mr-2 author");
            btnPublish.setAttribute("data-target", "#publish");
            btnPublish.setAttribute("data-toggle", "modal");
            btnPublish.textContent = "Publish";
        }
    }
    else{
        btnLog.setAttribute("class", "btn btn-outline-primary");
        btnLog.setAttribute("data-toggle", "modal")
        btnLog.setAttribute("data-target", "#exampleModal");
        btnLog.textContent = "Log in";
    }
}

IsUserAuthenticated(SetUiBasedOnRole);
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
            if(msg.error !== null){
                console.log(msg.error);
                let errorDiv = document.getElementById("errorDisplayDiv");
                errorDiv.innerHTML = "";
                msg.error.forEach(er => {
                    let erDiv = document.createElement("div");
                    erDiv.setAttribute("class", "form-group text-danger");
                    erDiv.textContent = er;
                    errorDiv.appendChild(erDiv);
                })
            }
            else{
                alert(msg.message);
                location.reload();
            }
        }
    }
    xhr.send(JSON.stringify({
        "Email": login,
        "Password": password,
        "RememberMe": rememberMe.checked,
    }));
}

function Logout()
{
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "Account/Logout", true);
    xhr.onload = function(){
        var msg = JSON.parse(this.responseText);
        console.log(msg);
    }
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.send();
}

function Register()
{
    let name = document.getElementById("name").value;
    let login = document.getElementById("email").value;
    let password = document.getElementById("password").value;
    let cnfPassword = document.getElementById("confirmpassword").value;
    let rememberMe = document.getElementById("checkboxRemember").checked;
    var role = document.querySelector('input[name="radio-stacked"]:checked').value;

    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Account/Register", true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onreadystatechange = function()
    {
        if(xhr.responseText !== "")
        {
            let msg = JSON.parse(this.responseText)
            if(typeof msg.error !== "undefined")
            {
                let errorDiv = document.getElementById("errorDisplayDiv");
                errorDiv.innerHTML = "";
                msg.error.forEach(er => {
                    let erDiv = document.createElement("div");
                    erDiv.setAttribute("class", "form-group text-danger");
                    erDiv.textContent = er;
                    errorDiv.appendChild(erDiv);
                })
            }
            else{
                alert(msg.message);
                location.reload();
            }
        }
    }
    xhr.send(JSON.stringify({
        "FullName": name,
        "Email": login,
        "Password": password,
        "ConfirmPassword": cnfPassword,
        "RememberMe": rememberMe,
        "Role" : role
    }));
}