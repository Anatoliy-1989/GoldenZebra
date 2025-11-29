using BlazorBootstrap;
using BlazorGoldenZebra.Data;
using BlazorGoldenZebra.Models;
using BlazorGoldenZebra.Models.ViewModels;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using NuGet.Protocol;
using System.Collections.Generic;

namespace BlazorGoldenZebra.Utills
{
    public class XmlWorker
    {
        public static MemoryStream GenerateExcel<T>(IEnumerable<T> data, string sheetName)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);
            worksheet.Cell(1, 1).InsertTable(data); // Automatically adds headers from property names

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0; // Reset stream position for reading/downloading
            return stream;
        }

        public static byte[] GenerateExcel2<T>(IEnumerable<T> data, string sheetName)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);
            worksheet.Cell(1, 1).InsertTable(data); // Automatically adds headers from property names

            // Save the workbook to a MemoryStream
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                // Convert the MemoryStream to a byte array
                return stream.ToArray();
            }
        }

        public void GetExcel()
        { 
        }

        public byte[] CreateTestXlsxDocumentAsByteArray()
        {
            using (var workbook = new XLWorkbook())
            {
                // Add a worksheet
                var worksheet = workbook.Worksheets.Add("SampleSheet");

                // Add some data
                worksheet.Cell("A1").Value = "Name";
                worksheet.Cell("B1").Value = "Age";
                worksheet.Cell("A2").Value = "John Doe";
                worksheet.Cell("B2").Value = 30;

                // Save the workbook to a MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    // Convert the MemoryStream to a byte array
                    return stream.ToArray();
                }
            }
        }

        public static byte[] CreateXlsxDocumentAsByteArray(List<OrderExportModel> orders, List<MetalType> metalTypes)
        {
            metalTypes = metalTypes.OrderBy(x => x.Id).ToList();

            using (var workbook = new XLWorkbook())
            {
                // Add a worksheet
                var worksheet = workbook.Worksheets.Add("GoldenZebra");

                CreateHeader(worksheet, metalTypes);

                var i = 2;
                foreach (var order in orders)
                {
                    CreateDataRow(worksheet, i, order, metalTypes);
                    i++;
                }

                // Save the workbook to a MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    // Convert the MemoryStream to a byte array
                    return stream.ToArray();
                }
            }
        }

        public static byte[] CreateXlsxDocumentAsByteArray(List<OrderStatisticsViewModel> orders, List<MetalType> metalTypes)
        {
            metalTypes = metalTypes.OrderBy(x => x.Id).ToList();

            using (var workbook = new XLWorkbook())
            {
                // Add a worksheet
                var worksheet = workbook.Worksheets.Add("GoldenZebra");

                CreateStatHeader(worksheet, metalTypes);

                var i = 2;
                foreach (var order in orders)
                {
                    CreateDataRow(worksheet, i, order, metalTypes);
                    i++;
                }

                // Save the workbook to a MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    // Convert the MemoryStream to a byte array
                    return stream.ToArray();
                }
            }
        }


        private static void CreateHeader(IXLWorksheet worksheet, List<MetalType> metalTypes)
        {
            var row = worksheet.Row(1);
            var i = 1;

            row.Cell(i).Value = "Дата"; i++;
            row.Cell(i).Value = "Отделение"; i++;
            row.Cell(i).Value = "Пользователь"; i++;
            
            foreach (var metalType in metalTypes)
            {
                row.Cell(i).Value = $"Вес лом гр. {metalType.Name}"; i++;
            }
            foreach (var metalType in metalTypes)
            {
                row.Cell(i).Value = $"Вес лом чис. {metalType.Name}"; i++;
            }
            foreach (var metalType in metalTypes)
            {
                row.Cell(i).Value = $"Вес лом стд. пробы {metalType.Name}"; i++;
            }
            foreach (var metalType in metalTypes)
            {
                row.Cell(i).Value = $"Сумма лом {metalType.Name}"; i++;
            }
            foreach (var metalType in metalTypes)
            {
                row.Cell(i).Value = $"Вес изделий. {metalType.Name}"; i++;
            }
            foreach (var metalType in metalTypes)
            {
                row.Cell(i).Value = $"Сумма изделий {metalType.Name}"; i++;
            }

            row.Cell(i).Value = "Кол. позиций"; i++;
        }

        private static void CreateStatHeader(IXLWorksheet worksheet, List<MetalType> metalTypes)
        {
            var row = worksheet.Row(1);
            var i = 1;

            row.Cell(i).Value = "Отделение"; i++;

            foreach (var metalType in metalTypes)
            {
                row.Cell(i).Value = $"Вес лом гр. {metalType.Name}"; i++;
            }
            foreach (var metalType in metalTypes)
            {
                row.Cell(i).Value = $"Вес лом чис. {metalType.Name}"; i++;
            }
            foreach (var metalType in metalTypes)
            {
                row.Cell(i).Value = $"Вес лом стд. пробы {metalType.Name}"; i++;
            }
            foreach (var metalType in metalTypes)
            {
                row.Cell(i).Value = $"Сумма лом {metalType.Name}"; i++;
            }
            foreach (var metalType in metalTypes)
            {
                row.Cell(i).Value = $"Вес изделий. {metalType.Name}"; i++;
            }
            foreach (var metalType in metalTypes)
            {
                row.Cell(i).Value = $"Сумма изделий {metalType.Name}"; i++;
            }

            row.Cell(i).Value = "Кол. позиций"; i++;
            row.Cell(i).Value = "Кол. пакетов"; i++;
        }

        private static void CreateDataRow(IXLWorksheet worksheet, int rowNumber, OrderExportModel orderExportModel, List<MetalType> metalTypes)
        {
            var row = worksheet.Row(rowNumber);
            var i = 1;
            row.Cell(i).Value = orderExportModel.Date; i++;
            row.Cell(i).Value = orderExportModel.Place; i++;
            row.Cell(i).Value = orderExportModel.User; i++;

            CreateMetalCells(metalTypes, orderExportModel.OrderItems, row, ref i);

            row.Cell(i).Value = orderExportModel.OrderItems.Count; i++;
        }

        private static void CreateMetalCells(List<MetalType> metalTypes, List<OrderItemExportModel> orderItems, IXLRow row, ref int i)
        {
            foreach (var metalType in metalTypes)
            {
                var metalOrderItems = GetItemsOfMetalType(orderItems, metalType.Id, ProductTypesEnum.Scrap);
                CreateValueCell(metalType, row, i, metalOrderItems, (oi) => oi.WeightDirty); i++;
            }
            foreach (var metalType in metalTypes)
            {
                var metalOrderItems = GetItemsOfMetalType(orderItems, metalType.Id, ProductTypesEnum.Scrap);
                CreateValueCell(metalType, row, i, metalOrderItems, (oi) => oi.WeightClean); i++;
            }
            foreach (var metalType in metalTypes)
            {
                var metalOrderItems = GetItemsOfMetalType(orderItems, metalType.Id, ProductTypesEnum.Scrap);
                CreateValueCell(metalType, row, i, metalOrderItems, (oi) => oi.DefaultFinessWeight); i++;
            }
            foreach (var metalType in metalTypes)
            {
                var metalOrderItems = GetItemsOfMetalType(orderItems, metalType.Id, ProductTypesEnum.Scrap);
                CreateValueCell(metalType, row, i, metalOrderItems, (oi) => oi.Cost); i++;
            }
            foreach (var metalType in metalTypes)
            {
                var metalOrderItems = GetItemsOfMetalType(orderItems, metalType.Id, ProductTypesEnum.Product);
                CreateValueCell(metalType, row, i, metalOrderItems, (oi) => oi.WeightClean); i++;
            }
            foreach (var metalType in metalTypes)
            {
                var metalOrderItems = GetItemsOfMetalType(orderItems, metalType.Id, ProductTypesEnum.Product);
                CreateValueCell(metalType, row, i, metalOrderItems, (oi) => oi.Cost); i++;
            }
        }

        private static List<OrderItemExportModel> GetItemsOfMetalType(List<OrderItemExportModel> orderItems, int MetalTypeId, ProductTypesEnum productType)
        {
            return orderItems.Where(oi => oi.MetalType == MetalTypeId && oi.ProductType == productType).ToList();
        }

        private static void CreateValueCell<T>(MetalType metalType, IXLRow row, int i, List<T> orderItems, Func<T, decimal?> func)
        {
            if (orderItems.Any())
            {
                var sum = Decimal.Round(orderItems.Sum(func) ?? 0, 2);
                if (sum > 0)
                {
                    row.Cell(i).Value = sum;
                }
            }
        }

        private static void CreateDataRow(IXLWorksheet worksheet, int rowNumber, OrderStatisticsViewModel orderExportModel, List<MetalType> metalTypes)
        {
            var row = worksheet.Row(rowNumber);
            var i = 1;
            row.Cell(i).Value = orderExportModel.Place; i++;

            CreateMetalCells(metalTypes, orderExportModel.OrderItems, row, ref i);

            row.Cell(i).Value = orderExportModel.Positions; i++;
            row.Cell(i).Value = orderExportModel.OrderAmount; i++;
        }
    }
}
