//-----------------------------------------------------------------------------
// <copyright file="EventCounter.cs" company="Reliable Controls Corporation">
//   Copyright (C) Reliable Controls Corporation.  All rights reserved.
// </copyright>
//
// Description:
//      This class implements the EventCounter interface that defines two
//      functions.  One function to parse the log file looking for the 
//      fault event pattern, and the other returning the count of the 
//      number of faults detected.
//
//-----------------------------------------------------------------------------

using RC.CodingChallenge;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace EventLogProcessor {
    class EventLogProcessor : EventCounter {
        /// <summary>
        /// Variable to hold the count of the fault event patterns detected
        /// </summary>
        private long _eventCounter;

        /// <summary>
        /// Parse and accumulate event information from the given log data.
        /// </summary>
        /// <param name="eventLog">A stream of lines representing time,value recordings.</param>
        public void ParseEvents(StreamReader eventLog) {
            bool inPossibleFault = false;
            DateTime lastDt = DateTime.MinValue;
            int lastStage = int.MinValue;

            _eventCounter = 0;
            while (eventLog.Peek() >= 0) {
                // read a line
                string line = eventLog.ReadLine();

                // replace all whitespace with a single space
                string cleanLine = Regex.Replace(line, @"\s+", " ");

                // determine if the pattern matches
                Regex regex = new Regex(@"^(\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}\s\d{1})$");
                Match match = regex.Match(cleanLine);
                if (match.Success) {
                    // process entry and determine if start of fault
 
                    // split the line into parts separated by spaces
                    string[] parts = match.Value.Split(' ');

                    // get datetime in order to do datetime math if stage 3
                    string dateTime = parts[0] + " " + parts[1];
                    DateTime dt = DateTime.ParseExact(dateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                    // get stage value
                    int stage = 0;
                    if (Int32.TryParse(parts[2], out stage)) {
                        // if in a possible fault, check for end condition of 0 or a reset condition of 1
                        if (inPossibleFault) {
                            // waiting for stage 0 to happen and then increment counter. 
                            // If I get a stage 1, then reset everything and start looking from scratch.
                            if (stage == 0) {
                                _eventCounter++;
                                inPossibleFault = false;

                            } else if (stage == 1) {
                                inPossibleFault = false;
                            }

                            lastStage = stage;
                            lastDt = dt;

                        } else {
                            // set first stage/datetime holder variables
                            if (lastStage == int.MinValue) {
                                lastStage = stage;
                                lastDt = dt;

                            } else {
                                // if stage 2, check for previous stage of 3
                                if (stage == 2) {
                                    // if previous stage is 3, then check for time difference
                                    if (lastStage == 3) {
                                        TimeSpan diffResult = dt.Subtract(lastDt);
                                        if (diffResult.TotalMinutes >= 5) inPossibleFault = true;
                                    }

                                    lastStage = stage;
                                    lastDt = dt;

                                // current stage is 3 and last stage is 3, then ignore entry
                                // if not stage is 3, then set stage/datetime holder varables
                                } else if (stage == 3) {
                                    if (lastStage != 3) {
                                        lastStage = stage;
                                        lastDt = dt;
                                    }

                                // if stage is not 2 or 3, then set stage/datetime holder variables
                                } else {
                                    lastStage = stage;
                                    lastDt = dt;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current count of events detected
        /// </summary>
        /// <returns>Total count of events detected</returns>
        public long EventCount() {
            return _eventCounter;            
        }
    }
}
