"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("receiveMessage", function (user, message) {
    console.log("receiveMessage");
    let initialHtml = document.getElementById("messagesList").innerHTML;
    let finalHtml = `
        <tr>
            <td>
                ${message}
            </td>
            <td>
                ${user}
            </td>
        </tr>` + initialHtml;

    document.getElementById("messagesList").innerHTML = finalHtml;
});

connection.start().then(function () {
    console.log("connected");
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    console.log("sendButton");
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("BroadcastMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
});