const uri = "/api/course/";
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
    
// Refactoring : take DOM out and make it a new function
void function DisplayAllCourses()
{
    const container = document.createElement("div");
    container.setAttribute("class", "container");
    container.style.paddingLeft = "0px";
    container.style.paddingRight = "0px";
    app.appendChild(container);
    
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
                    let card = document.createElement("div");
                    card = setCard(card);
    
                    const editDiv = document.createElement("div");
                    editDiv.setAttribute("class", "d-flex flex-column flex-md-row align-items-center bg-white");
    
                    const editbtn = document.createElement("button");
                    editbtn.setAttribute("class", "btn btn-outline-secondary d-flex mr-2");
                    editbtn.setAttribute("data-toggle", "modal");
                    editbtn.setAttribute("data-target", "#modal-edit");
                    editbtn.textContent = "Edit";
                    editbtn.onclick = (event) =>
                    {
                        ModalEdit(course.id, course.title, course.subject, course.info, course.authorID);
                    };
                    
                    delbtn = document.createElement("button");
                    delbtn.setAttribute("class", " btn btn-outline-danger d-flex justify-content-end");
                    //delbtn.type = "button";
                    delbtn.textContent = "Delete";
                    delbtn.style.margin = "2";
                    delbtn.onclick = (event) =>
                    {
                        var cnfm = confirm("Confirm delete?");
                        if(cnfm == true)
                        {
                            DeleteCourse(course.id);
                            location.reload(true);
                            alert("Course deleted!");
                        }
                        else console.log("Canceled");
                    };  
                    
                    const title_h2 = document.createElement("h2");
                    title_h2.setAttribute("class", "mb-0 mr-md-auto");
                    title_h2.textContent = course.title;
        
                    const subject_h5 = document.createElement("h5");
                    subject_h5.setAttribute("class", " d-block mb-3 text-muted");
                    subject_h5.textContent = course.subject;
    
                    const info_p = document.createElement("p");
                    info_p.textContent = course.info; 
        
                    card.appendChild(editDiv);
                    editDiv.appendChild(title_h2);
                    editDiv.appendChild(editbtn);
                    editDiv.appendChild(delbtn);
                    card.appendChild(subject_h5);
                    card.appendChild(document.createElement("hr"));
                    card.appendChild(info_p);
                    container.appendChild(card);
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
    const btnSubmit = document.getElementById("submit-edit");

    editTitle.value = title;
    editSubject.value = subject;
    editInfo.value = info;

    btnSubmit.onclick = (event) =>
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
    console.log("Course with id = ", id, " deleted!");
}

// function SetCard_AddCourse()
// {
//     let addCourse = document.getElementById("card-add-course");
//     addCourse = setCard(addCourse); 
// }
// SetCard_AddCourse();

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
        "subject" : subject,
        "title" : title,
        "info" : info,
        "authorID" : 2
    }));

    location.reload();
}

function Login()
{
    alert("login");
    let login = document.getElementById("email").value;
    let password = document.getElementById("password").value;
    let rememberMe = document.getElementById("checkboxRemember");
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Account/Login");
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onreadystatechange = function(){
        //edit the page
    }
    xhr.send(JSON.stringify({
        "Email": login,
        "Password": password,
        "RememberMe": rememberMe.checked
    }));
}

function Register()
{
    alert("registerr");
}