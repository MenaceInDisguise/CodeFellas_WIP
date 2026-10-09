using System;

namespace Prosjekt.Utils // Pass på at dette stemmer overens med navnet på prosjektet og mappen din
{
    public static class NorwegianTime
    {
        public static DateTime Now
        {
            get
            {
                // Finner riktig tidssone enten du kjører på Windows eller Mac/Linux/Docker
                var tzId = Environment.OSVersion.Platform == PlatformID.Win32NT
                    ? "W. Europe Standard Time"
                    : "Europe/Oslo";

                var norwayTimeZone = TimeZoneInfo.FindSystemTimeZoneById(tzId);

                // Henter UTC-tid og konverterer nøyaktig til norsk tid
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, norwayTimeZone);
            }
        }
    }
}