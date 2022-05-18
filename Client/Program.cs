using System;
using BusinessLayer;
using System.Collections.Generic;
using System.Linq;

namespace Client
{
    class Program
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
            List<int> points = Console.ReadLine().Split(' ').Select(int.Parse).ToList();

            Student student = new Student(name, age, points);

            operationToSend = TransformDataManager.Serialize(1);
            dataToSend = TransformDataManager.Serialize(student);

            ClientManager.SendMessage(operationToSend);
            ClientManager.SendMessage(dataToSend);
        }

        private static void CreateClassroom() 
        {
            Console.WriteLine("Name: ");
            string name = Console.ReadLine();

            Console.WriteLine("Subject: ");
            string subject = Console.ReadLine();  

            Classroom classroom = new Classroom(name, subject); 

            operationToSend = TransformDataManager.Serialize(2);
            dataToSend = TransformDataManager.Serialize(classroom);

            ClientManager.SendMessage(operationToSend);
            ClientManager.SendMessage(dataToSend);
        }

        private static void ViewStudent() 
        {
            operationToSend = TransformDataManager.Serialize(3);
            dataToSend = TransformDataManager.Serialize(string.Empty);

            ClientManager.SendMessage(operationToSend);
            ClientManager.SendMessage(dataToSend);

            receivedData = ClientManager.WaitForMessage();

            List<Student> students = receivedData[typeof(List<Student>)] as List<Student>;

            Console.WriteLine(Environment.NewLine + "Student Information: ");
            foreach (Student student in students)
            {
                Console.WriteLine("ID: {0}", student.ID);
                Console.WriteLine("Name: {0} # Age: {1} # Points: {2}", student.Name, student.Age, string.Join(", ", student.Points));

            }
            Console.WriteLine();
        }

        private static void ViewClassroom() 
        {
            operationToSend = TransformDataManager.Serialize(4);
            dataToSend = TransformDataManager.Serialize(string.Empty);

            ClientManager.SendMessage(operationToSend);
            ClientManager.SendMessage(dataToSend);

            receivedData = ClientManager.WaitForMessage();

            List<Classroom> classrooms = receivedData[typeof(List<Classroom>)] as List<Classroom>;

            Console.WriteLine(Environment.NewLine + "Classroom Information: ");

            foreach (Classroom classroom in classrooms) 
            {
                Console.WriteLine("ID: {0}", classroom.ID);
                Console.WriteLine("Name: {0} # Subject: {1}", classroom.Name, classroom.Subject);
            }
            Console.WriteLine();

        }

        public static void EndCommunication() 
        {
            operationToSend = TransformDataManager.Serialize(5);
            dataToSend = TransformDataManager.Serialize(string.Empty);

            ClientManager.SendMessage(operationToSend);
            ClientManager.SendMessage(dataToSend);
        }
    }
}
