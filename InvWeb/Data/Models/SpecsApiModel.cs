namespace InvWeb.Data.Models
{
    public class SpecsApiModel
    {
        public class Api_AddItem_CustomSpec
        {
            public int Id { get; set; }
            public int CustomSpecId { get; set; }
            public string SpecName { get; set; }
            public string SpecValue { get; set; }
            public int InvItemId { get; set; }
            public string Remarks { get; set; }
        }
    }
}
