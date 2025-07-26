namespace MarkovMadness;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
public class Poem
{
    int id { get; set; }
    public string title { get; set; }
    public string author { get; set; }
    public int grade { get; set; }
    public string text { get; set; }
    public int chars { get; set; }
}
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
