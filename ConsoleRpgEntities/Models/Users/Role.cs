using ConsoleRpgEntities.Models.Users;

namespace ConsoleRpgEntities.Models.Users {
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property for the many-to-many relationship with User
        public virtual ICollection<User> Users { get; set; } = new List<User>();

        // Navigation property for the many-to-many relationship with Permission
        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
};


