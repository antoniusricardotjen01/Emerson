namespace EmersonAPI.ModelParam
{
    public class GetVariableParam
    {
        public DateTime? startFilterDate { get; set; }
        
        public DateTime? endFilterDate { get; set; }
        
        public string? name { get; set; }
        
        public int? cityid { get; set; }
    }
}
