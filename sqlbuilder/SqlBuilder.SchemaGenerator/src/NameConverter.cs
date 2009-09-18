using System.Text.RegularExpressions;

namespace SqlBuilder.SchemaGenerator {
  public class NameConverter {
    public string ConvertName(string name) {
      // Up first letter
      name = name.Substring(0, 1).ToUpper() + name.Substring(1);

      // Up _w => W
      name = Regex.Replace(name, "_([a-z])", x => x.Groups[1].Value.ToUpper());

      return name;
    }
  }
}