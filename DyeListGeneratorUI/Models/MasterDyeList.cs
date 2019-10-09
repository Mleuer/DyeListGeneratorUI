using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using OfficeOpenXml;


namespace DyeListGenerator
{
    public class MasterDyeList
    {
        public ExcelPackage Package;

        public MasterDyeList(Stream dyeListData)
        {
            Package = new ExcelPackage();
            Package.Load(dyeListData);
        }

        public MasterDyeList(ExcelPackage package)
        {
            Package = package;
        }

        ~MasterDyeList()
        {
            Package.Dispose();
        }

        public void Write(List<Customer> customers, Stream output)
        {
            ISet<Yarn> yarnCounts = ExtractYarnCounts(customers);
            Dictionary<string, int> columnNumbers = FindCorrespondingColumns();
            Dictionary<string, int> rowNumbers = FindCorrespondingRows();
            List<Yarn> problemYarns = new List<Yarn>();
            
            foreach (var yarnItem in yarnCounts)
            {
                if (yarnItem.Color == null || yarnItem.YarnType == null)
                {
                    problemYarns.Add(yarnItem);
                    continue;
                }
                try
                {
                    (int, int) destinationCell = (rowNumbers[yarnItem.Color.ToUpper()], columnNumbers[yarnItem.YarnType.GetTextRepresentation()]);
                    //Package.Workbook.Worksheets[0].Cells[destinationCell.Item1, destinationCell.Item2].RichText.Text = ((Int16)yarnItem.NumberOfSkeins).ToString(CultureInfo.CurrentCulture);
                    //Package.Workbook.Worksheets[0].SetValue(destinationCell.Item1, destinationCell.Item2, ((Int16)yarnItem.NumberOfSkeins).ToString(CultureInfo.CurrentCulture));
                    Package.Workbook.Worksheets[0].Cells[destinationCell.Item1, destinationCell.Item2].Value = ((Int16)yarnItem.NumberOfSkeins).ToString(CultureInfo.CurrentCulture);
                }
                catch (KeyNotFoundException)
                {
                    problemYarns.Add(yarnItem);
                }
            }
            Package.SaveAs(output);
        }

        private Dictionary<string, int> FindCorrespondingColumns()
        {
            Dictionary<string, int> yarnTypes = new Dictionary<string, int>();

            for (int i = 4; Package.Workbook.Worksheets[0].Cells[1, i].IsPopulated(); i++)
            {
                var yarnType = Package.Workbook.Worksheets[0].Cells[1, i].RichText.Text;
                yarnTypes.Add(yarnType, i);
            }
            
            return yarnTypes;
        }

        private Dictionary<string, int> FindCorrespondingRows()
        {
            Dictionary<string, int> colors = new Dictionary<string, int>();
            
            for (int i = 2; Package.Workbook.Worksheets[0].Cells[i, 1].IsPopulated(); i++)
            {
                var colorName = Package.Workbook.Worksheets[0].Cells[i, 1].RichText.Text;
                colors.Add(colorName, i);
            }

            return colors;
        }

        public static ISet<Yarn> ExtractYarnCounts(List<Customer> customers)
        {
            HashSet<Yarn> yarnTotal = new HashSet<Yarn>();
            List<Yarn> combinedYarnOrders = extractListOfYarn(customers);
            
            for (var i = 0; i < combinedYarnOrders.Count; i++)
            {
                var yarn1 = combinedYarnOrders[i];
                for (var j = i + 1; j < combinedYarnOrders.Count; j++)
                { 
                    var yarn2 = combinedYarnOrders[j];

                    if (!ReferenceEquals(yarn1, yarn2))
                    {
                        if (Yarn.AreEquivalent(yarn1, yarn2))
                        {
                            yarn1 += yarn2;
                            combinedYarnOrders.RemoveAt(j);
                        }
                    }
                }
                yarnTotal.Add(yarn1);
            }
            return yarnTotal;
        }
        
        

        private static List<Yarn> extractListOfYarn(List<Customer> customers)
        {
            List<Yarn> combinedYarn = new List<Yarn>();
            foreach (var customer in customers)
            {
                foreach (var yarn in customer.Order)
                {
                    combinedYarn.Add(yarn);
                }
            }

            return combinedYarn;
        }
    }
}