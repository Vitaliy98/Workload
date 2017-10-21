
function showAddSubject(state) {

    document.getElementById('secondWindowAddSubject').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}
function showChangeSubject(state,subjectId) {

    document.getElementById('secondWindowChangeSubject').style.display = state;
    document.getElementById('mainWindow').style.display = state;
    document.getElementById('hiddenSubjectId').value = subjectId;
}
function showDeleteSubject(state) {

    document.getElementById('secondWindowDeleteSubject').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}
function showAddExecLesson(state) {

    document.getElementById('secondWindowAddExecLesson').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}
function showChangeExecLesson(state, lessonId, date, lessonNumber) {

    document.getElementById('secondWindowChangeExecLesson').style.display = state;
    document.getElementById('mainWindow').style.display = state;
    document.getElementById('hiddenLessonId').value = lessonId;
    document.getElementById('hiddenDate').value = date;
    document.getElementById('hiddenLessonNumber').value = lessonNumber;
}
function showDeleteExecLesson(state) {

    document.getElementById('secondWindowDeleteExecLesson').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}
function showAddUser(state) {

    document.getElementById('secondWindowAddUser').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}

function showChangeUser(state, firstName, middleName, surname, positionId, email, password, teacherId) {

    document.getElementById('secondWindowChangeUser').style.display = state;
    document.getElementById('mainWindow').style.display = state;
    document.getElementById('FirstName').value = firstName;
    document.getElementById('MiddleName').value = middleName;
    document.getElementById('Surname').value = surname;
    var i;
    var index;
    for (i = 0; i < document.getElementById("Position").length; i++) {
        if (document.getElementById("Position").options[i].value === positionId)
        {
            index = i;
        }
    }
    document.getElementById("Position").options[index].selected = true;
    document.getElementById('Email').value = email;
    document.getElementById('Password').value = password;
    document.getElementById('TeacherId').value = teacherId;

}

function showDeleteUser(state) {

    document.getElementById('secondWindowDeleteUser').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}

function showDeleteWorkload(state) {

    document.getElementById('secondWindowDeleteWorkload').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}

function showAddWorkload(state) {

    document.getElementById('secondWindowAddWorkload').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}
function showChangeWorkload(state) {

    document.getElementById('secondWindowChangeWorkload').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}
function showChangeProfil(state, firstName, middleName, surname, email, password) {

    document.getElementById('secondWindowChangeProfil').style.display = state;
    document.getElementById('mainWindow').style.display = state;
    document.getElementById('FirstName').value = firstName;
    document.getElementById('MiddleName').value = middleName;
    document.getElementById('Surname').value = surname;
    document.getElementById('Email').value = email;
    document.getElementById('Password').value = password;
}
function hideAllWindows() {

    document.getElementById('secondWindowAddWorkload').style.display = "none";
    document.getElementById('mainWindow').style.display = "none";
    document.getElementById('secondWindowDeleteWorkload').style.display = "none";
    document.getElementById('secondWindowAddSubject').style.display = "none";
    document.getElementById('secondWindowAddUser').style.display = "none";
    document.getElementById('secondWindowDeleteUser').style.display = "none";
    document.getElementById('secondWindowDeleteSubject').style.display = "none";
    ocument.getElementById('secondWindowChangeUser').style.display = "none";
}