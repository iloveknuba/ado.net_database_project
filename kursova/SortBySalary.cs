using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chyisKURSACH
{
    public class SortBySalary : IComparer<Employee> // наслідуємо інтерфейс для порівняння даних об'єктів класу Employee
    {
        public int Compare(Employee? x, Employee? y) // Релізуємо метод інтерфейсу для порівняння середньої зарплати 
        {
            Employee t1 = x;
            Employee t2 = y;
            if (t1.AverageSalary() < t2.AverageSalary()) return 1;
            if (t1.AverageSalary() > t2.AverageSalary()) return -1;
            return 0;
        }
    }
}
