using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chyisKURSACH
{
    public class Employee : IComparable // реалізуємо інтерфейс порівння об'єктів між собою
    {
        [Key] // прив'язоємо параметр ідентифікатору до самостійного інкрементування на 1
        public int Id;
        public string firstName;
        public string lastName;
        public string Surname;
        public string job;
        public string workedHours;
        public string salary;
        public string salaryType;


       public virtual int AverageSalary() // добавляємо метод для реалізації в похідних класах
        {
            return 0;
        }

        public int CompareTo(object? obj) // реалізуємо метод інтерфейсу і порівнюємо об'єкти
        {
            Employee b;
            b = (Employee)obj;
            return AverageSalary().CompareTo(b.AverageSalary);
        }
    }
}
