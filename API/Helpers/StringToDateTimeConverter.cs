using System;
using AutoMapper;

namespace API.Helpers
{
    public class StringToDateTimeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            object objDateTime = source;

            if (objDateTime == null)
            {
                return default;
            }

            return DateTime.TryParse(objDateTime.ToString(), out var dateTime) ? dateTime : default;
        }
    }
}