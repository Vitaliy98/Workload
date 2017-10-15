
function showAddSubject(state) {

    document.getElementById('secondWindowAddSubject').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}

function showDeleteSubject(state) {

    document.getElementById('secondWindowDeleteSubject').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}
function showAddExecLesson(state) {

    document.getElementById('secondWindowAddExecLesson').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}
function showChangeExecLesson(state) {

    document.getElementById('secondWindowChangeExecLesson').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}
function showDeleteExecLesson(state) {

    document.getElementById('secondWindowDeleteExecLesson').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}
function showAddUser(state) {

    document.getElementById('secondWindowAddUser').style.display = state;
    document.getElementById('mainWindow').style.display = state;
}

function showChangeUser(state) {

    document.getElementById('secondWindowChangeUser').style.display = state;
    document.getElementById('mainWindow').style.display = state;
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