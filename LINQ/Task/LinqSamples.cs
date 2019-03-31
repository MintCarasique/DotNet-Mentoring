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
    }
}

