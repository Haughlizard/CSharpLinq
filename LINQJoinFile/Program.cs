using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQJoinFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] names = System.IO.File.ReadAllLines(@"names.csv");
            string[] scores = System.IO.File.ReadAllLines(@"scores.csv");

            //根据ID匹配成绩表和名称表数据,并输出带名字的成绩单
            var scoresQuery1 =
                from name in names
                let nameField = name.Split(',')
                from score in scores
                let scoreField = score.Split(',')
                where Convert.ToInt32(nameField[2]) == Convert.ToInt32(scoreField[0])
                select $"{nameField[0]} {nameField[1]}:{scoreField[1]},{scoreField[2]},{scoreField[3]},{scoreField[4]}";

            foreach(var item in scoresQuery1)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }

        /*
         * Omelchenko Svetlana: 97, 92, 81, 60
         * O'Donnell Claire: 75, 84, 91, 39
         * Mortensen Sven: 88, 94, 65, 91
         * Garcia Cesar: 97, 89, 85, 82
         * Garcia Debra: 35, 72, 91, 70
         * Fakhouri Fadi: 99, 86, 90, 94
         * Feng Hanying: 93, 92, 80, 87
         * Garcia Hugo: 92, 90, 83, 78
         * Tucker Lance: 68, 79, 88, 92
         * Adams Terry: 99, 82, 81, 79
         * Zabokritski Eugene: 96, 85, 91, 60
         * Tucker Michael: 94, 92, 91, 91
         */
    }
}
