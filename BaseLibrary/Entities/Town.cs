
namespace BaseLibrary.Entities
{
    public class Town : BaseEntity
    {
        // Many to one relationship with City
        public City? City { get; set; }
        public int CityId { get; set; }
    }
}
