namespace Device.Api.DTOs
{
    public record DeviceCreateUpdateDto
    {
        public string Name { get; set; }
        public string Brand { get; set; }
    }
}