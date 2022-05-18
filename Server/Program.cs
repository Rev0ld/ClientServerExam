using System;
using System.Collections.Generic;
using BusinessLayer;

namespace Server
{
    class Program
    {
        static Dictionary<Type, object> recievedOperation = new Dictionary<Type, object>();
        static Dictionary<Type, object> recievedData = new Dictionary<Type, object>(); static BinaryMessage operationToSend = new BinaryMessage();
        static BinaryMessage dataToSend = new BinaryMessage();
        static List<Student> students = new List<Student>();
        static List<Classroom> classrooms = new List<Classroom>();
        static void Main(string[] args)
        {
            try
            {
                ServerManager.InitializeServer();

                while (true)
                {
                    ServerManager.ListenForNewConections();

                    do
                    {
                        recievedOperation = ServerManager.WaitForMessage(); 
                        recievedData = ServerManager.WaitForMessage(); 

                        ProcessClientOperation();

                        if (!ServerManager.CommunicationIsActive)
                        {
                            
                            break;
                        }
                    } while (true);

                    if (!ServerManager.ContinueListening())
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to close the program");
                Console.ReadKey();
            }
            finally
            {
                ServerManager.CloseConnection();
            }
        }

        private static void ProcessClientOperation()
        {
            Classroom classroom = null;
            Student student = null;

            int? index;

            int? operation = recievedOperation[typeof(Int32)] as int?;

            switch (operation)
            {
                case 1:
                    student = recievedData[typeof(Student)] as Student;
                    students.Add(student);
                    Console.WriteLine("User added successfully!");
                    break;

                case 2:
                    classroom = recievedData[typeof(Classroom)] as Classroom;
                    classrooms.Add(classroom);
                    Console.WriteLine("Message added successfully!");
                    break;

                case 3:
                    dataToSend = TransformDataManager.Serialize(students);
                    ServerManager.SendMessage(dataToSend);
                    break;

                case 4:
                    dataToSend = TransformDataManager.Serialize(classrooms);
                    ServerManager.SendMessage(dataToSend);
                    break;

                case 5:
                    ServerManager.CommunicationIsActive = false;
                    break;

                default:
                    break;
            }
        }
        public static void EndCommunication()
        {
            operationToSend = TransformDataManager.Serialize(6);
            dataToSend = TransformDataManager.Serialize(string.Empty);

            ServerManager.SendMessage(operationToSend);
            ServerManager.SendMessage(dataToSend);
        }

    }
}
