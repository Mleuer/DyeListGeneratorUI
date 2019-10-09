using System;
using System.IO;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;

namespace DyeListGenerator
{
    public static class Util
    {
        public static Stream CreateStreamFromString(String str)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(str); 
            MemoryStream stream = new MemoryStream(byteArray);
            return stream;
        }
    }

    public static class Extensions
    {
        public static bool IsPopulated(this ExcelRangeBase column)
        {
            return (column.Text != String.Empty);
        }
    }
}