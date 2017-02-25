using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Model
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string QQ { get; set; }
        public string Address { get; set; }
        public sbyte Status { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreateTime { get; set; }
        public sbyte IsManager { get; set; }
        public string AffCode { get; set; }
        public int ParentId { get; set; }
    }
}
