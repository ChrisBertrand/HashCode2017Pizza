using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCode2017Pizza
{
    class Program
    {
        List<slice> PizzaSlices = new List<slice>();
        public struct slice
        {
            public int[,] topleft;
            public int[,] botright;
        }

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

        public slice FindSliceFrom(string[,] target, int[,] statPos, int i)
        {
            slice result = new slice { botright = null, topleft = null };

            var rowLowerLimit = target.GetLowerBound(0);
            var rowUpperLimit = target.GetUpperBound(0) + 1;
            var colLowerLimit = target.GetLowerBound(1);
            var colUpperLimit = target.GetUpperBound(1) + 1;

            var StartRow = statPos.GetUpperBound(0) + 1;
            var StartCol = statPos.GetUpperBound(1) + 1;

            string sliceType = target[StartRow, StartCol];
            // Change this value to show in USE
           
            var searchType = sliceType == "M" ? "T" : "M";

            // Only Look forwards...
            for (int row = StartRow; row < rowUpperLimit; row++)
            {
                for (int col = StartCol; col < colUpperLimit; col++)
                {
                    // Havent hit a taken spot yet...
                    if ((target[row, col] == "M" || target[row, col] == "T"))
                    {
                        if (target[row, col] == searchType)
                        {
                            if (row == StartRow || col == StartCol)
                            {
                                //Valid position
                                result.topleft = statPos;
                                result.botright = new int[row, col];
                                // Change the pizza to show it's been taken.
                                target[StartRow, StartCol] = i.ToString();
                                // Rows we've found...
                                target[row, col] = i.ToString();
                                return result;
                            }
                            return result;
                        }
                    }
                    else
                    {
                        target[StartRow, StartCol] = sliceType;
                        return result;
                    }
                }
            }
            return result;
        }

        void CutPizza()
        {
            // Need to think of a algorithm to cut the pizza.
            //start letter
            Random r = new Random();
            
            for (int i =1; i< 50; i++)
            {
                int rX = r.Next(0, Rows);
                int rY = r.Next(0, Collumns);

                int[,] startCor = new int[rX, rY];

                slice newSlice = FindSliceFrom(Pizza, startCor, i);
                if (newSlice.botright != null)
                {
                    //valid slice, add to collection
                    PizzaSlices.Add(newSlice);
                }
            }
            // Flat pizza, doesn't help.
            //string[] flatPizza = Pizza.Cast<string>().ToArray();
            //Console.Write(string.Join(",", flatPizza));
            var fr = true;
        }

        void Initialise()
        {
            var FileDetails = System.IO.File.ReadAllLines("C:\\small.in");

            var PizzaConfig = FileDetails.First();
            Rows = Convert.ToInt32(PizzaConfig.Split(' ')[0]);
            Collumns = Convert.ToInt32(PizzaConfig.Split(' ')[1]);
            MinIngredientsPerSlice = Convert.ToInt32(PizzaConfig.Split(' ')[2]);
            MaxCellsPerSlice = Convert.ToInt32(PizzaConfig.Split(' ')[3]);

            Pizza = new string[Rows, Collumns];
            
            var PizzaDetails = FileDetails.Skip(1);
            //Draw the Pizza
            CreatePizza(PizzaDetails);
            PrintPizza(Pizza);
            CutPizza();
            PrintPizza(Pizza);
        }
      
        void CreatePizza(IEnumerable<string> PizzaDetails)
        {
            var pizzaRepresentation = new StringBuilder();

            int lineNum = 0;
            foreach (var line in PizzaDetails)
            {
                int cellNum = 0;
                foreach (var cell in line)
                {
                    //Create the pizza
                    Pizza[lineNum, cellNum] = cell.ToString();
                    cellNum++;
                }
                lineNum++;
            }
        }

        void PrintPizza(string[,] Pizza)
        {
            int rowLength = Pizza.GetLength(0);
            int colLength = Pizza.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0} ", Pizza[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.Write("---------------------------" + Environment.NewLine);

            //last debugging point to view console.
            var data = false;
        }
    }
}