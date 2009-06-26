namespace SqlBuilder {
  public static class Naming {
    private static int myNextTableNum;

    static Naming() {
      Reset();
    }

    public static string NewTableName() {
      return "t_" + (myNextTableNum++);
    }

    public static void Reset() {
      myNextTableNum = 1;
    }
  }
}