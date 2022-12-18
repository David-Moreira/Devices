namespace Device.Core.Models
{
    public record Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}