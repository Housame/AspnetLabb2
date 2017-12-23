$(document).ready(function () {
    function test() {
        $.ajax({
            url: '/api/user/index',
            type: 'Get',
        }).done(function (result) {
            console.log('test index');
            console.log(result);
        }).fail(function (xhr, status, error) {
            console.log('test fail');
        });
    }
    test();
    function addroles() {
        $.ajax({
            url: '/api/user/addroles',
            type: 'Post'
        }).done(function (result) {
            console.log('roles added');
        }).fail(function (xhr, status, error) {
            console.log('fail to add roles ');
        });
    }
    addroles();
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