using FluentAssertions;
using OpenEHR.RM.DataTypes.Text;

namespace OpenEHR.RM.DataTypes.Tests.Text;

public class DvCodedTextTests
{
    private static CodePhrase SnomedCode(string code) =>
        new CodePhrase(new TerminologyId("SNOMED-CT"), code);

    [Fact]
    public void Constructor_ValidArgs_SetsProperties()
    {
        var ct = new DvCodedText("Hypertension", SnomedCode("38341003"));
        ct.Value.Should().Be("Hypertension");
        ct.DefiningCode.CodeString.Should().Be("38341003");
    }

    [Fact]
    public void Constructor_NullDefiningCode_Throws()
    {
        var act = () => new DvCodedText("Hypertension", null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Equals_SameValueAndCode_ReturnsTrue()
    {
        var a = new DvCodedText("Hypertension", SnomedCode("38341003"));
        var b = new DvCodedText("Hypertension", SnomedCode("38341003"));
        a.Should().Be(b);
    }

    [Fact]
    public void Equals_DifferentCode_ReturnsFalse()
    {
        var a = new DvCodedText("Hypertension", SnomedCode("38341003"));
        var b = new DvCodedText("Hypertension", SnomedCode("11111111"));
        a.Should().NotBe(b);
    }

    [Fact]
    public void IsSubtypeOfDvText()
    {
        new DvCodedText("Test", SnomedCode("123")).Should().BeAssignableTo<DvText>();
    }
}
