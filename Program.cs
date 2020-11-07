using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace BlogsConsole
{
    class Program
    {
                // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program Started");
            string choice;
            var db = new BloggingContext();
            try
            {
                do{
                    // Menu
                    Console.WriteLine("1). Display all Blogs");
                    Console.WriteLine("2). Add Blog");
                    Console.WriteLine("3). Create Post");
                    Console.WriteLine("4). Display Posts");
                    Console.WriteLine("Enter q to quit");

                    choice = Console.ReadLine();


            if(choice =="1"){

                logger.Info("User selected option 1");
                // Display all Blogs from the database
                var query = db.Blogs.OrderBy(b => b.Name);

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                    }
            }    
            else if(choice == "2"){
                logger.Info("User selected option 2");
                // Create and save a new Blog
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                var blog = new Blog { Name = name };

                
                db.AddBlog(blog);
                logger.Info("Blog added - {name}", name);
                }
                
            } while(choice == "1" || choice == "2" || choice == "3" || choice == "4");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }
    }
}
