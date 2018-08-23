using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VAllens.LookupTable
{
    /// <summary>
    /// 文件夹助手
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// 获取指定目录下(包含子孙目录)，所有后缀名匹配的文件列表
        /// </summary>
        /// <param name="folderPath">要查找的目录路径</param>
        /// <param name="extensions">要匹配的多个后缀名，如果为null或空，则所有文件将会被匹配</param>
        /// <param name="matchFiles">被匹配的文件列表，不可以为null，结果可能为空。</param>
        /// <param name="excludedFiles">被忽略的文件列表，可以为null，结果可能为空。</param>
        public static void GetFilesOfRecursionFolder(string folderPath, ICollection<string> matchFiles,
            ICollection<string> excludedFiles = null, params string[] extensions)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            IEnumerable<FileSystemInfo> fileSystemInfos = directoryInfo.GetFileSystemInfos();
            foreach (FileSystemInfo fileSystemInfo in fileSystemInfos)
            {
                //判断是否为文件夹
                if (fileSystemInfo is DirectoryInfo)
                {
                    GetFilesOfRecursionFolder(fileSystemInfo.FullName, matchFiles, excludedFiles, extensions); //递归调用
                }
                else
                {
                    //不填后缀名过滤条件或者填写了星号(任何文件)的过滤条件
                    if (extensions == null || !extensions.Any() || extensions.Any(t => t == "*"))
                    {
                        matchFiles.Add(fileSystemInfo.FullName);
                    }
                    else
                    {
                        if (extensions.Any(t => t == fileSystemInfo.Extension))
                        {
                            matchFiles.Add(fileSystemInfo.FullName);
                        }
                        else
                        {
                            //没有后缀名的文件
                            if (extensions.Any(t => t == string.Empty))
                            {
                                matchFiles.Add(fileSystemInfo.FullName);
                            }
                            else
                            {
                                excludedFiles?.Add(fileSystemInfo.FullName);
                            }
                        }
                    }
                }
            }
        }
    }
}