using System;

namespace WorkerList
{
    internal struct Worker
    {
        /// <summary>
        /// Свойства.
        /// </summary>
        public uint ID { get; private set; } // ID сотрудника
        public DateTime CreateDateTime { get; private set; } // Дата и время записи
        public string FullName { get; set; } // Фамилия Имя Отчество
        public byte Age { get; set; } // Возраст
        public byte Height { get; set; } // Рост
        public DateTime DateOfBirth { get; set; } // Дата рождения
        public string PlaceOfBirth { get; set; } // Место рождения

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="args"></param>
        public Worker(string[] args)
        {
            ID = uint.Parse(args[0]);
            CreateDateTime = DateTime.Parse(args[1]);
            FullName = args[2];
            Age = byte.Parse(args[3]);
            Height = byte.Parse(args[4]);
            DateOfBirth = DateTime.Parse(args[5]);
            PlaceOfBirth = args[6];
        }
        public Worker(uint id, DateTime createDateTime, string fullName, byte age, byte height, DateTime dateOfBirth, string placeOfBirth)
        {
            ID = id;
            CreateDateTime = createDateTime; 
            FullName = fullName;
            Age = age;
            Height = height;
            DateOfBirth = dateOfBirth;
            PlaceOfBirth = placeOfBirth;
        }

        /// <summary>
        /// Вывод информации о сотруднике
        /// </summary>
        /// <returns></returns>
        public string Info()
        {
            return 
                $"ID: {ID}" +
                $"\nДата создания: {CreateDateTime.ToString("g")}" +
                $"\nФ.И.О: {FullName}" +
                $"\nВозраст: {Age}" +
                $"\nРост: {Height}" +
                $"\nДата рождения: {DateOfBirth.ToString("d")}" +
                $"\nМесто рождения: {PlaceOfBirth}\n";
        }

        /// <summary>
        /// Конвертирование данных в строку для записи в файл
        /// </summary>
        /// <param name="worker"></param>
        /// <returns></returns>
        public string FieldToString()
        {
            return 
                string.Join("#", 
                ID, 
                CreateDateTime.ToString("g"), 
                FullName, 
                Age, 
                Height, 
                DateOfBirth.ToString("d"), 
                PlaceOfBirth);
        }
    }
}
