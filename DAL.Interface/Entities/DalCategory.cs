﻿using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Entities
{
    public class DalCategory : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
