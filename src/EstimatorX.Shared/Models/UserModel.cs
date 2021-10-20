using System.Collections.Generic;

namespace EstimatorX.Shared.Models
{
    public class UserModel : ModelBase
    {
        public bool IsAuthenticated { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string EmailHash { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public List<string> Roles { get; set; } = new();

        public List<string> Organizations { get; set; } = new();
    }
}
