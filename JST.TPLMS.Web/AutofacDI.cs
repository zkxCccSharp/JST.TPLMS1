using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JST.TPLMS.Web
{
    public class AutofacDI:Autofac.Module
    {
        //重写Autofac管道的Load方法，在这里注册注入
        protected override void Load(ContainerBuilder builder)
        {
            //注册服务的对象，这里以命名空间名称中含有JST.TPLMS.Service和Repository字符串为标志，否则注册失败
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(AuthoriseService))).Where(u => u.Namespace == "JST.TPLMS.Service");
            builder.RegisterAssemblyTypes(GetAssemblyByName("JST.TPLMS.Repository")).Where(a => a.Namespace.EndsWith("Repository")).AsImplementedInterfaces();
        }
        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="AssemblyName">程序集名称</param>
        /// <returns></returns>
        public static Assembly GetAssemblyByName(String AssemblyName)
        {
            return Assembly.Load(AssemblyName);
        }
    }
}
