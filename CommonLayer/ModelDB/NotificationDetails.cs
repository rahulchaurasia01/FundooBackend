using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FundooCommonLayer.ModelDB
{
    [Table("NotificationDetails")]
    public class NotificationDetails
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [ForeignKey("UserDetails")]
        public int UserId { set; get; }
        
        public string Token { set; get; }

        [DefaultValue(false)]
        public bool IsDeleted { set; get; }

        public DateTime CreatedAt { set; get; }

        public DateTime ModifiedAt { set; get; }



    }
}
