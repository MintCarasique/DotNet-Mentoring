using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    class ExportAttribute : Attribute
    {
        public ExportAttribute()
        {
            
        }

        public ExportAttribute(Type type)
        {
            this.BaseType = type;
        }

        public Type BaseType { get; private set; }
    }
}
