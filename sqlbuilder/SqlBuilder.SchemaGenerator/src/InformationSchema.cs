namespace SqlBuilder.SchemaGenerator {
  public class DbTables {
    public static readonly SqlColumn TableName, TableSchema;
    public static readonly ISqlTable Table;

    static DbTables() {
      Table = new RealSqlTable("INFORMATION_SCHEMA.Tables");

      TableName = new SqlColumn("TABLE_NAME", Table);
      TableSchema = new SqlColumn("TABLE_SCHEMA", Table);
    }
  }

  public class DbColumns {
    public static readonly SqlColumn TableSchema, TableName, ColumnName;
    public static readonly ISqlTable Table;

    static DbColumns() {
      Table = new RealSqlTable("INFORMATION_SCHEMA.Columns");

      TableName = new SqlColumn("TABLE_NAME", Table);
      TableSchema = new SqlColumn("TABLE_SCHEMA", Table);
      ColumnName = new SqlColumn("COLUMN_NAME", Table);
    }
  }
  
  public class RoutinesTable {
    public static readonly SqlColumn SpecificName, RoutineSchema, RoutineType;
    public static readonly ISqlTable Table;

    static RoutinesTable() {
      Table = new RealSqlTable("INFORMATION_SCHEMA.ROUTINES");

      SpecificName = new SqlColumn("SPECIFIC_NAME", Table);
      RoutineSchema = new SqlColumn("ROUTINE_SCHEMA", Table);
      RoutineType = new SqlColumn("ROUTINE_TYPE", Table);
    }
  }
}