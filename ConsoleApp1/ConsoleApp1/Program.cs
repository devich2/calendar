using System;
using System.Globalization;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
               static void Main(string[] args)

        {

            //Print calendar

            Console.WriteLine("Please enter the year you want to print the calendar:");

            int year = Convert.ToInt32(Console.ReadLine());

            while (year<1900)

            {

                Console.WriteLine("Please enter an integer greater than 1900:");

                year = Convert.ToInt32(Console.ReadLine());

            }
            Console.WriteLine("Please enter the m you want to print the calendar:");

            int mnth = Convert.ToInt32(Console.ReadLine());
            for (int i = mnth; i <=i; i++)

            {

                int days = DateTime.DaysInMonth(year, i);//Get the number of days in the current year/month

                DateTime currday=new DateTime(year,i,1);//The first day of the month

                DateTime lastday = currday.AddMonths(1);//The first day of next month

                Console.WriteLine();

                switch (i)//Judge the month, display different foreground colors

                {

                    case   1:

                        Console.ForegroundColor = ConsoleColor.Red;

                        break;

                    case 2:

                        Console.ForegroundColor = ConsoleColor.Blue;

                        break;

                    case 3:

                        Console.ForegroundColor = ConsoleColor.Cyan;

                        break;

                    case 4:

                        Console.ForegroundColor = ConsoleColor.DarkBlue;

                        break;

                    case 5:

                        Console.ForegroundColor = ConsoleColor.DarkCyan;

                        break;

                    case 6:

                        Console.ForegroundColor = ConsoleColor.DarkGreen;

                        break;

                    case 7:

                        Console.ForegroundColor = ConsoleColor.DarkMagenta;

                        break;

                    case 8:

                        Console.ForegroundColor = ConsoleColor.Yellow;

                        break;

                    case 9:

                        Console.ForegroundColor = ConsoleColor.DarkYellow;

                        break;

                    case 10:

                        Console.ForegroundColor = ConsoleColor.Green;

                        break;

                    case 11:

                        Console.ForegroundColor = ConsoleColor.Magenta;

                        break;

                    case 12:

                        Console.ForegroundColor = ConsoleColor.White;

                        break;

                }

                Console.WriteLine("{0}year{1}month",year,i);

                Console.WriteLine("Sunday\tMonday\tTuesday\tWednesday\tThursday\tFriday\tSaturday");

                while (currday<lastday)

                {

                    int weekday =Convert.ToInt32(currday.DayOfWeek);//Get the day of the week

                    if (currday.Day == 1)//Judge the first day of the month

                    {

                        StringBuilder tmpSpace = new StringBuilder();

                        for (int j = 0; j < weekday; j++)//Output tab

                        {

                            tmpSpace.Append("  \t");

                        }

                        Console.Write("{0} {1}", tmpSpace.ToString(), currday.Day);

                    }

                    else if (currday.DayOfWeek == DayOfWeek.Sunday)//If it's Sunday

                    {

                        Console.Write(Environment.NewLine);

                        Console.Write(" {0}", currday.Day);

                    }

                    else

                    {

                        Console.Write(" \t {0}", currday.Day);

                    }

                    currday = currday.AddDays(1);

                }

            }

            Console.Read();

        }

    }
    
}