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
                PdfPTable table = new PdfPTable(9);
                table.HorizontalAlignment = 0;
                table.TotalWidth = 525f;
                table.LockedWidth = true;
                float[] widths = new float[] { 75f, 155f, 75f, 30f, 30f, 30f, 50f, 50f, 30f };
                table.SetWidths(widths);
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                //Cell no 1
                PdfPCell cell = new PdfPCell();

                //Cell
                cell = new PdfPCell(new Phrase(" 社員名：山本 太郎 "));
                cell.Colspan = 4;
                cell.BorderWidthRight = 0;
                cell.BorderWidthBottom = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("《社員別明細表》"));
                cell.Colspan = 3;
                cell.BorderWidthRight = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthBottom = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(" 1 ページ "));
                cell.Colspan = 2;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthBottom = 0;
                cell.MinimumHeight = 35;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(" 令和 3 年 10 月 1 日～令和 3 年 10 月 31 日 "));
                cell.Colspan = 9;
                cell.BorderWidthTop = 0;
                table.AddCell(cell);

                table.AddCell("月/日");
                table.AddCell("工事名");
                table.AddCell("就業時間");
                table.AddCell("基本");
                table.AddCell("時外");
                table.AddCell("深夜");
                table.AddCell("所定外");
                table.AddCell("法定休");
                table.AddCell("休深");

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