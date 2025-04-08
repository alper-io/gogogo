using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace webapp.Utilities
{
    // Singleton implementation for JSON export utility
    public class JsonExportUtils
    {
        private static readonly Lazy<JsonExportUtils> _instance = new Lazy<JsonExportUtils>(() => new JsonExportUtils());
        
        public static JsonExportUtils Instance => _instance.Value;
        
        // Private constructor for singleton pattern
        private JsonExportUtils() { }
        
        // Export any collection to JSON with optional column selection
        public string ExportToJson<T>(IEnumerable<T> data, IEnumerable<string> selectedProperties = null)
        {
            if (data == null)
                return "[]";
                
            // If no columns are selected, export all properties
            if (selectedProperties == null || !selectedProperties.Any())
            {
                return JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
            
            // If specific columns are selected, create custom objects with only those properties
            var result = data.Select(item => 
            {
                var type = typeof(T);
                var properties = type.GetProperties();
                var selectedPropertiesDict = new Dictionary<string, object>();
                
                foreach (var propName in selectedProperties)
                {
                    var property = properties.FirstOrDefault(p => 
                        string.Equals(p.Name, propName, StringComparison.OrdinalIgnoreCase));
                    
                    if (property != null)
                    {
                        selectedPropertiesDict[property.Name] = property.GetValue(item);
                    }
                }
                
                return selectedPropertiesDict;
            });
            
            return JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
    }
}
