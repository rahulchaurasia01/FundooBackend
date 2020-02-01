/*
 *  Purpose: Its a Model for creating Database table "UserDetails".
 * 
 *  Author: Rahul Chaurasia
 *  Date: 16-01-2020
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FundooCommonLayer.ModelDB
{

    /// <summary>
    /// Model For Creating Database table with table Name "UserDetails".
    /// </summary>
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
        [DefaultValue(true)]
        public bool IsActive { set; get; }

        [DefaultValue("Regular User")]
        public string UserRole { set; get; }

        [Required]
        public DateTime CreatedAt { set; get; }

        [Required]
        public DateTime ModifiedAt { set; get; }

    }
}
