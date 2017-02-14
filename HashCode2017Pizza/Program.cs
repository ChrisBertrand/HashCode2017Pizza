using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2017Pizza
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.Initialise();
        }

        public int Rows { get; set; }
        public int Collumns { get; set; }
        public int MinIngredientsPerSlice { get; set; }
        public int MaxCellsPerSlice { get; set; }

        public string[,] Pizza { get; set; }
        public string Tomato = "T";
        public string Mushroom = "M";

        void Initialise()
        {
            var FileDetails = System.IO.File.ReadAllLines("C:\\small.in");

            var PizzaConfig = FileDetails.First();
            Rows = PizzaConfig[0];
            Collumns = PizzaConfig[1];
            MinIngredientsPerSlice = PizzaConfig[2];
            MaxCellsPerSlice = PizzaConfig[3];

            Pizza = new string[Rows, Collumns];

            var PizzaDetails = FileDetails.Skip(1);
            //Draw the Pizza
            CreatePizza(PizzaDetails);
            CutPizza();
        }

        void CutPizza()
        {

        }

        void CreatePizza(IEnumerable<string> PizzaDetails)
        {
            //for (int row = 0; row < Rows; row++)
            //  {
            //      for (int col = 0; col < Collumns; col++)
            //      {
            //          Pizza[row, col] = PizzaDetails.Where(i => i.)
            //      }
            //  }
            var pizzaRepresentation = new StringBuilder();

            foreach (var line in PizzaDetails)
            {
                int lineNum = 0;
                foreach (var cell in line)
                {
                   int cellNum = 0;

                   //Create the pizza
                   Pizza[lineNum, cellNum] = cell.ToString();
                    //Draw the pizza
                    pizzaRepresentation.Append(cell);
                   cellNum++;
                }
                pizzaRepresentation.AppendLine();
                lineNum++;
            }
            Console.Write(pizzaRepresentation);

            // debugging point to view console.
            var data = false;
        }
    }
}
