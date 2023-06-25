using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiTools.Uno.Models
{
    public class SortingCondition
    {
        public string FieldName { get; set; }

        public SortingCondition(string fieldName)
        {
            FieldName = fieldName;
        }

        public bool IsDescending { get; set; }

        public override string ToString()
        {
            return $"[{FieldName}], descending:{IsDescending}";
        }
    }

}
