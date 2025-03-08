// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Performance.SDK;
using SamplePlugin.Parsing;

namespace SamplePlugin.Tables
{
    internal class Projector
    {
        public static Timestamp Timestamp(Event e)
        {
            return e.Timestamp;
        }

        public static int Data(SampleEvent e)
        {
            return e.Data;
        }
    }
}
