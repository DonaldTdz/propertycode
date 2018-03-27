using System.Web;
using System;
using System.Collections.Generic;
using System.Text;
using YK.BackgroundMgr.PresentationService;

namespace YK.BackgroundMgr.Common
{
    public class SessionService : ISessionService
    {
        private const string SessionMustHasName = "AdminUserInfo"; // ��¼ϵͳ�����������û���Ϣ��Session

        /// <summary>
        /// �ж�Session�Ƿ����
        /// </summary>
        public bool IsSessionExpire
        {
            get
            {
                return HttpContext.Current.Session[SessionMustHasName] == null;
            }
        }

        /// <summary>
        /// �ж��Ƿ�洢ָ����Session
        /// </summary>
        /// <param name="name">Session����</param>
        /// <returns>�Ƿ�洢ָ����Session</returns>
        public bool HasSession(string name)
        {
            return HttpContext.Current.Session[name] != null;
        }

        /// <summary>
        /// ����Session�Ĺ���ʱ��
        /// </summary>
        /// <param name="iExpireTime">����ʱ�䣬���Ӽ���</param>
        public void SetSessionExpireTime(int iExpireTime)
        {
            HttpContext.Current.Session.Timeout = iExpireTime;
        }

        /// <summary>
        /// ����session����ȡsession����
        /// </summary>
        /// <typeparam name="T">��ȡ��Session��������</typeparam>
        /// <param name="name">Session����</param>
        /// <returns>session����</returns>
        public T GetSession<T>(string name)
        {
            return (T)HttpContext.Current.Session[name];
        }

        /// <summary>
        /// ����session
        /// </summary>
        /// <param name="name">session ��</param>
        /// <param name="value">session ֵ</param>
        public void SetSession(string name, object value)
        {
            HttpContext.Current.Session.Add(name, value);
        }

        /// <summary>
        /// �Ƴ�Session
        /// </summary>
        /// <param name="name">Session����</param>
        public void RemoveSession(string name)
        {
            HttpContext.Current.Session.Remove(name);
        }

        /// <summary>
        /// ���Session��Ϣ
        /// </summary>
        public void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}
