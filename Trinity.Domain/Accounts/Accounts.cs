﻿using System.Collections.Generic;
using Trinity.Domain.Base;

namespace Trinity.Domain.Accounts
{
    public class Accounts : Document
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public IList<string> Roles { get; set; } = new List<string>();
    }
}