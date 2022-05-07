using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Digital_wellbeing
{
    public static class Config
    {
        public enum Property { MaxTimeMins, Password, LastOpenUnixSecs, PassedTodayMins }
        private const string PropertyValueSeparator = ": ";

        private static readonly string Location =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "digital-wellbeing-config.txt");
        private static readonly Dictionary<Property, string> PropertyName = new()
        {
            { Property.MaxTimeMins, "max-time-minutes" },
            { Property.Password, "password" },
            { Property.LastOpenUnixSecs, "last-open-unix-seconds" },
            { Property.PassedTodayMins, "passed-minutes-today" },
        };

        public static string? GetValueOrNull(Property property)
        {
            if (!File.Exists(Location))
                File.Create(Location).Close();
            
            string propName = PropertyName[property];
            
            return File.ReadAllLines(Location)
                .Where(line => line.Contains(propName))
                .Select(line => line.Replace(propName + PropertyValueSeparator, ""))
                .FirstOrDefault();
        }

        public static void SetValue(Property property, string value)
        {
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