using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtrEx3.Model
{
    public class Group
    {
        [Key]
        public int IDGroup { get; set; } // uwaga - kiedys bylo GroupId
        public string Name { get; set; }
        public byte[] Stamp { get; set; }
        public virtual List<Student> Students { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter cannot be cast to ThreeDPoint return false:
            Group p = obj as Group;
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return p.IDGroup == IDGroup &&
                p.Name == Name;
        }

        public override int GetHashCode()
        {
            return IDGroup.GetHashCode() + Name.GetHashCode();
        }
    }
}
