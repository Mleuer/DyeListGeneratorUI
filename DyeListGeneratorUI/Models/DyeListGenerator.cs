using System;
using System.Collections.Generic;
using System.IO;

namespace DyeListGenerator
{
    public static class DyeListGenerator
    {
        public static void GenerateDyeList(Stream csvData, Stream masterDyeListFile, DirectoryInfo directory)
        {
            List<Customer> customers = Customer.GenerateCustomers(csvData);

            MasterDyeList masterDyeList = new MasterDyeList(masterDyeListFile);

            masterDyeList.Write(customers, new FileStream($"{directory.FullName}/DyeList {DateTime.UtcNow:MM-dd-yy}.xlsx", FileMode.Create));
        }
    }
}