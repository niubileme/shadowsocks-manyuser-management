//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SSMM.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string QQ { get; set; }
        public string Address { get; set; }
        public sbyte Status { get; set; }
        public decimal Balance { get; set; }
        public System.DateTime CreateTime { get; set; }
        public sbyte IsManager { get; set; }
        public string AffCode { get; set; }
    }
}
