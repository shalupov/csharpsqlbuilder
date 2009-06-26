using System;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using NDesk.Options;

namespace SqlBuilder.SchemaGenerator
{
  internal class SchemaGeneratorProgram
  {
    private static void Main(string[] args)
    {
      string connectionString = null;
      string schema = null;
      string nameSpace = null;

      var p = new OptionSet
                {
                  {"h|?|help", v => ShowHelpAndExit()},
                  {"c|connection", v => connectionString = v},
                  {"s|schema", v => schema = v},
                  {"n|namespace", v => nameSpace = v},
                };
      p.Parse(args);

      if (connectionString == null || schema == null || nameSpace == null)
      {
        Console.Error.WriteLine("You must specify all arguments");
        ShowHelpAndExit();
      }

      using (IDbConnection connection = new MySqlConnection(connectionString))
      {
        connection.Open();

        var generator = new Generator(Console.Out, new NameConverter());
        var catalog = new Catalogs(connection);

        generator.WriteHeader(nameSpace);
        foreach (string table in catalog.GetTables(schema).ToList())
        {
          generator.WriteTableSchema(table, catalog.GetTableColumns(schema, table));
        }
        generator.WriteFooter();
      }
    }

    private static void ShowHelpAndExit()
    {
      Console.Error.WriteLine("Usage: SqlBuilder.SchemaGenerator.exe -c <connectionString> -s <database> -n <namespace>");
      Environment.Exit(1);
    }
  }
}