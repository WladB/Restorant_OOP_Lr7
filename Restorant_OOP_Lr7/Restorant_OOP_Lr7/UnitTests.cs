using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using NUnit.Framework.Internal;

namespace Restorant_OOP_Lr7
{
    [TestFixture]
    public class ReservationManagerTests
    {
        [Test]
        public void AddRestaurant_CorrectWorking()
        {
            // Arrange
            var manager = new ReservationManager();

            // Act
            manager.AddRestaurant("A", 5);

            // ClassicAssert
            ClassicAssert.AreEqual(1, manager.restaurants.Count);
            ClassicAssert.AreEqual("A", manager.restaurants[0].name);
            ClassicAssert.AreEqual(5, manager.restaurants[0].tables.Length);  
        }

        [Test]
        public void LoadFromFile_CorrectWorking()
        {
            // Arrange
            var manager = new ReservationManager();

            // Act
            manager.LoadFromFile("load.txt");

            // ClassicAssert
            ClassicAssert.AreEqual(3, manager.restaurants.Count);
        }

        [Test]
        public void LoadFromFile_BadPath()
        {
            // Arrange
            var manager = new ReservationManager();

            // Act
            manager.LoadFromFile(" ");

            // ClassicAssert
            ClassicAssert.AreEqual(0, manager.restaurants.Count);

        }
        [Test]
        public void FindAllFree_CorrectWorking()
        {
            // Arrange
            var manager = new ReservationManager();

            // Act
            manager.AddRestaurant("A", 1);
            manager.AddRestaurant("B", 1);
            manager.BookTable("A", new DateTime(2023, 12, 25), 0);
            manager.BookTable("B", new DateTime(2023, 12, 25), 0);


            // ClassicAssert
            ClassicAssert.AreEqual(0, manager.FindAllFreeTables(new DateTime(2023, 12, 25)).Count);
            ClassicAssert.AreNotEqual(2, manager.FindAllFreeTables(new DateTime(2023, 12, 25)).Count);
       
        }

        [Test]
        public void BookTable_CorrectWorking()
        {
            // Arrange
            var manager = new ReservationManager();

            // Act
            manager.AddRestaurant("A", 1);
            manager.AddRestaurant("B", 1);
    
            // ClassicAssert
            ClassicAssert.IsFalse(manager.BookTable("A", new DateTime(2023, 12, 25), -1));
            ClassicAssert.IsFalse(manager.BookTable("A", new DateTime(2023, 12, 25), 5));
            ClassicAssert.IsTrue(manager.BookTable("A", new DateTime(2023, 12, 25), 0));

            ClassicAssert.IsFalse(manager.BookTable("B", new DateTime(2023, 12, 25), -1));
            ClassicAssert.IsFalse(manager.BookTable("B", new DateTime(2023, 12, 25), 5));
            ClassicAssert.IsTrue(manager.BookTable("B", new DateTime(2023, 12, 25), 0));
        }

        [Test]
        public void SortByAvailability_CorrectWorking()
        {
            // Arrange
            var manager = new ReservationManager();

            // Act
            manager.AddRestaurant("A", 1);
            manager.AddRestaurant("B", 10);
            manager.AddRestaurant("D", 30);
            manager.AddRestaurant("C", 15);

            manager.SortByAvailability(new DateTime(2023, 12, 25));

            // ClassicAssert

            ClassicAssert.AreEqual("D", manager.restaurants[0].name);
            ClassicAssert.AreEqual("C", manager.restaurants[1].name);
            ClassicAssert.AreEqual("B", manager.restaurants[2].name);
            ClassicAssert.AreEqual("A", manager.restaurants[3].name);
        }

        [Test]
        public void CountAvailableTables_CorrectWorking()
        {
            // Arrange
            var manager = new ReservationManager();
            var date = new DateTime(2023, 12, 25);

            // Act
            manager.AddRestaurant("A", 1);
            manager.AddRestaurant("B", 2);
            manager.BookTable("A", date, 0);
            manager.BookTable("B", date, 0);


            // ClassicAssert
            ClassicAssert.AreEqual(0, manager.CountAvailableTables(manager.restaurants[0], date));
            ClassicAssert.AreEqual(1, manager.CountAvailableTables(manager.restaurants[1], date));

        }

       
        [Test]
        public void MergeSort_CorrectWorking()
        {
            // Arrange
            var date = new DateTime(2023, 12, 25);
            ReservationManager manager = new ReservationManager();

            // Act
            manager.AddRestaurant("A", 5);
            manager.AddRestaurant("B", 6);
            manager.AddRestaurant("C", 10);
            manager.AddRestaurant("D", 7);
            manager.BookTable("A", date, 0);
            manager.BookTable("B", date, 0);
            Restaurant[] mergedArray = manager.MergeSort(manager.restaurants.ToArray(), date);

             //ClassicAssert
             ClassicAssert.AreEqual(4, mergedArray.Length);
             ClassicAssert.AreEqual("C", mergedArray[0].name);
             ClassicAssert.AreEqual("D", mergedArray[1].name);
             ClassicAssert.AreEqual("B", mergedArray[2].name);
             ClassicAssert.AreEqual("A", mergedArray[3].name);
        }
        
        [Test]
        public void MergeSortTest_InvalidValue()
        {
            // Arrange
            var date = new DateTime(2023, 12, 25);
            ReservationManager manager = new ReservationManager();

            // Act
            manager.AddRestaurant("A", 5);
            manager.AddRestaurant("B", 6);
            manager.AddRestaurant("C", 10);
            manager.AddRestaurant("D", -1);
            manager.BookTable("A", date, 0);
            manager.BookTable("B", date, 0);
            Restaurant[] mergedArray = manager.MergeSort(manager.restaurants.ToArray(), date);

            //ClassicAssert
            ClassicAssert.AreEqual(3, mergedArray.Length);
            ClassicAssert.AreEqual("C", mergedArray[0].name);
            ClassicAssert.AreEqual("B", mergedArray[1].name);
            ClassicAssert.AreEqual("A", mergedArray[2].name);
        }

        [Test]
        public void MergeSort_EmptyRestorants()
        {
            // Arrange
            var date = new DateTime(2023, 12, 25);
            ReservationManager manager = new ReservationManager();

            // Act
            manager.AddRestaurant("A", 0);
            manager.AddRestaurant("B", 0);
            manager.AddRestaurant("C", 0);
            manager.AddRestaurant("D", 0);

            Restaurant[] mergedArray = manager.MergeSort(manager.restaurants.ToArray(), date);

            //ClassicAssert
            ClassicAssert.AreEqual(4, mergedArray.Length);
            ClassicAssert.AreEqual("A", mergedArray[0].name);
            ClassicAssert.AreEqual("B", mergedArray[1].name);
            ClassicAssert.AreEqual("C", mergedArray[2].name);
            ClassicAssert.AreEqual("D", mergedArray[3].name);
        }

        [Test]
        public void MergeSort_NoRestorants()
        {
            // Arrange
            var date = new DateTime(2023, 12, 25);
            ReservationManager manager = new ReservationManager();

            // Act
            Restaurant[] mergedArray = manager.MergeSort(manager.restaurants.ToArray(), date);

            //ClassicAssert
            ClassicAssert.AreEqual(0, mergedArray.Length);
        }
        [Test]
        public void MergeTest()
        {
            // Arrange
            var date = new DateTime(2023, 12, 25);
            var leftArray = new Restaurant[2];
            var rightArray = new Restaurant[2];
            var manager = new ReservationManager();

            // Act
            manager.AddRestaurant("A", 5);
            manager.AddRestaurant("B", 10);
            
            leftArray = manager.restaurants.GetRange(0, 1).ToArray();
            rightArray = manager.restaurants.GetRange(1, 1).ToArray();
            Restaurant[] mergedArray = manager.Merge(leftArray, rightArray, date);

            // ClassicAssert
            ClassicAssert.AreEqual(2, mergedArray.Length);
            ClassicAssert.AreEqual("B", mergedArray[0].name);
            ClassicAssert.AreEqual("A", mergedArray[1].name);
        }
        
        [Test]
        public void MergeTest_LeftArray()
        {
            // Arrange
            var date = new DateTime(2023, 12, 25);
            var leftArray = new Restaurant[1];
            var rightArray = new Restaurant[0];
            var manager = new ReservationManager();

            // Act
            manager.AddRestaurant("A", 5);

            leftArray = manager.restaurants.GetRange(0, 1).ToArray();
            Restaurant[] mergedArray = manager.Merge(leftArray, rightArray, date);

            // ClassicAssert
            ClassicAssert.AreEqual(1, mergedArray.Length);
            ClassicAssert.AreEqual("A", mergedArray[0].name);
        }
        
        [Test]
        public void MergeTest_RightArray()
        {
            // Arrange
            var date = new DateTime(2023, 12, 25);
            var leftArray = new Restaurant[0];
            var rightArray = new Restaurant[1];
            var manager = new ReservationManager();

            // Act
            manager.AddRestaurant("B", 10);

            rightArray = manager.restaurants.GetRange(0, 1).ToArray();
            Restaurant[] mergedArray = manager.Merge(leftArray, rightArray, date);

            // ClassicAssert
            ClassicAssert.AreEqual(1, mergedArray.Length);
            ClassicAssert.AreEqual("B", mergedArray[0].name);
        }
       
        [Test]
        public void MergeTest_EmptyArrays()
        {
            // Arrange
            var date = new DateTime(2023, 12, 25);
            var leftArray = new Restaurant[0];
            var rightArray = new Restaurant[0];
            var manager = new ReservationManager();

            // Act
            Restaurant[] mergedArray = manager.Merge(leftArray, rightArray, date);

            // ClassicAssert
            ClassicAssert.AreEqual(0, mergedArray.Length);
          
        }

    }
    [TestFixture]
    public class RestaurantTableTests
    {
        [Test]
        public void Book_CorrectWorking()
        {
            // Arrange
            var firstTable = new RestaurantTable();
            var secondTable = new RestaurantTable();
           
            // Act
            firstTable.Book(new DateTime(2023, 12, 25));
            secondTable.Book(new DateTime(2023, 12, 26));


            // ClassicAssert
            ClassicAssert.IsTrue(firstTable.IsBooked(new DateTime(2023, 12, 25)));
            ClassicAssert.IsFalse(secondTable.IsBooked(new DateTime(2023, 12, 25)));

        }

        [Test]
        public void IsBook_CorrectWorking()
        {
            // Arrange
            var firstTable = new RestaurantTable();
            var secondTable = new RestaurantTable();

            // Act
            firstTable.Book(new DateTime(2023, 12, 25));
            secondTable.Book(new DateTime(2023, 12, 26));
            bool bookedTrue = firstTable.IsBooked(new DateTime(2023, 12, 25));
            bool bookedFalse = secondTable.IsBooked(new DateTime(2023, 12, 25));

            // ClassicAssert
            ClassicAssert.IsTrue(bookedTrue);
            ClassicAssert.IsFalse(bookedFalse);

        }
    }
 }
