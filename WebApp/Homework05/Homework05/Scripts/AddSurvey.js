$(document).ready(function () {

    //to prevent back
   // window.onload  = window.history.forward();
    var app = new ViewModel();
    ko.applyBindings(app);

    var tokenKey = 'accessToken';

    self.surveysDataTable = $("#surveysTable").DataTable(
        {
            select: true,
            data: self.questions,
            dom: 'Bfrtip',
            buttons: [
                'print'
            ],
            columns: [{ data: "SurveyId" },{ data: "SurveyName" }]
        });

    self.questionsDataTable = $("#questionsTable").DataTable(
        {
            select: true,
            data: self.questions,
            dom: 'Bfrtip',
            columns: [
                { data: "Id" },
                { data: "Id" },
                { data: "QuestionText" },
                { data: "QuestionType" },
                { data: "Options" },
                { data: "Range" }]
        });

    LoadSurveys();
    LoadQuestions();

    function LoadQuestions() {
        console.log("loading questions");
        var headers = {};
        var token = sessionStorage.getItem(tokenKey);
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        console.log(token);
        $.ajax({
            type: 'GET',
            url: '/api/Questions',
            headers: headers,
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            self.questions = data;
            /*for (var i = 0; i < data.length; i++) {
                self.questions.push(data[i]);
            }*/
            BindQuestionsToDataTable();
        }).fail(showError);
    }
    
    function LoadSurveys() {
        var headers = {};
        var token = sessionStorage.getItem(tokenKey);

        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        console.log(token);
        $.ajax({
            type: 'GET',
            url: '/api/Surveys',
            headers: headers,
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            self.surveys = data;
            BindSurveysToDataTable();
        }).fail(showError);
    }

    function BindSurveysToDataTable(data) {
        self.surveysDataTable.clear();
        self.surveysDataTable.destroy();
        self.surveysDataTable = $("#surveysTable").DataTable(
            {
                select: true,
                data: self.surveys,
                dom: 'Bfrtip',
                buttons: [
                    'print'
                ],
                columns: [{ data: "SurveyId" },{ data: "SurveyName" }]
            });
    }

    function BindQuestionsToDataTable(data) {
        self.questionsDataTable.clear();
        self.questionsDataTable.destroy();
        self.questionsDataTable = $("#questionsTable").DataTable(
            {
                'columnDefs': [
                    {
                        'targets': 0,
                        'checkboxes': true
                    }],
                select: true,
                data: self.questions,
                dom: 'Bfrtip',
                columns: [
                    {
                        data: "Active",
                        render: function (data, type, row) {
                            if (type === 'display') {
                                return '<input type="checkbox" class="editor-active">';
                            }
                            return data;
                        },
                        className: "dt-body-center"
                    },
                    { data: "Id" },
                    { data: "QuestionText" },
                    { data: "QuestionType" },
                    { data: "Options" },
                    { data: "Range" }]
            });
    }

    $('#question-select-all').on('click', function () {
        // Check/uncheck all checkboxes in the table
        var rows = questionsDataTable.rows({ 'search': 'applied' }).nodes();
        $('input[type="checkbox"]', rows).prop('checked', this.checked);
    });

    // Handle click on checkbox to set state of "Select all" control
    $('#questionsTable tbody').on('change', 'input[type="checkbox"]', function () {
        // If checkbox is not checked
        if (!this.checked) {
            var el = $('#question-select-all').get(0);
            // If "Select all" control is checked and has 'indeterminate' property
            if (el && el.checked && ('indeterminate' in el)) {
                // Set visual state of "Select all" control 
                // as 'indeterminate'
                el.indeterminate = true;
            }
        }
    });

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

        //self.questions = {};
        self.surveys = {};
        self.surveyName = ko.observable();
        self.questions = ko.observable();

        self.result = ko.observable();
        self.errors = ko.observableArray([]);

        self.AddSurvey = function () {

            self.result('');
            self.errors.removeAll();

            var questionIds = '';

            //var tabledata = self.questionsDataTable.rows().nodes();
            //tabledata.each(function (value, index) {

            //    console.log(value.Active);
            //    //if (value.Active == true) {
            //    //    questionIds.append(value.Id + ",");
            //    //}
            //});

            self.questionsDataTable.rows().every(function (rowIdx, tableLoop, rowLoop) {
                var data = this.node();
                if ($(data).find('input').prop('checked')) {
                    questionIds = questionIds + self.questionsDataTable.cell(rowIdx,1).data() + ",";
                }
            });

            var data = {
                SurveyName: self.surveyName(),
                SurveyType: 0,
                QuestionIds_String: questionIds
            };

            var headers = {};
            var token = sessionStorage.getItem(tokenKey);
            if (token) {
                headers.Authorization = 'Bearer ' + token;
            }
            $.ajax({
                type: 'POST',
                url: '/api/Surveys/Post',
                headers: headers,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(data)
            }).done(function (data) {
                console.log(data);
                self.result("Done!");
                $('#addSurveyModel').modal('toggle');
                //Load questions
                LoadSurveys();
            }).fail(showError);
        }
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

})

