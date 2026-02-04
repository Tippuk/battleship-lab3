using System.Text.Json;

namespace BattleshipLab3.Game;

public record GameLogEntry(
    DateTime Time,
    string Player,
    int X,
    int Y,
    string Outcome);

public class GameLogger
{
    private readonly List<GameLogEntry> _entries = new();

    public void Add(string player, int x, int y, string outcome)
    {
        _entries.Add(new GameLogEntry(DateTime.Now, player, x, y, outcome));
    }

    public void SaveToFile()
    {
        if (_entries.Count == 0)
            return;

        var fileName = $"battle_log_{DateTime.Now:yyyyMMdd_HHmmss}.json";
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(_entries, options);
        File.WriteAllText(fileName, json);
    }
}

