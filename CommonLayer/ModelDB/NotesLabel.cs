using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FundooCommonLayer.ModelDB
{
    [Table("NotesLabel")]
    public class NotesLabel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [ForeignKey("NotesDetails")]
        public int NotesId { set; get; }

        [ForeignKey("LabelDetails")]
        public int LabelId { set; get; }

        public DateTime CreatedAt { set; get; }

        public DateTime ModifiedAt { set; get; }

    }
}
