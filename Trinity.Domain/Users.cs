using System.Collections.Generic;
using Trinity.Domain.Base;

namespace Trinity.Domain
{
    public class Users : Document
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public IList<Roles> Roles { get; set; } = new List<Roles>();
    }
}
