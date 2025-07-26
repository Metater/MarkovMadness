using System.Globalization;
using System.Text;
using System.Web;
using CsvHelper;
using MarkovMadness;

// https://github.com/whipson/PoKi-Poems-by-Kids

Console.WriteLine("Hello, World!");

using var reader = new StreamReader(@"C:\Users\Connor\Documents\Projects\MarkovMadness\poki.csv");
using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
var records = csv.GetRecords<Poem>();

var poems = new List<Poem>();

foreach (var poem in records)
{
    poem.title = HttpUtility.HtmlDecode(poem.title);
    poem.author = HttpUtility.HtmlDecode(poem.author);
    poem.text = HttpUtility.HtmlDecode(poem.text);
    poems.Add(poem);
}

Dictionary<string, List<string>> chain = [];
foreach (var poem in poems)
{
    string[] words = poem.text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    for (int i = 0; i < words.Length - 1; i++)
    {
        string word = words[i].ToLowerInvariant();
        string nextWord = words[i + 1].ToLowerInvariant();

        if (!chain.TryGetValue(word, out var list))
        {
            list = [];
            chain[word] = list;
        }

        list.Add(nextWord);
    }

    if (words.Length > 0)
    {
        string lastWord = words[^1].ToLowerInvariant();
        if (!chain.TryGetValue(lastWord, out var list))
        {
            list = [];
            chain[lastWord] = list;
        }

        list.Add(""); // Add an empty string to allow for sentence termination
    }
}

var random = new Random();
string GeneratePoem(int maxLines = 4, int maxWordsPerLine = 10)
{
    if (chain.Count == 0)
        return ""; // No words in the chain

    var result = new StringBuilder();
    int lineCount = 0;

    while (lineCount < maxLines)
    {
        // Start with a random word
        var startingWords = chain.Keys.ToList();
        if (startingWords.Count == 0)
            break;

        string currentWord = startingWords[random.Next(startingWords.Count)];

        // Capitalize the first word of each line
        result.Append(char.ToUpper(currentWord[0]) + (currentWord.Length > 1 ? currentWord.Substring(1) : ""));

        int wordsInLine = 1;

        // Generate words for this line
        while (wordsInLine < maxWordsPerLine)
        {
            // Get possible next words
            if (!chain.TryGetValue(currentWord.ToLowerInvariant(), out var nextWords) || nextWords.Count == 0)
                break; // No more words can follow

            // Choose a random next word
            string nextWord = nextWords[random.Next(nextWords.Count)];

            // If next word is empty, end of line
            if (string.IsNullOrEmpty(nextWord))
                break;

            result.Append(" " + nextWord);
            wordsInLine++;
            currentWord = nextWord;
        }

        lineCount++;
        if (lineCount < maxLines)
            result.AppendLine();
    }

    return result.ToString();
}

Console.WriteLine("Generated Poem:");
Console.WriteLine(GeneratePoem());

int entries = 0;
foreach (var entry in chain)
{
    entries += entry.Value.Count;
}

Console.WriteLine($"Total entries in chain: {entries}");
