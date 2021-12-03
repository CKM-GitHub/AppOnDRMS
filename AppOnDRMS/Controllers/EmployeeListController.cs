using DRMS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User_BL;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;
using iTextSharp.text.html.simpleparser;
using iTextSharp.tool.xml;

namespace AppOnDRMS.Controllers
{
    public class EmployeeListController : Controller
    {
        UserBL user_bl = new UserBL();
        // GET: EmployeeList
        public ActionResult EmployeeList()
        {
            DateDifferenceModel date_model = new DateDifferenceModel();
            HttpCookie cookie = HttpContext.Request.Cookies.Get("Admin_Member_ID");
            if (cookie != null)
            {
                UserLoginModel login_Model = user_bl.GetUserLoginModel();
                ViewBag.CompanyName = login_Model.company_name;
                return View(date_model);
            }

            else
                return RedirectToAction("UserLogin", "User");
        }
        [HttpPost]
        public ActionResult EmployeeList(DateDifferenceModel model)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 35, 35, 35, 50);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                //Add border to page
                PdfContentByte content = pdfWriter.DirectContent;
                Rectangle rectangle = new Rectangle(pdfDoc.PageSize);
                rectangle.Left += pdfDoc.LeftMargin;
                rectangle.Right -= pdfDoc.RightMargin;
                rectangle.Top -= pdfDoc.TopMargin;
                rectangle.Bottom += pdfDoc.BottomMargin;
                content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                content.Stroke();

                //Table
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                //0=Left, 1=Centre, 2=Right
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                //Cell no 1
                PdfPCell cell = new PdfPCell();

                //Table
                table = new PdfPTable(5);
                table.WidthPercentage = 100;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                //Cell
                cell = new PdfPCell(new Phrase("Colspan2-1"));
                cell.Colspan = 2;
                cell.BorderWidthRight = 0;
                cell.BorderWidthBottom = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Colspan2-2"));
                cell.Colspan = 2;
                cell.BorderWidthRight = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthBottom = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Colspan1"));
                cell.BorderWidthLeft = 0;
                cell.BorderWidthBottom = 0;
                cell.MinimumHeight = 35;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Date Difference"));
                cell.Colspan = 5;
                cell.BorderWidthTop = 0;
                table.AddCell(cell);

                table.AddCell("S.No");
                table.AddCell("NYC Junction");
                table.AddCell("Item");
                table.AddCell("Cost");
                table.AddCell("Date");

                table.AddCell("1");
                table.AddCell("David Food Store");
                table.AddCell("Fruits & Vegetables");
                table.AddCell("$100.00");
                table.AddCell("June 1");

                table.AddCell("2");
                table.AddCell("Child Store");
                table.AddCell("Diaper Pack");
                table.AddCell("$6.00");
                table.AddCell("June 9");

                table.AddCell("3");
                table.AddCell("Punjabi Restaurant");
                table.AddCell("Dinner");
                table.AddCell("$29.00");
                table.AddCell("June 15");

                table.AddCell("4");
                table.AddCell("Wallmart Albany");
                table.AddCell("Grocery");
                table.AddCell("$299.50");
                table.AddCell("June 25");

                table.AddCell("5");
                table.AddCell("Singh Drugs");
                table.AddCell("Back Pain Tablets");
                table.AddCell("$14.99");
                table.AddCell("June 28");

                pdfDoc.Add(table);

                pdfWriter.CloseStream = false;
                pdfDoc.Close();

                byte[] bytes = myMemoryStream.ToArray();
                // Write out PDF from memory stream.                
                string FolderName = Server.MapPath("/output/staff/");
                var date = DateTime.Now.ToString("yyyyMMdd"); 
                Random r = new Random();
                int num = r.Next();
                Random ra = new Random();
                int num1 = ra.Next(10, 99);
                string fileName = "staff_"+ date + "_" + num + num1 +".pdf";
                using (FileStream fs = new FileStream(Server.MapPath(Path.Combine("~/output/staff/", fileName)), FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(bytes, 0, (int)bytes.Length);
                }
                // Write output PDF on user download path 
                Response.Buffer = true;                Response.ContentType = "application/pdf";                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);                Response.Cache.SetCacheability(HttpCacheability.NoCache);                Response.BinaryWrite(bytes);                Response.End();
            }
            return View();
        }
    }
}