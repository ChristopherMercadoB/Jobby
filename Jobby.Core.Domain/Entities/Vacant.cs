using Jobby.Core.Domain.Common;


namespace Jobby.Core.Domain.Entities
{
    public class Vacant : BaseEntity
    {
        public string EnterpriseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
