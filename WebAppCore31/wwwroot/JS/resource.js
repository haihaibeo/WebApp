export function IsUserAuthenticated(callback)
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

export function Login(login, password, rememberMe)
{
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Account/Login");
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.onreadystatechange = function(){
        if(xhr.responseText !== "")
        {
            let msg = JSON.parse(xhr.responseText);
            if(typeof msg.error !== "undefined"){
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
export function DisplayCourse(course, DomApp)
{
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

    //#region buttons
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
    author_a.textContent = course.author.name;
    author_a.href = "user/"+course.author.id;
    author_p.appendChild(author_a);
    subject_h5.appendChild(author_p);

    card.appendChild(editDiv);
    editDiv.appendChild(title_h2);
    card.appendChild(subject_h5);
    card.appendChild(document.createElement("hr"));
    card.appendChild(content_p);
    container.appendChild(card);
}