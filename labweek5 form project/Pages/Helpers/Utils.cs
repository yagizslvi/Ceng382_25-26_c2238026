using System.Text.Json;
using System.Reflection;

namespace labweek5_form_project.Helpers
{
    public sealed class Utils
    {
        private static readonly Lazy<Utils> lazy = new Lazy<Utils>(() => new Utils());
        public static Utils Instance => lazy.Value;

        private Utils() { }

        public string ToJson<T>(List<T> data, List<string> selectedColumns = null)
        {
            if (selectedColumns == null || selectedColumns.Count == 0)
            {
                return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            }

            var filteredData = data.Select(item =>
            {
                var dict = new Dictionary<string, object>();
                foreach (var prop in typeof(T).GetProperties())
                {
                    if (selectedColumns.Contains(prop.Name))
                    {
                        dict[prop.Name] = prop.GetValue(item);
                    }
                }
                return dict;
            });

            return JsonSerializer.Serialize(filteredData, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
