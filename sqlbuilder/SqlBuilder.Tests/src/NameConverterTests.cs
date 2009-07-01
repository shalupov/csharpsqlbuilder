using NUnit.Framework;
using SqlBuilder.SchemaGenerator;

namespace SqlBuilder.Tests {
  [TestFixture]
  public class NameConverterTests {
    [Test]
    public void Smoke() {
      var c = new NameConverter();

      Assert.AreEqual("Id", c.ConvertName("id"));
      Assert.AreEqual("GroupId", c.ConvertName("group_id"));
      Assert.AreEqual("User_123", c.ConvertName("user_123"));
    }
  }
}