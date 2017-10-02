using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;

using ClosedXML.Excel;
using AspExcelFileMvc.Models;

namespace AspExcelFileMvc.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            TestBaseEntities entities = new TestBaseEntities();
            return View(from customer in entities.Customers.Take(10)
                        select customer);
        }

        [HttpPost]
        public FileResult Export()
        {
            TestBaseEntities entities = new TestBaseEntities();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("CustomerId"),
                                            new DataColumn("ContactName"),
                                            new DataColumn("City"),
                                            new DataColumn("Country") });

            var customers = from customer in entities.Customers.Take(10)
                            select customer;

            foreach (var customer in customers)
            {
                dt.Rows.Add(customer.CustomerID, customer.ContactName, customer.City, customer.Country);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
        }
    }
}