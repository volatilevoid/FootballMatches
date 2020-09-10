$(document).ready(function () {
    // -------------------------- Team -------------------------- //
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

    // Check if new player name is not empty
    $("#new-player-input").on("input", function () {
        if ($(this).val().length !== 0) {
            $("#add-player-confirm").prop("disabled", false)
        }
        else {
            $("#add-player-confirm").prop("disabled", false)
        }
    });


    // -------------------------- Match -------------------------- //


    // ---------------- New ---------------- //
    function getCurrentDate() {
        let today = new Date();
        return (today.getMonth() + 1) + "/" + today.getDate() + "/" + today.getFullYear();
    }

    // Datepicker init
    $("#selected-date").datepicker({
        startDate: getCurrentDate(),
        autoclose: true
    });
    // Get available teams for new date
    $("#selected-date").on("change", function () {
        // Hide validation labels
        resetAllValidation();
        $.ajax({
            type: "GET",
            url: "/Match/AvailableTeams",
            data: { "matchDate": $(this).val() },
            dataType: "json",
            success: function (teams) {
                clearPlayers("host");
                clearPlayers("guest");
                setTeams(teams, "host");
                setTeams(teams, "guest");
            }
        });
    });

    // Team can't play against itself -> update other select input
    // selector = host || guest
    function updateAvailableTeams(selector, disabledTeamID) {
        $("select#" + selector + "-team option." + selector).prop("disabled", false);
        $("select#" + selector + "-team option." + selector + "-" + disabledTeamID).prop("disabled", true);
    }
    // Clear all players
    function clearPlayers(hostOrGuest) {
        $("." + hostOrGuest + "-players").html("");
    }

    // Set available teams for selected match date
    function setTeams(teams, hostOrGuest) {
        let capitalizedPlace = hostOrGuest.charAt(0).toUpperCase() + hostOrGuest.slice(1);
        let content = '<option selected value="">Select ' + capitalizedPlace + '</option>';
        $(hostOrGuest + "-team").html("");
        if (Array.isArray(teams, hostOrGuest)) {
            teams.forEach(team => {
                content += '<option class="' + hostOrGuest + ' ' + hostOrGuest + '-' + team.id + '" value="' + team.id + '">' + team.name + '</option>';
            });    
            $("#" + hostOrGuest + "-team").html(content);
        }
    }

    // Set available players for selected team
    function setPlayers(players, teamId, hostOrGuest) {
        let content = "";
        // Clear prevous players
        $("." + hostOrGuest + "-players").html("");
        if (players !== undefined && Array.isArray(players) && players.length !== 0) {
            for (let i = 0, len = players.length; i < len; i++) {
                content += '<div class="form-check">';
                content += '<input class="form-check-input" type="checkbox" name="player-team-' + teamId + '" id="' + hostOrGuest + '-player-' + players[i].id + '" value="' + players[i].id + '">';
                content += '<label class="" for="' + hostOrGuest + '-player-' + players[i].id + '">' + players[i].name + '</label>';
                content += '</div>';
            }
        }
        // Set new players
        $("." + hostOrGuest + "-players").html(content);
    }
    
    // Team picker -> get team players
    // Host
    $("#host-team").on("change", function () {
        let teamId = $(this).val();
        updateAvailableTeams("guest", teamId);
        $.ajax({
            type: "GET",
            url: "/Match/AvailablePlayers",
            data: {"teamID": teamId},
            dataType: "json",
            success: function (response) {
                let teamId = $("#host-team").val();
                setPlayers(response, teamId, "host");
            }
        });
    });
    // Guest
    $("#guest-team").on("change", function () {
        let teamId = $(this).val();
        updateAvailableTeams("host", teamId);
        $.ajax({
            type: "GET",
            url: "/Match/AvailablePlayers",
            data: { "teamID": teamId },
            dataType: "json",
            success: function (response) {
                let teamId = $("#guest-team").val();
                setPlayers(response, teamId, "guest");
            }
        });
    });

    // Custom validation for "New match" form + format data for submission
    function formatFormData(formData) {
        let minPlayersRequred = 6;
        let playerIdPrefix = "player-team-";
        let allPlayers = [];
        // Create object to store all "new match" form data
        let formatedData = {
            isValid: {
                hostId: true,
                guestId: true,
                matchPlace: true,
                matchDate: true,
                hostPlayers: true,
                guestPlayers: true,
                equalNumOfPlayers: true
            },
            content: {
                hostPlayerIDs: [],
                guestPlayerIDs: []
            }
        };

        if (formData !== undefined && Array.isArray(formData)) {
            formData.forEach(formField => {
                switch (formField.name) {
                    case "host-team":
                        formatedData.content.hostId = formField.value;
                        if (formField.value === "") {
                            formatedData.isValid.hostId = false;
                        }
                        break;
                    case "guest-team":
                        //guestId = formField.value;
                        formatedData.content.guestId = formField.value;
                        if (formField.value === "") {
                            formatedData.isValid.guestId = false
                        }
                        break;
                    case "match-place":
                        formatedData.content.matchPlace = formField.value;
                        if (formField.value === "") {
                            formatedData.isValid.matchPlace = false;
                        }
                        break;
                    case "match-date":
                        formatedData.content.matchDate = formField.value;
                        // Don't check date validity since invalid values are disabled in datepicker
                        formatedData.isValid.matchDate = true;
                        break;
                    default:
                        if (formField.name.includes(playerIdPrefix)) {
                            allPlayers.push(formField);
                        }
                }
            });
        }
        // Separate host and guest players
        allPlayers.forEach(player => {
            if (player.name.replace(playerIdPrefix, "") === formatedData.content.hostId) {
                formatedData.content.hostPlayerIDs.push(player.value);
            }
            else if (player.name.replace(playerIdPrefix, "") === formatedData.content.guestId) {
                formatedData.content.guestPlayerIDs.push(player.value)
            }
        });
        // Check if more than min players is selected
        if (formatedData.content.hostPlayerIDs.length < minPlayersRequred) {
            formatedData.isValid.hostPlayerIDs = false;
        }
        if (formatedData.content.guestPlayerIDs.length < minPlayersRequred) {
            formatedData.isValid.guestPlayerIDs = false;
        }
        // Check of equal number of players are selected
        if (formatedData.content.hostPlayerIDs.length !== formatedData.content.guestPlayerIDs.length) {
            formatedData.isValid.equalNumOfPlayers = false;
        }
        return formatedData;
    }

    // Global variable for "new match", property -> css selector
    var propToIdSelector = {
        "guestId": "#guest-invalid",
        "hostId": "#host-invalid",
        "matchPlace": "#place-invalid",
        "hostPlayerIDs": "#host-team-invalid",
        "guestPlayerIDs": "#guest-team-invalid",
        "equalNumOfPlayers": "#equal-players-invalid"
    };
    // Remove all invalid markers
    function resetAllValidation() {
        for (let validityProperty in propToIdSelector) {
            $(propToIdSelector[validityProperty]).addClass("invisible");
        }
    }
    // Validate new match form
    function isNewMatchFormValid(form) {
        let isValid = true;
        for (let validityProperty in form.isValid) {
            if (!form.isValid[validityProperty]) {
                if (isValid) {
                    isValid = false;
                }
                // Show warning
                $(propToIdSelector[validityProperty]).removeClass("invisible");
            }
        }
        return isValid;
    }
    // Submit new match form if valid
    $("#add-match-confirm").on("click", function () {
        let formData = $("#add-match-form").serializeArray();
        let form = formatFormData(formData);
        if (isNewMatchFormValid(form)) {
            $.ajax({
                type: "POST",
                url: "/Match/Create",
                data: form.content,
                dataType: "json",
                success: function (isMatchCreated) {
                    if (isMatchCreated === true) {
                        window.location.href = '/Match';
                    }
                }
            });
        }
    });

});

// ---------------- Details ---------------- //
// Confirm host goal form
$("#host-goal-confirm").on("click", function () {
    $("#host-goal-form").submit();
});
// Confirm guest goal form
$("#guest-goal-confirm").on("click", function () {
    $("#guest-goal-form").submit();
});