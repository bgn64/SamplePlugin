// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Performance.SDK.Extensibility.DataCooking.SourceDataCooking;
using Microsoft.Performance.SDK.Extensibility.DataCooking;
using Microsoft.Performance.SDK.Extensibility;
using Microsoft.Performance.SDK;
using System.Collections.Generic;
using System.Threading;
using System;
using SamplePlugin.Parsing;

namespace SamplePlugin.Cookers
{
    public sealed class SampleEventCooker
        : SourceDataCooker<Event, ParsingContext, Type>
    {
        public static readonly DataCookerPath DataCookerPath =
            DataCookerPath.ForSource(nameof(TraceSourceParser), nameof(SampleEventCooker));

        public SampleEventCooker()
            : base(DataCookerPath)
        {
            this.SampleEvents = new List<SampleEvent>();
        }

        public override string Description => "Stack Event cooker.";

        public override ReadOnlyHashSet<Type> DataKeys =>
            new ReadOnlyHashSet<Type>(new HashSet<Type>(new[] { typeof(SampleEvent) }));

        [DataOutput]
        public List<SampleEvent> SampleEvents { get; }

        public override DataProcessingResult CookDataElement(
            Event data,
            ParsingContext context,
            CancellationToken cancellationToken)
        {
            SampleEvents.Add((SampleEvent)data);

            return DataProcessingResult.Processed;
        }
    }
}
