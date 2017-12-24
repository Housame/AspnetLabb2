$(document).ready(function () {
    console.log('Hello world');

    var resetUsers = $('#reset-users');
    var usersSelect = $('#sel1');
    var loginBtn = $('#login');
    var userName = $('#logged-User');
    var openPolicy = $('#open-policy');
    var hiddenPolicy = $('#hidden-policy');
    var hiddenPolicyAdult = $('#hidden-policy-20');
    var publishSportPolicy = $('#publish-sport-policy');
    var publishCulturePolicy = $('#publish-culture-policy');
    var usersAndClaims = $("#GetAllUsersWithClaims");
    var infoCol = $('#info-col');
    var claimInfo = $('#info-claim');
    //line: 34
    getUsers();
    //On clicking all users will be removed from db and re-added
    resetUsers.on('click', function () {
        usersSelect.find('option').remove();
        console.log("add users calles");
        $.ajax({
            url: '/api/user/reset',
            type: 'Get'
        }).done(function (result) {
            console.log('reset-success');
            console.log(result);
            getUsers();
        }).fail(function (xhr, status, error) {
            console.log('error');
        }); 
    });
    //Getting all users into logging select-form
    function getUsers() {
        $.ajax({
            url: '/api/user/getusers',
            type: 'Get'
        }).done(function (result) {
            console.log('success');
            console.log(result);
            $.each(result, function (key, value) {
                usersSelect
                    .append($("<option></option>")
                        .attr("value", value.id)
                        .text(value.email));
            });
        }).fail(function (xhr, status, error) {
            console.log('error');
        });
    }
    //Login when user is selected
    loginBtn.on('click', function () {
        $('#info-access').text(''); 
        $('#info-age').text('');
        console.log('login');
        var selected = usersSelect.find(":selected").text();
        console.log(selected);
        $.ajax({
            url: '/api/user/login',
            data: { email: selected },
            type: 'Post'
        }).done(function (result) {
            console.log('login succes');
            console.log(result);
            userName.text(selected);
            $('#info-name').append("<h4></h4>").text('Namn: ' + result.firstName);
            if (result.age != null) {
                $('#info-age').append("<h4></h4>").text('Ålder: ' + result.age);
            }
            $('#info-email').append("<h4></h4>").text('Email: ' + result.email);
            //$('#info-role').append("<h4></h4>").text('Namn: ' + result.role);
        }).fail(function (xhr, status, error) {
            console.log('error');
        });

    });
    //Get all users with their claims
    usersAndClaims.click(function () {
        console.log("get users with their claims");
        $.ajax({
            url: '/api/user/usersnclaims',
            method: 'GET'
        }).done(function (result) {
            console.log(result);

        }).fail(function (xhr, status, error) {
            console.log('error');
        });
    });
    //Gettinga all users that can see  open news
    openPolicy.on('click', function () {
        console.log('open-Policy');
        $.ajax({
            url: '/api/user/open',
            type: 'Get'
        }).done(function (result) {
            console.log('open-Policy succes');
            console.log(result);
            $('#info-access').removeClass("access-red").addClass("access-green");
            $('#info-access').append('<h3></h3>').text('Se öppna nyheter, kod 200');
        }).fail(function (xhr, status, error) {
            console.log('error');
            $('#info-access').removeClass("access-green").addClass("access-red");
            $('#info-access').append('<h3></h3>').text('Se öppna nyheter, kod 403');
        });

    });
    //Getting all users that can see hidden news
    hiddenPolicy.on('click', function () {
        console.log('hidden-Policy');
        $.ajax({
            url: '/api/user/hidden',
            type: 'Get'
        }).done(function (result) {
            console.log('hidden-Policy succes');
            console.log(result);
            $('#info-access').removeClass("access-red").addClass("access-green");
            $('#info-access').append('<h3></h3>').text('Se dolda nyheter, kod 200');
        }).fail(function (xhr, status, error) {
            console.log('error');
            $('#info-access').removeClass("access-green").addClass("access-red");
            $('#info-access').append('<h3></h3>').text('Se dolda nyheter, kod 403');
        });

    });
    //Getting users that can se all hidden news and >= than 20 years
    hiddenPolicyAdult.on('click', function () {
        console.log('hidden-Policy-20');
        $.ajax({
            url: '/api/user/age',
            type: 'Get'
        }).done(function (result) {
            console.log('hidden-Policy-20 succes');
            console.log(result);
            $('#info-access').removeClass("access-red").addClass("access-green");
            $('#info-access').append('<h3></h3>').text('Se dolda nyheter och är äldre än 20år, kod 200');
        }).fail(function (xhr, status, error) {
            console.log('error');
            $('#info-access').removeClass("access-green").addClass("access-red");
            $('#info-access').append('<h3></h3>').text('Se dolda nyheter och är äldre än 20år, kod 403');
        });

    });
    //Getting users that can publish Sport news
    publishSportPolicy.on('click', function () {
        console.log('publish-Sport-Policy');
        $.ajax({
            url: '/api/user/sport',
            type: 'Get'
        }).done(function (result) {
            console.log('publish-sport-Policy succes');
            console.log(result);
            $('#info-access').removeClass("access-red").addClass("access-green");
            $('#info-access').append('<h3></h3>').text('Publicera sport nyheter, kod 200');
        }).fail(function (xhr, status, error) {
            console.log('error');
            $('#info-access').removeClass("access-green").addClass("access-red");
            $('#info-access').append('<h3></h3>').text('Publicera sport nyheter, kod 403');
        });

    });
    //Getting users that can publish Culture news
    publishCulturePolicy.on('click', function () {
        console.log('publish-Culture-Policy');
        $.ajax({
            url: '/api/user/culture',
            type: 'Get'
        }).done(function (result) {
            console.log('publish-Culture-Policy succes');
            console.log(result);
            $('#info-access').removeClass("access-red").addClass("access-green");
            $('#info-access').append('<h3></h3>').text('Publicera kultur nyheter, kod 200');
        }).fail(function (xhr, status, error) {
            console.log('error');
            $('#info-access').removeClass("access-green").addClass("access-red");
            $('#info-access').append('<h3></h3>').text('Publicera kultur nyheter, kod 403');
        });

    });
});