using System;

namespace YK.BackgroundMgr.MVCCore
{
    /// <summary>
    /// 通用的参数检查类
    /// </summary>
    public static class ArgumentValidator
    {
        /// <summary>
        /// 检查参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="arg">参数</param>
        /// <param name="argName">参数名</param>
        /// <param name="predicate">检查逻辑</param>
        public static void Validate<T>(T arg,string argName, Func<T, bool> predicate)
        {
            if (predicate!=null)
            {
                if (predicate(arg))
                {
                    throw new ArgumentException(argName);
                }
            }
        }
    }
}
