namespace UiTools.Uno.My.Models
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
