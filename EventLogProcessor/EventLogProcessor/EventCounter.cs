//-----------------------------------------------------------------------------
// <copyright file="EventCounter.cs" company="Reliable Controls Corporation">
//   Copyright (C) Reliable Controls Corporation.  All rights reserved.
// </copyright>
//
// Description:
//      A service that identifies and counts events which are associated with a
//      specific sequence of operations.
//
//      The input logs are comprised of lines of text that indicate the time of a
//      recording, and the value recorded
//
//      1998-03-07 06:25:32	2
//      1998-03-07 09:15:55	3
//      1998-03-07 12:00:02	3
//      1998-03-07 14:28:27	0
//
//      The columns (1) date+time in ISO-8601 format, (2) value indicating HVAC
//      unit stage by tabs.
//
//-----------------------------------------------------------------------------

using System.IO;

namespace RC.CodingChallenge
{
    public interface EventCounter
    {
        /// <summary>
        /// Parse and accumulate event information from the given log data.
        /// </summary>
        /// <param name="eventLog">A stream of lines representing time,value recordings.</param>
        void ParseEvents(StreamReader eventLog);

        /// <summary>
        /// Gets the current count of events detected
        /// </summary>
        /// <returns>Total count of events detected</returns>
        long EventCount();
    }
}
