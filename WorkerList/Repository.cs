using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace WorkerList
{
    internal class Repository
    {
        private string fileName;
        private string[] notFound = { "<<<Список пуст>>>" };

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="fileName"></param>
        public Repository(string fileName)
        {
            this.fileName = fileName;
            CheckFile();
        }

        // Вспомогательные методы

        /// <summary>
        /// Проверяет наличие файла, если его нет, то создает пустой файл
        /// </summary>
        /// <param name="fileName"></param>
        private void CheckFile()
        {
            FileInfo file = new FileInfo(fileName);
            if (!file.Exists) ClearFile();
        }

        /// <summary>
        /// Считывает данные с файла, создает список, и возвращает его
        /// </summary>
        /// <returns></returns>
        private Worker[] ReturnWorkerList()
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                uint count = (uint)File.ReadAllLines(fileName).Length;

                if (count > 0)
                {
                    Worker[] dataWorkers = new Worker[count];

                    for (uint step = 0; step < count; step++)
                    {
                        string[] dataPerson = sr.ReadLine().Split('#');

                        dataWorkers[step] = new Worker(dataPerson);
                    }
                    return dataWorkers;
                }
                return null;
            }
        }

        /// <summary>
        /// Возвращает отсортированный по ID список
        /// </summary>
        /// <param name="workerList"></param>
        /// <returns></returns>
        private Worker[] SortedWorkerList(Worker[] workerList)
        {
            if (workerList != null)
            {
                IEnumerable<Worker> workers = workerList.OrderBy(w => w.ID);
                return workers.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Перезаписывает файл после сортировки списка
        /// </summary>
        /// <param name="fileName"></param>
        private void ReWriteFile()
        {
            Worker[] workers = ReturnWorkerList();
            if (workers != null)
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    Worker[] sortList = SortedWorkerList(workers);
                    if (sortList != null)
                        foreach (Worker worker in sortList)
                            sw.WriteLine(worker.FieldToString());
                }
        }

        /// <summary>
        /// Возвращает номер свободного ID при добавлении сотрудника
        /// </summary>
        /// <param name="workers"></param>
        /// <returns></returns>
        private uint GetID()
        {
            Worker[] workers = ReturnWorkerList();
            if (workers != null)
            {
                uint count = (uint)workers.Length;
                for (uint id = 0; id < count; id++)
                {
                    if (workers[id].ID != id + 1)
                        return id + 1;
                }
                return count + 1;
            }
            return 1;
        }


        // Основные методы

        /// <summary>
        /// Возвращает данные о всех сотрудниках
        /// </summary>
        /// <param name="workerList"></param>
        /// <returns></returns>
        public string[] GetWorkerList()
        {
            Worker[] workerList = ReturnWorkerList();

            if (workerList != null)
            {
                string[] lines = new string[workerList.Length];

                for (uint index = 0; index < lines.Length; index++)
                    lines[index] = workerList[index].Info();
                return lines;
            }
            return notFound;
        }

        /// <summary>
        /// Возвращает данные о сотруднике по его ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetWorkerByID(uint id)
        {
            Worker[] workers = ReturnWorkerList();
            if (workers != null)
                foreach (Worker worker in workers)
                    if (worker.ID == id)
                        return worker.Info();
            return $"Сотрудника с таким идентификатором нет в списке.";
        }

        /// <summary>
        /// Добавляет сотрудника в файл, после чего перезаписывает файл
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="age"></param>
        /// <param name="height"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="placeOfByrth"></param>
        public void AddWorker(DateTime dateCreate,string fullName, byte age, byte height, DateTime dateOfBirth, string placeOfByrth)
        {
            Worker worker = new Worker(GetID(), dateCreate, fullName, age, height, dateOfBirth, placeOfByrth);
            using (StreamWriter sw = new StreamWriter(fileName, true))
                sw.WriteLine(worker.FieldToString());
            ReWriteFile();
        }

        /// <summary>
        /// Удаляет из списка сотрудника по заданному ID
        /// </summary>
        /// <param name="id"></param>
        public bool RemoveWorker(uint id)
        {
            Worker[] workers = ReturnWorkerList();
            bool flag = false;

            if (workers != null)
            {
                string nameWorker = string.Empty;
                using (StreamWriter sw = new StreamWriter(fileName, false))
                    foreach (Worker elem in workers)
                        if (elem.ID != id)
                        {
                            sw.WriteLine(elem.FieldToString());
                        }
                        else
                            flag = true;
                return flag;
            }
            return false;
        }

        /// <summary>
        /// Выборка элементов списка по дате занесения в список
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public void SampleByDate(DateTime startDate, DateTime endDate)
        {
            Worker[] workers = ReturnWorkerList();
            if (workers != null)
            {
                bool flag = false;
                for (uint index = 0; index < workers.Length; index++)
                {
                    if (workers[index].CreateDateTime.Date >= startDate.Date &&
                        workers[index].CreateDateTime.Date <= endDate.Date)
                    {
                        flag = true;
                        WriteLine(workers[index].Info());
                    }
                }
                if (!flag)
                    WriteLine("Ни один сотрудник не попал в данный диапазон.");
            }
            else
                WriteLine("Список сотрудников пуст.");
        }

        /// <summary>
        /// Заносит в файл сотрудника со случайно и последовательно введенными данными
        /// </summary>
        /// <param name="coutnElem"></param>
        public void CreateRandElem(uint coutnElem)
        {
            Random rand = new Random();

            for (uint step=0; step < coutnElem; step++)
            {
                string fullName = $"Имя_{step + 1}";
                double randDay = rand.Next(-100, 0);
                DateTime dateCreate = DateTime.Now.AddDays(randDay); // Чтоб можно было поиграться выборкой по дате
                byte age = (byte)rand.Next(18, 80);
                byte height = (byte)rand.Next(150, 200);
                DateTime dateOfBirth = DateTime.Now.AddYears(-age);

                AddWorker(dateCreate, fullName, age, height, dateOfBirth, "Not Place");
            }
        }

        /// <summary>
        /// Очищает файл сос списком сотрудников
        /// </summary>
        public void ClearFile()
        {
            using (StreamWriter sw = new StreamWriter(fileName, false));
        }
    }
}
