const lobbyConnection = new signalR.HubConnectionBuilder()
    .withUrl("/lobbyHub")
    .build();

lobbyConnection.on("RefreshLobbyList", function () {
    const container = $("#games-list-container");

    container.animate({ opacity: 0.5 }, 200, function () {
        $.get("/FindGame/GetGamesList", function (data) {
            container.html(data).animate({ opacity: 1 }, 200);
        });
    });
});

lobbyConnection.start().catch(err => console.error(err.toString()));