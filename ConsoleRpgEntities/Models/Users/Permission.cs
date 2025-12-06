using ConsoleRpgEntities.Models.Users;

namespace ConsoleRpgEntities.Models.Users {
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property for the many-to-many relationship with Role
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
};


