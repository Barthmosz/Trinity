using System;

namespace Trinity.Domain.Base
{
    public interface IDocument
    {
        public string Id { get; }
        public DateTime RegistrationDate { get; }
        public DateTime LastModifiedDate { get; }
    }
}
