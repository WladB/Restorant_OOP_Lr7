using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restorant_OOP_Lr7
{
    public class CustomException : Exception
    {
        protected const string logHeader = "\n-----------------------------LOG-----------------------------";
        protected const string logFooter = "\n-------------------------------------------------------------\n";

        public CustomException() { }
        protected string FormatErrorMessage(string message)
        {
            return $"{logHeader}\n{message}{logFooter}";
        }
    }
    public class OpenFileExeption : CustomException
    {
        public string filePath { get; }
        public OpenFileExeption(string path)
        {
            filePath = path;
        }
        public override string ToString()
        {

            return FormatErrorMessage($"Шлях '{filePath}' до файлу є некоректним, невдалося зчитати файл");
        }
    }

    public class AddRestaurantException : CustomException
    {
        public string restaurantCaption { get; }
        public int countTable { get; }

        public AddRestaurantException(string restaurantName, int tablesCount)
        {
            restaurantCaption = restaurantName;
            countTable = tablesCount;
        }
        public override string ToString()
        {
            string errorMessage = "При додаваннi ресторану до списку виникли такi помилки:";
            if (string.IsNullOrWhiteSpace(restaurantCaption))
            {
                errorMessage += $"\n - Змiнна з назвою ресторану мiстить порожнiй рядок або їй не присвоєно значення (назва ресторану: '{restaurantCaption}');";
            }
            if (countTable < 0)
            {
                errorMessage += $"\n - Змiнна tablesCount меньше 0;";
            }
            return FormatErrorMessage(errorMessage);
        }
    }

    public class DateException : CustomException
    {
        public override string ToString()
        {
            return FormatErrorMessage("- Змiнна типу DateTime вказана некоректно");
        }
    }

    public class BookTableException : CustomException
    {
        public int countAllTables { get; }
        public int bookTableNumber { get; }

        public BookTableException(int countTables, int tableNumber)
        {
            countAllTables = countTables;
            bookTableNumber = tableNumber;
        }
        public override string ToString()
        {
            string errorMessage = "При бронюваннi столика виникли такi помилки:";

            if (bookTableNumber < 0)
            {
                errorMessage += $"\n - Номер броньованого столика меньше за 0;";
            }
            if (bookTableNumber >= countAllTables)
            {
                errorMessage += $"\n - Номер броньованого столика бiльший за кiлькiсть всiх наявних столикiв;";
            }

            return FormatErrorMessage(errorMessage);
        }
    }

    public class TablesAvailabilityException : CustomException
    {
        public Restaurant selectedRestaurant { get; }
        public DateTime bookDate { get; }
        public TablesAvailabilityException(Restaurant restaurant, DateTime date)
        {
            selectedRestaurant = restaurant;
            bookDate = date;
        }
        public override string ToString()
        {
            string errorMesage = "При пiдрахунку кiлькостi столикiв виникли такi помилки:";
            if (selectedRestaurant == null)
            {
                errorMesage += "\n- Змiнну типу Restaurant не iнiцiалiзовано";
            }
            if (bookDate == DateTime.MinValue)
            {
                errorMesage += "\n- Змiнна типу DateTime вказана некоректно";
            }
            return FormatErrorMessage(errorMesage);
        }
    }
    public class SortException : CustomException
    {
        public override string ToString()
        {
            return FormatErrorMessage("- Неможливо запустити сортування, List порожнiй або не iнiцiалiзований");
        }
    }
}
