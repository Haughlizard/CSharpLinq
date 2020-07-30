using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQAndDirectory
{
    class Program
    {
        static void Main(string[] args)
        {
            string startPath = @"D:\北京交大微联科技有限公司\CSM-JD型信号集中监测系统";
            string folder2 = @"D:\北京交大微联科技有限公司\CSM-JD型信号集中监测系统\Config";
            //FindFileByExtension(startPath);
            //QuerySize(startPath);
            //GroupFileByExtension(startPath);
            //CompareDirs(startPath, folder2);
            QueryFilesBySize(startPath);
            Console.ReadKey();
        }

        public static void QueryFilesBySize(string startFolder)
        {
            int startFolderLength = startFolder.Length;
            DirectoryInfo dir = new DirectoryInfo(startFolder);

            var fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);

            //查询最大文件的字节数
            long maxSize = (
                from file in fileList
                select GetFileLength(file.FullName)
                ).Max();

            //查询最大文件的文件信息
            var longestFile = (
                from file in fileList
                let length = GetFileLength(file.FullName)
                where length > 0
                orderby length descending
                select file).First();

            Console.WriteLine($"占用空间最大的文件为:{longestFile.FullName.Substring(startFolderLength)},占用字节数:{maxSize}");

            //查询最小文件的文件信息
            var smallestFile = (
                from file in fileList
                let length = GetFileLength(file.FullName)
                where length > 0
                orderby length
                select file).First();
            Console.WriteLine($"占用空间最小文件为:{smallestFile.FullName.Substring(startFolderLength)}");

            //查询最大的10个文件的文件信息
            var queryTenLargest = (
                from file in fileList
                let length = GetFileLength(file.FullName)
                where length > 0
                orderby length descending
                select file).Take(10);

            Console.WriteLine($"占用空间最大的十个文件为:");
            foreach(var file in queryTenLargest)
            {
                Console.WriteLine($"\t{file.FullName.Substring(startFolderLength)}");
            }

            //按文件大小对文件分组查询
            var querySizeGroups =
                from file in fileList
                let len = GetFileLength(file.FullName)
                where len > 0
                group file by (len / 1000000) into fileGroup
                where fileGroup.Key > 2
                orderby fileGroup.Key descending
                select fileGroup;

            foreach(var fileGroup in querySizeGroups)
            {
                Console.WriteLine($"文件大小为:{fileGroup.Key}");
                foreach(var file in fileGroup)
                {
                    Console.WriteLine($"\t{file.FullName.Substring(startFolderLength)}");
                }
            }
        }

        public static void QuerySize(string startFolder)
        {
            var fileList = Directory.GetFiles(startFolder, "*.*", SearchOption.AllDirectories);
            var fileQuery =
                from file in fileList
                select GetFileLength(file);
            long[] fileLengths = fileQuery.ToArray();

            long largestFile = fileLengths.Max();

            float totalBytes = fileLengths.Sum();

            Console.WriteLine("There are {0} bytes in {1} files under {2}",
                totalBytes, fileList.Count(), startFolder);
            Console.WriteLine("The largest files is {0} bytes.", largestFile);
            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }

        static long GetFileLength(string fileName)
        {
            long fileLength = 0;
            try
            {
                FileInfo fi = new FileInfo(fileName);
                fileLength = fi.Length;
            }
            catch(FileNotFoundException)
            {
                fileLength = 0;
            }
            return fileLength;
        }

        /// <summary>
        /// 按照拓展名查找文件
        /// </summary>
        /// <param name="startPath"></param>
        public static void FindFileByExtension(string startPath)
        {
            DirectoryInfo dir = new DirectoryInfo(startPath);

            var fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);

            var fileQuery =
                from file in fileList
                where file.Extension == ".zip"
                orderby file.Name
                select file;

            foreach(var file in fileQuery)
            {
                Console.WriteLine(file.FullName);
            }

            var newestFile =
                (from file in fileList
                 orderby file.CreationTime
                 select new { file.FullName, file.CreationTime }).Last();

            Console.WriteLine($"最新文件为:{ newestFile.FullName},{newestFile.CreationTime}");
        }

        /// <summary>
        /// 按照拓展名对文件进行分组
        /// </summary>
        /// <param name="startPath"></param>
        public static void GroupFileByExtension(string startPath)
        {
            int trimLength = startPath.Length;
            DirectoryInfo dir = new DirectoryInfo(startPath);

            var fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);

            var fileGroupQuery =
                from file in fileList
                group file by file.Extension.ToLower() into fileGroup
                orderby fileGroup.Key
                select fileGroup;

            PageOutput(trimLength, fileGroupQuery);
        }

        /// <summary>
        /// 分页显示查询结果
        /// </summary>
        /// <param name="rootLength"></param>
        /// <param name="groupByExtList"></param>
        private static void PageOutput(int rootLength,
            IEnumerable<IGrouping<string, FileInfo>> groupByExtList)
        {
            bool goAgain = true;
            int numLines = Console.WindowHeight - 3;
            foreach(var fileGroup in groupByExtList)
            {
                int currentLine = 0;

                do
                {
                    Console.Clear();
                    Console.WriteLine(fileGroup.Key == string.Empty ? "[None]" : fileGroup.Key);

                    var resultPage = fileGroup.Skip(currentLine).Take(numLines);

                    foreach (var f in resultPage)
                    {
                        Console.WriteLine("\t{0}", f.FullName.Substring(rootLength));
                    }

                    currentLine += numLines;
                    Console.WriteLine("Press any key to continue or the 'End' key to break...");
                    ConsoleKey key = Console.ReadKey().Key;
                    if (key == ConsoleKey.End)
                    {
                        goAgain = false;
                        break;
                    }
                } while (currentLine < fileGroup.Count());

                if (goAgain == false)
                    break;
            }
        }

        /// <summary>
        /// 比较两个文件夹差异
        /// </summary>
        /// <param name="folder1"></param>
        /// <param name="folder2"></param>
        public static void CompareDirs(string folder1, string folder2)
        {
            int folder1Length = folder1.Length;
            int folder2Length = folder2.Length;
            DirectoryInfo dir1 = new DirectoryInfo(folder1);
            DirectoryInfo dir2 = new DirectoryInfo(folder2);
            IEnumerable<FileInfo> fileList1 = dir1.GetFiles("*.*", SearchOption.AllDirectories);
            IEnumerable<FileInfo> fileList2 = dir2.GetFiles("*.*", SearchOption.AllDirectories);

            FileCompare myFileCompare = new FileCompare();

            bool areIdentical = fileList1.SequenceEqual(fileList2, myFileCompare);

            if (areIdentical)
            {
                Console.WriteLine("两个文件夹内容一致");
            }
            else
            {
                Console.WriteLine("两个文件夹内容不一致");
            }

            var queryCommonFiles = fileList1.Intersect(fileList2, myFileCompare);
            if (queryCommonFiles.Any())
            {
                Console.WriteLine("以下文件是两个文件夹共有的:");
                foreach(var fi in queryCommonFiles)
                {
                    Console.WriteLine($"{fi.FullName.Substring(folder1Length)}");
                }
            }
            else
            {
                Console.WriteLine("两个文件夹没有相同文件");
            }

            var queryList1Only = fileList1.Except(fileList2, myFileCompare);
            Console.WriteLine("以下文件是folder1有而folder2没有的");
            foreach (var v in queryList1Only)
            {
                Console.WriteLine(v.FullName.Substring(folder1Length));
            }
        }

        class FileCompare : IEqualityComparer<FileInfo>
        {
            public bool Equals(FileInfo x, FileInfo y)
            {
                return (x.Name == y.Name
                    && x.Length == y.Length);
            }

            public int GetHashCode(FileInfo obj)
            {
                string s = $"{obj.Name}{obj.Length}";
                return s.GetHashCode();
            }
        }
    }
}
