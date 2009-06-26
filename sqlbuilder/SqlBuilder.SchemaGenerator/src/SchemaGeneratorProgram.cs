using System;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace SqlBuilder.SchemaGenerator {
  internal class SchemaGeneratorProgram {
    private static void Main(string[] args) {
      string connectionString = "Server=localhost; Database=prod; User ID=root; Password=; charset=utf8;";
      string schema = "prod";
      string nameSpace = "Mnemoply.Schema";

      using (IDbConnection connection = new MySqlConnection(connectionString)) {
        connection.Open();

        var generator = new Generator(Console.Out, new NameConverter());
        var catalog = new Catalogs(connection);

        generator.WriteHeader(nameSpace);
        foreach (string table in catalog.GetTables(schema).ToList()) {
          generator.WriteTableSchema(table, catalog.GetTableColumns(schema, table));
        }
        generator.WriteFooter();
      }
    }
  }
}