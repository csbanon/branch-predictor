# Branch Predictor
 
## Overview

* **Year:** 2019
* **Language(s):** C#
* **Discipline(s):** Computer Architecture, Computer Engineering, Computer Science
* **Keywords:** Branch Prediction, Branch Predictor, GBH, Global Branch Predictor, Global Branch History, Global History Record, gshare, gshare Table, Missprediction Ratio, Pattern History Table, PC Bits

## Description

*Branch Predictor* is a C# program that runs a *gshare* branch prediction simulation, according to a specified number of Global Buffer Table (GBT) and Global History Record (GHR) bits.

Using these inputs, the program analyzes a specified input file that contains a list of branch instructions, made up of (a) the address of each instruction and (b) either a *t* or a *n*, which correspond to *taken* and *not-taken*, respectively.

The following are the inputs for this program:

1. **GBT:** The number of bits used for the Global Buffer Table (GBT).
2. **GHR:** The number of bits used for the Global History Record (GHR).

Finally, the program outputs the Misprediction Ratio of the branch predictor simulated with the input parameters.

The following files contain the list of branch instructions:

1. `GoBMK.txt`
2. `MCF.txt`

*Note: Both files are already included with the executables. For best results, use one of these for the simulation.*
