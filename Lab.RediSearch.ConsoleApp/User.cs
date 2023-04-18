using System.ComponentModel.DataAnnotations;

namespace Lab.RediSearch.ConsoleApp
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

    public class User1
    {
        public int personNumber { get; set; }

        [StringLength(15)]
        public string? firstName { get; set; }

        [StringLength(15)]
        public string? lastName { get; set; }

        [StringLength(10)]
        public string? email { get; set; }

    }
}