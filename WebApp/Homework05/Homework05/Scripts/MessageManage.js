﻿$(document).ready(function () {

    var app = new ViewModel();
    ko.applyBindings(app);

    var tokenKey = 'accessToken';

    console.log("document loaded");
   
    self.surveysDataTable = $("#surveysTable").DataTable(
        {
            data: self.surveys,
            dom: 'Bfrtip',
            buttons: [
                'print'
            ],
            columns: [{ data: "QuestionText" }, { data: "QuestionFrequency" }, { data: "ResponseText" }, { data: "ResponseReceivedTime" }]
        });
    LoadSurveys();

    $('#surveyTable tbody').on('click', 'tr', function () {
        
    });

   

    function LoadSurveys() {
        var headers = {};
        var token = sessionStorage.getItem(tokenKey);
        var user = sessionStorage.getItem('user');
        console.log(user);
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        console.log(token);
        $.ajax({
            type: 'GET',
            url: '/api/SurveyResponses?userId='+user,
            headers: headers,
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            console.log(data);
            self.surveys = data;
            BindSurveysToDatatable(data);
        }).fail(showError);
    }

    function BindSurveysToDatatable(data) {
        console.log(self.surveys);
        self.surveysDataTable.clear();
        self.surveysDataTable.destroy();
        self.surveysDataTable = $("#surveysTable").DataTable(
            {
                data: self.surveys,
                dom: 'Bfrtip',
                buttons: [
                    'print'
                ],
                columns: [{ data: "QuestionText" }, { data: "QuestionFrequency" }, { data: "ResponseText" }, { data: "ResponseReceivedTime" }]
            });
    }
    function ViewModel() {

        self.userName = ko.observable();
        self.userPassword = ko.observable();
        self.studyGroups = ko.observableArray([]);
        self.surveys = {}
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