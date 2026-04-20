using Hooome.Application.Interfaces;
using Hooome.WebApi.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Hooome.WebApi.Services;

public class ChatModerationService : IChatModerationService
{
    private static readonly HashSet<string> _badWords;
    private static readonly Regex _badWordsRegex;

    static ChatModerationService()
    {
        var jsonString = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "profanity.json"));
        var data = JsonSerializer.Deserialize<BadWordsData>(jsonString);
        _badWords = new HashSet<string>(data?.Words ?? [], StringComparer.OrdinalIgnoreCase);

        var pattern = $@"\b({string.Join("|", _badWords.Select(Regex.Escape))})\b";
        _badWordsRegex = new Regex(pattern,
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }

    public string Filter(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return message;

        return _badWordsRegex.Replace(message, match =>
            new string('*', match.Value.Length));
    }
}
