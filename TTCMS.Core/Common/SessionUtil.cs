using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TTCMS.Core
{
    public class SessionUtil
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Save target object to session 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="target"></param>
        public static void SetSession(string key, object target)
        {
            HttpContext.Current.Session[key] = target;
        }

        /// <summary>
        /// Remove value in session 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="target"></param>
        public static void RemoveFromSession(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        /// <summary>
        /// Get value from session
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Type GetFromSession<Type>(string key)
        {
            if (HttpContext.Current.Session[key] == null)
            {
                return default(Type);
            }

            var value = HttpContext.Current.Session[key];
            if (value == null)
            {
                return default(Type);
            }

            if (value is Type == false)
            {
                string message = string.Format("Key:Value of {0} is not the same as the specified type. The actual type [{1}]", key, value.GetType());
                logger.ErrorFormat(message);
                throw new Exception(message);
            }

            return (Type)value;
        }

        /// <summary>
        /// Save To Page Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="target"></param>
        public static void SaveToPageSession(string key, object target, bool isSaveFromPopup = false)
        {
            Dictionary<string, object> dict
                = (Dictionary<string, object>)HttpContext.Current.Session[SystemConsts.SESSION_FOR_PAGE_SESSION];

            if (dict == null)
            {
                dict = new Dictionary<string, object>();
            }

            if (!isSaveFromPopup)
            {
                if (!dict.ContainsKey(SystemConsts.SESSION_FOR_PAGE_SESSION_URL)
                    || dict[SystemConsts.SESSION_FOR_PAGE_SESSION_URL].ToString() != HttpContext.Current.Request.Url.AbsoluteUri)
                {
                    dict.Clear();
                    dict.Add(SystemConsts.SESSION_FOR_PAGE_SESSION_URL, HttpContext.Current.Request.Url.AbsoluteUri);
                }
            }

            if (!dict.ContainsKey(key))
            {
                dict.Add(key, target);
            }
            else
            {
                dict[key] = target;
            }

            HttpContext.Current.Session[SystemConsts.SESSION_FOR_PAGE_SESSION] = dict;
            HttpContext.Current.Items["IsNotRedundancePageSession"] = true;
        }

        /// <summary>
        /// Get page session by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetFromPageSession(string key)
        {
            Dictionary<string, object> dict
                = (Dictionary<string, object>)HttpContext.Current.Session[SystemConsts.SESSION_FOR_PAGE_SESSION];

            if (dict == null || !dict.ContainsKey(key))
            {
                return null;
            }

            return dict[key];
        }

        /// <summary>
        /// Clear page session of url different from current url
        /// Create new page session for current url
        /// Should call from master page
        /// </summary>
        public static void CheckAndRemoveRedundancePageSession()
        {
            if (HttpContext.Current.Items["IsNotRedundancePageSession"] != null
                    && (bool)HttpContext.Current.Items["IsNotRedundancePageSession"] == true)
            {
                return;
            }

            Dictionary<string, object> dict
                = (Dictionary<string, object>)HttpContext.Current.Session[SystemConsts.SESSION_FOR_PAGE_SESSION];

            if (dict == null)
            {
                dict = new Dictionary<string, object>();
                dict.Add(SystemConsts.SESSION_FOR_PAGE_SESSION_URL, HttpContext.Current.Request.Url.AbsoluteUri);
                HttpContext.Current.Session[SystemConsts.SESSION_FOR_PAGE_SESSION] = dict;
                return;
            }

            if (HttpContext.Current.Request.HttpMethod != "POST"
                    || !dict.ContainsKey(SystemConsts.SESSION_FOR_PAGE_SESSION_URL)
                    || dict[SystemConsts.SESSION_FOR_PAGE_SESSION_URL].ToString() != HttpContext.Current.Request.Url.AbsoluteUri)
            {
                dict.Clear();
                dict.Add(SystemConsts.SESSION_FOR_PAGE_SESSION_URL, HttpContext.Current.Request.Url.AbsoluteUri);
                HttpContext.Current.Session[SystemConsts.SESSION_FOR_PAGE_SESSION] = dict;
                return;
            }
        }

        /// <summary>
        /// Remove from page sesssion
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveFromPageSession(string key)
        {
            Dictionary<string, object> dict
                = (Dictionary<string, object>)HttpContext.Current.Session[SystemConsts.SESSION_FOR_PAGE_SESSION];

            if (dict == null || !dict.ContainsKey(key))
            {
                return;
            }

            dict.Remove(key);
            HttpContext.Current.Session[SystemConsts.SESSION_FOR_PAGE_SESSION] = dict;

        }
    }
}
