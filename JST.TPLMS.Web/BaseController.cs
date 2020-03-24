using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JST.TPLMS.Util;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace JST.TPLMS.Web
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region 登录用户验证
            var result = GetSession(UserInfoKey.UserName.ToString());
            base.OnActionExecuting(filterContext);
            //1.判断Session对象是否存在
            if (filterContext.HttpContext.Session==null)
            {
                filterContext.HttpContext.Response.WriteAsync("<script type ='text/javascript'>alert('alert('~登录已过期，请重新登录');window.top.location='/';</script>')");
                filterContext.Result = new RedirectResult("Home/Login");
                return;
            }
            //2，登录验证
            if (string.IsNullOrEmpty(result))
            {
                var name = filterContext.ActionDescriptor.DisplayName;
                bool islogin = name.Contains(".Login") || name.Contains(".SubmitLogin");
                if (!islogin)
                {
                    filterContext.HttpContext.Response.WriteAsync("<script type='text/javascript'>alter('登录已过期，请重新登录');window.top.location='/';<script>");
                    filterContext.Result = new RedirectResult("/Home/Login");
                    return;
                }
            }
            #endregion
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public AjaxResult Success()
        {
            AjaxResult res = new AjaxResult
            {
                Sussess = true,
                Msg = "请求成功!",
                Data = null
            };
            return res;
        }
        public AjaxResult Success(string id, string no)
        {
            AjaxResult res = new AjaxResult()
            {
                Sussess = true,
                Msg = "请求成功",
                Data = null,
                Id = id,
                No = no
            };
            return res;
        }
        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public AjaxResult Error(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                Sussess = false,
                Msg = msg,
                Data=null
            };
            return res;
        }
        public string GetLoginKey(string userName)
        {
            return string.Format($"{userName}Login");
        }
        /// <summary>
        /// 设置本地
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="minutes">过期时长，单位：分钟</param>
        protected void SetCookies(string key, string value, int minutes = 30)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
        }
        /// <summary>
        /// 删除指定的Cookie
        /// </summary>
        /// <param name="key">键</param>
        protected void DeleteCookies(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }
        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetCookies(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }
        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void SetSession(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
        }
        /// <summary>   
        /// 获取Session
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        protected string GetSession(string key)
        {
            string value = HttpContext.Session.GetString(key);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }
        public enum UserInfoKey
        {
            UserName=1,
            UserInfo,
            UserRole
        }
    }
}
