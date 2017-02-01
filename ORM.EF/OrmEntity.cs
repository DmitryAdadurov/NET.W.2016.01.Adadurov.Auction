using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.EF
{
    public interface IOrmEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
