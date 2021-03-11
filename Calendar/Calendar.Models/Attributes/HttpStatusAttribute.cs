using System;

namespace Calendar.Models.Attributes
{
    public class HttpStatusAttribute : Attribute
    {
        public int Value { get; private set; }
        public HttpStatusAttribute(int statusCode)
        {
            Value = statusCode;
        }
    }
}