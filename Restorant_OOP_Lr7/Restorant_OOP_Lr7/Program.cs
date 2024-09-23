using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Restorant_OOP_Lr7
{
    public class TableReservationApp
    {
        static void Main(string[] args)
        {
            ReservationManager reservationManager = new ReservationManager();
            reservationManager.AddRestaurant("A", 10);
            reservationManager.AddRestaurant("B", 5);


            //Замінити значення
            Console.WriteLine(reservationManager.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
            Console.WriteLine(reservationManager.BookTable("A", new DateTime(2023, 12, 25), 3)); // False

            Console.ReadLine();
        }
    }


    public class ReservationManager
    {

        public List<Restaurant> restaurants;

        public ReservationManager()
        {
            restaurants = new List<Restaurant>();
        }


        public void AddRestaurant(string restaurantName, int tablesCount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(restaurantName) || tablesCount < 0)
                {
                    throw new AddRestaurantException(restaurantName, tablesCount);
                }
                Restaurant restaurant = new Restaurant();
                restaurant.name = restaurantName;
                restaurant.tables = new RestaurantTable[tablesCount];
                for (int i = 0; i < tablesCount; i++)
                {
                    restaurant.tables[i] = new RestaurantTable();
                }
                restaurants.Add(restaurant);
            }
            catch (AddRestaurantException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public void LoadFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new OpenFileExeption(filePath);
                }
                string[] lines = File.ReadAllLines(filePath);
                foreach (string l in lines)
                {
                    var parts = l.Split(',');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                    {
                        AddRestaurant(parts[0], tableCount);
                    }
                    else
                    {
                        Console.WriteLine(l);
                    }
                }
            }
            catch (OpenFileExeption ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public List<string> FindAllFreeTables(DateTime date)
        {
            try
            {
                if (date == DateTime.MinValue)
                {
                    throw new DateException();
                }

                List<string> restaurantFree = new List<string>();
                foreach (var restaurant in restaurants)
                {
                    for (int i = 0; i < restaurant.tables.Length; i++)
                    {
                        if (!restaurant.tables[i].IsBooked(date))
                        {
                            restaurantFree.Add($"{restaurant.name} - Table {i + 1}");
                        }
                    }
                }
                return restaurantFree;
            }
            catch (DateException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return new List<string>();
        }

        public bool BookTable(string restaurantName, DateTime date, int tableNumber)
        {
            try
            {
                foreach (var restaurant in restaurants)
                {
                    if (restaurant.name == restaurantName)
                    {
                        if (tableNumber < 0 || tableNumber >= restaurant.tables.Length)
                        {
                            throw new BookTableException(restaurant.tables.Length, tableNumber); //Invalid table number
                        }

                        return restaurant.tables[tableNumber].Book(date);
                    }
                }
            }
            catch (BookTableException ex)
            {
                Console.WriteLine(ex.ToString());
            }


            return false; //Restaurant not found
        }

        public void SortByAvailability(DateTime date)
        {
            try
            {
                if (restaurants == null|| restaurants.Count == 0)
                {
                    throw new SortException();
                }
                restaurants = new List<Restaurant>(MergeSort(restaurants.ToArray(), date));
            }
            catch(SortException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public Restaurant[] MergeSort(Restaurant[] restaurantArray, DateTime date)
        {
            if (restaurantArray.Length == 1)
            {
                return restaurantArray;
            }

            Restaurant[] leftArray = new Restaurant[restaurantArray.Length / 2];
            Restaurant[] rightArray = new Restaurant[restaurantArray.Length - (restaurantArray.Length / 2)];

            for (int i = 0; i < restaurantArray.Length; i++)
            {
                if (i < restaurantArray.Length / 2)
                {
                    leftArray[i] = restaurantArray[i];
                }
                else
                {
                    rightArray[i - (restaurantArray.Length / 2)] = restaurantArray[i];
                }
            }

            leftArray = MergeSort(leftArray, date);
            rightArray = MergeSort(rightArray, date);

            return Merge(leftArray, rightArray, date);
        }

        public static Restaurant[] Merge(Restaurant[] leftArray, Restaurant[] rightArray, DateTime date)
        {
            Restaurant[] mergedArray = new Restaurant[0];
            while (leftArray.Length > 0 && rightArray.Length > 0)
            {

                if (CountAvailableTables(leftArray[0], date) >= CountAvailableTables(rightArray[0], date))
                {
                    mergedArray = mergedArray.Append<Restaurant>(leftArray[0]).ToArray();
                    leftArray = leftArray.Skip(1).ToArray();
                }
                else
                {
                    mergedArray = mergedArray.Append<Restaurant>(rightArray[0]).ToArray();
                    rightArray = rightArray.Skip(1).ToArray();
                }
            }
            while (leftArray.Length > 0 || rightArray.Length > 0)
            {
                if (leftArray.Length > 0)
                {
                    mergedArray = mergedArray.Append<Restaurant>(leftArray[0]).ToArray();
                    leftArray = leftArray.Skip(1).ToArray();
                }
                else if (rightArray.Length > 0)
                {
                    mergedArray = mergedArray.Append<Restaurant>(rightArray[0]).ToArray();
                    rightArray = rightArray.Skip(1).ToArray();
                }
            }

            return mergedArray;
        }

        public static int CountAvailableTables(Restaurant restaurant, DateTime date)
        {
            try
            {
                if (restaurant == null || date == DateTime.MinValue)
                {
                    throw new TablesAvailabilityException(restaurant, date);
                }
                int count = 0;
                foreach (var tables in restaurant.tables)
                {
                    if (!tables.IsBooked(date))
                    {
                        count++;
                    }
                }
                return count;
            }
            catch (TablesAvailabilityException ex)
            {
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }
    }

    public class Restaurant
    {
        public string name;
        public RestaurantTable[] tables;
    }

    public class RestaurantTable
    {
        private List<DateTime> bookDates;

        public RestaurantTable()
        {
            bookDates = new List<DateTime>();
        }

        public bool Book(DateTime date)
        {
            try
            {
                if (date == DateTime.MinValue)
                {
                    throw new DateException();
                }

                if (bookDates.Contains(date))
                {
                    return false;
                }
                bookDates.Add(date);
                return true;
            }
            catch (DateException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool IsBooked(DateTime date)
        {
            return bookDates.Contains(date);
        }
    }
}
