using DAL.Interface;
using DAL.Interface.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Entities
{
    public class DalCommentary : IEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int User { get; set; }
        public int Lot { get; set; }
        public string Text { get; set; }
    }
}
