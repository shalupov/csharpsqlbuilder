using System;
using System.IO;
using NUnit.Framework;
using SqlBuilder.SchemaGenerator;

namespace SqlBuilder.Tests {
  [TestFixture]
  public class GeneratorTests {
    #region Setup/Teardown

    [SetUp]
    public void SetUp() {
      myWriter = new StringWriter();
      myGenerator = new Generator(myWriter, new NameConverter());
    }

    #endregion

    private Generator myGenerator;
    private StringWriter myWriter;

    public string WriterString {
        get {
            return myWriter.ToString().Replace("\r", "");
        }
    }
    
    [Test]
    public void Footer() {
      myGenerator.WriteFooter();
      Assert.AreEqual("}\n", WriterString);
    }

    [Test]
    public void Header() {
      myGenerator.WriteHeader("some.namespace");
      Assert.AreEqual("using SqlBuilder;\n\nnamespace some.namespace {\n", WriterString);
    }

    [Test]
    public void Table() {
      myGenerator.WriteTableSchema("users_p", new[] {"id", "group_id"});
      Assert.AreEqual("  public class UsersPTable {\n" +
                      "    public SqlColumn Id, GroupId;\n" +
                      "    public ISqlTable Table;\n" +
                      "\n" +
                      "    public UsersPTable(string tableName) {\n" +
                      "      Table = new RealSqlTable(\"users_p\", tableName);\n" +
                      "      Id = new SqlColumn(\"id\", Table);\n" +
                      "      GroupId = new SqlColumn(\"group_id\", Table);\n" +
                      "    }\n" +
                      "  }\n\n" +
                      
                      "  public class UsersP {\n" +
                      "    public static readonly SqlColumn Id, GroupId;\n" +
                      "    public static readonly ISqlTable Table;\n" +
                      "\n" +
                      "    static UsersP() {\n" +
                      "      Table = new RealSqlTable(\"users_p\");\n" +
                      "      Id = new SqlColumn(\"id\", Table);\n" +
                      "      GroupId = new SqlColumn(\"group_id\", Table);\n" +
                      "    }\n\n" +
                      "    public static UsersPTable Clone() {\n" +
                      "      return new UsersPTable(Naming.NewTableName());\n" +
                      "    }\n" +
                      "  }\n\n", WriterString);
    }
  }
}
