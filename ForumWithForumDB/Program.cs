using System;
using Microsoft.Data.Sqlite;

using ForumWithForumDB;

namespace Forum
{
    class Program
    {
   
        static void Main(string[] args)
        {

            var repo = new SqliteInitialization();
            repo.PrintVersion();

            var postRepo = new SqlitePostRepository();
            var threadRepo = new SqliteThreadRepository();
            var userRepo = new SqliteUserRepository();

            while (true)
            {

                Console.WriteLine("Welcome to Watches Forum! \n\n" + 
                                  "Enter 1 to open the forum. \n" +
                                  "Enter 2 to see active users. \n");

                string x = "";
                x = Console.ReadLine();

                if (x == "1")
                {

                    Console.Clear();
                    threadRepo.GetThreads();

                    while (true)
                    {
                        PrintThreads(threadRepo);

                        var people = threadRepo.GetThreads();
                        Console.WriteLine("Enter Thread id to view it." +
                            "\nType c to create a new thread." +
                            "\nType x to go back.");

                        string input = Console.ReadLine();

                        if (input == "x")
                        {
                            Console.Clear();
                            break;
                        }

                        if (input == "c")
                        {

                            Console.Clear();
                                     
                            Thread newThread = new Thread();
                            Console.WriteLine("Write a topic:");
                            newThread.Topic = Console.ReadLine();

                            Console.WriteLine("Write a text:");
                            newThread.Text = Console.ReadLine();
                            newThread.PostCount = 100;

                            threadRepo.AddThread(newThread);

                            Console.WriteLine("Thread added.");

                            continue;
                        }

                        int threadId = int.Parse(input);

                        Console.Clear();
                        var threadCreator = userRepo.GetPersonWithIdWhoCreatedThread(threadId);
                        Console.WriteLine("User: " + threadCreator.FirstName);
                        ListPostInThread(postRepo, threadId);


                        while (true)
                        {
                            Console.WriteLine("Enter 1 to add a post");
                            Console.WriteLine("Enter 2 to edit a post");
                            Console.WriteLine("Enter 3 to delete a post");
                            Console.WriteLine("Enter x to go back.");

                            string threadInput = Console.ReadLine();

                            if (threadInput == "x")
                            {
                                Console.Clear();
                                break;
                            }

                            if (threadInput == "1")
                            {
                                Console.WriteLine(threadId);

                                Post newPost = new Post();
                                Console.WriteLine("Write a text:");
                                newPost.Text = Console.ReadLine();

                                Console.WriteLine("Enter the owners ID:");
                                newPost.UserId = Console.ReadLine();
                                newPost.ThreadId = threadId.ToString();
                                postRepo.AddPost(newPost);

                                var obj = threadRepo.GetThreadWithId(threadId);
                                obj.PostCount = obj.PostCount + 1;
                                threadRepo.UpdateThreadPostCount(obj);


                                Console.Clear();
                                break;
                            }

                            if (threadInput == "2")
                            {
                                Console.WriteLine("Enter the ID of the post you want to edit?");
                                string editPostId = Console.ReadLine();

                                int editPostInt = int.Parse(editPostId);

                                Post postObject = new Post();

                                postObject = postRepo.GetPostWithId(editPostInt);

                                Console.WriteLine("Write the new text.");
                                string newText = Console.ReadLine();

                                postObject.Text = newText;

                                postRepo.UpdatePost(postObject);

                            }

                            if (threadInput == "3")
                            {
                                Console.WriteLine("Enter the ID of the post you want to delete?");
                                string deletePostId = Console.ReadLine();

                                int editPostInt = int.Parse(deletePostId);

                                Post postObject = new Post();

                                postObject = postRepo.GetPostWithId(editPostInt);

                                postRepo.DeletePost(postObject);

                                var obj = threadRepo.GetThreadWithId(threadId);
                                obj.PostCount = obj.PostCount - 1;
                                threadRepo.UpdateThreadPostCount(obj);

                                Console.Clear();
                                break;
                            }
                        }
                    }
                    continue;
                }

                if (x == "2")
                {

                    Console.Clear();

                    userRepo.GetUsers();
                    PrintUsers(userRepo);
                    continue;
                }
                x = "";
            }
        }

        public class SqliteInitialization
        {
            private const string _connectionString = "Data Source=.\\Forum.db";

            public void PrintVersion()
            {
                using var connection = new SqliteConnection(_connectionString);

                System.Console.WriteLine(connection.ServerVersion);
            }
        }
        public static void ListPostInThread(SqlitePostRepository repository, int threadId)
        {

            var thread = repository.GetThreadWithId(threadId);
            var posts = repository.GetPostsFromThread(thread);
            Console.WriteLine($"Topic: {thread.Topic}\nText: {thread.Text}\n");
            foreach (var post in posts)
            {
                Console.WriteLine($"PostId: {post.PostId} {post.User.FirstName} (UserId: {post.UserId}): {post.Text}");
            }
            Console.WriteLine();
        }

        public static void PrintThreads(SqliteThreadRepository repository)
        {

            var threads = repository.GetThreads();
            foreach (var thread in threads)
            {

                Console.WriteLine("Thread id: " + thread.ThreadId);
                Console.WriteLine("Topic: " + thread.Topic);
                Console.WriteLine("Posts: " + thread.PostCount + "\n");
            }
        }

        public static void PrintUsers(SqliteUserRepository repository)
        {
            var people = repository.GetUsers();
            foreach (var person in people)
            {
                Console.WriteLine(person.FirstName + " " + person.LastName);
            }
        }
    }
}

