using Microsoft.AspNetCore.SignalR;
using SignalR.WSA.Shared;

namespace SignalR.WSA.Hubs;

public class GameHub : Hub
{
    private static readonly List<GameRoom> rooms = new();

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"Player with Id '{Context.ConnectionId}' connected.");

        await Clients.Caller.SendAsync("Rooms", rooms.OrderBy(x => x.Name));
    }

    public async Task<GameRoom> CreateRoom(string roomName, string playerName)
    {
        var roomId = Guid.NewGuid().ToString();
        var room = new GameRoom(roomId, roomName);

        rooms.Add(room);

        var newPlayer = new Player(Context.ConnectionId, playerName);
        room.TryAddPlayer(newPlayer);

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.All.SendAsync("Rooms", rooms.OrderBy(x => x.Name));

        return room;
    }

    public async Task<GameRoom?> JoinRoom(string roomId, string playerName)
    {
        var room = rooms.FirstOrDefault(x=>x.Id == roomId);

        if(room is not null)
        {
            var newPlayer = new Player(Context.ConnectionId, playerName);

            if (room.TryAddPlayer(newPlayer))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                await Clients.Group(roomId).SendAsync("PlayerJoined", newPlayer);

                return room;
            }
        }

        return null;
    }
}
