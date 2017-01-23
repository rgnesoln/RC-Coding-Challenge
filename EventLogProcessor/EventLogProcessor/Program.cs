//-----------------------------------------------------------------------------
// <copyright file="Program.cs" company="Reliable Controls Corporation">
//   Copyright (C) Reliable Controls Corporation.  All rights reserved.
// </copyright>
//
// Description:
//      A console application that scans log files of a specific format for a
//      fault event pattern and counts the instances of fault.
//
//      A fault event pattern is where the unit is in stage 3 for 5 or more
//      minutes and then goes to stage 2.  The unit can flip between stages
//      2 and 3 any number of times, but then goes to stage 0 indicating
//      a positive fault event pattern, where it gets counted.
//
//      The input for the application is a log file that gets scanned and 
//      the output is a count of the number of fault event patterns detected.
//
//-----------------------------------------------------------------------------

using System;
using System.IO;

namespace EventLogProcessor {
    class Program {
        /// <summary>
        /// Main function of the application
        /// </summary>
        /// <param name="args">A array of arguments passed into the application from the command line</param>
        static void Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("Usage: EventLogProcessor <log file>");

            } else {
                try {   // Open the text file using a stream reader.
                    if (File.Exists(args[0])) {
                        using (StreamReader sr = new StreamReader(args[0])) {
                            // instantiate processor class and process the log file by passing the stream reference
                            EventLogProcessor elp = new EventLogProcessor();
                            elp.ParseEvents(sr);

                            // get and display the fault count to the console
                            long eCount = elp.EventCount();
                            Console.WriteLine("Number of Faults Detected: " + eCount.ToString());
                        }
                    } else {
                        Console.WriteLine("Error: Log file does not exist");
                    }

                } catch (Exception e) {
                    Console.WriteLine("Error: Could not read file (" + args[0] + "): ");
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
