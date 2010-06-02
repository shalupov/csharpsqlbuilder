using System;
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

    public void WriteTableSchema(string schema, string table, IEnumerable<string> columns) {
      WriteNonStaticTableSchema(schema, table, columns);
      myWriter.WriteLine("  public class {0} {{", myNameConverter.ConvertName(table));
      myWriter.WriteLine("    public static readonly SqlColumn {0};",
                         ", ".Join(columns.Map(x => myNameConverter.ConvertName(x))));
      myWriter.WriteLine("    public static readonly ISqlTable Table;");
      myWriter.WriteLine();

      myWriter.WriteLine("    static {0}() {{", myNameConverter.ConvertName(table));
      myWriter.WriteLine("      Table = new RealSqlTable(\"{0}\");", schema + "." + table);

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

    private void WriteNonStaticTableSchema(string schema, string table, IEnumerable<string> columns) {
      myWriter.WriteLine("  public class {0}Table {{", myNameConverter.ConvertName(table));
      myWriter.WriteLine("    public readonly SqlColumn {0};",
                   ", ".Join(columns.Map(x => myNameConverter.ConvertName(x))));
      myWriter.WriteLine("    public readonly ISqlTable Table;");
      myWriter.WriteLine();
      
      myWriter.WriteLine("    public {0}Table(string tableName) {{", myNameConverter.ConvertName(table));
      myWriter.WriteLine("      Table = new RealSqlTable(\"{0}\", tableName);", schema + "." + table);
      
      foreach (string column in columns) {
        myWriter.WriteLine("      {0} = new SqlColumn(\"{1}\", Table);",
                           myNameConverter.ConvertName(column), column);
      }
      
      myWriter.WriteLine("    }");
      
      myWriter.WriteLine("  }");
      myWriter.WriteLine();
    }

    public void WriteRoutineSchema(string schema, string routine) {
      var convertName = myNameConverter.ConvertName(routine);
      myWriter.WriteLine("  public class {0} : StoredFunction {{", convertName);
      myWriter.WriteLine("    public {0}(params Expression[] args) : base(\"{1}.{2}\", args) {{}}", convertName, schema, routine);
      myWriter.WriteLine("  }");
    }
  }
}
