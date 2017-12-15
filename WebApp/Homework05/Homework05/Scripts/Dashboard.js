$(document).ready(function () {

    //to prevent back
   // window.onload  = window.history.forward();
    var app = new ViewModel();
    ko.applyBindings(app);

    var tokenKey = 'accessToken';

    console.log("document loaded");

    self.usersDataTable = $("#usersTable").DataTable(
        {
            select: true,
            data: self.users,
            dom: 'Bfrtip',
            buttons: [
                'print'
            ],
            columns: [{ data: "UserName" }, { data: "CoordinatorName" }, { data: "StudyGroupName" } ]
        });

    self.coordinatorsTable = $("#coordinatorsTable").DataTable(
        {
            select: true,
            data: self.coordinators,
            dom: 'Bfrtip',
            buttons: [
                'print'
            ],
            columns: [{ data: "UserName" }]
        });

    LoadStudyGroups();
    LoadUsers();
    LoadCoordinators();

    function LoadUsers() {
        var headers = {};
        var token = sessionStorage.getItem(tokenKey);
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        console.log(token);
        $.ajax({
            type: 'GET',
            url: '/api/Users',
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
    function LoadCoordinators() {
        var headers = {};
        var token = sessionStorage.getItem(tokenKey);
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        console.log(token);
        $.ajax({
            type: 'GET',
            url: '/api/Users/coordinators',
            headers: headers,
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            console.log(data);
            self.coordinators = data;
            /*for (var i = 0; i < data.length; i++) {
                self.users.push(data[i]);
                console.log("users in table are" + data[i]);
            }*/

            BindCoordinatorsToDatatable(data);
        }).fail(showError);
    }
    

    function LoadStudyGroups() {
        var headers = {};
        var token = sessionStorage.getItem(tokenKey);
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        console.log(token);
        $.ajax({
            type: 'GET',
            url: '/api/StudyGroups',
            headers: headers,
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            console.log(data);
            for (var i = 0; i < data.length; i++) {
                self.studyGroups.push(data[i]);
            }
        }).fail(showError);
    }
    function BindCoordinatorsToDatatable(data) {
        console.log(self.coordinators);
        self.coordinatorsTable.clear();
        self.coordinatorsTable.destroy();
        self.coordinatorsTable = $("#coordinatorsTable").DataTable(
            {
                select: true,
                data: self.coordinators,
                dom: 'Bfrtip',
                buttons: [
                    'print'
                ],
                columns: [{ data: "UserName" }]
            });
        $('#coordinatorsTable tbody').on('click', 'tr', function () {
            var data = self.coordinatorsTable.row(this).data();
            //alert('You clicked on ' + data + '\'s row');
            console.log(data.Id);
            sessionStorage.setItem('user', data.Id);
            window.location.href = yourApp.Urls.userMessagesUrl;
        });
    }
    function BindUsersToDatatable(data) {
        console.log(self.users);
        self.usersDataTable.clear();
        self.usersDataTable.destroy();
        self.usersDataTable = $("#usersTable").DataTable(
            {
                select:true,
                data: self.users,
                dom: 'Bfrtip',
                buttons: [
                    'print'
                ],
                columns: [{ data: "UserName" }, { data: "CoordinatorName" }, { data: "StudyGroupName" }]
            });
        $('#usersTable tbody').on('click', 'tr', function () {
            var data = self.usersDataTable.row(this).data();
            //alert('You clicked on ' + data + '\'s row');
            console.log(data.Id);
            //sessionStorage.setItem('user', data.Id);
            //window.location.href = yourApp.Urls.userMessagesUrl;
        });
    }
    $('#btnFilter').click(function () {
        LoadUsersFiltered();

    })

    function LoadUsersFiltered() {
        var headers = {};
        var token = sessionStorage.getItem(tokenKey);
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        //string id = Application["groupId"].ToString();

        var urlTopass;// = self.selectedStudyGroupForSurvey();

        if ($('#filterUsers').val() == 0) {
            urlTopass = '/api/Users/Coordinators'
        } else {
            urlTopass = '/api/Users/StudyGroups'
        }
        console.log(id);
        console.log(token);
        $.ajax({
            type: 'GET',
            url: '/api/SurveyResponses?studyGroupId=' + id,
            headers: headers,
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            console.log(data);
            self.responses = data;
            BindSurveysToDatatable(data);
        }).fail(showError);
    }


    function guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
            s4() + '-' + s4() + s4() + s4();
    }

    function ViewModel() {
        
        self.userName = ko.observable();
        self.userPassword = ko.observable();
        self.userConfirmPassword = ko.observable();
        self.studyGroups = ko.observableArray([]);
        self.users = {}
        self.userEmail = ko.observable();
        self.selectedStudyGroup = ko.observable();
        self.selectedStudyGroupForSurvey = ko.observable();

        self.result = ko.observable();
        self.errors = ko.observableArray([]);
       
        AddStudyCoordinator = function () {

            self.result('');
            self.errors.removeAll();

            var data = {
                UserName: self.userName(),
                Password: self.userPassword(),
                ConfirmPassword: self.userConfirmPassword(),
                Email: self.userEmail()
            };
            var headers = {};
            var token = sessionStorage.getItem(tokenKey);
            if (token) {
                headers.Authorization = 'Bearer ' + token;
            }
            console.log("Data to add" + data);
            $.ajax({
                type: 'POST',
                url: '/api/Account/AddStudyCoordinator',
                headers: headers,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(data)
            }).done(function (data) {
                self.result("Done!");

                $('#addCoordinatorModal').modal('toggle');
                //Load users
                LoadCoordinators();
            }).fail(showError);
        }
        self.AddUser = function () {

            self.result('');
            self.errors.removeAll();

            var data = {
                UserName: self.userName(),
                Password: self.userPassword(),
                Email: self.userEmail(),
                StudyGroupId: self.selectedStudyGroup()
            };
            var headers = {};
            var token = sessionStorage.getItem(tokenKey);
            if (token) {
                headers.Authorization = 'Bearer ' + token;
            }
            console.log("Data to add" + data);
            $.ajax({
                type: 'POST',
                url: '/api/Account/AddUser',
                headers: headers,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(data)
            }).done(function (data) {
                self.result("Done!");

                $('#myModal').modal('toggle');
                //Load users
                LoadUsers();
            }).fail(showError);
        }

        
      
    }

    $("#presentSurvey").click(function () {

        console.log("indied button click");
        var tokenKey = 'accessToken';
        var headers = {};
        var token = sessionStorage.getItem(tokenKey);
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        console.log(token);
        var surveyData = {
            StudyGroupId: self.selectedStudyGroupForSurvey()
        }
        $.ajax({
            type: 'POST',
            url: '/api/GenerateSurvey',
            headers: headers,
            data: surveyData,

        }).done(function (data) {
            console.log("data is received");

            }).fail(showError);
    })

    
    $('#navigateToSurveyManager').click(function () {
        // Response.Redirect("~/Views/Survey/Manage.cshtml");

        window.location.href = yourApp.Urls.surveyManageUrl;
            //replace("~/Views/Survey/Manage");

    })


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

    

})

