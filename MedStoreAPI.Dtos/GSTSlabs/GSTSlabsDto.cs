namespace MedStoreAPI.Dtos.GSTSlabs
{
    public class GSTSlabsRequestDto
    {
        public decimal Percentage { get; set; }
    }

    public class GSTSlabsResponseDto
    {
        public int GSTSlabID { get; set; }
        public decimal Percentage { get; set; }
    }
}
