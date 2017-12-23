$(document).ready(function () {
    console.log('Hello world');

    var addUsers = $('#add-Users');
    var usersSelect = $('#sel1');
    var loginBtn = $('#login');
    var userName = $('#logged-User');
    //line: 17
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
            type: 'Get',
        }).done(function (result) {
            console.log('success');
            console.log(result);
            $.each(result, function (key, value) {
                usersSelect
                    .append($("<option></option>")
                        .attr("value", value.id)
                        .text(value.email));
            })
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
});