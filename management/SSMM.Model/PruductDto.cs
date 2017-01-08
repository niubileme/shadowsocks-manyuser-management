using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Model
{
    public class PruductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Traffic { get; set; }
        public int ExpirationDay { get; set; }
        public decimal Price { get; set; }
        public sbyte IsRest { get; set; }
        public int SortNum { get; set; }
    }
}
