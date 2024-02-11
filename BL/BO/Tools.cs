
namespace BO;
/// <summary>
/// 
/// </summary>
internal class Tools
{
    public static string ToStringProperties<T>(T obj)
    {
        string str = "";
        var properties = typeof(T).GetProperties();

        //for each property, print the name and than the value
        foreach (var property in properties)
        {
            var value = property.GetValue(obj);
            //if the value itself is a collection, like dependencies
            if (value is IEnumerable<object> items)
            {
                str += $"{property.Name}:\n";
                foreach (var item in items)
                {
                    str += $"- {item}\n";
                }
            }
            else
            {
                str += $"{property.Name}: {value}\n";
            }
        }

        return str;
    }
}