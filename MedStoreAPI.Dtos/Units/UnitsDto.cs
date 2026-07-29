namespace MedStoreAPI.Dtos.Units
{
    public class UnitsRequestDto
    {
        public string UnitName { get; set; } = string.Empty;
    }

    public class UnitsResponseDto
    {
        public int UnitID { get; set; }
        public string UnitName { get; set; } = string.Empty;
    }
}
