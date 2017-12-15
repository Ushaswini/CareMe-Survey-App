$(document).ready(function () {

    var app = new ViewModel();
    ko.applyBindings(app);

    var tokenKey = 'accessToken';

    var userRole = sessionStorage.getItem("userRole");
    var userId = sessionStorage.getItem("userId");

    self.responsesTable = $("#responsesTable").DataTable(
        {
            select: true,
            data: self.users,
            dom: 'Bfrtip',
            buttons: [
                'print'
            ],
            columns: [{ data: "StudyGroupName" }, { data: "SurveyName" }, { data: "UserName" }, { data: "QuestionResponsesJson" }]
        });

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
            self.studyGroups = data
            BindStudyGroupDatatable();
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
            url: '/api/StudyGroups/GetResponsesForStudyCoordinator?coordinatorId=' + userId,
            headers: headers,
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            console.log(data);
            self.studyGroups = data
            BindStudyGroupDatatable();
        }).fail(showError);
    }

    function BindStudyGroupDatatable(data) {
        self.responsesTable.clear();
        self.responsesTable.destroy();
        self.responsesTable = $("#responsesTable").DataTable(
            {
                select: true,
                data: self.studyGroups,
                dom: 'Bfrtip',
                buttons: [
                    'print'
                ],
                columns: [{ data: "StudyGroupName" }, { data: "SurveyName" }, { data: "UserName" }, { data: "QuestionResponsesJson" }]
            });
    }

    if (userRole == 'admin')
        LoadStudyGroupsForAdmin();
    else
        LoadStudyGroups();

    if (userRole == 'admin')
        LoadResponsesForAdmin();
    else
        LoadResponses();

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

    
    function ViewModel() {
        
        self.studyGroups = ko.observableArray([]);
        self.responses = {}
        self.userEmail = ko.observable();
        self.selectedStudyGroup = ko.observable();
        self.selectedStudyGroupForSurvey = ko.observable();

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