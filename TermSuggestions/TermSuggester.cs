namespace TermSuggestions;

// Ranks candidates by sliding-window character mismatches.
// Shorter candidates than the term are skipped.
public class TermSuggester : IAmTheTest
{
    public IEnumerable<string> GetSuggestions(
        string term,
        IEnumerable<string> choices,
        int numberOfSuggestions)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(choices);
        if (numberOfSuggestions < 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfSuggestions));

        if (numberOfSuggestions == 0 || term.Length == 0)
            return [];

        return choices
            .Select(choice => (choice, score: ComputeMinDifferences(term, choice)))
            .Where(x => x.score.HasValue)
            .OrderBy(x => x.score!.Value)
            .ThenBy(x => Math.Abs(x.choice.Length - term.Length))
            .ThenBy(x => x.choice, StringComparer.OrdinalIgnoreCase)
            .Take(numberOfSuggestions)
            .Select(x => x.choice);
    }

    private static int? ComputeMinDifferences(string term, string candidate)
    {
        if (candidate.Length < term.Length)
            return null;

        int minDiff = int.MaxValue;
        int windowCount = candidate.Length - term.Length + 1;

        for (int start = 0; start < windowCount; start++)
        {
            int diff = 0;
            for (int j = 0; j < term.Length; j++)
            {
                if (candidate[start + j] != term[j])
                    diff++;
            }

            if (diff == 0)
                return 0;

            if (diff < minDiff)
                minDiff = diff;
        }

        return minDiff;
    }
}
