namespace CarMake.Core.integration
{
    public class CarMakeIntegrationResponse
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public string SearchCriteria { get; set; }
        public List<CarMakeModel> Results { get; set; }
    }
}
