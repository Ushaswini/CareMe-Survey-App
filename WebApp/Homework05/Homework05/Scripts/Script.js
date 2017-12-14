
$(document).ready(function () {

    var tokenKey = 'accessToken';

    $("#menu-toggle").click(function (e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
    });

    $("#logout").click(function (e) {
        e.preventDefault();

        
       /* var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }*/

        $.ajax({
            type: 'POST',
            url: '/Home/LogOffAsync'
        }).done(function (data) {
            // Successfully logged out. Delete the token.
            // self.user('');
            window.location.href = yourApp.Urls.homeScreenUrl;
            sessionStorage.removeItem(tokenKey);
            //$("#sidebar-wrapper").css('display', 'none');
            //$("#lblGreetings").css('display', 'none');

        }).fail(showError);
    })

    function showError(jqXHR) {
        console.log(jqXHR);
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
            if (response.error_description) self.errors.push(response.error_description);
        }
    }
});