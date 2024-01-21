namespace UiTools.Uno.My.Models
{
    public class SortingCondition(string fieldName)
    {
        public string FieldName { get; set; } = fieldName;

        public bool IsDescending { get; set; }

        public override string ToString()
        {
            return $"[{FieldName}], descending:{IsDescending}";
        }
    }

}
