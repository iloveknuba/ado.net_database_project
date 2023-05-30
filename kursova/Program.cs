
using chyisKURSACH;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Text;

// Задаєм умову для відображення українських символів та встановлюємо роздільність консолі

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WindowWidth = 150;
Console.WindowHeight = 44;

string serverName = "andbut";
string databaseName = "wokers";
string tableName = "work_db";




// Об'являємо рядок для координації в програмі

string input = "";


// Об'являємо списки людей з фіксованою та погодинною зарплатою
var fxdEmployees = new List<FixedSalaryEmployee>();
var byhourEmployees = new List<SalaryByHourEmployee>();


// Об'являємо списки для поєднання та сортування попередніх списків
var checkInputList = new List<Employee>();
var employees = new List<Employee>();


// Добавляємо рядок підключення до SQL бази даних та створюємо об'єкт користувацького класу DataBase
string connectionString = $"Data Source={serverName};Initial Catalog={databaseName};Encrypt=False;Integrated Security=true;";
DataBase database = new DataBase(connectionString);


// Генеруємо таблицю в консолі
refreshTable1();




// Координуємо функції 
while (input != "6")
{

    Console.WriteLine("Введіть 1, щоб добавити елемент,\n2 - щоб видалити елемент,\n3 - щоб відкрити таблицю,\n4 - знайти працівників по оплаті,\n5 - знайти людей з більшою зарплатою за задане,\n6 - щоб закрити програму");
    input = Console.ReadLine();
    if (input == "1")
    {
        Console.WriteLine("Якщо добавити - 1, якщо відредагувати - 2");
        string addinput = Console.ReadLine();
        if (addinput == "1")
        {
            // Добавляємо новий елемент в список 
            addTolist1();
            database.AddNewRow(employees[employees.Count - 1], tableName);

            Console.WriteLine("Рядок добавлено \n ");
        }
        else if(addinput == "2")
        {
           
            Console.WriteLine("Введіть id рядка");
            addinput = Console.ReadLine();

            // Змінюємо дані існуючого рядка за його ідентифікатором, а саме addinput

            addTolist1();
            database.ReplaceRow(employees[employees.Count-1], int.Parse(addinput), tableName);
        }
       

    }
    else if (input == "2")
    {
        try
        {
            Console.WriteLine("Виберіть рядок для видалення");
            string id = Console.ReadLine();
            
            // Реалізуємо метод, а саме видаляємо рядок за його ідентифікатором, введеним в консоль зі змінною id

            database.DeleteItem("work_db", int.Parse(id), employees);
        }
        catch(Exception ex)
        {
            // Перевіряємо чи введено ідентифікатор існуючого рядка

            Console.WriteLine (ex.Message);
        }

    }
    else if (input == "3")
    {
      // Регенеруємо дані в консоль

        refreshTable1();

    }
    else if (input == "4")
    {
        // Створюємо об'єкт класу сортування

        SortBySalary sa = new SortBySalary();
        string salaryTypeInput= "";
    

        Console.WriteLine("введіть тип зарплати(fixed/byhour), або знайти доцентів (Доцент)");
        salaryTypeInput = Console.ReadLine();
        // Перевіряємо, чи користувач вибрав вивдененя списку людей з фіксованою або з погодинною зарлатою
        if (salaryTypeInput == "fixed")
        {
            fxdEmployees.Sort(sa); // сортуємо список
            database.resfreshList(fxdEmployees); // виводимо відстортований список в консоль


        }
        
        else if (salaryTypeInput == "byhour")
        {
            byhourEmployees.Sort(sa);
            database.resfreshList(byhourEmployees);
        }
        // Перевіряємо чи вирішив користувач вивести список з доцентами
        else if (salaryTypeInput == "Доцент")
        {
            checkInputList.Clear(); // очищаємо список якщо він вже тримав в памяті дані
            foreach(var emp in employees)
            {
                if(emp.job == salaryTypeInput)
                {
                    checkInputList.Add(emp); // перебираєчи кожен елемент з головного спику добавляємо в новий
                    
                }
            }
            database.resfreshList(checkInputList); // виводмио на екран новий список
        }
        

    }
    else if( input == "5")
    {
        Console.WriteLine("Введіть зарплату");
        string salaryInput = Console.ReadLine();

        database.FindWithBiggerSalary(employees,checkInputList, int.Parse(salaryInput)); // реалізуємо метод класа передачі в новий список
        SortBySalary sa = new SortBySalary();
        checkInputList.Sort(sa);
        database.resfreshList(checkInputList); // виводмио на екран цей відсортований список
    }
    
   

}

void refreshTable1()
{
    database.TableToObjectList("work_db", fxdEmployees, byhourEmployees);
    employees = fxdEmployees.Cast<Employee>().Concat(byhourEmployees.Cast<Employee>()).ToList();
    SortBySalary sa = new SortBySalary();
    employees.Sort(sa);
    database.resfreshList(employees);

}

void addTolist1()
{
    string[] inputParameters = new string[] { "Ім'я", "Прізвище", "По-батькові", "Посаду", "Тип зарплати(fixed/byhour)", "Зарплату", "Відроблені години" }; // ініціалізуємо вхідні дані

    for (int i = 0; i < inputParameters.Length; i++)
    {
        Console.WriteLine("Введіть {0}:", inputParameters[i]);
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input)) // перевірка на входження пустого рядка
        {
            Console.WriteLine("Помилка! Введене значення є порожнім або містить тільки пробіли. Спробуйте ще раз.");
            i--; // повторити цикл для тієї ж вхідної змінної
            continue;
        }

        inputParameters[i] = input; // перевизначаємо дані
    }

    employees.Add(new Employee()
    {
        firstName = inputParameters[0],
        lastName = inputParameters[1],
        job = inputParameters[3],
        Surname = inputParameters[2],
        salaryType = inputParameters[4],
        salary = inputParameters[5],
        workedHours = inputParameters[6]
    });

}






