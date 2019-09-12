using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fastdev.Log
{
    /// <summary>
    /// 负责真正的写日志到文件，
    /// 处理日志改写入那个文件，按时间小时来划分
    /// 支持同时写入同一个文件
    /// </summary>
    internal class TextWriter
    {
        //文件夹名称
        private readonly string FileName;
        /// <summary>
        /// 默认写入到Logs文件夹下
        /// </summary>
        public TextWriter()
        {
            FileName = $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}";
        }
        /// <summary>
        /// 日志写入到指定的文件,地址由上层控制
        /// </summary>
        /// <param name="filename"></param>
        public TextWriter(string filename)
        {
            FileName = filename;
        }
        /// <summary>
        /// 获取写日志的文件夹全部地址(例如指到/20190802文件夹)
        /// </summary>
        /// <returns></returns>
        private string GetFilePath(DateTime time)
        {
            return Path.Combine(FileName, time.ToString("yyyyMMdd"));
        }
        /// <summary>
        /// 获取最终写日志的文件的fileinfo
        /// 如果文件夹不存在就创建文件夹，放回fileinfo为null
        /// </summary>
        /// <returns></returns>
        private static FileInfo GetLaseWriteFile(string filepath, DateTime time)
        {
            FileInfo res = null;
            DirectoryInfo directoryInfo = new DirectoryInfo(filepath);
            if (directoryInfo.Exists)
            {
                FileInfo[] infos = directoryInfo.GetFiles();
                foreach (var info in infos)
                {
                    if (info.CreationTime.Hour == time.Hour)
                    {
                        res = info;
                        break;
                    }
                }
            }
            else
            {
                directoryInfo.Create();
            }
            return res;
        }
        /// <summary>
        /// 获取写入日志的filestream，如果实例对象fileinfo为null就创建文件
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="filepath">文件夹所在路径不包括最高的分隔符</param>
        /// <param name="time"></param>
        /// <returns></returns>
        private static FileStream GetWriteFileStream(FileInfo fileInfo, string filepath, DateTime time)
        {
            FileStream fileStream = null;
            try
            {
                if (fileInfo != null)
                {
                    fileStream = fileInfo.OpenWrite();
                }
                else
                {
                    string filename = Path.Combine($"{filepath}{Path.DirectorySeparatorChar}{time.Hour.ToString()}.log");
                    fileStream = File.Create(filename);
                }
            }
            catch
            {

            }
            return fileStream;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <returns></returns>
        internal bool WriteLog(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }

            DateTime timespan = DateTime.Now;
            string filepath = GetFilePath(timespan);
            FileInfo fileinfo = GetLaseWriteFile(filepath, timespan);
            FileStream filestream = GetWriteFileStream(fileinfo, filepath, timespan);
            if (filestream == null)
            {
                return false;
            }
            try
            {
                StreamWriter sw = new StreamWriter(filestream);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.Write(content);
                sw.Flush();
                sw.Close();
                return true;
            }
            finally
            {
                filestream.Close();
                filestream.Dispose();
            }
        }
    }
}
