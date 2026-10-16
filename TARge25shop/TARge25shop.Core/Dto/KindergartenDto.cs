namespace TARge25shop.Core.Dto
{
    public class KindergartenDto
    {
        public Guid? Id { get; set; }

        public string GroupName { get; set; } = string.Empty;
        public string KindergartenName { get; set; } = string.Empty;

        public int ChildrenCount { get; set; }
        public string TeacherName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}