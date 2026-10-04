using SallyFaceAccess;
using Xunit;

public class ItemHintLogicTests
{
    private readonly ItemHintLogic _h = new();

    [Fact]
    public void NoHintWhenNoRelevantItemIsCarried() =>
        Assert.Empty(_h.Relevant("F4r2/sleepAid", new[] { 3 }, new[] { 2, 7 }));

    [Fact]
    public void HintWhenARelevantItemIsCarried() =>
        Assert.Equal(new[] { 3 }, _h.Relevant("F4r2/sleepAid", new[] { 3, 9 }, new[] { 2, 3 }));

    [Fact]
    public void HintClearsOnceTriedWithThoseItems()
    {
        _h.Tried("F4r2/sleepAid", new[] { 2, 3 });
        Assert.Empty(_h.Relevant("F4r2/sleepAid", new[] { 3 }, new[] { 2, 3 }));
    }

    [Fact]
    public void HintReturnsWhenANewRelevantItemArrives()
    {
        _h.Tried("F4r2/sleepAid", new[] { 3 });
        Assert.Equal(new[] { 9 }, _h.Relevant("F4r2/sleepAid", new[] { 3, 9 }, new[] { 3, 9 }));
    }

    [Fact]
    public void TryingOneObjectDoesNotClearAnother()
    {
        _h.Tried("F4r2/sleepAid", new[] { 3 });
        Assert.Equal(new[] { 3 }, _h.Relevant("F2r4/charley", new[] { 3 }, new[] { 3 }));
    }

    [Theory]
    [InlineData("off", "tea", null)]
    [InlineData("subtle", "tea", "something new to try here")]
    [InlineData("full", "tea", "try the tea here")]
    [InlineData("full", "tea and laxative", "try the tea and laxative here")]
    public void Wording(string mode, string items, string expected) =>
        Assert.Equal(expected, ItemHintLogic.Wording(mode, items));
}
