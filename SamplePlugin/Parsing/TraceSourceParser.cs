// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Performance.SDK.Extensibility.SourceParsing;
using Microsoft.Performance.SDK.Processing;
using System.Collections.Generic;
using System;
using System.Threading;
using Microsoft.Performance.SDK;
using System.Linq;
using System.Diagnostics;


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
            List<PresentEvent> presentEvents = new List<PresentEvent>();

            Console.WriteLine("ME: " + "\"" + dataSource +"\"");

            string etlFilePath = dataSource.Uri.LocalPath;

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ".\\PresentMon-dev-x64.exe",
                Arguments = "-etl_file \"" + etlFilePath + "\" -output_stdout -qpc_time_s -session_name tmp",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            Process process = new Process { StartInfo = startInfo };

            process.Start();
            //process.WaitForExit();

            Console.WriteLine("ME: Started PresentMon");

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            Console.WriteLine("ME: " + error);

            Console.WriteLine("ME: Output Length Recieved: " + output.Length);

            string[] output_lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < output_lines.Length; i++)
            {
                string line = output_lines[i];
                
                string[] presentEventData = line.Split(new[] { ',' });

                string ts = presentEventData[presentEventData.Length - 1];

                PresentEvent tmp = new PresentEvent()
                {
                    TimeStamp = Timestamp.FromSeconds(double.Parse(ts)),
                    Process = presentEventData[0],
                    ProcessId = long.Parse(presentEventData[1]),
                    ThreadId = long.Parse(presentEventData[2]),
                    SwapChainAddress = presentEventData[3],
                    Runtime = presentEventData[4],
                    SyncInterval = int.Parse(presentEventData[5]),
                    PresentFlags = int.Parse(presentEventData[6]),
                    PresentResult = presentEventData[7],
                    TimeInSeconds = double.Parse(presentEventData[8]),
                    msInPresentAPI = double.Parse(presentEventData[9]),
                    msBetweenPresents = double.Parse(presentEventData[10]),
                    AllowsTearing = int.Parse(presentEventData[11]),
                    PresentMode = presentEventData[12],
                    msUntilRenderComplete = double.Parse(presentEventData[13]),
                    msUtilDisplayed = double.Parse(presentEventData[14]),
                    msBetweenDisplayChange = double.Parse(presentEventData[15])
                };

                presentEvents.Add(tmp);
            }

            foreach (PresentEvent e in presentEvents)
            {
                dataProcessor.ProcessDataElement(e, context, cancellationToken);
            }

            if (presentEvents.Count != 0)
            {
                firstEventTimestamp = presentEvents[0].TimeStamp;
                lastEventTimestamp = presentEvents[presentEvents.Count - 1].TimeStamp;
            }
        }
    }
}