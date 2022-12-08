using System;
using static System.Console;

namespace WorkerList
{
    internal class Program
    {
        static void Main()
        {
            Repository bd = new Repository(@"personal.txt");

            while (true)
            {
                InitMenu();

                Write("Выберите действие: ");
                string action = ReadLine();
                if (action == "0")
                    break;
                else if (action == "1")
                {
                    Clear();
                    WriteLine("Список сотрудников:\n");
                    foreach (string worker in bd.GetWorkerList())
                        WriteLine(worker);
                    ReadKey(true);
                }    
                else if (action == "2")
                {
                    Clear();
                    do
                    {
                        Write("Введите ID сотрудника: ");
                        uint id = uint.Parse(ReadLine());

                        WriteLine("\nИнформация о сотруднике:");
                        WriteLine(bd.GetWorkerByID(id));
                        Write("Продолжить(д/н)\n");
                    } while (ReadKey(true).KeyChar == 'д');
                }
                else if (action == "3")
                {
                    Clear();
                    do
                    {
                        WriteLine("Введите данные:");
                        Write("Фамилия Имя Отчество: ");
                        string fullName = ReadLine();

                        Write("Возраст: ");
                        byte age = byte.Parse(ReadLine());

                        Write("Рост: ");
                        byte height = byte.Parse(ReadLine());

                        Write("Дата рождения: ");
                        DateTime birthOfDay = DateTime.Parse(ReadLine());

                        Write("Место рождения: ");
                        string placeOfBirt = ReadLine();

                        bd.AddWorker(DateTime.Now, fullName, age, height, birthOfDay, placeOfBirt);
                        WriteLine("\nСотрудник внесен в список.");
                        WriteLine("Продолжить запись(д/н)");
                    } while (ReadKey(true).KeyChar == 'д');
                }
                else if (action == "4")
                {
                    Clear();
                    do
                    {
                        Write("\nВведите ID сотрудника: ");
                        uint id = uint.Parse(ReadLine());

                        bool flag = bd.RemoveWorker(id);
                        if (flag)
                            WriteLine($"\nСотрудник c ID={id} удален из списка.");
                        else
                            WriteLine("\nСписок пуст или сотрудника с данным ID нет в списке.");
                        Write("Продолжить удаление из списка(д/н)");
                    } while (ReadKey(true).KeyChar == 'д');
                }
                else if (action == "5")
                {
                    Clear();

                    Write("Введите начальную дату: ");
                    DateTime startDate = DateTime.Parse(ReadLine());

                    Write("Введите конечную дату: ");
                    DateTime endDate = DateTime.Parse(ReadLine());

                    WriteLine($"Выборка из диапазона дат от {startDate.Date} до {endDate.Date}:");

                    bd.SampleByDate(startDate, endDate);
                    ReadKey(true);
                }
                else if (action == "6")
                {
                    Clear();
                    Write("Введите количество сотрудников: ");
                    uint count = uint.Parse(ReadLine());
                    bd.CreateRandElem(count);
                    WriteLine("Запись завершена.");
                    ReadKey(true);
                }
                else if (action == "7")
                {
                    Clear();
                    bd.ClearFile();
                    WriteLine("Файл очищен.");
                    ReadKey(true);
                }
            }
        }

        static void InitMenu()
        {
            Clear();
            WriteLine("1.Посмотреть список всех сотрудников.");
            WriteLine("2.Посмотреть информацию о сотруднике.");
            WriteLine("3.Добавить сотрудника в список.");
            WriteLine("4.Удалить сотрудника из списка.");
            WriteLine("5.Выборка по дате занесения в список.");
            WriteLine("6.Записать случайных сотрудников.");
            WriteLine("7.Стереть все записи в файле.");
            WriteLine("0.Выход.");
        }
    }
}
