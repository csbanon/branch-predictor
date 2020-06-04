// Carlos Santiago Bañón

// Branch Predictor
// BranchPredictor.cs

using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace BranchPredictor
{
    class BranchPredictor
    {
        // Global variables for statistical analysis.
        public static double numMisses = 0;
        public static double total = 0;

        // Converts a BitArray to its corresponding integer value.
        public static int bitArraytoInt(BitArray bitArray)
        {
            // Ensure the BitArray can be represented as an int.
            if (bitArray.Length > 32)
                throw new ArgumentException("This number is too large. It cannot be represented in a 32-bit integer.");

            // Perform the conversion.
            var integer = new int[1];
            bitArray.CopyTo(integer, 0);
            return integer[0];
        }

        // Looks for an entry in the Global Buffer Table and updates it according to the outcome of the prediction.
        // Updates the Global History Buffer based on the results, and returns it.
        public static BitArray updateGlobalBufferTable(int[] GBT, BitArray GHR, BitArray entry, bool outcome)
        {
            switch (outcome)
            {
                case true:
                    if (GBT[bitArraytoInt(entry)] <= 1)
                        numMisses++;

                    GBT[bitArraytoInt(entry)]++;

                    // Ensure the largest value is 'Strongly Taken'.
                    if (GBT[bitArraytoInt(entry)] > 3)
                        GBT[bitArraytoInt(entry)] = 3;

                    break;

                case false:
                    if (GBT[bitArraytoInt(entry)] >= 2)
                        numMisses++;

                    GBT[bitArraytoInt(entry)]--;

                    // Ensure the largest value is 'Strongly Not Taken'.
                    if (GBT[bitArraytoInt(entry)] < 0)
                        GBT[bitArraytoInt(entry)] = 0;
                    break;
            }

            if (GHR != null)
            {
                // Update GHR.
                GHR.RightShift(1);
                GHR.Set(GHR.Length - 1, outcome);
            }

            return GHR;
        }

        static void Main(string[] args)
        {
            // Parse command line arguments.
            var M = int.Parse(args[0]);
            var N = int.Parse(args[1]);
            var traceFile = args[2];

            // Create a new Global Buffer Table (GBT).
            int[] GBT = new int[(int)Math.Pow(2, M)];

            // Initialize all GBT entries to 'Weakly Taken'.
            for (int i = 0; i < GBT.Length; i++)
                GBT[i] = 2;

            // Create and initialize a new Global History Record (GHR).
            BitArray GHR = new BitArray(N, false);

            // Read the data from the trace file.
            var lines = File.ReadAllLines(traceFile);

            foreach (var line in lines)
            {
                var data = line.Split(' ');
                var address = int.Parse(data[0], NumberStyles.HexNumber);
                bool outcome;

                total++;

                // Determine the outcome.
                switch (data[1][0])
                {
                    case 't':
                        outcome = true;
                        break;

                    case 'n':
                        outcome = false;
                        break;

                    default:
                        outcome = false;
                        throw new System.Exception("Error! Invalid operation. Must be true or false.");
                }

                // Convert the address to a BitArray.
                BitArray entry = new BitArray(new[] { address });

                // Process the address.
                entry.RightShift(2);
                entry.Length = M;

                if (N != 0)
                {
                    // Clone the GHR, process it, and XOR it with the entry.
                    BitArray GHRClone = GHR.Clone() as BitArray;
                    GHRClone.Length = M;
                    GHRClone.LeftShift(M - N);
                    entry = entry.Xor(GHRClone);

                    // Update the GBT and the GHR.
                    GHR = updateGlobalBufferTable(GBT, GHR, entry, outcome);
                }
                else
                {
                    updateGlobalBufferTable(GBT, null, entry, outcome);
                }
            }

            // Calculate and print the Missprediction Ratio.
            var missPredictionRatio = Math.Round(100 * (numMisses / total), 2, MidpointRounding.ToEven);
            Console.WriteLine(M + " " + N + " " + missPredictionRatio);
        }
    }
}
