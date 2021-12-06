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
using System.Data;
using Newtonsoft.Json;

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
        public Font CreateJapaneseFontHeader()
        {
            string font_folder = Server.MapPath("~/fonts/");
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFT,13,Font.BOLD);
            return font;
        }
        public Font CreateJapaneseFont()
        {
            string font_folder = Server.MapPath("~/fonts/");
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFT);
            return font;
        }
        [HttpPost]
        public ActionResult EmployeeList(DateDifferenceModel model)
        {
            UserLoginModel login_Model = new UserLoginModel();
            login_Model.member_id = model.Radio_Value;
            string staff_Name = user_bl.GetUser(login_Model);
            DataTable dt_staffName = JsonConvert.DeserializeObject<DataTable>(staff_Name);


            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 35, 35, 35, 50);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                Font font_Header = CreateJapaneseFontHeader();
                Font font = CreateJapaneseFont();

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
                table.HorizontalAlignment = 1;
                table.TotalWidth = 525f;
                table.LockedWidth = true;
                float[] widths = new float[] { 75f, 155f, 75f, 30f, 30f, 30f, 50f, 50f, 30f };
                table.SetWidths(widths);
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                //Cell no 1
                PdfPCell cell = new PdfPCell();

                //Cell
                cell = new PdfPCell(new Phrase("社員名："+ dt_staffName.Rows[0]["member_name"].ToString() + "",font_Header));
                cell.Colspan = 4;
                cell.BorderWidthRight = 0;
                cell.BorderWidthBottom = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("《社員別明細表》", font_Header));
                cell.Colspan = 3;
                cell.BorderWidthRight = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthBottom = 0;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("1 ページ", font_Header));
                cell.Colspan = 2;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthBottom = 0;
                cell.MinimumHeight = 35;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);

                DateTime date1 = new DateTime(2020, 12, 31, 5, 10, 20);
                string aa = user_bl.GetTextDateJapan(date1);

                cell = new PdfPCell(new Phrase(aa));
                cell = new PdfPCell(new Phrase(" 令和 3 年 10 月 1 日～令和 3 年 10 月 31 日 ",font));
                cell.Colspan = 9;
                cell.BorderWidthTop = 0;
                table.AddCell(cell);

                table.AddCell(new Phrase("月/日",font));
                table.AddCell(new Phrase("工事名", font));
                table.AddCell(new Phrase("就業時間", font));
                table.AddCell(new Phrase("基本", font));
                table.AddCell(new Phrase("時外", font));
                table.AddCell(new Phrase("深夜", font));
                table.AddCell(new Phrase("所定外", font));
                table.AddCell(new Phrase("法定休", font));
                table.AddCell(new Phrase("休深", font));

                pdfDoc.Add(table);

                pdfWriter.CloseStream = false;
                pdfDoc.Close();

                byte[] bytes = myMemoryStream.ToArray();
                // Write out PDF from memory stream.                
                string FolderName = Server.MapPath("/output/staff/");
               
                string name = "staff";
                string fileName = user_bl.GetPDF(name);
                using (FileStream fs = new FileStream(Server.MapPath(Path.Combine("~/output/staff/", fileName)), FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(bytes, 0, (int)bytes.Length);
                }
                // Write output PDF on user download path 
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
            }
            return View();
        }
    }
}