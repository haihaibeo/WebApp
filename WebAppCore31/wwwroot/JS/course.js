const courseId = window.location.href.split("#")[1];

function DisplayCourse(course, DomApp)
{
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
    
    const delbtn = document.createElement("button");
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
    author_a.textContent = "Authorr"
    author_a.href = "user/"+course.authorId;
    author_p.appendChild(author_a);
    subject_h5.appendChild(author_p);

    card.appendChild(editDiv);
    editDiv.appendChild(title_h2);
    card.appendChild(subject_h5);
    card.appendChild(document.createElement("hr"));
    card.appendChild(content_p);
    container.appendChild(card);
}

void function DisplayCourseAllCourse()
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
                DisplayCourse(course, DomApp);
            }
           else console.log("error!");
        }
    };
    xhr.send();
}();    

function SetLogBtn(role)
{
    let btnLog = document.getElementById("btnLog");
    if(role != ""){
        btnLog.setAttribute("class", "btn btn-outline-secondary");
        btnLog.setAttribute("data-target", "#");
        btnLog.textContent = "Log out";
        btnLog.onclick = () => {
            Logout();
            location.reload();
        }
    }
    else{
        btnLog.setAttribute("class", "btn btn-outline-primary");
        btnLog.setAttribute("data-toggle", "modal")
        btnLog.setAttribute("data-target", "#exampleModal");
        btnLog.textContent = "Log in";
    }
}

IsUserAuthenticated(SetLogBtn);

function IsCourseSubscribed_ByCurrentUser(callback){
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "course/IsSubscribed/" + courseId);
    xhr.onload = function(){
        callback(JSON.parse(this.responseText));
    }
}

function IsUserAuthenticated(callback)
{
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "Account/IsAuthenticated", true);
    xhr.onload = function(){
        let msg = JSON.parse(this.responseText);
        if(msg.error == "" && msg.role.length == 1){
            callback(msg.role[0]);
        }
        else {
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

GetCommentByCourseId(courseId, Display);

function Display(comments)
{
    let Dom = document.getElementById("ulComment");
    comments.forEach(cmt => {
        DisplayComments(cmt ,Dom);
    });
}

function DisplayComments(comment, Dom)
{
    console.log(comment);
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
    strong.textContent = "@"+comment.studentId;
    div_media.appendChild(strong);

    const comm_string_p = document.createElement("p");
    comm_string_p.textContent = comment.commentString;
    div_media.appendChild(comm_string_p);
    li_media.appendChild(div_media);
    Dom.appendChild(li_media);
}