using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            //查询第一门课程分数大于90的学生
            //var studentQuery =
            //    from student in students
            //    where student.Scores[0] > 90
            //    select student;

            //查询第一门课程分数大于90的学生并倒叙排序
            //var studentQuery =
            //    from student in students
            //    where student.Scores[0] > 90
            //    orderby student.Scores[0] descending
            //    select student;

            //按照名称首字母对查询分组
            //var studentQuery =
            //    from student in students
            //    group student by student.Last[0];

            //按照名称首字母对查询分组,分组后按照键值对进行排序
            //var studentQuery =
            //    from student in students
            //    group student by student.Last[0] into studentGroup
            //    orderby studentGroup.Key
            //    select studentGroup;

            //引入let关键字,缓存查询中的计算结果,标识的计算结果可参与查询
            //查询平均分数小于第一门分数的学生信息
            //var studentQuery =
            //    from student in students
            //    let totalScores = student.Scores[0] + student.Scores[1] + student.Scores[2] + student.Scores[3]
            //    where totalScores / 4 < student.Scores[0]
            //    select student;

            //select子句转换或投影
            //使用投影输出所需信息
            //下面的例子只查询学生姓名及第一门课的成绩
            //var studentQuery =
            //    from student in students
            //    where student.Scores[0] > 90
            //    orderby student.Scores[0] descending
            //    select new { student.Last,student.First, FirstScore = student.Scores[0]};

            //计算学生的总成绩
            var studentQuery =
                from student in students
                let x = student.Scores[0] + student.Scores[1] + student.Scores[2] + student.Scores[3]
                select x;
            //计算学生的总平均成绩
            var averageTotalScore = studentQuery.Average();

            //查询总分大于班级总平均分的学生姓名及总分
            var studentQuery1 =
                from  student in students
                let x = student.Scores[0] + student.Scores[1] + student.Scores[2] + student.Scores[3]
                where x > averageTotalScore
                select new { student.Last, student.First, TotalScore = x };




            foreach (var student in studentQuery1)
            {
                //student匿名类型只包含学生姓名及第一门课成绩
                Console.WriteLine($"{student.First} {student.Last}:{student.TotalScore}");
            }

            //foreach (var student in studentQuery)
            //{
            //    Console.WriteLine($"{student.First} {student.Last}:{student.Scores[0]}");
            //}

            //分组后的查询类型变化,生成以字母为键的若干序列,需要两层循环输出
            //foreach (var group in studentQuery)
            //{
            //    Console.WriteLine($"{group.Key}");
            //    foreach(var student in group)
            //    {
            //        Console.WriteLine($"    {student.First} {student.Last}:{student.Scores[0]}");
            //    }
            //}

            Console.ReadKey();
        }

        // Create a data source by using a collection initializer.
        static List<Student> students = new List<Student>
        {
            new Student {First="Svetlana", Last="Omelchenko", ID=111, Scores= new List<int> {97, 92, 81, 60}},
            new Student {First="Claire", Last="O'Donnell", ID=112, Scores= new List<int> {75, 84, 91, 39}},
            new Student {First="Sven", Last="Mortensen", ID=113, Scores= new List<int> {88, 94, 65, 91}},
            new Student {First="Cesar", Last="Garcia", ID=114, Scores= new List<int> {97, 89, 85, 82}},
            new Student {First="Debra", Last="Garcia", ID=115, Scores= new List<int> {35, 72, 91, 70}},
            new Student {First="Fadi", Last="Fakhouri", ID=116, Scores= new List<int> {99, 86, 90, 94}},
            new Student {First="Hanying", Last="Feng", ID=117, Scores= new List<int> {93, 92, 80, 87}},
            new Student {First="Hugo", Last="Garcia", ID=118, Scores= new List<int> {92, 90, 83, 78}},
            new Student {First="Lance", Last="Tucker", ID=119, Scores= new List<int> {68, 79, 88, 92}},
            new Student {First="Terry", Last="Adams", ID=120, Scores= new List<int> {99, 82, 81, 79}},
            new Student {First="Eugene", Last="Zabokritski", ID=121, Scores= new List<int> {96, 85, 91, 60}},
            new Student {First="Michael", Last="Tucker", ID=122, Scores= new List<int> {94, 92, 91, 91}}
        };

        public class Student
        {
            public string First { get; set; }
            public string Last { get; set; }
            public int ID { get; set; }
            public List<int> Scores;
        }
    }
}
