using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace DAL
{
    public static class MessageConvert
    {
        private static readonly JsonSerializerSettings Settings;
        static MessageConvert()
        {
            Settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
        public static string SerializeObject(this object obj) =>
            obj == null ? "" : JsonConvert.SerializeObject(obj, Settings);
        public static T? DeserializeObject<T>(this string json) =>
            JsonConvert.DeserializeObject<T>(json, Settings);
        public static object? DeserializeObject(this string json, Type type)
        {
            try { return JsonConvert.DeserializeObject(json, type, Settings); }
            catch { return null; }
        }
    }

    /// <summary>Helper chuyển đổi DataTable <-> List&lt;T&gt;</summary>
    public static class CollectionHelper
    {
        public static IList<T>? ConvertTo<T>(this DataTable? table)
        {
            if (table == null) return null;
            var rows = new List<DataRow>();
            foreach (DataRow row in table.Rows) rows.Add(row);
            return ConvertTo<T>(rows);
        }
        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = new List<T>();
            foreach (DataRow row in rows)
            {
                T item = CreateItem<T>(row)!;
                list.Add(item);
            }
            return list;
        }
        public static T? CreateItem<T>(DataRow row)
        {
            if (row == null) return default;
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in row.Table.Columns)
            {
                PropertyInfo? prop = obj!.GetType().GetProperty(column.ColumnName);
                if (prop == null) continue;
                try
                {
                    object value = row[column.ColumnName];
                    if (value == DBNull.Value) continue;
                    Type type = prop.PropertyType;
                    if (column.ColumnName.Contains("json"))
                        prop.SetValue(obj, MessageConvert.DeserializeObject((value + "").Replace("$", ""), type), null);
                    else if (type.Name == "String")
                        prop.SetValue(obj, Convert.ToString(value), null);
                    else if (type.Name == "Nullable`1" || type.Name == "DateTime")
                    {
                        var t = Nullable.GetUnderlyingType(type) ?? type;
                        prop.SetValue(obj, Convert.ChangeType(value, t), null);
                    }
                    else
                        prop.SetValue(obj, value, null);
                }
                catch { throw; }
            }
            return obj;
        }
    }
}
