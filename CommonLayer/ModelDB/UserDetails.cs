using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FundooCommonLayer.ModelDB
{
    [Table("UserDetails")]
    public class UserDetails
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { set; get; }

        [Required]
        public string FirstName { set; get; }

        [Required]
        public string LastName { set; get; }

        [Required]
        [EmailAddress]
        public string EmailId { set; get; }

        [Required]
        public string Password { set; get; }

        [Required]
        public string Type { set; get; }

        [Required]
        public bool IsActive { set; get; }

        [Required]
        public DateTime CreatedAt { set; get; }

        [Required]
        public DateTime ModifiedAt { set; get; }

    }
}
