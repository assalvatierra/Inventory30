namespace CoreLib.Models.API
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


        public class Api_AddItem_SteelSpec
        {
            public int InvItemId { get; set; }
            public string SpecFor { get; set; }
            public string SizeValue { get; set; }
            public string SizeDesc { get; set; }
            public string WtValue { get; set; }
            public string WtDesc { get; set; }
            public string SpecInfo { get; set; }
        }
    }
}
