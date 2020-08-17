using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;

namespace RPN
{
    public static class JsonHelper
    {
        public static dynamic Parse(string json)
        {
            var jsonElement = JsonDocument.Parse(json).RootElement;

            var output = ParseJsonElement(jsonElement);

            return output;
        }
        static object ParseJsonElement(JsonElement jsonElement)
        {
            var output = new ExpandoObject();
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var property in jsonElement.EnumerateObject())
                    {
                        ParsePropertyValue(property, output);
                    }
                    return output;
                case JsonValueKind.Array:
                    var array = new ArrayList();
                    foreach (var item in jsonElement.EnumerateArray())
                    {
                        array.Add(GetPropertyValue(item));
                    }
                    return array.ToArray();
            }
            throw new ArgumentException();
        }

        static void ParsePropertyValue(JsonProperty property, ExpandoObject output)
        {
            var value = property.Value;
            var name = property.Name;
            output.TryAdd(name, GetPropertyValue(value));
        }
        static object GetPropertyValue(JsonElement jsonElement)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.String:
                    return jsonElement.GetString();
                case JsonValueKind.Number:
                    int integerOutput;

                    if (jsonElement.TryGetInt32(out integerOutput))
                    {
                        return integerOutput;
                    }
                    else
                    {
                        return jsonElement.GetDecimal();
                    }

                case JsonValueKind.True:
                case JsonValueKind.False:
                    return jsonElement.GetBoolean();
                case JsonValueKind.Null:
                    return null;
                case JsonValueKind.Object:
                case JsonValueKind.Array:
                    return ParseJsonElement(jsonElement);
            }

            throw new ArgumentException();
        }
    }
}