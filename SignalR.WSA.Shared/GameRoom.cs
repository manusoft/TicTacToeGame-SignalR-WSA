namespace SignalR.WSA.Shared;

public class GameRoom(string id, string name)
{
    public string Id { get; set; } = id;
    public string Name { get; set; } = name;
    public List<Player> Players { get; set; } = new();
    public TicTacToeGame Game { get; set; } = new();

    public bool TryAddPlayer(Player newPlayer)
    {
        if (Players.Count < 2 && !Players.Any(x => x.Id == newPlayer.Id))
        {
            Players.Add(newPlayer);

            if (Players.Count == 1)
            {
                Game.PlayerXId = newPlayer.Id;
            }
            else if (Players.Count == 2)
            {
                Game.PlayerOId = newPlayer.Id;
            }

            return true;
        }

        return false;
    }
}
