using System;

namespace Estimatorx.Data.Mongo.Security
{
    public class Claim
    {
        public const string Organization = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/organization";
        public const string DisplayName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/DisplayName";

        public string Type { get; set; }

        public string Value { get; set; }
    }
}
