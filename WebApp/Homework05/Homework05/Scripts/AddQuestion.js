$(document).ready(function () {

    //to prevent back
   // window.onload  = window.history.forward();
    var app = new ViewModel();
    ko.applyBindings(app);

    var tokenKey = 'accessToken';

    self.questionsDataTable = $("#questionsTable").DataTable(
        {
            select: true,
            data: self.questions,
            dom: 'Bfrtip',
            buttons: [
                'print'
            ],
            columns: [{ data: "QuestionText" },
                { data: 'QuestionType' }]
        });

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
            console.log(data);
            self.questions = data;
            /*for (var i = 0; i < data.length; i++) {
                self.questions.push(data[i]);
            }*/
            BindQuestionsToDataTable();
        }).fail(showError);
    }

    function BindQuestionsToDataTable(data) {
        console.log(self.users);
        self.questionsDataTable.clear();
        self.questionsDataTable.destroy();
        self.questionsDataTable = $("#questionsTable").DataTable(
            {
                select: true,
                data: self.questions,
                dom: 'Bfrtip',
                buttons: [
                    'print'
                ],
                columns: [{ data: "QuestionText" },
                { data: 'QuestionType' }]
            });
       /* $('#questionsTable tbody').on('click', 'tr', function () {
            var data = self.questionsDataTable.row(this).data();
            //alert('You clicked on ' + data + '\'s row');
            console.log(data.Id);
            sessionStorage.setItem('user', data.Id);
            window.location.href = yourApp.Urls.userMessagesUrl;
        });*/
    }

    $('#questionType').on('change', function () {
        console.log(this.value);
        if (this.value == 0) {
            $('#options').css('visibility', 'hidden');
            $('#likeScaleProperties').css('visibility', 'hidden');
        }
        if (this.value == 1) {
            $('#options').css('visibility', 'visible');
            $('#likeScaleProperties').css('visibility', 'hidden');
        }
        if (this.value == 2) {
            $('#options').css('visibility', 'hidden');
            $('#likeScaleProperties').css('visibility', 'visible');
        }
        if (this.value == 3) {
            $('#options').css('visibility', 'hidden');
            $('#likeScaleProperties').css('visibility', 'hidden');
        }
    })

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

        self.questions = {}
        self.questionText = ko.observable();
        self.options = ko.observable();
        self.minBound = ko.observable();
        self.maxBound = ko.observable();
        self.interval = ko.observable();

        self.result = ko.observable();
        self.errors = ko.observableArray([]);

        self.AddQuestion = function () {

            self.result('');
            self.errors.removeAll();

            var data = {
                QuestionText: self.questionText(),
                QuestionType: $('#questionType').val(),
                Options: self.options(),
                Minimum: self.minBound(),
                Maximum: self.maxBound(),
                StepSize: self.interval()

            };

            console.log(JSON.stringify(data));

            var headers = {};
            var token = sessionStorage.getItem(tokenKey);
            if (token) {
                headers.Authorization = 'Bearer ' + token;
            }
            $.ajax({
                type: 'POST',
                url: '/api/Questions',
                headers: headers,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(data)
            }).done(function (data) {
                console.log(data);
                self.result("Done!");
                $('#addQuestionModel').modal('toggle');
                //Load questions
                LoadQuestions();
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

