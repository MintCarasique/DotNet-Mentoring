// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
    [Title("LINQ Module")]
    [Prefix("Linq")]
    public class LinqSamples : SampleHarness
    {

        private DataSource dataSource = new DataSource();

        [Category("Tasks")]
        [Title("Task 1")]
        [Description("List of all customers whose total turnover (the sum of all orders) exceeds a certain value X")]
        public void Linq1()
        {
            var x = 15000;

            var clients = dataSource.Customers.Where(_ => _.Orders.Sum(o => o.Total) >= x).Select(_ => new
            {
                _.CustomerID,
                TotalOrdersSum = _.Orders.Sum(order => order.Total)
            });

            foreach (var client in clients)
            {
                ObjectDumper.Write(client);
            }
        }

        [Category("Tasks")]
        [Title("Task 2")]
        [Description("List of suppliers located in the same country and the same city")]
        public void Linq2()
        {
            var customersAndSuppliers = dataSource.Customers.GroupJoin(
                dataSource.Suppliers,
                c => new { c.City, c.Country },
                s => new { s.City, s.Country },
                (customer, suppliers) => new
                {
                    customer.CustomerID,
                    customer.City,
                    customer.Country,
                    Suppliers = suppliers.Select(s => new { s.SupplierName, s.City, s.Country })
                });
            foreach (var customer in customersAndSuppliers)
            {
                ObjectDumper.Write(customer, 2);
                Console.WriteLine(@"---------------------------------------------------------");
            }

            Console.WriteLine("WITHOUT GROUPING");

            var customersAndSuppliers2 = dataSource.Customers.Select(
                customer => new
                {
                    customer.CustomerID,
                    customer.City,
                    customer.Country,
                    Suppliers = dataSource.Suppliers
                        .Where(supplier =>
                            supplier.City.Equals(customer.City, StringComparison.OrdinalIgnoreCase)
                            && supplier.Country.Equals(customer.Country, StringComparison.OrdinalIgnoreCase))
                        .Select(supplier => new { supplier.SupplierName, supplier.City, supplier.Country })
                });

            foreach (var customer in customersAndSuppliers2)
            {
                ObjectDumper.Write($"Customer ID: {customer.CustomerID}, City: {customer.City}, Country: {customer.Country}");
                foreach (var supplier in customer.Suppliers)
                {
                    ObjectDumper.Write($"Supplier name: {supplier.SupplierName}, City: {supplier.City}, Country: {supplier.Country}");
                }
                Console.WriteLine(@"---------------------------------------------------------");
            }
        }

        [Category("Tasks")]
        [Title("Task 3")]
        [Description("List of all customers who had orders that exceed the sum of X")]
        public void Linq3()
        {
            var x = 12000;
            var customers = dataSource.Customers.Where(_ => _.Orders.Any(t => t.Total >= x));

            foreach (var customer in customers)
            {
                ObjectDumper.Write(customer, 2);
                Console.WriteLine(@"---------------------------------------------------------");
            }
        }

        [Category("Tasks")]
        [Title("Task 4")]
        [Description("List of all customers with indication of the beginning of which month of which year they became clients")]
        public void Linq4()
        {
            var customers = dataSource.Customers.Where(c => c.Orders.Any()).Select(_ => new
            {
                _.CustomerID,
                FirstDate = _.Orders.OrderBy(o => o.OrderDate).Select(d => d.OrderDate).First()
            });
            foreach (var customer in customers)
            {
                ObjectDumper.Write($"Customer ID: {customer.CustomerID}, First order date: {customer.FirstDate}");
            }
        }
        [Category("Tasks")]
        [Title("Task 5")]
        [Description("Task 4 sorted by Year, Month, TotalSum (desc), CustomerID")]
        public void Linq5()
        {
            var customers = dataSource.Customers.Where(c => c.Orders.Any()).Select(_ => new
            {
                _.CustomerID,
                FirstDate = _.Orders.OrderBy(o => o.OrderDate).Select(d => d.OrderDate).First(),
                TotalSum = _.Orders.Sum(order => order.Total)
            }).OrderBy(y => y.FirstDate.Year).ThenBy(m => m.FirstDate.Month).ThenByDescending(t => t.TotalSum).ThenBy(id => id.CustomerID);
            foreach (var customer in customers)
            {
                ObjectDumper.Write(
                    $@"Customer ID: {customer.CustomerID}, Year: {customer.FirstDate.Year}, Month: {customer.FirstDate.Month}, Sum: {customer.TotalSum}");
            }
        }

        [Category("Tasks")]
        [Title("Task 6")]
        [Description("All customers who have a non-digital postal code, or not filled region or the phone does not contain the operator code")]
        public void Linq6()
        {
            var customers = dataSource.Customers.Where(_ =>
                !int.TryParse(_.PostalCode, out var postal)
                || string.IsNullOrEmpty(_.Region)
                || !_.Phone.StartsWith("(")
            );
            foreach (var customer in customers)
            {
                ObjectDumper.Write($"ID: {customer.CustomerID}, Postal: {customer.PostalCode}, Region: {customer.Region}, Phone: {customer.Phone}");
            }
        }

        [Category("Tasks")]
        [Title("Task 7")]
        [Description("All products grouped into categories, inside - by stock availability, within the last group, sort by cost.")]
        public void Linq7()
        {
            var products = dataSource.Products.GroupBy(c => c.Category).Select(group => new
                {
                    Category = group.Key,
                    UnitsInStock = group.GroupBy(p => p.UnitsInStock > 0).Select(inGroup => new
                    {
                        AreUnitsInStock = inGroup.Key,
                        Products = inGroup.OrderBy(pr => pr.UnitPrice)
                    })
                });
            foreach (var product in products)
            {
                ObjectDumper.Write(product, 3);
                Console.WriteLine(@"------------------------------------");
            }
        }

        [Category("Tasks")]
        [Title("Task 8")]
        [Description("Group products into groups of ''cheap'', ''average price'', ''expensive''. Define the boundaries of each group.")]
        public void Linq8()
        {
            decimal cheapBorder = 50, expensiveBorder = 100;

            var productGroups = dataSource.Products
                .GroupBy(product => product.UnitPrice < cheapBorder ? "Cheap"
                    : product.UnitPrice < expensiveBorder ? "Average" : "Expensive");

            foreach (var productsGroup in productGroups)
            {
                Console.WriteLine($@"{productsGroup.Key} products:");
                foreach (var product in productsGroup)
                {
                    Console.WriteLine($@"Product name: {product.ProductName}, Unit price: {product.UnitPrice}");
                }
            }
        }

        [Category("Tasks")]
        [Title("Task 9")]
        [Description("Calculate the average profitability of each city and the average intensity")]
        public void Linq9()
        {
            var cities = dataSource.Customers.GroupBy(c => c.City).Select(cityGroup => new
            {
                City = cityGroup.Key,
                Profit = cityGroup.Average(p => p.Orders.Sum(o => o.Total)),
                Intensity = cityGroup.Average(o => o.Orders.Length)
            });
            foreach (var city in cities)
            {
                ObjectDumper.Write($"City: {city.City}, Profitability: {Math.Round(city.Profit, 2)}, Intensity: {Math.Round(city.Intensity, 2)}");
            }
            
        }

        [Category("Tasks")]
        [Title("Task 10")]
        [Description("Calculate the average profitability of each city and the average intensity")]
        public void Linq10()
        {
            var customerGroups = dataSource.Customers
                .Select(customer => new
                {
                    customer.CustomerID,
                    StatForMonths = customer.Orders
                        .GroupBy(order => order.OrderDate.Month)
                        .Select(group => new { Month = group.Key, OrdersCount = group.Count() }),
                    StatForYears = customer.Orders
                        .GroupBy(order => order.OrderDate.Year)
                        .Select(group => new { Year = group.Key, OrdersCount = group.Count() }),
                    StatForMonthsAndYears = customer.Orders
                        .GroupBy(order => new { order.OrderDate.Year, order.OrderDate.Month })
                        .Select(group => new { group.Key.Year, group.Key.Month, OrdersCount = group.Count() })
                });

            foreach (var customer in customerGroups)
            {
                Console.WriteLine($@"CustomerID: {customer.CustomerID}");
                Console.WriteLine(@"	Statistic for months:");
                foreach (var monthStat in customer.StatForMonths)
                {
                    Console.WriteLine($@"		Month: {monthStat.Month}, Orders count: {monthStat.OrdersCount}");
                }
                Console.WriteLine(@"	Statistic for years:");
                foreach (var yearStat in customer.StatForYears)
                {
                    Console.WriteLine($@"		Year: {yearStat.Year}, Orders count: {yearStat.OrdersCount}");
                }
                Console.WriteLine(@"	Statistic for months and years:");
                foreach (var monthYearStat in customer.StatForMonthsAndYears)
                {
                    Console.WriteLine($@"		Month: {monthYearStat.Month}, Year: {monthYearStat.Year}, Orders count: {monthYearStat.OrdersCount}");
                }
            }
        }
    }
}

