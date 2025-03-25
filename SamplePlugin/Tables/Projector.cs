// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Performance.SDK;
using SamplePlugin.Parsing;

namespace SamplePlugin.Tables
{
    internal class Projector
    {
        public static Timestamp EndTime(Event e)
        {
            return e.Timestamp;
        }

        public static TimestampDelta Duration(PresentEvent e)
        {
            return TimestampDelta.FromMilliseconds(e.msBetweenPresents);
        }

        public static string? Process(PresentEvent e)
        {
            return e.Process;
        }

        public static long ProcessId(PresentEvent e)
        {
            return e.ProcessId;
        }
        public static long ThreadId(PresentEvent e)
        {
            return e.ThreadId;
        }
        public static string? SwapChainAddress(PresentEvent e)
        {
            return e.SwapChainAddress;
        }
        public static string? Runtime(PresentEvent e)
        {
            return e.Runtime;
        }
        public static int SyncInterval(PresentEvent e)
        {
            return e.SyncInterval;
        }

        public static int PresentFlags(PresentEvent e)
        {
            return e.PresentFlags;
        }
        public static string? PresentResult(PresentEvent e)
        {
            return e.PresentResult;
        }
        public static double TimeInSeconds(PresentEvent e)
        {
            return e.TimeInSeconds;
        }
        public static double msInPresentAPI(PresentEvent e)
        {
            return e.msInPresentAPI;
        }
        public static double MsBetweenPresents(PresentEvent e)
        {
            return e.msBetweenPresents;
        }
        public static double FramesPerSecond(PresentEvent e)
        {
            return 1000 / e.msBetweenPresents;
        }
        public static int AllowsTearing(PresentEvent e)
        {
            return e.AllowsTearing;
        }
        public static string? PresentMode(PresentEvent e)
        {
            return e.PresentMode;
        }
        public static double msUntilRenderComplete(PresentEvent e)
        {
            return e.msUntilRenderComplete;
        }
        public static double msUtilDisplayed(PresentEvent e)
        {
            return e.msUtilDisplayed;
        }
        public static double msBetweenDisplayChange(PresentEvent e)
        {
            return e.msBetweenDisplayChange;
        }
    }
}
