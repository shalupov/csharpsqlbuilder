using System.Collections.Generic;
using System.IO;
using SqlBuilder;

namespace SqlBuilder.SchemaGenerator {
  // Quick and Dirty (c)
  public class Generator {
    private readonly NameConverter myNameConverter;
    private readonly TextWriter myWriter;

    public Generator(TextWriter writer, NameConverter nameConverter) {
      myWriter = writer;
      myNameConverter = nameConverter;
    }

    public void WriteHeader(string nameSpace) {
      myWriter.WriteLine("using SqlBuilder;");
      myWriter.WriteLine();
      myWriter.WriteLine("namespace {0} {{", nameSpace);
    }

    public void WriteFooter() {
      myWriter.WriteLine("}");
    }

    public void WriteTableSchema(string table, IEnumerable<string> columns) {
      WriteNonStaticTableSchema(table, columns);
      myWriter.WriteLine("  public class {0} {{", myNameConverter.ConvertName(table));
      myWriter.WriteLine("    public static readonly SqlColumn {0};",
                         ", ".Join(columns.Map(x => myNameConverter.ConvertName(x))));
      myWriter.WriteLine("    public static readonly ISqlTable Table;");
      myWriter.WriteLine();

      myWriter.WriteLine("    static {0}() {{", myNameConverter.ConvertName(table));
      myWriter.WriteLine("      Table = new RealSqlTable(\"{0}\");", table);

      foreach (string column in columns) {
        myWriter.WriteLine("      {0} = new SqlColumn(\"{1}\", Table);",
                           myNameConverter.ConvertName(column), column);
      }

      myWriter.WriteLine("    }");
      myWriter.WriteLine();
            
      WriteClone(table);

      myWriter.WriteLine("  }");
      myWriter.WriteLine();
    }

    private void WriteClone(string table) {
      myWriter.WriteLine("    public static {0}Table Clone() {{", myNameConverter.ConvertName(table));
      myWriter.WriteLine("      return new {0}Table(Naming.NewTableName());",
                         myNameConverter.ConvertName(table));
      myWriter.WriteLine("    }");

    }

    private void WriteNonStaticTableSchema(string table, IEnumerable<string> columns) {
      myWriter.WriteLine("  public class {0}Table {{", myNameConverter.ConvertName(table));
      myWriter.WriteLine("    public SqlColumn {0};",
                   ", ".Join(columns.Map(x => myNameConverter.ConvertName(x))));
      myWriter.WriteLine("    public ISqlTable Table;");
      myWriter.WriteLine();
      
      myWriter.WriteLine("    public {0}Table(string tableName) {{", myNameConverter.ConvertName(table));
      myWriter.WriteLine("      Table = new RealSqlTable(\"{0}\", tableName);", table);
      
      foreach (string column in columns) {
        myWriter.WriteLine("      {0} = new SqlColumn(\"{1}\", Table);",
                           myNameConverter.ConvertName(column), column);
      }
      
      myWriter.WriteLine("    }");
      
      myWriter.WriteLine("  }");
      myWriter.WriteLine();
    }
  }
}
