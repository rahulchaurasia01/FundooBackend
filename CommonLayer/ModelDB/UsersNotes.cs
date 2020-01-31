using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FundooCommonLayer.ModelDB
{
    [Table("UsersNotes")]
    public class UsersNotes
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [ForeignKey("UserDetails")]
        public int UserId { set; get; }

        [ForeignKey("NotesDetails")]
        public int NoteId { set; get; }

        public DateTime CreatedAt { set; get; }

        public DateTime ModifiedAt { set; get; }

    }
}
