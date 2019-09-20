using OfficeOpenXml;
using OpenDataStorage.ViewModels.ReportsViewModel;
using OpenDataStorageCore.Entities.CharacteristicValues;
using OpenDataStorageCore.Entities.NestedSets;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace OpenDataStorage.API.Reports
{
    [RoutePrefix("api/Reports")]
    public class ReportsController : BaseApiController
    {
        [Route("Generate")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Generate([FromUri]ReportViewModel vm)
        {
            if (vm != null && vm.FromDate.HasValue && vm.ToDate.HasValue && vm.FromDate > vm.ToDate)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "FromDate must by less than ToDate");
            }

            ICollection<ObjectType> types = null;
            if(vm != null && vm.TypeId.HasValue)
            {
                types = await _dbContext.ObjectTypeContext.GetChildNodes(vm.TypeId.Value, true);
            }

            ICollection<HierarchyObject> objects = null;
            if (vm != null && vm.ObjectId.HasValue)
            {
                objects = await _dbContext.HierarchyObjectContext.GetChildNodes(vm.ObjectId.Value, true);
            }
            else
            {
                objects = await _dbContext.HierarchyObjectContext.GetTree();
            }

            if (types != null && types.Any())
            {
                objects = objects.Where(o => types.Any(t => t.Id == o.ObjectTypeId)).ToList();
            }

            ICollection<Characteristic> characteristics = null;
            if(vm != null && vm.CharacterisitcId.HasValue)
            {
                characteristics = await _dbContext.CharacteristicContext.GetChildNodes(vm.CharacterisitcId.Value, true);
            }

            ICollection<BaseCharacteristicValue> values = await _dbContext.CharacteristicValueDbSetManager.GetAllQuery().ToListAsync();
            ICollection<NumberCharacteristicValue> data = values
                .Where(v => v.ValueType == CharacteristicType.Number && 
                    v.CreationDate.HasValue &&
                    objects.Any(o => o.Id == v.HierarchyObjectId))
                .Cast<NumberCharacteristicValue>().ToList();
            if(characteristics != null && characteristics.Any())
            {
                data = data.Where(v => characteristics.Any(c => c.Id == v.CharacteristicId)).ToList();
            }

            if (vm != null)
            {
                if (vm.FromDate.HasValue) data = data.Where(v => 
                    (!v.IsTimeIntervalValue && v.CreationDate >= vm.FromDate.Value) ||
                    (v.IsTimeIntervalValue && v.EndCreationDate >= vm.FromDate.Value)).ToList();
                if (vm.ToDate.HasValue) data = data.Where(v => v.CreationDate <= vm.ToDate.Value).ToList();
            }

            ExcelPackage excelDocument = new ExcelPackage();
            var reportname = $"report-{DateTime.Now}.xlsx";
            excelDocument.Workbook.Worksheets.Add(reportname);
            ExcelWorksheet workSheet = excelDocument.Workbook.Worksheets.First();

            int row = 1;
            int column = 1;

            workSheet.Cells[row, column].Value = UserLexicon.GetString("Object");
            workSheet.Cells[row, column + 1].Value = UserLexicon.GetString("SubjectOfMonitoring");
            workSheet.Cells[row, column + 2].Value = UserLexicon.GetString("Characteristic");
            workSheet.Cells[row, column + 3].Value = UserLexicon.GetString("CreationDate");
            workSheet.Cells[row, column + 4].Value = UserLexicon.GetString("Value");
            row++;

            data = data.OrderBy(d => d.CreationDate).ToList();
            foreach (var value in data)
            {
                workSheet.Cells[row, column].Value = value.HierarchyObject.Name;
                workSheet.Cells[row, column + 1].Value = value.SubjectOfMonitoring;
                workSheet.Cells[row, column + 2].Value = value.Characteristic.Name;
                workSheet.Cells[row, column + 3].Value = value.IsTimeIntervalValue ?
                    $"{value.CreationDate.Value.ToString("dd.MM.yyyy")}-{value.EndCreationDate.Value.ToString("dd.MM.yyyy")}" :
                    value.CreationDate.Value.ToString("dd.MM.yyyy");
                workSheet.Cells[row, column + 4].Value = value.Value;
                row++;
            }

            var memoryStream = new MemoryStream();
            excelDocument.Workbook.Properties.Author = HttpContext.Current.User.Identity.Name;
            excelDocument.SaveAs(memoryStream);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(memoryStream.ToArray());
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = reportname;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

            return response;
        }
    }
}