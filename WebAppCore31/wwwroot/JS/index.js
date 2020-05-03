const uri = "/course/";
var app = document.getElementById("root");

function setCard(element)
{
    element.style.padding = "10px";
    element.style.marginBottom = "10px";
    element.style.background = "white";
    element.style.boxShadow = "2px 4px 25px rgba(0,0,0,0.1)";
    element.style.borderRadius = "8px";
    element.style.overflow = "hidden";

    return element;
}

function DisplayCourse(course, DomApp)
{
    const container = document.createElement("div");
    container.setAttribute("class", "container");
    container.style.paddingLeft = "0px";
    container.style.paddingRight = "0px";
    DomApp.appendChild(container);

    let card = document.createElement("div");
    card = setCard(card);

    const editDiv = document.createElement("div");
    editDiv.setAttribute("class", "d-flex flex-column flex-md-row align-items-center bg-white");

    const editbtn = document.createElement("button");
    editbtn.setAttribute("class", "btn btn-outline-secondary d-flex mr-2 author");
    editbtn.setAttribute("data-toggle", "modal");
    editbtn.setAttribute("data-target", "#modal-edit");
    editbtn.textContent = "Edit";
    editbtn.onclick = (event) =>
    {
        ModalEdit(course.id, course.title, course.subject, course.contentCourse, course.authorID);
    };
    
    delbtn = document.createElement("button");
    delbtn.setAttribute("class", " btn btn-outline-danger d-flex mr-2 author");
    delbtn.textContent = "Delete";
    delbtn.style.margin = "2";
    delbtn.onclick = (event) =>
    {
        var cnfm = confirm("Confirm delete?");
        if(cnfm == true)
        {
            DeleteCourse(course.id);
            alert("Course deleted!");
        }
        else console.log("Canceled");
    };  

    const subscribebtn = document.createElement("button");
    subscribebtn.setAttribute("class", " btn btn-outline-primary d-flex justify-content-end student");
    subscribebtn.textContent = "Subscribe";
    subscribebtn.style.margin = "2";
    subscribebtn.onclick = (event) =>{
        var cnfm = confirm("Subscribe?");
        if(cnfm == true)
        {
            SubscribeCourse(course.id);
        }
    }

    const title_h2 = document.createElement("h2");
    title_h2.setAttribute("class", "mb-0 mr-md-auto");
    title_h2.textContent = course.title;
    title_h2.style.cursor = "pointer";
    title_h2.onclick = (event) => {
        alert("Go to courseId = "+ course.id);
        GetToThisCourse(course.id);
    }

    const subject_h5 = document.createElement("h5");
    subject_h5.setAttribute("class", " d-block mb-3 text-muted");
    subject_h5.textContent = course.subject;

    const content_p = document.createElement("p");
    content_p.textContent = course.contentCourse; 
    
    const author_p = document.createElement("p");
    author_p.textContent = "by ";
    const author_a = document.createElement("a");
    author_a.textContent = course.author.name;
    author_a.href = "user/"+course.author.id;
    author_p.appendChild(author_a);
    subject_h5.appendChild(author_p);

    card.appendChild(editDiv);
    editDiv.appendChild(title_h2);
    editDiv.appendChild(editbtn);
    editDiv.appendChild(delbtn);
    editDiv.appendChild(subscribebtn);
    card.appendChild(subject_h5);
    card.appendChild(document.createElement("hr"));
    card.appendChild(content_p);
    container.appendChild(card);
}

// Refactoring : take DOM out and make it a new function
void function DisplayAllCourses()
{
    let xhr = new XMLHttpRequest();
    xhr.open("GET", uri, true); // true : asynchronous, false : deprecated
    
    xhr.onload = function()
    {
        var data = JSON.parse(this.response);
        if(xhr.readyState === 4)
        {
            if(xhr.status === 200)
            {
                data.forEach(course => {
                    DisplayCourse(course, app);
               });
            }
           else console.log("error!");
        }
    };
    xhr.send();
}();

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

function DeleteCourse(id)
{
    let xhr = new XMLHttpRequest();
    xhr.open("DELETE", uri + id, true);
    xhr.send();
    location.reload(true);
    console.log("Course with id = ", id, " deleted!");
}

function AddNewCourse()
{
    let title = document.getElementById("new-title").value;
    let subject = document.getElementById("new-subject").value;
    let info = document.getElementById("content-course").value;

    console.log(title);
    console.log(subject);
    console.log(info);

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

function SetLogBtn()
{
    let btnLog = document.getElementById("btnLog");
    btnLog.setAttribute("class", "btn btn-outline-secondary");
    btnLog.setAttribute("data-target", "#");
    btnLog.textContent = "Log out";
    btnLog.onclick = () => {
        Logout();
        location.reload();
    }
}

void function IsUserAuthenticated()
{
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "Account/IsAuthenticated", true);
    xhr.onload = function(){
        let msg = JSON.parse(this.responseText);
        if(msg.error == ""){
            if(msg.role.length == 1)
            {
                ChangeUIRole(msg.role[0]);
            }
        }
        else {
            console.log(msg.error);
            ChangeUIGuest();
        }
    }
    xhr.send();
}();

function ChangeUIRole(role)
{
    SetLogBtn();
    switch (role) {
        case "Author":
            console.log(role);
            ChangeUIAuthor();
            break;
        case "Student":
            console.log(role);
            ChangeUIStudent();
            break;
        default:
            break;
    }
}

function ChangeUIGuest()
{
    let btnAuthor = document.querySelectorAll(".author");
    let btnStudent = document.querySelectorAll(".student");
    console.log("foooo");
    btnAuthor.forEach(btn => {
        btn.parentElement.removeChild(btn);
    });
    
    btnStudent.forEach(btn => {
        btn.parentElement.removeChild(btn);
    });
}

function ChangeUIAuthor()
{

}

function ChangeUIStudent()
{
    let btnAuthorEdit = document.querySelectorAll(".author");
    btnAuthorEdit.forEach(btn => {
        btn.parentElement.removeChild(btn);
    });
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
            if(typeof msg.error !== "undefined"){
                console.log(msg.error);
            }
            else{
                location.reload();
                SetLogBtn();
            }
        }
    }
    xhr.send(JSON.stringify({
        "Email": login,
        "Password": password,
        "RememberMe": rememberMe.checked
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
    alert("registerr");
}