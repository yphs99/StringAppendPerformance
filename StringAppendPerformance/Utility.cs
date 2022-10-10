using System.Text;

namespace StringAppendPerformance
{
    /// <summary>
    /// 常用通用元件
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// 字串串接(使用+=)
        /// </summary>
        /// <param name="n">串接次數</param>
        /// <returns></returns>
        public string Concat(int n)
        {
            string resilt = string.Empty;
            for (int i = 0; i < n; i++)
            {
                resilt += i.ToString();
            }
            return resilt;
        }

        /// <summary>
        /// 字串串接(StringBuilder)
        /// </summary>
        /// <param name="n">串接次數</param>
        /// <returns></returns>
        public string Append(int n)
        {
            StringBuilder resilt = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                resilt.Append(i.ToString());
            }
            return resilt.ToString();
        }
    }
}