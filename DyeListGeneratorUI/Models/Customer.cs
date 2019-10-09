using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using MissingFieldException = CsvHelper.MissingFieldException;


namespace DyeListGenerator
{
    public class Customer
    {
        public String Name { get; private set; } 
        public List<Yarn> Order { get; private set; }

        public Customer()
        {
            Order = new List<Yarn>();
        }

        public Customer(String name, List<Yarn> order)
        {
            Name = name;
            Order = order;
        }


        public static List<Customer> GenerateCustomers(Stream stream)
        {
            var customers = new List<Customer>();
            
            using (var reader = new StreamReader(stream))
            {
                using (var csv = new CsvReader(reader))
                {
                    bool yarnDataParsable = false;
                    Customer currentCustomer = null;

                    while (csv.Read())
                    {
                        String[] line = csv.Parser.Context.Record;
                        if (line.Contains("Quantity"))
                        {
                            csv.ReadHeader();
                            yarnDataParsable = true;
                        }

                        if (yarnDataParsable)
                        {
                            var firstEntryOfLine = csv.Parser.Context.Record[0];
                            
                            if ((firstEntryOfLine != String.Empty) && (IsTotalLine(firstEntryOfLine) == false))
                            {
                                string customerName = firstEntryOfLine;
                                currentCustomer = new Customer() {Name = customerName};
                                customers.Add(currentCustomer);
                                continue;
                            }
                            
                            if (currentCustomer != null)
                            {
                                try
                                {
                                    Yarn yarn = Yarn.CreateYarnFromCSV(csv);
                                    currentCustomer.Order.Add(yarn);
                                }
                                catch (MissingFieldException) { }
                            }
                        }
                    }
                }
            }
            return customers;
        }

        public static bool IsTotalLine(String firstEntryOfLine)
        {
            return firstEntryOfLine.EndsWith(" Total:");
        }
    }
    
}