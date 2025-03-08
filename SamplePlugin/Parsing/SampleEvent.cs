// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Performance.SDK;
using System;

namespace SamplePlugin.Parsing
{
    public class SampleEvent : Event
    {
        public override Timestamp Timestamp => TimeStamp;

        public override Type GetKey()
        {
            return typeof(SampleEvent);
        }

        public Timestamp TimeStamp { get; set; }

        public int Data { get; set; }
    }
}
