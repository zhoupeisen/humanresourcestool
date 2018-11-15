using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace project_final
{
    class Program
    {
        public static void Main(string[] args)
        {
            List<DataCell> ppl = new List<DataCell>();
            string nameInput = "";
            int i = 0;
            double x;
            ConsoleKeyInfo info;
            //populate my people(ppl) list
            ppl = sqlPopulate();
            //infinite loop instead of sentinel loop with breaks built in
            while(true)
            {
                try
                {
                    Console.WriteLine("***Blank instances of this application require you press enter/return to proceed.***");
                    Console.WriteLine("***There in no back feature***");
                    Console.WriteLine("input Full Name: ");
                    nameInput = Console.ReadLine();
                    nameInput = nameInput.ToLower();

                    //check my peoples names to see if it has the name from sqlPopulate
                    while(!nameInput.Equals(ppl[i].fName.ToLower() + " " + ppl[i].lName.ToLower()))
                    {
                        i++;
                    }
                }
                catch(Exception)
                {
                    Console.WriteLine("***name not found***");
                    Console.WriteLine("to see employee list, input: 'p'");
                    Console.WriteLine("otherwise enter/return will exit.");
                    info = Console.ReadKey();

                    if(info.KeyChar == 'p')
                    {
                        everyone(ppl);
                        Console.WriteLine("input 'r' to repeat, any other key exits.");
                        info = Console.ReadKey();
                        if(info.KeyChar == 'r')
                        {
                            goto Rep;
                        }
                    }
                    Environment.Exit(0);
                }
                x = ppl[i].vacationHours*(double)ppl[i].payRate;
                x = Math.Round(x, 2);
                Console.WriteLine("{0} has ${1} available via PTO Exchange.", ppl[i].fName, x);
                Console.WriteLine("if you want to modify {0} input: 'y'", ppl[i].fName);
                if(Console.ReadLine().Equals("y"))
                {
                    ppl[i] = modify(ppl[i]);
                }
                //
                Rep:
                    nameInput = "";
                //just placeholder code for Rep label to exist
            }
        }
        //print all the people via to string
        public static void everyone(List<DataCell> people)
        {
            foreach (var person in people) 
            {
                Console.WriteLine(person.ToString());
                Console.WriteLine();
            }
        }
        //user can modify the people within the database
        public static DataCell modify(DataCell person)
        {
            int i = 0;
            string n;
            decimal number;

            Console.WriteLine("input '1' to change first name, input '2' to change last name.");
            Console.WriteLine("input '3' to change job title, input '4' to change birth date.");
            Console.WriteLine("input '5' to change hire date, input '6' to change marital status.");
            Console.WriteLine("input '7' to change pay rate, input '8' to change vacation hours.");
            Console.WriteLine("input '9' to change sick leave hours.");

            try
            {
                i = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
            switch(i)
            {
                case 1:
                    Console.WriteLine("Input first name:");
                    n = person.fName;
                    person.fName = Console.ReadLine();
                    sqlUpdate(person, n, 1);
                    return person;
                case 2:
                    Console.WriteLine("Input last name:");
                    n = person.lName;
                    person.lName = Console.ReadLine();
                    sqlUpdate(person, n, 2);
                    return person;
                case 3:
                    Console.WriteLine("Input Job Title:");
                    n = Console.ReadLine();
                    sqlUpdate(person, n, 3);
                    return person;
                case 4:
                    Console.WriteLine("Input Birthdate (0000-00-00):");
                    n = Console.ReadLine();
                    person.birthDate = Convert.ToDateTime(n);
                    sqlUpdate(person, n, 4);
                    return person;
                case 5:
                    Console.WriteLine("Input Hire date:");
                    n = Console.ReadLine();
                    person.hireDate = Convert.ToDateTime(n);
                    sqlUpdate(person, n, 5);
                    return person;
                case 6:
                    Console.WriteLine("Input Marital status (S,M):");
                    n = Console.ReadLine();
                    person.maritalStatus = Convert.ToChar(n);
                    sqlUpdate(person, n, 6);
                    return person;
                case 7:
                    Console.WriteLine("Input pay rate:");
                    n = Console.ReadLine();
                    try
                    {
                        if (Decimal.TryParse(n, out number))
                        {
                            Console.WriteLine(number);
                            sqlUpdate(person, n, 7);
                        }
                    }
                    catch(FormatException)
                    {
                        Console.WriteLine("***error***");
                    }
                    return person;
                case 8:
                    Console.WriteLine("Input vacation time:");
                    n = Console.ReadLine();
                    person.vacationHours = Convert.ToInt32(n);
                    sqlUpdate(person, n, 8);
                    return person;
                case 9:
                    Console.WriteLine("Input sick leace time:");
                    n = Console.ReadLine();
                    person.sickleaveHours = Convert.ToInt32(n);
                    sqlUpdate(person, n, 9);
                    return person;
                default:
                    return person;
            }
        }
        public static List<DataCell> sqlPopulate()
        {
            string connectionString =  "Data Source=(local);Initial Catalog=AdventureWorks;"
            + "Integrated Security=true";
            string queryString = "SELECT FirstName, LastName, JobTitle, BirthDate, MaritalStatus, Rate, HireDate, VacationHours, SickLeaveHours, HumanResources.Employee.BusinessEntityID FROM HumanResources.Employee INNER JOIN Person.Person ON HumanResources.Employee.BusinessEntityID = Person.Person.BusinessEntityID INNER JOIN HumanResources.EmployeePayHistory ON HumanResources.Employee.BusinessEntityID = HumanResources.EmployeePayHistory.BusinessEntityID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<DataCell> ppl = new List<DataCell>();
                try
                {
                    while (reader.Read())
                    {
                        DataCell a = new DataCell(reader["FirstName"].ToString(), reader["LastName"].ToString(), 
                            Convert.ToString(reader["JobTitle"]), Convert.ToDateTime(reader["BirthDate"]),
                            Convert.ToChar(reader["MaritalStatus"]), Convert.ToDecimal(reader["Rate"]),
                            Convert.ToDateTime(reader["HireDate"]), Convert.ToInt32(reader["VacationHours"]), 
                            Convert.ToInt32(reader["SickLeaveHours"]), Convert.ToInt32(reader["BusinessEntityID"]));
                        ppl.Add(a);
                    }
                }
                finally
                {
                    reader.Close();
                }
                return ppl;
            }
        }
        //update sql database method
        public static void sqlUpdate(DataCell x, string old, int num)
        {
            string queryString="";

            if(num == 1 ){queryString = "UPDATE Person.Person SET FirstName = '" + x.fName + "' WHERE FirstName = '" + old + "' AND LastName = '" + x.lName + "'";}
            else if(num == 2 ){queryString = "UPDATE Person.Person SET LastName = '" + x.lName + "' WHERE LastName = '" + old + "' AND FirstName = '" + x.fName + "'";}
            else if(num == 3 ){queryString = "UPDATE HumanResources.Employee SET JobTitle = '" + old + "' WHERE HumanResources.Employee.BusinessEntityID = " + x.businessID;}
            else if(num == 4 ){queryString = "UPDATE HumanResources.Employee SET BirthDate = '" + old + "' WHERE HumanResources.Employee.BusinessEntityID = " + x.businessID;}
            else if(num == 5 ){queryString = "UPDATE HumanResources.Employee SET HireDate = '" + old + "' WHERE HumanResources.Employee.BusinessEntityID = " + x.businessID;}
            else if(num == 6 ){queryString = "UPDATE HumanResources.Employee SET MaritalStatus = '" + x.maritalStatus + "' WHERE HumanResources.Employee.BusinessEntityID = " + x.businessID;}
            else if(num == 7 ){queryString = "UPDATE HumanResources.EmployeePayHistory SET Rate = " + x.payRate + " WHERE HumanResources.EmployeePayHistory.BusinessEntityID = " + x.businessID;}
            else if(num == 8 ){queryString = "UPDATE HumanResources.Employee SET VacationHours = '" + x.vacationHours + "' WHERE HumanResources.Employee.BusinessEntityID = " + x.businessID;}
            else if(num == 9 ){queryString = "UPDATE HumanResources.Employee SET SickLeaveHours = '" + x.sickleaveHours + "' WHERE HumanResources.Employee.BusinessEntityID = " + x.businessID;}

            string connectionString =  "Data Source=(local);Initial Catalog=AdventureWorks; Integrated Security=true";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
    //my class for the datacell objects. i was thinking of these as cells in a
    //spreadsheet or a table with cells in html
    class DataCell
    {
        public string fName;
        public string lName;
        public string jobTitle;
        public DateTime birthDate, hireDate;
        public char maritalStatus;
        public decimal payRate;
        public int vacationHours, sickleaveHours, businessID;

        public DataCell(string f, string l, string jt, DateTime bd, char ms, decimal rate, DateTime hd, int vh, int slh, int bid)
        {
            fName = f;
            lName = l;
            jobTitle = jt;
            birthDate = bd;
            maritalStatus = ms;
            payRate = rate;
            hireDate = hd;
            vacationHours = vh;
            sickleaveHours = slh;
            businessID = bid;
        }

        public override string ToString()
        {
            return "First name: " + fName + "\n " + "Last name: " + lName + "\n " + "Job title: " 
            + jobTitle + "\n " + "Birthdate: "+  birthDate + "\n " + "Marital status: " + maritalStatus + "\n " 
            + "Payrate: " + payRate + "\n " + "Date hired: " + hireDate + "\n " + "Vacation hours: " + vacationHours 
            + "\n " + "Sick leave hours: " + sickleaveHours + "\n" + "ID: " + businessID;
        }
    }
}