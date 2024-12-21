using System.Text;

public class FileUtil {

    public void SaveToCsv(List<object> data, string filePath)
    {
        var lines = new List<string>();
        try
        {
            var properties = typeof(object).GetProperties();
            var header = string.Join(",", properties.Select(p => p.Name));
            lines.Add(header);

            foreach (var item in data)
            {
                var line = string.Join(",", properties.Select(p => p.GetValue(item)));
                lines.Add(line);
            }

            File.WriteAllText(filePath, string.Join("\n", lines), Encoding.UTF8);
        }
        catch (Exception ex)
        {
            throw new Exception("Error while saving data to csv file.", ex);
        }
    }
}