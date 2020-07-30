using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*************************************************************
 * 标准查询运算符是组成Linq模式的方法,其中大多数方法都作用于序列.
 * 其中,序列指类型实现IEnumerable<T>接口或IQueryable<T>接口的对象.
 * 标准查询运算符提供包括筛选,投影,聚合,排序等查询功能
 *************************************************************/
namespace StandardQueryOperator
{
    class Program
    {
        static void Main(string[] args)
        {
            string sentence = "the quick brown fox jumps over the lazy dog";
            string[] words = sentence.Split(' ');

            //按单词长度升序排列
            var query1 =
                from word in words
                orderby word.Length
                select word;

            //方法语法的升序排列
            var query2 =
                words.Select(p => p).
                OrderBy(p => p.Length);

            //按单词长度降序排列
            var query3 =
                from word in words
                orderby word.Length descending
                select word;

            //方法语法的降序排列
            var query4 =
                words.Select(p => p).
                OrderByDescending(p => p.Length);

            //按单词长度降序排列
            var query5 =
                from word in words
                orderby word.Length, word[0]
                select word;

            //方法语法的降序排列
            var query6 =
                words.Select(p => p).
                OrderByDescending(p => p.Length).
                ThenBy(p=>p[0]);

            //反转元素
            //var query = words.Reverse();




            //查询语法
            //按照单词从长度进行分组,并对组按长度排序
            //var query =
            //    from word in words
            //    group word.ToUpper() by word.Length into gr
            //    orderby gr.Key
            //    select new { Length = gr.Key, Words = gr };

            //方法语法
            //var query2 = words.
            //    GroupBy(w => w.Length, w => w.ToUpper()).
            //    Select(g => new { Length = g.Key, Words = g }).
            //    OrderBy(o => o.Length);

            //foreach (var obj in query)
            //{
            //    Console.WriteLine("Words of length {0}:", obj.Length);
            //    foreach (string word in obj.Words)
            //        Console.WriteLine(word);
            //}


            //string[] planets1 = { "Mercury", "Venus", "Earth", "Jupiter" };
            //string[] planets2 = { "Mercury", "Earth", "Mars", "Jupiter" };
            //IEnumerable<string> query = from planet in planets1.Except(planets2)
            //                            select planet;
            //foreach (var str in query)
            //{
            //    Console.WriteLine(str);
            //}

            ///*This code produces the following output:
            //*
            //*Venus
            //*/

            //string[] planets1 = { "Mercury", "Venus", "Earth", "Jupiter" };
            //string[] planets2 = { "Mercury", "Earth", "Mars", "Jupiter" };

            ////返回序列包含两个序列的共有元素
            //IEnumerable<string> query = from planet in planets1.Intersect(planets2)
            //                            select planet;
            //foreach (var str in query)
            //{
            //    Console.WriteLine(str);
            //}

            ///*This code produces the following output:
            //*Mercury
            //* Earth
            //* Jupiter
            //*/

            string[] planets1 = { "Mercury", "Venus", "Earth", "Jupiter" };
            string[] planets2 = { "Mercury", "Earth", "Mars", "Jupiter" };
            IEnumerable<string> query = from planet in planets1.Union(planets2)
                                        select planet;
            foreach (var str in query)
            {
                Console.WriteLine(str);
            }
            /*This code produces the following output:
            * Mercury
            * Venus
            * Earth
            * Jupiter
            * Mars
            */
            Example();
            Console.ReadKey();
        }

        //class Market
        //{
        //    public string Name { get; set; }
        //    public string[] Items { get; set; }
        //}
        //public static void Example()
        //{
        //    List<Market> markets = new List<Market>
        //    {
        //    new Market { Name = "Emily's", Items = new string[] { "kiwi", "cheery", "banana" } },
        //    new Market { Name = "Kim's", Items = new string[] { "melon", "mango", "olive" } },
        //    new Market { Name = "Adam's", Items = new string[] { "kiwi", "apple", "orange" } },
        //    };
        //    // 确定集合中的元素长度是否等于5,输出符合条件的项
        //    IEnumerable<string> names = from market in markets
        //                                where market.Items.All(item => item.Length == 5)
        //                                select market.Name;
        //    foreach (string name in names)
        //    {
        //        Console.WriteLine($"{name} market");
        //    } 
        //    // This code produces the following output:
        //    //
        //    // Kim's market
        //}

        class Product
        {
            public string Name { get; set; }
            public int CategoryId { get; set; }
        }
        class Category
        {
            public int Id { get; set; }
            public string CategoryName { get; set; }
        }
        public static void Example()
        {
            List<Product> products = new List<Product>
            {
                new Product { Name = "Cola", CategoryId = 0 },
                new Product { Name = "Tea", CategoryId = 0 },
                new Product { Name = "Apple", CategoryId = 1 },
                new Product { Name = "Kiwi", CategoryId = 1 },
                new Product { Name = "Carrot", CategoryId = 2 },
            };
            List<Category> categories = new List<Category>
            {
                new Category { Id = 0, CategoryName = "Beverage" },
                new Category { Id = 1, CategoryName = "Fruit" },
                new Category { Id = 2, CategoryName = "Vegetable" }
            };
            // Join products and categories based on CategoryId
            //var query = from product in products
            //            where product.CategoryId == 0
            //            join category in categories on product.CategoryId equals category.Id
            //            select new { product.Name, category.CategoryName };
            var query = from category in categories
                        join product in products on category.Id equals product.CategoryId into productGroup
                        select productGroup;
            foreach (IEnumerable<Product> productGroup in query)
            {
                Console.WriteLine("Group");
                foreach (Product product in productGroup)
                {
                    Console.WriteLine($"{product.Name,8}");
                }
            }
            // This code produces the following output:
            //
            // Group
            //   Cola
            //   Tea
            // Group
            //   Apple
            //   Kiwi
            // Group
            //   Carrot
        }
    }
}
