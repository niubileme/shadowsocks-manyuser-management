using AutoMapper;
using SSMM.Entity;
using SSMM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMM.Services.Core
{
    public class UseAutoMapper
    {
        public static void Init()
        {
            Mapper.Initialize(cfg => {

                cfg.CreateMap<User, UserDto>();





            });
        }
    }
}
