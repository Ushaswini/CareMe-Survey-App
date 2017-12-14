﻿function ViewModel() {

    var self = this;
    var tokenKey = 'accessToken';

    self.result = ko.observable();
    self.user = ko.observable(); 
    self.loginEmail = ko.observable();
    self.loginPassword = ko.observable();
    self.errors = ko.observableArray([]);

    function showError(jqXHR) {
        console.log(jqXHR);
        self.result(jqXHR.status + ': ' + jqXHR.statusText);

        var response = jqXHR.resultObject;
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
            if (response.error_description) self.errors.push(response.error_description);
        }
    }

    self.login = function () {
        var loginData = {
            grant_type: 'password',
            username: self.loginEmail(),
            password: self.loginPassword()
        };
        $.ajax({
            type: 'POST',
            url: '/Home/LoginAsync',
            data: loginData
        }).done(function (data) {
            if (data.success == true) {
                sessionStorage.setItem(tokenKey, data.resultObject.Access_Token);
                window.location.href = data.resultObject.UserRole + '/Dashboard';
            } else {
                self.errors.removeAll();
                self.errors.push(data.responseText);
            }
            
        }).fail(showError);
    }
    
    function error (error) {
        //self.result('Only admin can login');
        self.errors.push('Only Admin  can login!!');
        console.log("user login");
    }

}

var app = new ViewModel();
ko.applyBindings(app);

