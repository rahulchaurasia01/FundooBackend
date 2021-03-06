﻿/*
 *  Purpose: Its a Model for creating Database table "NotesDetails".
 * 
 *  Author: Rahul Chaurasia
 *  Date: 20-01-2020
 * 
 */


using FundooCommonLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FundooCommonLayer.ModelDB
{
    [Table("NotesDetails")]
    public class NotesDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotesId { set; get; }

        [ForeignKey("UserDetails")]
        public int UserId { set; get; }

        public string Title { set; get; }

        public string Description { set; get; }

        [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Enter a Valid Color")]
        public string Color { set; get; }

        public string Image { set; get; }

        [DefaultValue(false)]
        public bool IsPin { set; get; }

        [DefaultValue(false)]
        public bool IsArchived { set; get; }

        [DefaultValue(false)]
        public bool IsDeleted { set; get; }

        public DateTime? Reminder { set; get; }

        public DateTime CreatedAt { set; get; }

        public DateTime ModifiedAt { set; get; }


        public List<NotesLabel> Labels { set; get; }

    }
}
