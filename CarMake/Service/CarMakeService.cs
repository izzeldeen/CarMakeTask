using CarMake.Core;
using CarMake.Core.Filters;
using CarMake.Core.integration;
using CarMake.IService;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Formats.Asn1;
using System.Globalization;
using System;
using System.Linq;
using System.Reflection;
using CsvHelper.Configuration;
using CsvHelper;

namespace CarMake.Service
{
    public class CarMakeService : ICarMakeService
    {
        List<CarMakeModel> DbCars = new List<CarMakeModel>();
        List<CarExcel> carExcelData = new List<CarExcel>();
        Dictionary<string , string> searched = new Dictionary<string , string>();

        public CarMakeService()
        {
            if (carExcelData.Count == 0)
            {
                GetDataFromExcel();
            }
        }


        public async Task<List<string>> GetCars(CarFilter filter)
        {


            var isSearched = searched.Any(x => x.Key == filter.Modelyear && x.Value == filter.Make);
            if (isSearched)
            {
                return DbCars.Where(x => x.Make_Name.Contains(filter.Make)).Select(x=> x.Model_Name).ToList();
            }
            else
            {
                using (var httpClient = new HttpClient())
                {
                    List<CarExcel> carExcel = carExcelData.Where(x => x.make_name.Contains(filter.Make)).ToList();
                    carExcel.ForEach(async item =>
                    {
                        //must be config
                        string url = $"https://vpic.nhtsa.dot.gov/api/vehicles/GetModelsForMakeIdYear/makeId/{item.make_id}/modelyear/{filter.Modelyear}?format=json";
                        string response = await httpClient.GetAsync(url)
                                  .Result.Content.ReadAsStringAsync();
                        if (response != null)
                        {
                            var responseJson = JsonConvert.DeserializeObject<CarMakeIntegrationResponse>(response);
                            DbCars.AddRange(responseJson.Results);
                            
                        }
                    });
                    searched.Add(filter.Modelyear, filter.Make);

                    return DbCars.Where(x => x.Make_Name.Contains(filter.Make)).Select(x => x.Model_Name).ToList();
                }
            }


        }

        private void GetDataFromExcel()
        {
        
            using (var reader = new StreamReader("~/../Excel/CarMake.csv"))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                carExcelData = csv.GetRecords<CarExcel>().ToList();
            }
        }

    }
}
