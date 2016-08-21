using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xb
{
    /// <summary>
    /// Method Extensions
    /// 拡張メソッド群
    /// </summary>
    public static class Ext
    {
        /// <summary>
        /// Task.Timeout
        /// </summary>
        /// <param name="task"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <remarks>
        /// thanks:
        /// http://neue.cc/2012/10/16_383.html
        /// </remarks>
        public static async Task Timeout(this Task task, TimeSpan timeout)
        {
            var delay = Task.Delay(timeout);
            if (await Task.WhenAny(task, delay) == delay)
            {
                throw new TimeoutException();
            }
        }


        /// <summary>
        /// Task<T>.Timeout
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <remarks>
        /// thanks:
        /// http://neue.cc/2012/10/16_383.html
        /// </remarks>
        public static async Task<T> Timeout<T>(this Task<T> task, TimeSpan timeout)
        {
            await ((Task)task).Timeout(timeout);
            return await task;
        }
    }
}
