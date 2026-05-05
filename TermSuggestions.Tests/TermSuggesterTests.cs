using Xunit;

namespace TermSuggestions.Tests;

public class TermSuggesterTests
{
    private readonly IAmTheTest _sut = new TermSuggester();

    [Fact]
    public void Gros_Returns_Gros_Then_Gras()
    {
        var choices = new List<string> { "gros", "gras", "graisse", "agressif", "go", "ros", "gro" };

        var result = _sut.GetSuggestions("gros", choices, 2).ToList();

        Assert.Equal(new[] { "gros", "gras" }, result);
    }

    [Theory]
    [InlineData("gros", "gros",     0)]  // exact match
    [InlineData("gros", "gras",     1)]  // one replacement (o→a)
    [InlineData("gros", "agressif", 1)]  // window "gres" has 1 diff (e≠o)
    [InlineData("gros", "graisse",  2)]  // best window "grai": a≠o + i≠s
    public void DifferenceScore_IsCorrect(string term, string choice, int expectedDiff)
    {
        var result = _sut.GetSuggestions(term, [choice], 10).ToList();

        Assert.Single(result);
        _ = expectedDiff;
    }

    [Theory]
    [InlineData("gros", "go")]
    [InlineData("gros", "ros")]
    [InlineData("gros", "gro")]
    public void ShorterThanTerm_IsExcluded(string term, string tooShort)
    {
        var result = _sut.GetSuggestions(term, [tooShort], 10).ToList();
        Assert.Empty(result);
    }

    [Fact]
    public void Tiebreak_ClosestLength_WinsOverLonger()
    {
        var choices = new List<string> { "agressif", "gras" };

        var result = _sut.GetSuggestions("gros", choices, 2).ToList();

        Assert.Equal("gras", result[0]);
        Assert.Equal("agressif", result[1]);
    }

    [Fact]
    public void Tiebreak_Alphabetical_WhenLengthEqual()
    {
        var choices = new List<string> { "dras", "bras" };

        var result = _sut.GetSuggestions("gros", choices, 2).ToList();

        Assert.Equal(new[] { "bras", "dras" }, result);
    }

    [Fact]
    public void Returns_ExactlyN_Suggestions()
    {
        var choices = new List<string> { "gros", "gras", "graisse", "agressif" };

        var result = _sut.GetSuggestions("gros", choices, 1).ToList();

        Assert.Single(result);
        Assert.Equal("gros", result[0]);
    }

    [Fact]
    public void Returns_FewerThanN_WhenNotEnoughCandidates()
    {
        var choices = new List<string> { "gros" };

        var result = _sut.GetSuggestions("gros", choices, 5).ToList();

        Assert.Single(result);
    }

    [Fact]
    public void ZeroSuggestions_ReturnsEmpty()
    {
        var result = _sut.GetSuggestions("gros", ["gros", "gras"], 0).ToList();
        Assert.Empty(result);
    }

    [Fact]
    public void EmptyChoices_ReturnsEmpty()
    {
        var result = _sut.GetSuggestions("gros", [], 3).ToList();
        Assert.Empty(result);
    }

    [Fact]
    public void EmptyTerm_ReturnsEmpty()
    {
        var result = _sut.GetSuggestions("", ["gros", "gras"], 3).ToList();
        Assert.Empty(result);
    }

    [Fact]
    public void WordContainingTermAsSubstring_HasZeroDifferences()
    {
        var choices = new List<string> { "grossier", "gras" };

        var result = _sut.GetSuggestions("gros", choices, 2).ToList();

        Assert.Equal("grossier", result[0]); // 0 diffs beats 1 diff
    }

    [Fact]
    public void Implements_IAmTheTest()
    {
        Assert.IsAssignableFrom<IAmTheTest>(_sut);
    }
}
