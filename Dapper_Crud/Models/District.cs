namespace Dapper_Crud.Models
{
    public class District
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int StateId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
