// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {

    // Asdd new team
    // Submit 
    $("#add-team-confirm").on("click", function () {
        $("#add-team-form").submit();
    });
    // Clear modal form fields on cancel
    $("#add-team-cancel").on("click", function () {
        $("#add-team-name").val("");
        $("#add-team-description").val("");
    });


    // Remove player from team - confirm modal
    $(".remove-player").on("click", function () {
        console.log(this);
        let teamId = $(this).data("teamid");
        let id = $(this).data("id");
        console.log(teamId);
        console.log(id);
        $(".remove-player-confirm").on("click", function () {
            $.ajax({
                type: "POST",
                url: "/Team/RemovePlayer",
                data: {
                    "id": id,
                    "teamId": teamId
                },
                dataType: "json",
                success: function (response) {
                    if (response.playerRemoved) {
                        location.reload();
                    }
                }
            });
        });
    });
    // Submit new player form
    $("#add-player-confirm").on("click", function () {
        $("#add-player-form").submit();
    });
    // Cancel new player
    $("#add-player-cancel").on("click", function () {
        $("#playerName").val("");
    });
});