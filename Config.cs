using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Digital_wellbeing
{
    public static class Config
    {
        public enum Property { MaxTimeMins, IdleThresholdMins, ResetHour, Password, LastOpenUnixSecs, PassedTodaySecs }
        private const string PropertyValueSeparator = ": ";

        private static readonly string Location =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "digital-wellbeing-config.txt");
        private static readonly Dictionary<Property, string> PropertyName = new()
        {
            { Property.LastOpenUnixSecs, "last open unix (seconds)" },
            { Property.PassedTodaySecs, "passed time today (seconds)" },
            { Property.MaxTimeMins, "max time (minutes)" },
            { Property.IdleThresholdMins, "idle threshold (minutes)" },
            { Property.ResetHour, "reset hour" },
            { Property.Password, "password" },
        };

        public static string? GetValueOrNull(Property property)
        {
            Debug.WriteLine("Getting config value.");
            if (!File.Exists(Location))
                File.Create(Location).Close();
            
            string propName = PropertyName[property];
            
            return File.ReadAllLines(Location)
                .Where(line => line.Contains(propName))
                .Select(line => line.Replace(propName + PropertyValueSeparator, ""))
                .FirstOrDefault();
        }
        
        public static int? GetIntOrNull(Property property) => int.TryParse(GetValueOrNull(property), out int value) ? value : null;
        public static long? GetLongOrNull(Property property) => long.TryParse(GetValueOrNull(property), out long value) ? value : null;
        public static float? GetFloatOrNull(Property property) => float.TryParse(GetValueOrNull(property), out float value) ? value : null;
        
        public static void SetValue(Property property, object value)
        {
            Debug.WriteLine("Setting config value.");
            if (!File.Exists(Location))
                File.Create(Location).Close();

            string propName = PropertyName[property];
            string configLine = propName + PropertyValueSeparator + value;
            string[] lines = File.ReadAllLines(Location);
            
            bool set = false;
            for (int i = 0; i < lines.Length; ++i)
            {
                if (!lines[i].Contains(propName))
                    continue;
                
                lines[i] = configLine;
                set = true;
                break;
            }

            if (set)
                File.WriteAllLines(Location, lines);
            else
                File.AppendAllText(Location, configLine + "\n");
        }
    }
}