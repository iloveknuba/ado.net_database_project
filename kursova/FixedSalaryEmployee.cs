using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chyisKURSACH
{
    public class FixedSalaryEmployee : Employee
    {
        
        public override int AverageSalary()
        {
            return int.Parse(salary);
        }

    }
}
