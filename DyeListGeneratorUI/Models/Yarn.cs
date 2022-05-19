using System;
using System.Linq;
using CsvHelper;
using OfficeOpenXml.FormulaParsing.Excel.Operators;
using MissingFieldException = System.MissingFieldException;

namespace DyeListGenerator
{
    public class Yarn
    {
        public double NumberOfSkeins { get; set; }
        public YarnType YarnType { get; set; }
        public String YarnTypeDescription { get; set; }
        public String Color { get; set; }

        public bool IsMiniSkein
        {
            get { return DetermineIfMiniSkein(YarnTypeDescription); }
        }

        public Yarn(double numberOfSkeins, YarnType yarnType, String yarnTypeDescription)
        {
            NumberOfSkeins = numberOfSkeins;
            YarnType = yarnType;
            YarnTypeDescription = yarnTypeDescription;
        }
        public Yarn() {}

        public override bool Equals(object obj)
        {
            if ((obj == null) || this.GetType() != obj.GetType()) 
            {
                return false;
            }
            else { 
                Yarn yarn = (Yarn) obj; 
                return (this.YarnType == yarn.YarnType) && 
                       (this.Color == yarn.Color) &&
                       (this.YarnTypeDescription.Equals((yarn.YarnTypeDescription))) &&
                       (this.IsMiniSkein == yarn.IsMiniSkein);
            }   
        }
        
        public static bool AreEquivalent(Yarn yarn1, Yarn yarn2)
        {
            return (yarn1.IsMiniSkein == yarn2.IsMiniSkein &&
                    yarn1.YarnType == yarn2.YarnType &&
                    yarn1.YarnTypeDescription == yarn2.YarnTypeDescription &&
                    yarn1.Color == yarn2.Color);
        }

        public static Yarn operator +(Yarn yarn, Yarn addend)
        {
            if (AreEquivalent(yarn, addend))
            {
                double sum = yarn.NumberOfSkeins + addend.NumberOfSkeins;
                Yarn newYarn = new Yarn() {NumberOfSkeins = sum, YarnType = yarn.YarnType, YarnTypeDescription = yarn.YarnTypeDescription, Color = yarn.Color};
                return newYarn;
            }
            throw new ArgumentException("Yarn are different types");
        }

        public static Yarn CreateYarnFromText(string inputText)
        {
            inputText = inputText.TrimStart(',');
            String[] inputs = inputText.Split(',');

            double quantity = double.Parse(inputs[0]);
            YarnType yarnType = YarnFactory.CreateYarnTypeFromText(inputs[1]);
            String yarnTypeDescription = inputs[2];

            (yarnType, quantity) = YarnFactory.ModifyValuesForMiniSkeins(yarnTypeDescription, quantity, yarnType);

            Yarn yarn = new Yarn(quantity, yarnType, yarnTypeDescription);
            
            if (inputs.Length > 3)
            {
                yarn.Color = inputs[3];
            }

            return yarn;
        }

        public static Yarn CreateYarnFromText(CsvReader reader)
        {
            double quantity = reader.GetField<double>(1);
            YarnType yarnType = YarnFactory.CreateYarnTypeFromText(reader.GetField<String>(2));
            String yarnTypeDescription = reader.GetField<String>(3);

            (yarnType, quantity) = YarnFactory.ModifyValuesForMiniSkeins(yarnTypeDescription, quantity, yarnType);

            Yarn yarn = new Yarn(quantity, yarnType, yarnTypeDescription);
            
            if (reader.TryGetField<String>(4, out String colorName))
            {
                yarn.Color = colorName;
            }

            return yarn;
        }

        public static bool DetermineIfMiniSkein(String yarnDescription)
        {
            return yarnDescription.Contains("mini", StringComparison.CurrentCultureIgnoreCase);
        }
        public static Yarn CreateYarnFromCSV(CsvReader reader)
        {
            Yarn yarn;
            try
            {
                yarn = CreateYarnFromText(reader);

                return yarn;
            }

            catch (MissingFieldException exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }

    public enum YarnType
    {
        [YarnTypeProperties(5, MiniClassy)] 
        Classy,

        [YarnTypeProperties(5, MiniClassyWCashmere, "Classy/Cashmere")]
        ClassyWCashmere,
        
        [YarnTypeProperties(4, MiniSmooshy)] 
        Smooshy,

        [YarnTypeProperties(4, MiniSmooshyWCashmere, "Smooshy/Cashmere")]
        SmooshyWCashmere,
        
        [YarnTypeProperties(4, MiniJilly)] 
        Jilly,

        [YarnTypeProperties(4, MiniJillyWCashmere, "Jilly Cash")]
        JillyWCashmere,

        [YarnTypeProperties(4, MiniJillyLaceCashmere, "Jilly Lace Cash")]
        JillyLaceCashmere,
        
        [YarnTypeProperties(4, MiniCosette)] 
        Cosette,
        
        [YarnTypeProperties(5, MiniCity)] 
        City,
        
        [YarnTypeProperties(1, MiniMohair)]
        Mohair,
        
        [YarnTypeProperties(4, MiniButterfly, "Butterfly")] 
        Butterfly,
        
        [YarnTypeProperties(4, MiniBFLSock, "BFL Sock")] 
        BFLSock,
        
        [YarnTypeProperties(4, MiniJuliette, "Juliette")] 
        Juliette,
        
        [YarnTypeProperties(4, MiniRiley, "Riley")] 
        Riley,
        
        [YarnTypeProperties(4, MiniSuzette, "Suzette")] 
        Suzette,
        
        [YarnTypeProperties(4, MiniSavvy, "Savvy")] 
        Savvy,
        
        [YarnTypeProperties(1, MiniClassy, "Mini Classy")]
        MiniClassy,
        
        [YarnTypeProperties(1, MiniClassyWCashmere, "Mini Classy Cash")]
        MiniClassyWCashmere,
        
        [YarnTypeProperties(1, MiniSmooshy, "Mini Smooshy")]
        MiniSmooshy,
        
        [YarnTypeProperties(1, MiniSmooshyWCashmere, "Mini Smooshy Cash")]
        MiniSmooshyWCashmere,
        
        [YarnTypeProperties(1, MiniJilly, "Mini Jilly")]
        MiniJilly,
        
        [YarnTypeProperties(1, MiniJillyWCashmere, "Mini Jilly Cash")]
        MiniJillyWCashmere,
        
        [YarnTypeProperties(1, MiniJillyLaceCashmere, "Mini Jilly Lace Cash")]
        MiniJillyLaceCashmere,
        
        [YarnTypeProperties(1, MiniCosette, "Mini Cosette")]
        MiniCosette,
        
        [YarnTypeProperties(1, MiniCity, "Mini City")]
        MiniCity,
        
        [YarnTypeProperties(1, MiniMohair, "Mini Mohair")]
        MiniMohair,
        
        [YarnTypeProperties(1, MiniButterfly, "Mini Butterfly")]
        MiniButterfly,
        
        [YarnTypeProperties(1, MiniBFLSock, "Mini BFL Sock")]
        MiniBFLSock,
        
        [YarnTypeProperties(1, MiniJuliette, "Mini Juliette")]
        MiniJuliette,
        
        [YarnTypeProperties(1, MiniRiley, "Mini Riley")]
        MiniRiley,
        
        [YarnTypeProperties(1, MiniSuzette, "Mini Suzette")]
        MiniSuzette,
        
        [YarnTypeProperties(1, MiniSavvy, "Mini Savvy")]
        MiniSavvy
    }
}