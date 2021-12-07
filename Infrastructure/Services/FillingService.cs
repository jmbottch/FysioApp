using ApplicationCore.Abstractions.Api;
using ApplicationCore.Entities.ApiEntities;
using ExcelDataReader;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class FillingService : IFillingService
    {
        //vektis is diagnose
        //dcsph is verrichting/behandeling
        //door de vele files is er bij mij verwarring ontstaan over de naamgeving.
        private readonly string filePath1 = @"D:\School\SSWPI\Projects\FysioApp\API\DataFiles\Vektis_Lijst_Verrichtingen.xlsx";
        private readonly string filePath2 = @"D:\School\SSWPI\Projects\FysioApp\API\DataFiles\Vektis_lijst_diagnoses.xlsx";
        private readonly ApiDbContext _context;

        public FillingService(ApiDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Diagnose> FillDiagnoses()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(filePath2, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader reader;
                reader = ExcelReaderFactory.CreateReader(stream);

                var config = new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                };

                var dataset = reader.AsDataSet(config);
                List<string> CodeList = new List<string>();
                List<string> DescriptionList = new List<string>();
                List<string> PathologyList = new List<string>();

                var rowLength = dataset.Tables[0].Rows.Count;

                for (var i = 0; i < rowLength; i++)
                {
                    CodeList.Add(dataset.Tables[0].Rows[i]["Code"].ToString());
                    DescriptionList.Add(dataset.Tables[0].Rows[i]["lichaamslocalisatie"].ToString());
                    PathologyList.Add(dataset.Tables[0].Rows[i]["pathologie"].ToString());
                }

                List<Diagnose> list = new List<Diagnose>();

                for (int i = 0; i < CodeList.Count; i++)
                {
                    Diagnose item = new Diagnose();
                    item.Code = CodeList[i];
                    item.BodyLocalization = DescriptionList[i];
                    item.Pathology = PathologyList[i];
                    list.Add(item);
                }                

                foreach (var vektisItem in list)
                {
                    _context.Diagnoses.Add(vektisItem);
                    _context.SaveChangesAsync();
                    
                }               
                

                return list;
            }
        }

        public IEnumerable<Operation> FillOperations()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using(var stream = File.Open(filePath1, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader reader;
                reader = ExcelReaderFactory.CreateReader(stream);

                var config = new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                };

                var dataset = reader.AsDataSet(config);
                List<string> CodeList = new List<string>();
                List<string> DescriptionList = new List<string>();
                List<bool> DescrequiredList = new List<bool>();

                var RowLength = dataset.Tables[0].Rows.Count;
                for (var i = 0; i < RowLength; i++)
                {
                    CodeList.Add(dataset.Tables[0].Rows[i]["Waarde"].ToString());
                    DescriptionList.Add(dataset.Tables[0].Rows[i]["Omschrijving"].ToString());
                    Debug.WriteLine(dataset.Tables[0].Rows[i]["Toelichting verplicht"].ToString());
                    if(dataset.Tables[0].Rows[i]["Toelichting verplicht"].ToString() == "Ja")
                    {
                        DescrequiredList.Add(true);
                    } else
                    {
                        DescrequiredList.Add(false);
                    }
                }

                List<Operation> list = new List<Operation>();

                for (int i = 0; i < CodeList.Count; i++)
                {
                    Operation item = new Operation();
                    item.Code = CodeList[i];
                    item.Description = DescriptionList[i];
                    item.DescriptionRequired = DescrequiredList[i];
                    list.Add(item);
                }

                foreach(Operation operation in list)
                {
                    _context.Operations.Add(operation);
                    _context.SaveChanges();
                }

                return list;

            }
        }


    }
}
