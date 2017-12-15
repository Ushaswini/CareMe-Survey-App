$(document).ready(function () {

    //to prevent back
   // window.onload  = window.history.forward();
    var app = new ViewModel();
    ko.applyBindings(app);

    var tokenKey = 'accessToken';

    self.studyGroupTable = $("#studyGroupTable").DataTable(
        {
            select: true,
            data: self.users,
            dom: 'Bfrtip',
            buttons: [
                'print'
            ],
            columns: [{ data: "Id" }, { data: "StudyGroupName" }, { data: "CreatedTime" }]
        });

    LoadStudyGroups();

    function LoadStudyGroups() {
        var headers = {};
        var token = sessionStorage.getItem(tokenKey);
        var userId = sessionStorage.getItem("userId");
        console.log(userId);
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
            self.studyGroups = data
            BindStudyGroupDatatable();
        }).fail(showError);
    }
    
    function BindStudyGroupDatatable(data) {
        self.studyGroupTable.clear();
        self.studyGroupTable.destroy();
        self.studyGroupTable = $("#studyGroupTable").DataTable(
            {
                select:true,
                data: self.studyGroups,
                dom: 'Bfrtip',
                buttons: [
                    'print'
                ],
                columns: [{ data: "Id" }, { data: "StudyGroupName" }, { data: "CreatedTime" }]
            });
        $('#studyGroupTable tbody').on('click', 'tr', function () {
            var data = self.studyGroupTable.row(this).data();
            //alert('You clicked on ' + data + '\'s row');
            console.log(data.Id);
            sessionStorage.setItem('group', data.Id);
            window.location.href = yourApp.Urls.userListUrl;
        });
    }

    function ViewModel() {

        self.groupName = ko.observable();

        self.studyGroups = {}

        self.result = ko.observable();
        self.errors = ko.observableArray([]);
       

        self.AddStudyGroup = function () {

            self.result('');
            self.errors.removeAll();

            var coordinatorId = sessionStorage.getItem("userId");

            var data = {
                StudyGroupName: self.groupName(),
                StudyGroupCreadtedTime: new Date($.now()),
                StudyCoordinatorId: coordinatorId
            };
            var headers = {};
            var token = sessionStorage.getItem(tokenKey);
            if (token) {
                headers.Authorization = 'Bearer ' + token;
            }
            console.log("Data to add" + data);
            $.ajax({
                type: 'POST',
                url: '/api/StudyGroups',
                headers: headers,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(data)
            }).done(function (data) {
                self.result("Done!");

                $('#studyGroupModel').modal('toggle');
                //Load users
                LoadStudyGroups();
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

