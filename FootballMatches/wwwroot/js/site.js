// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    // ------------- Team ------------- //
    // Add new team
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

    // ------------- Match ------------- //

    // New match

    function getCurrentDate() {
        let today = new Date();
        return (today.getMonth() + 1) + "/" + today.getDate() + "/" + today.getFullYear();
    }

    // Datepicker
    $("#selected-date").datepicker({
        startDate: getCurrentDate()
    });
    // Team picker
    $("#host-team").on("change", function () {
        let teamId = $(this).val();
        updateAvailableTeams("guest", teamId);
        $.ajax({
            type: "GET",
            url: "/Match/AvailablePlayers",
            data: {"teamID": teamId},
            dataType: "json",
            success: function (response) {
                console.log(response);
            }
        });
        //console.log(teamId);
    });
    // Team can't play against itself
    // selector = host || guest
    function updateAvailableTeams(selector, disabledTeamID) {
        $("select#" + selector + "-team option." + selector).prop("disabled", false);
        $("select#"+ selector +"-team option."+ selector +"-" + disabledTeamID).prop("disabled", true);
    }

    function setPlayers(playersSelector, players) {

    }
});