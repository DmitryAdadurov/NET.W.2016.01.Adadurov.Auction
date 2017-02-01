using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entities
{
    public class BllComment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int User { get; set; }
        public int Lot { get; set; }
        public string Text { get; set; }
    }
}
