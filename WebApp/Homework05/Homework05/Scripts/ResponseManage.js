$(document).ready(function () {

    var app = new ViewModel();
    ko.applyBindings(app);

    var tokenKey = 'accessToken';

    var userRole = sessionStorage.getItem("userRole");
    var userId = sessionStorage.getItem("userId");

    alert(userRole);

    console.log("aove table");

    self.responsesTable = $("#responsesTable").DataTable(
        {
            select: true,
            data: self.responses,
            dom: 'Bfrtip',
            buttons: [
                'print'
            ],
            columns: [{ data: "StudyGroupName" }, { data: "SurveyName" }, { data: "UserName" }, { data: "SurveyType" }]
        });

    if (userRole == "Admin")
        LoadStudyGroupsForAdmin();
    else
        LoadStudyGroups();

    if (userRole == "Admin")
        LoadResponsesForAdmin();
    else
        LoadResponses();

    function LoadResponsesForAdmin() {
        var headers = {};
        var token = sessionStorage.getItem(tokenKey);
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        console.log(token);
        $.ajax({
            type: 'GET',
            url: '/api/SurveyResponses',
            headers: headers,
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            console.log(data);
            self.responses = data
            BindResponsesDatatable();
        }).fail(showError);
    }

    function LoadResponses() {
        var headers = {};
        var token = sessionStorage.getItem(tokenKey);
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        console.log(token);
        $.ajax({
            type: 'GET',
            url: '/api/SurveyResponses/CoordinatorSurveyResponses?coordinatorId=' + userId,
            headers: headers,
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            console.log(data);
            self.responses = data
            BindResponsesDatatable();
        }).fail(showError);
    }

    function BindResponsesDatatable(data) {
        self.responsesTable.clear();
        self.responsesTable.destroy();
        self.responsesTable = $("#responsesTable").DataTable(
            {
                select: true,
                data: self.responses,
                dom: 'Bfrtip',
                buttons: [
                    'print'
                ],
                columns: [{ data: "StudyGroupName" }, { data: "SurveyName" }, { data: "UserName" }, { data: "SurveyType" }]
            });
    }

    function LoadStudyGroupsForAdmin() {
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

    function LoadStudyGroups() {
        var headers = {};
        var token = sessionStorage.getItem(tokenKey);
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        console.log(token);
        $.ajax({
            type: 'GET',
            url: '/api/StudyGroups/Coordinator?coordinatorId=' + userId,
            headers: headers,
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            console.log(data);
            for (var i = 0; i < data.length; i++) {
                self.studyGroups.push(data[i]);
            }
        }).fail(showError);
    }
    
    function ViewModel() {
        
        self.studyGroups = ko.observableArray([]);
        //self.responses = {}
        self.selectedStudyGroupForSurvey = ko.observable();
        self.responses = ko.observableArray([]);

        self.result = ko.observable();
        self.errors = ko.observableArray([]);
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