$(document).ready(function () {

    var app = new ViewModel();
    ko.applyBindings(app);

    var tokenKey = 'accessToken';
    var user = sessionStorage.getItem('user');
    var headers = {};
    var token = sessionStorage.getItem(tokenKey);
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }
    /*$.ajax({
        type: 'GET',
        url: '/api/Users/Profile?id=' + user,
        headers: headers,
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        console.log(data);
        self.currentSC(data.UserName);
        title.innerText = title.innerText + " for " + data.UserName;
        titleGroup.innerText = titleGroup.innerText + " for " + data.UserName;
        //self.users = data;
        /*for (var i = 0; i < data.length; i++) {
            self.users.push(data[i]);
            console.log("users in table are" + data[i]);
        }

        // BindUsersToDatatable(data);
    }).fail(showError);*/

    console.log("document loaded");

    self.usersDataTable = $("#usersTable").DataTable(
        {
            select: true,
            data: self.users,
            dom: 'Bfrtip',
            buttons: [
                'print'
            ],
            columns: [{ data: "UserName" }, { data: "StudyGroupName" }]
        });
    LoadUsers();
    
    function LoadUsers() {
        var headers = {};
        var groupId = sessionStorage.getItem('group');
        var token = sessionStorage.getItem(tokenKey);
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        console.log(token);
        $.ajax({
            type: 'GET',
            url: '/api/Users/StudyGroups?groupId=' + groupId,
            headers: headers,
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            console.log(data);
            self.users = data;
            /*for (var i = 0; i < data.length; i++) {
                self.users.push(data[i]);
                console.log("users in table are" + data[i]);
            }*/

            BindUsersToDatatable(data);
        }).fail(showError);
    }

    function BindUsersToDatatable(data) {
        console.log(self.users);
        self.usersDataTable.clear();
        self.usersDataTable.destroy();
        self.usersDataTable = $("#usersTable").DataTable(
            {
                select: true,
                data: self.users,
                dom: 'Bfrtip',
                buttons: [
                    'print'
                ],
                columns: [{ data: "UserName" }, { data: "StudyGroupName" }]
            });
        
    }
   
    
    function ViewModel() {

        self.userName = ko.observable();
        self.userPassword = ko.observable();
        self.studyGroups = ko.observableArray([]);
        self.surveys = {}
        self.users = {}
        self.groups = {}
        self.userEmail = ko.observable();
        self.selectedStudyGroup = ko.observable();
        self.selectedStudyGroupForSurvey = ko.observable();

        self.result = ko.observable();
        self.errors = ko.observableArray([]);
        self.currentSC = ko.observable();
    }

    function showError(jqXHR) {
        //console.log(jqXHR);
        self.result(jqXHR.status + ': ' + jqXHR.statusText);

        var response = jqXHR.responseJSON;
        if (response) {
            if (response.Message) self.errors.push(response.Message);
            if (response.ModelState) {
                var modelState = response.ModelState;
                for (var prop in modelState) {
                    if (modelState.hasOwnProperty(prop)) {
                        var msgArr = modelState[prop]; // expect array here
                        if (msgArr.length) {
                            for (var i = 0; i < msgArr.length; ++i) self.errors.push(msgArr[i]);
                        }
                    }
                }
            }
            if (response.error) self.errors.push(response.error);
            if (response.error_description) {
                self.errors.push(response.error_description);
                console.log(response.error_description);
            }
        }
    }



});