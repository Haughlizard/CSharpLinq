using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToObject
{
    class Program
    {
        static string text = @"Historically, the world of data and the world of objects" +
            @" have not been well integrated. Programmers work in C# or Visual Basic" +
            @" and also in SQL or XQuery. On the one side are concepts such as classes," +
            @" objects, fields, inheritance, and .NET Framework APIs. On the other side" +
            @" are tables, columns, rows, nodes, and separate languages for dealing with" +
            @" them. Data types often require translation between the two worlds; there are" +
            @" different standard functions. Because the object world has no notion of query, a" +
            @" query can only be represented as a string without compile-time type checking or" +
            @" IntelliSense support in the IDE. Transferring data from SQL tables or XML trees to" +
            @" objects in memory is often tedious and error-prone.";

        static void Main(string[] args)
        {
            //CountWords();
            FindSentences();
            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }

        //计算字符串中单词出现的数量
        public static void CountWords()
        {
            string searchTerm = "data";

            //分割字符串为单词
            string[] source = text.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' },
                StringSplitOptions.RemoveEmptyEntries);

            //查找与查询单词匹配的单词序列
            var matchQuery =
                from word in source
                where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                select word;

            //执行查询,计算单词数目
            int wordCount = matchQuery.Count();

            Console.WriteLine("{0} occurrences(s) of the search term \"{1}\" were found.", wordCount, searchTerm);
        }

        /// <summary>
        /// 查询包含一组指定词语的句子
        /// </summary>
        public static void FindSentences()
        {
            //分割字符串为句子
            string[] sentences = text.Split(new char[] { '.', '?', '!' });

            string[] wordsToMatch = { "Historically", "data", "integrated" };

            //查询含有匹配单词的句子
            var sentenceQuery =
                from sentence in sentences
                let words = sentence.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' },
                StringSplitOptions.RemoveEmptyEntries)
                where words.Distinct().Intersect(wordsToMatch).Count() == wordsToMatch.Count()
                select sentence;

            foreach (string str in sentenceQuery)
            {
                Console.WriteLine(str);
            }
        }

        /// <summary>
        /// 查询字符串中的字符
        /// </summary>
        public static void QueryAString()
        {

        }

    }
}
