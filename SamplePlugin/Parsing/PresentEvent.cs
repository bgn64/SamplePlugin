// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Performance.SDK;
using System;

namespace SamplePlugin.Parsing
{
    public class PresentEvent : Event
    {
        public override Timestamp Timestamp => TimeStamp;

        public override Type GetKey()
        {
            return typeof(PresentEvent);
        }

        public Timestamp TimeStamp { get; set; }

        public string? Process { get; set; }
        public long ProcessId { get; set; }
        public long ThreadId { get; set; }
        public string? SwapChainAddress { get; set; }
        public string? Runtime { get; set; }
        public int SyncInterval { get; set; }
        public int PresentFlags { get; set; }
        public string? PresentResult { get; set; }
        public double TimeInSeconds { get; set; }
        public double msInPresentAPI { get; set; }
        public double msBetweenPresents { get; set; }
        public int AllowsTearing { get; set; }
        public string? PresentMode { get; set; }
        public double msUntilRenderComplete { get; set; }
        public double msUtilDisplayed { get; set; }
        public double msBetweenDisplayChange { get; set; }

    }
}
