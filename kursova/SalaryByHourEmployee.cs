using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chyisKURSACH
{
    public class SalaryByHourEmployee : Employee
    {
    
        public override int AverageSalary() // реалізуємо метод знаходження середньомісячної зарплати з батьківського класу
        {
            
            switch (job)
            {
                case "Викладач": salary = "50"; break;
                case "Доцент": salary = "85";break;
                case "Професор": salary = "100";break;
            }

            return int.Parse(workedHours) * int.Parse(salary);
        }
    }
}
