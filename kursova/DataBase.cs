using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;

namespace chyisKURSACH
{
    public class DataBase
    {
        private readonly string connectionString;

        public DataBase(string connectionString) // створюємо конструкстор класа для використання рядку підключення до бази даних
        {
            this.connectionString = connectionString;
        }

      

        public void TableToObjectList(string tableName,List<FixedSalaryEmployee>fdxEmpl, List<SalaryByHourEmployee> byhourEmpl)
        {
            string query = $"SELECT * FROM {tableName};"; // створюємо запит для вибрання всіх даних з бд
           
             fdxEmpl.Clear();
            byhourEmpl.Clear();  // очищаємо списки якщо вони тримали в собі дані
           
            using (SqlConnection connection = new SqlConnection(connectionString)) // підключаємося до бд
            {
                connection.Open(); // відкриваємо підключення

                using (SqlCommand command = new SqlCommand(query, connection)) // реалізуємо запит
                {
                    SqlDataReader reader = command.ExecuteReader(); // зчитуємо рядки бд

                 
                    while (reader.Read()) // створюєм цикл для зчитування кожного рядка бд
                    {
                        
                        // Передаємо в списки дані з бази фільтручи по типу зарплати
                            
                            if ((string)reader["salaryType"] == "byhour")
                            {
                                byhourEmpl.Add(new SalaryByHourEmployee()
                                {
                                    Id = (int)reader["id"],
                                    firstName = (string)reader["firstName"],
                                    lastName = (string)reader["lastName"],
                                    Surname = (string)reader["surname"],
                                    job = (string)reader["position"],
                                    salaryType = (string)reader["salaryType"],
                                    salary = (string)reader["salary"],
                                    workedHours = (string)reader["workedHours"],
                                    

                                });
                            }
                            if((string)reader["salaryType"] == "fixed")
                            {
                                fdxEmpl.Add(new FixedSalaryEmployee()
                                {
                                    Id = (int)reader["id"],
                                    firstName = (string)reader["firstName"],
                                    lastName = (string)reader["lastName"],
                                    Surname = (string)reader["surname"],
                                    job = (string)reader["position"],
                                    salaryType = (string)reader["salaryType"],
                                    salary = (string)reader["salary"],
                                    workedHours = (string)reader["workedHours"],

                                });
                            }
                           
                        
                        

                    }
                    reader.Close(); // закриваємо зчитування рядків
                    
                    connection.Close(); // закриваємо підключення
                }
            }
        }
      
        public void resfreshList<T>(List<T> employees) where T : Employee // Створюємо метод для виведення даних списку
        {
            Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}{4,-15}{5,-15}{6,-15}{7,-15}{8,-15}","Id", "Ім'я", "Прізвище", "По-Батькові", "Посада", "Тип зарплати", "Зарплата", "Відпрацьовані години", "середня запрлата");
            foreach(T employee in employees)
            {
                Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}{4,-15}{5,-15}{6,-15}{7,-15}{8,-15}", employee.Id,employee.firstName, employee.lastName, employee.Surname, employee.job, employee.salaryType, employee.salary, employee.workedHours, employee.AverageSalary());
            }
        }




     
        public void AddNewRow(Employee employee, string tableName)
        {
            
             // Створюємо запит добавлення нового рядка
            string addquery = $"INSERT INTO {tableName} (firstName, lastName, surname, position, salaryType, salary, workedHours) VALUES (@firstName, @lastName, @surname, @position, @salaryType, @salary, @workedHours);";
          
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(addquery, connection))
                {
                    // підставляємо параметри нового об'єкту в запит

                    command.Parameters.AddWithValue("@firstName", employee.firstName);
                    command.Parameters.AddWithValue("@lastName", employee.lastName );
                    command.Parameters.AddWithValue("@surname",employee.Surname );
                    command.Parameters.AddWithValue("@position",employee.job );
                    command.Parameters.AddWithValue("@salaryType", employee.salaryType);
                    command.Parameters.AddWithValue("@salary", employee.salary);
                    command.Parameters.AddWithValue("@workedHours", employee.workedHours);
                    

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void ReplaceRow(Employee employee, int id, string tableName)
        {

            // Створюмо запит зміни рядка з заданим ідентифікатором

            string replacequery = $"UPDATE {tableName} SET firstName = @firstName, lastName = @lastName, surname = @surname, position = @position, salaryType = @salaryType, salary = @salary, workedHours = @workedHours WHERE id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(replacequery, connection))
                {

                    command.Parameters.AddWithValue("@firstName", employee.firstName);
                    command.Parameters.AddWithValue("@lastName", employee.lastName);
                    command.Parameters.AddWithValue("@surname", employee.Surname);
                    command.Parameters.AddWithValue("@position", employee.job);
                    command.Parameters.AddWithValue("@salaryType", employee.salaryType);
                    command.Parameters.AddWithValue("@salary", employee.salary);
                    command.Parameters.AddWithValue("@workedHours", employee.workedHours);
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void DeleteItem(string tableName, int id, List<Employee> objectList)
        {
            string query = $"DELETE FROM {tableName} WHERE id = @ID"; // створюємо запит видалення рядка з заданим ідентифікатором
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Знаходимо об'єкт за його ідентифікатором
                    Employee objectToDelete = objectList.FirstOrDefault(obj => obj.Id == id);

                    // Видаляємо об'єкт зі списку
                    if (objectToDelete != null)
                    {
                        objectList.Remove(objectToDelete);
                        command.Parameters.AddWithValue("@ID", id);
                        Console.WriteLine($"Об'єкт з ідентифікатором {id} був успішно видалений.");
                    }
                    else
                    {
                        Console.WriteLine($"Об'єкт з ідентифікатором {id} не знайдено.");
                    }
                    
                    
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }


        public void FindWithBiggerSalary(List<Employee> objectList, List<Employee>sortedList, int salary)
        {
            sortedList.Clear();
            foreach (Employee emp in objectList)
            {
                if (emp.AverageSalary()> salary)
                {
                    sortedList.Add(emp); // передаємо в новий список всі елементи основного списку, який задовільняє умову
                }
            }

        }
       
    }
}
