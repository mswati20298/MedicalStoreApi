namespace MedStoreAPI.Dtos.PaymentModes
{
    public class PaymentModesRequestDto
    {
        public string ModeName { get; set; } = string.Empty;
    }

    public class PaymentModesResponseDto
    {
        public int PaymentModeID { get; set; }
        public string ModeName { get; set; } = string.Empty;
    }
}
