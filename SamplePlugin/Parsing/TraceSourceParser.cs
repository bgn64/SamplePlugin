// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Performance.SDK.Extensibility.SourceParsing;
using Microsoft.Performance.SDK.Processing;
using System.Collections.Generic;
using System;
using System.Threading;
using Microsoft.Performance.SDK;
using System.Linq;

namespace SamplePlugin.Parsing
{
    public sealed class TraceSourceParser
        : SourceParser<Event, ParsingContext, Type>
    {
        private ParsingContext context;
        private IEnumerable<IDataSource> dataSources;
        private DataSourceInfo? dataSourceInfo;

        public TraceSourceParser(IEnumerable<IDataSource> dataSources)
        {
            context = new ParsingContext();

            // Store the datasources so we can parse them later
            this.dataSources = dataSources;
        }

        // The ID of this Parser.
        public override string Id => nameof(TraceSourceParser);

        // Information about the Data Sources being parsed.
        public override DataSourceInfo? DataSourceInfo => dataSourceInfo;

        public override void ProcessSource(ISourceDataProcessor<Event, ParsingContext, Type> dataProcessor, ILogger logger, IProgress<int> progress, CancellationToken cancellationToken)
        {
            Timestamp? firstEventTimestamp = null;
            Timestamp? lastEventTimestamp = null;

            foreach (IDataSource dataSource in dataSources)
            {
                ProcessDataSource(dataSource, ref firstEventTimestamp, ref lastEventTimestamp, dataProcessor, progress, cancellationToken);
            }

            long firstEventTimestampNanoseconds = firstEventTimestamp.HasValue ? firstEventTimestamp.Value.ToNanoseconds : 0;
            long lastEventTimestampnanoseconds = lastEventTimestamp.HasValue ? lastEventTimestamp.Value.ToNanoseconds : firstEventTimestampNanoseconds + 1;
            DateTime firstEventWallClockUtc = DateTime.UtcNow;
            dataSourceInfo = new DataSourceInfo(firstEventTimestampNanoseconds, lastEventTimestampnanoseconds, firstEventWallClockUtc);
        }

        public void ProcessDataSource(IDataSource dataSource, ref Timestamp? firstEventTimestamp, ref Timestamp? lastEventTimestamp,
            ISourceDataProcessor<Event, ParsingContext, Type> dataProcessor, IProgress<int> progress, CancellationToken cancellationToken)
        {
            // Parse data from file here
            List<SampleEvent> sampleEvents = new List<SampleEvent>()
            {
                new SampleEvent()
                {
                    TimeStamp = Timestamp.FromSeconds(1),
                    Data = 1
                },
                new SampleEvent()
                {
                    TimeStamp = Timestamp.FromSeconds(2),
                    Data = 2
                },
                new SampleEvent()
                {
                    TimeStamp = Timestamp.FromSeconds(3),
                    Data = 3
                },
            };

            foreach (SampleEvent e in sampleEvents)
            {
                dataProcessor.ProcessDataElement(e, context, cancellationToken);
            }
        }
    }
}