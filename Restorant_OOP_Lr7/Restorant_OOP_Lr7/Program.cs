using System;
using System.Collections.Generic;
using System.IO;


public class TableReservationApp
{
    static void Main(string[] args)
    {
        ReservationManager m = new ReservationManager();
        m.AddRestaurant("A", 10);
        m.AddRestaurant("B", 5);

        Console.WriteLine(m.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(m.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
    }
}


public class ReservationManager
{

    public List<Restaurant> restaurants;

    public ReservationManager()
    {
        restaurants = new List<Restaurant>();
    }


    public void AddRestaurant(string n, int t)
    {
        try
        {
            Restaurant restaurant = new Restaurant();
            restaurant.name = n;
            restaurant.tables = new RestaurantTable[t];
            for (int i = 0; i < t; i++)
            {
                restaurant.tables[i] = new RestaurantTable();
            }
            restaurants.Add(restaurant);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }


    private void LoadFromFile(string fileP)
    {
        try
        {
            string[] ls = File.ReadAllLines(fileP);
            foreach (string l in ls)
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
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }


    public List<string> FindAllFreeTables(DateTime date)
    {
        try
        { 
            List<string> free = new List<string>();
            foreach (var restaurant in restaurants)
            {
                for (int i = 0; i < restaurant.tables.Length; i++)
                {
                    if (!restaurant.tables[i].IsBooked(date))
                    {
                        free.Add($"{restaurant.name} - Table {i + 1}");
                    }
                }
            }
            return free;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return new List<string>();
        }
    }

    public bool BookTable(string restaurantName, DateTime date, int tableNumber)
    {
        foreach (var restaurant in restaurants)
        {
            if (restaurant.name == restaurantName)
            {
                if (tableNumber < 0 || tableNumber >= restaurant.tables.Length)
                {
                    throw new Exception(null); //Invalid table number
                }

                return restaurant.tables[tableNumber].Book(date);
            }
        }

        throw new Exception(null); //Restaurant not found
    }

    public void SortByAvailability(DateTime date)
    {
        try
        { 
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < restaurants.Count - 1; i++)
                {
                    int avTc = CountAvailableTables(restaurants[i], date);
                    int avTn = CountAvailableTables(restaurants[i + 1], date); 

                    if (avTc < avTn)
                    {
                        var temp = restaurants[i];
                        restaurants[i] = restaurants[i + 1];
                        restaurants[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }
    public int CountAvailableTables(Restaurant restaurant, DateTime date)
    {
        try
        {
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
        catch (Exception ex)
        {
            Console.WriteLine("Error");
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
            if (bookDates.Contains(date))
            {
                return false;
            }
            bookDates.Add(date);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return false;
        }
    }

    public bool IsBooked(DateTime date)
    {
        return bookDates.Contains(date);
    }
}
