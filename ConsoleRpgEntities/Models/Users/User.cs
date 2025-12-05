using ConsoleRpgEntities.Models.Users;

namespace ConsoleRpgEntities.Models.Users {
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        // Navigation property for the many-to-many relationship with Role
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
};


