/*
 *  Purpose: Its a Model for creating Database table "Label Details".
 * 
 *  Author: Rahul Chaurasia
 *  Date: 22-01-2020
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FundooCommonLayer.ModelDB
{
    [Table("LabelDetails")]
    public class LabelDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId { set; get; }

        [ForeignKey("UserDetails")]
        public int UserId { set; get; }

        [Required]
        public string Name { set; get; }

        public DateTime CreatedAt { set; get; }

        public DateTime ModifiedAt { set; get; }

    }
}
