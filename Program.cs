using System;
using NLog.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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
            int i = 0;
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

                // Displays number of Blogs in database
                Console.WriteLine($"There are {query.Count()} Blogs in the database\n");
                

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                    }
            }    
            else if(choice == "2"){
                logger.Info("User selected option 2");
                    // asks for name of blog
                    Console.Write("Enter a name for a new Blog: ");
                
                    var name = Console.ReadLine();
                    // checks if blog name in empty and logs error
                    if(name.Length == 0){
                        logger.Error("Blog name cannot be null");
                    } 
                else{
                // creates and saves blog
                var blog = new Blog { Name = name };
                
                db.AddBlog(blog);
                i++;
                logger.Info("Blog added - {name}\nBlog id - {id} ", name, blog.BlogId);
                }
                
            } else if(choice == "3"){
                
                Console.WriteLine("Select the blog you would like to post to:");
                // orders blogs by blog id
                var query = db.Blogs.OrderBy(b => b.BlogId);
                // displays blogs in order
                foreach (var item in query)
                {
                    Console.WriteLine(item.BlogId.ToString() + ") " + item.Name);
                    }
                    
                var option = Int32.Parse(Console.ReadLine());
                // if string was entered log error
                if(option.Equals(typeof(string))){
                    logger.Error("Not a valid integer for blog");
                } 
                // if option does not equal a correct blog id, 
                // log error
                else if(option != db.Blogs.Find(option).BlogId){
                    logger.Error("Blog not found with that Blog Id");
                }

                // else ask for title and content of post
                // create and save post to blog selected
                else{
                
                Console.WriteLine($"Please enter title");
                var title = Console.ReadLine();
                
                Console.WriteLine("Please enter content of post");
                
                var content = Console.ReadLine(); 
                var post = new Post{ Title = title, Content = content};
                
                db.AddPost(db.Blogs.Find(option), post);

                // logs post number and amount of posts for blog selected    
                logger.Info($"Post #{db.Blogs.Find(option).Posts.Count()} added to Blog: {db.Blogs.Find(option).Name}");
                }
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
