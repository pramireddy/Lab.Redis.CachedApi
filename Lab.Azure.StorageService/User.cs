using System.ComponentModel.DataAnnotations;

namespace Lab.Azure.StorageService
{
    public class User
    {
        public int PersonNumber { get; set; }

        [StringLength(15)]
        public string? FirstName { get; set; }

        [StringLength(15)]
        public string? LastName { get; set; }

        [StringLength(10)]
        public string? Email { get; set; }

    }
}
