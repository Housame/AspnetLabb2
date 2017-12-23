$(document).ready(function () {
    console.log('Hello world');
    
    var addUsers = $('#add-Users');
    var usersSelect = $('#sel1');
    //line: 15
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
});