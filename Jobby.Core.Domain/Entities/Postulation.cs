namespace Jobby.Core.Domain.Entities
{
    public class Postulation
    {
        public string UserId { get; set; }
        public int VacantId { get; set; }
        public Vacant Vacant { get; set; }
    }
}
