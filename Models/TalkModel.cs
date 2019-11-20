using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data;

namespace CoreCodeCamp.Models
{
    public class TalkModel
    {
        public Camp Camp { get; set; }
        [Required, MaxLength(20)]
        public string Title { get; set; }
        public string Abstract { get; set; }
        [Range(100, 900)]
        public int Level { get; set; }
        public Speaker Speaker { get; set; }
    }
}
