$(document).ready(function () {
    console.log('Hello world');
    var addUsers = $('#add-Users');

    addUsers.on('click', function () {
        console.log("add users calles");
        $.ajax({
            url: '/api/user/add',
            type: 'Get',
        }).done(function (result) {
            console.log('success');
            getUsers();
        }).fail(function (xhr, status, error) {
            console.log('error');
        });
    });

    function getUsers() {
        alert("users called!");
    }
});