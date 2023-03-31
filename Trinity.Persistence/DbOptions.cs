using System.Diagnostics.CodeAnalysis;

namespace Trinity.Persistence
{
    [ExcludeFromCodeCoverage]
    public class DbOptions
    {
        public string? Connection { get; set; }
        public string? Name { get; set; }
    }
}
