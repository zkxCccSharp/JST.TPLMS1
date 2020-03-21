using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JST.TPLMS.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using JST.TPLMS.Entitys;

namespace JST.TPLMS.Web.Models
{
    public class SeedData
    {
        public static void Initializa(IServiceProvider serviceProvider)
        {
            using (var context = new TPLMSDbContext(serviceProvider.GetRequiredService<DbContextOptions<TPLMSDbContext>>()))
            {
                //Lool for any User
                if (context.User.Any())
                {
                    return; //DB has been seeded
                }

                context.User.AddRange(
                    new User
                    {
                        UserId = "admin",
                        PassWord = "admin",
                        Name = "管理员",
                        Sex = 1,
                        Status = 1,
                        Type = 0,
                        BizCode = string.Empty,
                        CreateId = 0,
                        CreateTime = DateTime.Now,
                        Address = string.Empty,
                        Mobile = string.Empty,
                        Email = string.Empty
                    },
                    new User
                    {
                        UserId = "test",
                        PassWord = "test",
                        Name = "Test",
                        Sex = 0,
                        Status = 1,
                        Type = 0,
                        BizCode = string.Empty,
                        CreateId = 0,
                        CreateTime = DateTime.Now,
                        Address = "上海黄埔",
                        Mobile = "58805505",
                        Email = string.Empty
                    },
                    new User
                    {
                        UserId = "wang",
                        PassWord = "wang",
                        Name = "王五",
                        Sex = 1,
                        Status = 1,
                        Type = 0,
                        BizCode = string.Empty,
                        CreateId = 0,
                        CreateTime = DateTime.Now,
                        Address = "上海松江",
                        Mobile = "13358805505",
                        Email = string.Empty
                    },
                    new User
                    {
                        UserId = "shOper",
                        PassWord = "shoper",
                        Name = "张三",
                        Sex = 0,
                        Status = 1,
                        Type = 0,
                        BizCode = string.Empty,
                        CreateId = 0,
                        CreateTime = DateTime.Now,
                        Address = "上海奉贤",
                        Mobile = "13900805505",
                        Email = string.Empty
                    },
                    new User
                    {
                        UserId = "10001",
                        PassWord = "10001",
                        Name = "西门庆",
                        Sex = 1,
                        Status = 1,
                        Type = 0,
                        BizCode = string.Empty,
                        CreateId = 0,
                        CreateTime = DateTime.Now,
                        Address = "北京朝阳",
                        Mobile = "18900804444",
                        Email = string.Empty
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
