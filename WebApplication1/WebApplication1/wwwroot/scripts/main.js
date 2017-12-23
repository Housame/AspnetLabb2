$(document).ready(function () {
    console.log('Hello world');

    var addUsers = $('#add-Users');
    var usersSelect = $('#sel1');
    var loginBtn = $('#login');
    var userName = $('#logged-User');
    var openPolicy = $('#open-policy');
    var hiddenPolicy = $('#hidden-policy');
    var hiddenPolicyAdult = $('#hidden-policy-20');
    var publishSportPolicy = $('#publish-sport-policy');
    var publishCulturePolicy = $('#publish-culture-policy');
    //line: 22
    getUsers();
    //On clicking all users will be removed from db and re-added
    addUsers.on('click', function () {
        console.log("add users calles");

        getUsers();
    });
    //Getting all users into logging select-form
    function getUsers() {
        $.ajax({
            url: '/api/user/add',
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
        console.log('login');
        var selected = usersSelect.find(":selected").text();
        console.log(selected);
        $.ajax({
            url: '/api/user/login',
            data: { email: selected },
            type: 'Post'
        }).done(function (result) {
            console.log('login succes');
            userName.text(selected);
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
        }).fail(function (xhr, status, error) {
            console.log('error');
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
        }).fail(function (xhr, status, error) {
            console.log('error');
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
        }).fail(function (xhr, status, error) {
            console.log('error');
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
        }).fail(function (xhr, status, error) {
            console.log('error');
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
        }).fail(function (xhr, status, error) {
            console.log('error');
        });

    });
});