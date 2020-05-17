$(document).ready(function () {

    var editButton = '<button title="Edit" class="editButton btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></button>';
    var deleteButton = '<button title="Delete" class="deleteButton btn btn-danger btn-xs"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></button>';
    var deactivateButton = '<button title="Update Status" class="deactivateButton btn btn-primary btn-xs"><span class="glyphicon glyphicon-briefcase" aria-hidden="true"></span></button>';

    var defaultContentLastCol = '<a href="/Home/Edit" class="editor_edit">'
        + editButton
        + '</a>   <a href="/Home/Delete" class="editor_remove">'
        + deleteButton
        + '</a>   <a href="#" class="editor_deactivate">'
        + deactivateButton
        + '</a>'


    $.getJSON("/api/Contact", function (result) {

        console.log(result);
        $('#contactGrid').DataTable({
            data: result.data,
            columns: [
                { "data": "ContactId" },
                { "data": "FirstName" },
                { "data": "LastName" },
                { "data": "Email" },
                { "data": "PhoneNo" },
                { "data": "Status" },
                {
                    "data": null,
                    "defaultContent": defaultContentLastCol
                }
            ],
            columnDefs: [
                {
                    targets: 0,
                    className: 'ContactId'
                },
                {
                    targets: 1,
                    className: 'FirstName'
                },
                {
                    targets: 2,
                    className: 'LastName'
                },
                {
                    targets: 3,
                    className: 'Email'
                },
                {
                    targets: 4,
                    className: 'PhoneNo'
                },
                {
                    targets: 5,
                    className: 'Status',
                    render: function (data, type, row) {
                        return data == true ? 'Active' : 'Inactive';
                    }
                }
            ]
        });


        $(".editButton").on("click mouseenter", function () {
            var id = $(this).closest("tr").find(".ContactId").text();
            console.log(id);
            var url = "/Home/Edit/" + id;
            $(this).parent("a").attr("href", url);
        });

        $(".deleteButton").on("click mouseenter", function () {
            var id = $(this).closest("tr").find(".ContactId").text();
            console.log(id);
            var url = "/Home/Delete/" + id;
            $(this).parent("a").attr("href", url);
        });

        $(".deactivateButton").on("click", function () {

            var id = $(this).closest("tr").find(".ContactId").text();
            var FirstName = $(this).closest("tr").find(".FirstName").text();
            var LastName = $(this).closest("tr").find(".LastName").text();
            var Email = $(this).closest("tr").find(".Email").text();
            var PhoneNo = $(this).closest("tr").find(".PhoneNo").text();
            console.log(id);

            var status = $(this).closest("tr").find(".Status").text();
            status = status == 'Inactive' ? false : true; //convert to  true meaning
            status = status == false ? true : false;   // toggle as there could be only one reason

            var newContact = {
                "ContactId": id,
                "FirstName": FirstName,
                "LastName": LastName,
                "Email": Email,
                "PhoneNo": PhoneNo,
                "Status": status

            };

            $.ajax({
                type: "PUT",
                dataType: "json",
                url: "/api/Contact/" + id,
                data: newContact,
                success: function (result) {

                    console.log(result);
                    var url = "/home/index";
                    $(location).attr('href', url);
                }
            });


        });

    });

    $("#deleteResultsButton").on("click", function () {

        $.ajax({
            type: "DELETE",
            dataType: "json",
            url: "/api/Contact/" + $("#ContactId").text().trim(),
            success: function (result) {
                console.log(result);
                var url = "/home/index";
                $(location).attr('href', url);
            }
        });
    });

    $("#putResultsButton").on("click", function () {

        var id = $("#ContactId").val();
        var fname = $("#FirstName").val();
        var lname = $("#LastName").val();
        var email = $("#Email").val();
        var phoneNo = $("#PhoneNo").val();

        if (fname == "" || lname == "" || email == "" || phoneNo == "") {
            console.log("empty input");
            $("#warningMessage").show();
        }
        else {
            $("#warningMessage").hide();
            var status = ($("#Status").val() == "" || $("#Status").val() == undefined) ? false : $("#Status").val();
            var newContact = {
                "ContactId": id,
                "FirstName": fname,
                "LastName": lname,
                "Email": email,
                "PhoneNo": phoneNo,
                "Status": true
            };

            $.ajax({
                type: "PUT",
                dataType: "application/json",
                url: "/api/Contact/" + $("#ContactId").val(),
                data: newContact,
                success: function (result) {

                    console.log(result);
                    var url = "/";
                    $(location).attr('href', url);
                }
            });
        }


        
    });

    $("#createButton").on("click", function () {

        var fname = $("#FirstName").val();
        var lname = $("#LastName").val();
        var email = $("#Email").val();
        var phoneNo = $("#PhoneNo").val();

        if (fname == "" || lname == "" || email == "" || phoneNo == ""){
            console.log("empty input");
            $("#warningMessage").show();
        }
        else {
            $("#warningMessage").hide();
            var newContact = {

                "FirstName": fname,
                "LastName": lname,
                "Email": email,
                "PhoneNo": phoneNo,
                "Status": true,

            };

            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/api/Contact",
                data: newContact,
                success: function (result) {
                    console.log(result);
                    var url = "/home/index";
                    $(location).attr('href', url);
                }
            });
        }
        
    });



});