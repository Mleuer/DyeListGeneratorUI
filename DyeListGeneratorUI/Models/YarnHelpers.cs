using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DyeListGenerator
{
    public static class YarnFactory
    {
        public static YarnType CreateYarnTypeFromText(string input)
        {
            String upperInput = input.ToUpper();
            switch (upperInput)
            {
                case "VM":
                    return YarnType.Classy;
                case "CC":
                    return YarnType.ClassyWCashmere;
                case "VS":
                    return YarnType.Smooshy;
                case "VC":
                    return YarnType.SmooshyWCashmere;
                case "JY":
                    return YarnType.Jilly;
                case "JC":
                    return YarnType.JillyWCashmere;
                case "JLC":
                    return YarnType.JillyLaceCashmere;
                case "CS":
                    return YarnType.Cosette;
                case "CT":
                    return YarnType.City;
                case "SL":
                    return YarnType.Mohair;
                case "B2":
                    return YarnType.Butterfly;
                case "BSK":
                    return YarnType.BFLSock;
                case "BDK":
                    return YarnType.Juliette;
                case "VR":
                    return YarnType.Riley;
                case "SZ":
                    return YarnType.Suzette;
                case "SV":
                    return YarnType.Savvy;
                default:
                    throw new ArgumentException("Yarn Type Does Not Exist");
            }
        }

        public static (YarnType, double) ModifyValuesForMiniSkeins(string yarnDescription, double quantity,
            YarnType yarnType)
        {
            if (Yarn.DetermineIfMiniSkein(yarnDescription))
            {
                double conversionConstant = yarnType.GetMiniConversionConstant();
                quantity *= conversionConstant;
                YarnType miniType = yarnType.GetCorrespondingMiniType();
                return (miniType, quantity);
            }

            return (yarnType, quantity);
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class YarnTypePropertiesAttribute : Attribute
    {
        public double MiniConversionConstant { get; }
        public YarnType CorrespondingMiniType { get; }
        
        public String TextRepresentation { get; }

        public YarnTypePropertiesAttribute(double miniConversionConstant, YarnType miniType, string textRepresentation = "")
        {
            MiniConversionConstant = miniConversionConstant;
            CorrespondingMiniType = miniType;
            TextRepresentation = textRepresentation;
        }
    }

    public static class YarnTypeExtension
    {
        public static double GetMiniConversionConstant(this YarnType yarnType)
        {
            YarnTypePropertiesAttribute yarnTypePropertiesAttribute = GetYarnTypeProperties(yarnType);
            double conversionConstant = yarnTypePropertiesAttribute.MiniConversionConstant;
            return conversionConstant;
        }

        public static YarnType GetCorrespondingMiniType(this YarnType yarnType)
        {
            YarnTypePropertiesAttribute yarnTypePropertiesAttribute = GetYarnTypeProperties(yarnType);
            YarnType miniType = yarnTypePropertiesAttribute.CorrespondingMiniType;
            return miniType;
        }

        public static String GetTextRepresentation(this YarnType yarnType)
        {
            YarnTypePropertiesAttribute yarnTypePropertiesAttribute = GetYarnTypeProperties(yarnType);
            String textRepresentation = yarnTypePropertiesAttribute.TextRepresentation;
            if (textRepresentation != String.Empty)
            {
                return textRepresentation;
            }
            else
            {
                return yarnType.ToString();
            }
        }
        private static YarnTypePropertiesAttribute GetYarnTypeProperties(YarnType yarnType)
        {
            Type type = yarnType.GetType();

            string name = Enum.GetName(type, yarnType);

            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    YarnTypePropertiesAttribute yarnTypeProperties =
                        Attribute.GetCustomAttribute(field, typeof(YarnTypePropertiesAttribute)) as YarnTypePropertiesAttribute;
                    if (yarnTypeProperties != null)
                    {
                        return yarnTypeProperties;
                    }
                }
            }

            throw new ArgumentException();
        }
    }
}