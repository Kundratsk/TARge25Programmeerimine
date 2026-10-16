namespace TARge25shop.Models.Kindergarten
{
    public class KindergartenImageViewModel
    {
        public Guid? ImageId { get; set; }
        public Guid? KindergartenId { get; set; }
        public string? ImageTitle { get; set; }
        public byte[]? Image { get; set; }
        public string? ImageData { get; set; }   // base64 data-URL, mida <img src> kasutab
    }
}