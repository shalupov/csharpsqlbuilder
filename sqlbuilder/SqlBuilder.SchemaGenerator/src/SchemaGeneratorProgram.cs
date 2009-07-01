using System;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace SqlBuilder.SchemaGenerator {
  internal class SchemaGeneratorProgram {
    private static void Main(string[] args) {
      const string connectionString = "Server=127.0.0.1; Database=prod; User ID=root; Password=; charset=utf8;";
      const string schema = "prod";
      const string nameSpace = "Mnemoply.Schema";

      using (IDbConnection connection = new MySqlConnection(connectionString)) {
        connection.Open();

        var generator = new Generator(Console.Out, new NameConverter());
        var catalog = new Catalogs(connection);

        generator.WriteHeader(nameSpace);
        foreach (string table in catalog.GetTables(schema).ToList()) {
          generator.WriteTableSchema(schema, table, catalog.GetTableColumns(schema, table));
        }
        generator.WriteFooter();
      }
    }
  }
}