// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.


using Microsoft.Performance.SDK;
using Microsoft.Performance.SDK.Extensibility;
using Microsoft.Performance.SDK.Processing;
using SamplePlugin.Cookers;
using SamplePlugin.Parsing;
using System;
using System.Collections.Generic;

namespace SamplePlugin.Tables
{
    [Table]
    public sealed class StackTable
    {
        public static TableDescriptor TableDescriptor =>
           new TableDescriptor(
              Guid.Parse("{b637d246-4415-4ac1-9323-d62ce8ff6342}"),
              "Sample Events",
              "Sample Events",
              "Sample Events",
              requiredDataCookers: new List<DataCookerPath>
              {
              SampleEventCooker.DataCookerPath
              });

        private static readonly ColumnConfiguration countColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("58b4cdb2-6a6b-452f-96a2-d159880c3724"), "Count"),
            new UIHints
            {
                IsVisible = true,
                Width = 100,
                AggregationMode = AggregationMode.Sum
            });

        private static readonly ColumnConfiguration timestampColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("e53200ed-9dd4-4839-a591-8ee3acf5db70"), "Timestamp"),
            new UIHints
            {
                IsVisible = true,
                Width = 100,
                CellFormat = TimestampFormatter.FormatMillisecondsGrouped,
            });

        private static readonly ColumnConfiguration processColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("00c3ff8c-8905-4fad-9e30-0fa41106afc5"), "Process"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration processIdColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("07dbadf5-6eac-45fe-b701-3c2eeefb6db3"), "ProcessId"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration threadIdColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("1d51d95b-b665-40b0-b8f0-2e75ae8a47bf"), "ThreadId"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration swapChainAddressColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("bcf51200-0799-4850-846d-9ecbb693e3bf"), "SwapChainAddress"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration runtimeColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("a4cc7165-e624-4172-9a0b-559ce0745b6d"), "Runtime"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration syncIntervalColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("c5cdf807-62ed-446e-888b-fd8385e1544a"), "SyncInterval"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration presentFlagsColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("d5fd3898-3fed-44ac-ac03-a1fdc679e139"), "PresentFlags"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration presentResultColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("af8c8ec0-17b2-4950-95ef-005bb9ebeb20"), "PresentResult"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration timeInSecondsColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("ac0b958b-ad85-49b5-b671-b842b71f3409"), "TimeInSeconds"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration msInPresentAPIColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("0018cc67-a16b-44eb-8f82-0d68c75535e5"), "msInPresentAPI"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration msBetweenPresentsColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("4d781576-7367-495d-b100-b05ff2f43c46"), "msBetweenPresents"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration allowsTearingColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("61cfd035-e109-49e4-a47f-4054724db57f"), "AllowsTearing"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration presentModeColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("6b01f5a5-1fd8-453e-bc91-cf1517d5fa37"), "PresentMode"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration msUntilRenderCompleteColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("fce76c26-8c78-4192-bfb2-d604ed3488a9"), "msUntilRenderComplete"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration msUntilDisplayed = new ColumnConfiguration(
            new ColumnMetadata(new Guid("db8bd632-7736-494a-8e0f-259366c9d6ca"), "msUtilDisplayed"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        private static readonly ColumnConfiguration msBetweenDisplayChange = new ColumnConfiguration(
            new ColumnMetadata(new Guid("0c574b00-60fa-4cd9-a253-f48949334a47"), "msBetweenDisplayChange"),
            new UIHints
            {
                IsVisible = true,
                Width = 100
            });

        //
        // This method, with this exact signature, is required so that the runtime can 
        // build your table once all cookers have processed their data.
        //
        public static void BuildTable(
            ITableBuilder tableBuilder,
            IDataExtensionRetrieval requiredData
        )
        {
            List<PresentEvent> data =
                requiredData.QueryOutput<List<PresentEvent>>(new DataOutputPath(SampleEventCooker.DataCookerPath, nameof(SampleEventCooker.SampleEvents)));

            ITableBuilderWithRowCount tableBuilderWithRowCount = tableBuilder.SetRowCount(data.Count);

            IProjection<int, PresentEvent> baseProjection = Projection.Index(data);
            IProjection<int, Timestamp> timestampProjection = baseProjection.Compose(Projector.Timestamp);
            IProjection<int, string?> processProjection = baseProjection.Compose(Projector.Process);
            IProjection<int, long> processIdProjection = baseProjection.Compose(Projector.ProcessId);
            IProjection<int, long> threadIdProjection = baseProjection.Compose(Projector.ThreadId);
            IProjection<int, string?> swapChainAddressProjection = baseProjection.Compose(Projector.SwapChainAddress);
            IProjection<int, string?> runtimeProjection = baseProjection.Compose(Projector.Runtime);
            IProjection<int, int> syncIntervalProjection = baseProjection.Compose(Projector.SyncInterval);
            IProjection<int, int> presentFlagsProjection = baseProjection.Compose(Projector.PresentFlags);
            IProjection<int, string?> presentResultProjection = baseProjection.Compose(Projector.PresentResult);
            IProjection<int, double> timeInSecondsProjection = baseProjection.Compose(Projector.TimeInSeconds);
            IProjection<int, double> msInPresentAPIProjection = baseProjection.Compose(Projector.msInPresentAPI);
            IProjection<int, double> msBetweenPresentsProjection = baseProjection.Compose(Projector.msBetweenPresents);
            IProjection<int, int> allowsTearingProjection = baseProjection.Compose(Projector.AllowsTearing);
            IProjection<int, string?> presentModeProjection = baseProjection.Compose(Projector.PresentMode);
            IProjection<int, double> msUntilRenderCompleteProjection = baseProjection.Compose(Projector.msUntilRenderComplete);
            IProjection<int, double> msUntilDisplayedProjection = baseProjection.Compose(Projector.msUtilDisplayed);
            IProjection<int, double> msBwteenDisplayChangeProjection = baseProjection.Compose(Projector.msBetweenDisplayChange);

            tableBuilderWithRowCount.AddColumn(countColumn, Projection.Constant(1));
            tableBuilderWithRowCount.AddColumn(timestampColumn, timestampProjection);
            tableBuilderWithRowCount.AddColumn(processColumn, processProjection);
            tableBuilderWithRowCount.AddColumn(processIdColumn, processIdProjection);
            tableBuilderWithRowCount.AddColumn(threadIdColumn, threadIdProjection);
            tableBuilderWithRowCount.AddColumn(swapChainAddressColumn, swapChainAddressProjection);
            tableBuilderWithRowCount.AddColumn(runtimeColumn, runtimeProjection);
            tableBuilderWithRowCount.AddColumn(syncIntervalColumn, syncIntervalProjection);
            tableBuilderWithRowCount.AddColumn(presentFlagsColumn, presentFlagsProjection);
            tableBuilderWithRowCount.AddColumn(presentResultColumn, presentResultProjection);
            tableBuilderWithRowCount.AddColumn(timeInSecondsColumn, timeInSecondsProjection);
            tableBuilderWithRowCount.AddColumn(msInPresentAPIColumn, msInPresentAPIProjection);
            tableBuilderWithRowCount.AddColumn(msBetweenPresentsColumn, msBetweenPresentsProjection);
            tableBuilderWithRowCount.AddColumn(allowsTearingColumn, allowsTearingProjection);
            tableBuilderWithRowCount.AddColumn(presentModeColumn, presentModeProjection);
            tableBuilderWithRowCount.AddColumn(msUntilRenderCompleteColumn, msUntilRenderCompleteProjection);
            tableBuilderWithRowCount.AddColumn(msUntilDisplayed, msUntilDisplayedProjection);
            tableBuilderWithRowCount.AddColumn(msBetweenDisplayChange, msBwteenDisplayChangeProjection);


            var tableConfig = new TableConfiguration("Stacks")
            {
                Columns = new[]
                {
                    processColumn,
                    processIdColumn,
                    threadIdColumn,
                    presentModeColumn,
                    swapChainAddressColumn,
                    runtimeColumn,
                    presentFlagsColumn,
                    presentResultColumn,
                    allowsTearingColumn,
                    TableConfiguration.PivotColumn,
                    syncIntervalColumn,
                    timeInSecondsColumn,
                    msInPresentAPIColumn,
                    msBetweenPresentsColumn,
                    msUntilRenderCompleteColumn,
                    msUntilDisplayed,
                    msBetweenDisplayChange,
                    countColumn,
                    TableConfiguration.GraphColumn,
                    timestampColumn
                },
            };

            tableBuilder.AddTableConfiguration(tableConfig);
            tableBuilder.SetDefaultTableConfiguration(tableConfig);
        }
    }
}