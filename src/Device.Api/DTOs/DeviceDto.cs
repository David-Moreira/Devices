namespace Device.Api.DTOs
{
    public record DeviceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}