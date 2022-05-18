using System;
using BusinessLayer;
using System.Collections.Generic;

namespace Client
{
    internal class Program
    {
        static Dictionary<Type, object> receivedOperation = new Dictionary<Type, object>();
        static Dictionary<Type, object> receivedData = new Dictionary<Type, object>();
        static BinaryMessage operationToSend = new BinaryMessage();
        static BinaryMessage dataToSend = new BinaryMessage();

        static void Main(string[] args)
        {
            try
            {
                ClientManager.InitializeClient();
                do
                {
                    ShowMenu();
                    if (!ClientManager.CommunicationIsActive) 
                    {
                        break;
                    }
                } while (ClientManager.ContinueCommunication());

                EndCommunication();

                Console.WriteLine("Press any key to close the client API!");
                Console.ReadKey();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to close the program!");
                Console.ReadKey();
            }
            finally
            {
                ClientManager.CloseConnection();
            }
        }

        private static void ShowMenu() 
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Select option (number): ");
            Console.WriteLine("1) Create Student");
            Console.WriteLine("2) Create Classroom");
            Console.WriteLine("3) View Student");
            Console.WriteLine("4) View Classroom");
            Console.WriteLine("5) Exit");

            Console.Write("Your choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1: CreateStudent(); break;
                case 2: CreateClassroom(); break;
                case 3: ViewStudent(); break;
                case 4: ViewClassroom(); break;
                case 5: ClientManager.CommunicationIsActive = false; break;
                default:throw new ArgumentException("Invalid option!");
            }
        }
        private static void CreateStudent() 
        {
            Console.WriteLine("Name: ");
            string name = Console.ReadLine();

            Console.WriteLine("Age: ");
            byte age = Convert.ToByte(Console.ReadLine());

            Console.WriteLine("Points: ");
            List<int> points = Console.ReadLine().Split('').Select(int.Parse).ToList();

            Student student = new Student(name, age);
        }
    }
}
