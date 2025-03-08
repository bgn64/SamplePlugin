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

        private static readonly ColumnConfiguration dataColumn = new ColumnConfiguration(
            new ColumnMetadata(new Guid("00c3ff8c-8905-4fad-9e30-0fa41106afc5"), "Data"),
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
            List<SampleEvent> data =
                requiredData.QueryOutput<List<SampleEvent>>(new DataOutputPath(SampleEventCooker.DataCookerPath, nameof(SampleEventCooker.SampleEvents)));

            ITableBuilderWithRowCount tableBuilderWithRowCount = tableBuilder.SetRowCount(data.Count);

            var baseProjection = Projection.Index(data);
            var timestampProjection = baseProjection.Compose(Projector.Timestamp);
            var dataProjection = baseProjection.Compose(Projector.Data);

            tableBuilderWithRowCount.AddColumn(countColumn, Projection.Constant(1));
            tableBuilderWithRowCount.AddColumn(timestampColumn, timestampProjection);
            tableBuilderWithRowCount.AddColumn(dataColumn, dataProjection);


            var tableConfig = new TableConfiguration("Stacks")
            {
                Columns = new[]
                {
                    dataColumn,
                    TableConfiguration.PivotColumn,
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