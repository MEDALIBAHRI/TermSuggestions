namespace TermSuggestions;

public interface IAmTheTest
{
    // Example: GetSuggestions("gros", new List<string>(){"gros", "gras", "graisse", "aggressif", "go"}, 2);
    // returns the suggestions in the right order {"gros", "gras"}
    IEnumerable<string> GetSuggestions(string term, IEnumerable<string> choices, int numberOfSuggestions);
}
