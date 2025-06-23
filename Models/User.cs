using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGatoMia.Models
{
    [Table("tb_User")]
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public DateTime DateRegistration { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public int UserTypeId { get; set; } = 3;

        // Retirado o required, agora anulável
        public UserType? UserType { get; set; }

        public User() { }

        public User(string name, string email, string passwordHash, string? phone, DateTime dateRegistration, bool isActive, int userTypeId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Phone = phone;
            DateRegistration = dateRegistration;
            IsActive = isActive;
            UserTypeId = userTypeId;
        }
    }
}
