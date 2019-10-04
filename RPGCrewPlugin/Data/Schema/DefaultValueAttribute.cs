using System;

namespace RPGCrewPlugin.Data.Schema
{
    public class DefaultValueAttribute : Attribute
    {
        public object DefaultValue { get;  }
        public DefaultValueAttribute(object value)
        {
            DefaultValue = value;
        }   
    }
}