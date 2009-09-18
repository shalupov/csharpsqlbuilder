using System.Collections.Generic;
using System.Data;

namespace SqlBuilder.SchemaGenerator {
  public class Catalogs {
    private readonly IDbConnection myConnection;

    public Catalogs(IDbConnection connection) {
      myConnection = connection;
    }

    public IEnumerable<string> GetTables(string schema) {
      using (IDbCommand command = myConnection.CreateCommand()) {
        command.CommandText = Sql.Select(DbTables.TableName).Where(DbTables.TableSchema == schema).ToSQL();
        using (IDataReader reader = command.ExecuteReader()) {
          while (reader.Read()) {
            yield return reader.GetString(0);
          }
          reader.Close();
        }
      }
    }

    public IEnumerable<string> GetTableColumns(string schema, string table) {
      using (IDbCommand command = myConnection.CreateCommand()) {
        var sql = Sql.Select(DbColumns.ColumnName)
          .Where(DbColumns.TableSchema == schema).Where(DbColumns.TableName == table).ToSQL();
        command.CommandText = sql;
        using (IDataReader reader = command.ExecuteReader()) {
          while (reader.Read()) {
            yield return reader.GetString(0);
          }
          reader.Close();
        }
      }
    }

    public IEnumerable<string> GetRoutines(string schema) {
      using (IDbCommand command = myConnection.CreateCommand()) {
        command.CommandText =
          Sql.Select(RoutinesTable.SpecificName).Where(RoutinesTable.RoutineSchema == schema,
                                                       RoutinesTable.RoutineType == "FUNCTION").ToSQL();
        using (IDataReader reader = command.ExecuteReader()) {
          while (reader.Read()) yield return reader.GetString(0);
          reader.Close();
        }
      }
    }
  }
}