using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtrEx3.Model
{
    public class Student
    {
        [Key]
        [Column("IDStudent", TypeName = "int")]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNo { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public int IDGroup { get; set; }
        [Timestamp]
        public byte[] Stamp { get; set; }

        public virtual Group Group { get; set; }
    }
}
