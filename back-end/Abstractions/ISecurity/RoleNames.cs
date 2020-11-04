﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Abstractions.ISecurity
{
    [ExcludeFromCodeCoverage]
    public static class RoleNames
    {
        public const string Admin = "admin";

        public const string Demo = "demo";

        public static List<string> GetRoles()
        {
            return new List<string>
            {
                Admin,
                Demo
            };
        }
    }
}
