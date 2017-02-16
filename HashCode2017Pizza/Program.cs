using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace HashCode2017Pizza
{
    public class Program
    {
        List<slice> PizzaSlices = new List<slice>();

        public struct Point
        {
            public int x, y;
        };

        public struct slice
        {
            public int[,] topleft;
            public int[,] botright;

            public int r1;
            public int c1;
            public int r2;
            public int c2;

            public ArrayList cellHistory;
            public int cellCount;
            public int mushCount;
            public int tomCount;
        }

        struct pCell
        {
            public int x;
            public int y;
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
        public IEnumerable<string> PizzaDetails { get; set; }

        public slice FindSliceFrom(string[,] target, int[,] statPos, int i, int sliceSize)
        {
            slice result = new slice { botright = null, topleft = null };
            // Set the arryList
            result.cellHistory = new ArrayList();
            // Get the grid Boundaries
            var rowLowerLimit = target.GetLowerBound(0);
            var rowUpperLimit = target.GetUpperBound(0) + 1;
            var colLowerLimit = target.GetLowerBound(1);
            var colUpperLimit = target.GetUpperBound(1) + 1;
            // Get the starting position values
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
                    // Haven't hit a taken spot yet...
                    if ((target[row, col] == "M" || target[row, col] == "T"))
                    {
                        if(target[row, col] == "M") { result.mushCount++; }
                        if (target[row, col] == "T") { result.tomCount++; }
                        result.cellCount++;
                        result.cellHistory.Add(new pCell { x = row, y = col });

                        // This will loop over multiple times, starting with the biggest slices possible.
                        if (target[row, col] == searchType && result.cellCount == sliceSize && result.mushCount >= MinIngredientsPerSlice && result.tomCount >= MinIngredientsPerSlice && result.cellCount <= MaxCellsPerSlice)
                        {
                            //if (row == StartRow || col == StartCol)
                            // if is square / rectangle ???
                            if (isSquare(result.cellHistory))
                            {
                                //Valid position
                                result.topleft = statPos;
                                result.botright = new int[row, col];

                                result.r1 = StartRow;
                                result.r2 = row;
                                result.c1 = StartCol;
                                result.c2 = col;

                                // Change the pizza to show what's been taken.
                                target[StartRow, StartCol] = i.ToString(); // FIRST CELL
                                foreach (pCell pc in result.cellHistory)
                                {
                                    //string[] traverseHistoryRec = rec.Split(',');
                                    // Mark each cell
                                    //                                    target[Convert.ToInt32(traverseHistoryRec[0]), Convert.ToInt32(traverseHistoryRec[1])] = i.ToString();
                                    target[pc.x, pc.y] = i.ToString();
                                }
                                // Rows we've found...
                                target[row, col] = i.ToString(); // LAST CELL
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

        public bool isSquare(ArrayList cellHistory)
        {
            int maxX = 0;
            int maxY = 0;
            foreach (pCell pc in cellHistory)
            {
                if (pc.x > maxX) { maxX = pc.x; }
                if (pc.y > maxY) { maxY = pc.y; }
            }

            pCell lastCell = (pCell)cellHistory[cellHistory.Count - 1];

            if (lastCell.x == maxX && lastCell.y == maxY)
            { return true; }

            return false;
        }

        public void CutPizza()
        {
            // Need to think of a algorithm to cut the pizza.
            //start letter
            Random r = new Random();
            // Clear any slices that may be left over from other runs.
            PizzaSlices.Clear();
            // Need to clear the pizza too.
            CreatePizza(PizzaDetails);
            // loop over enough times to get the slices needed.
            for (int i =1; i< (Rows * Collumns * 2); i++)
            {
                int rX = r.Next(0, Rows);
                int rY = r.Next(0, Collumns);

                int[,] startCor = new int[rX, rY];

                // start with the biggest slices possible.
                for (int sliceSize = MaxCellsPerSlice; sliceSize > 0; sliceSize--)
                {
                    slice newSlice = FindSliceFrom(Pizza, startCor, i, sliceSize);

                    // If this is valid add!
                    if (newSlice.botright != null)
                    {
                        //valid slice, add to collection
                        PizzaSlices.Add(newSlice);
                    }
                }
               
            }
            // Flat pizza, doesn't help.
            //string[] flatPizza = Pizza.Cast<string>().ToArray();
            //Console.Write(string.Join(",", flatPizza));
            StreamWriter output = File.CreateText("output.txt");
            //First Line
            output.WriteLine(PizzaSlices.Count());
            for (int i=0; i<PizzaSlices.Count(); i++)
            {
                output.WriteLine(PizzaSlices[i].r1 + " " + PizzaSlices[i].c1 + " " + PizzaSlices[i].r2 + " " + PizzaSlices[i].c2);
            }
            output.Close();
        }

        public void Initialise()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var FileDetails = File.ReadAllLines("small.in");

            var PizzaConfig = FileDetails.First();
            Rows = Convert.ToInt32(PizzaConfig.Split(' ')[0]);
            Collumns = Convert.ToInt32(PizzaConfig.Split(' ')[1]);
            MinIngredientsPerSlice = Convert.ToInt32(PizzaConfig.Split(' ')[2]);
            MaxCellsPerSlice = Convert.ToInt32(PizzaConfig.Split(' ')[3]);

            Pizza = new string[Rows, Collumns];
            
            PizzaDetails = FileDetails.Skip(1);
            //Draw the Pizza
            CreatePizza(PizzaDetails);
            PrintPizza(Pizza);

            double perc = 0;
            double total = 0;
            double max = 0;
            while (perc < 85)
            {
                // Try cutting up the pizza.
                CutPizza();

                total = PizzaSlices.Sum(p => p.cellCount);
                max = (Rows * Collumns);
                perc = (total / max * 100);
            }
           
            Console.Write("Score = " + total + "/" + max + " %" + (total / max * 100) + Environment.NewLine + Environment.NewLine);

            PrintPizza(Pizza);

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            var end = true;
        }
      
        public void CreatePizza(IEnumerable<string> PizzaDetails)
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

        public void PrintPizza(string[,] Pizza)
        {
            int rowLength = Pizza.GetLength(0);
            int colLength = Pizza.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0:D4} ", Pizza[i, j]));
                }
                Console.Write(Environment.NewLine);
            }
            Console.Write("---------------------------" + Environment.NewLine);
            //last debugging point to view console.
            var data = false;
        }
    }
}