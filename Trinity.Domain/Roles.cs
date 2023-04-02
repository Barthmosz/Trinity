using System.Collections.Generic;
using Trinity.Domain.Base;

namespace Trinity.Domain
{
    public class Roles : Document
    {
        public string Name { get; set; } = string.Empty;
        public IList<Users> Users { get; set; } = new List<Users>();
    }
}
