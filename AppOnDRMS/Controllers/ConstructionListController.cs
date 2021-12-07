using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using User_BL;
using ConstructionList_BL;
using DRMS_Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;

namespace AppOnDRMS.Controllers
{
    public class ConstructionListController : Controller
    {
        UserBL userbl = new UserBL();
        // GET: ConstructionList
        public ActionResult ConstructionList()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("Admin_Member_ID");
            if (cookie != null)
                return View();
            else
                return RedirectToAction("UserLogin", "User");
        }

        //[HttpPost]
        //[ValidateInput(false)]
        //public FileResult ConstructionList(string ExportData)
        //{
        //    string name = "project";
        //    string fileName = userbl.GetPDF(name);
        //    using (MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        //StringReader reader = new StringReader(ExportData);
        //        var pdfData = ChangeToStream(ExportData);
        //        Document PdfFile = new Document(PageSize.A4, 35, 35, 35, 50);
        //        PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
        //        PdfFile.Open();

        //        Font font = CreateJapaneseFont();

        //        //Add border to page
        //        PdfContentByte content = writer.DirectContent;
        //        Rectangle rectangle = new Rectangle(PdfFile.PageSize);
        //        rectangle.Left += PdfFile.LeftMargin;
        //        rectangle.Right -= PdfFile.RightMargin;
        //        rectangle.Top -= PdfFile.TopMargin;
        //        rectangle.Bottom += PdfFile.BottomMargin;
        //        content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
        //        content.Stroke();

        //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, pdfData, System.Text.Encoding.ASCII);
        //        PdfFile.Close();

        //        byte[] bytes = stream.ToArray();
        //        using (FileStream fs = new FileStream(Server.MapPath(Path.Combine("~/output/staff/", fileName)), FileMode.OpenOrCreate, FileAccess.Write))
        //        {
        //            fs.Write(bytes, 0, (int)bytes.Length);
        //        }

        //        // Write output PDF on user download path 
        //        Response.Buffer = true;
        //        Response.ContentType = "application/pdf";
        //        Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.BinaryWrite(bytes);
        //        Response.End();

        //        return File(stream.ToArray(), "application/pdf", fileName);
        //    }
        //}
        [HttpPost]
        public ActionResult ConstructionList(ConstructionListModel model)
        {
            ConstructionListBL constructionListBL = new ConstructionListBL();
            DateTime from_Date = new DateTime(model.startDate.Year, model.startDate.Month, model.startDate.Day);
            string f_Date = userbl.GetTextDateJapan(from_Date);

            DateTime to_Date = new DateTime(model.endDate.Year, model.endDate.Month, model.endDate.Day);
            string t_Date = userbl.GetTextDateJapan(to_Date);

            DataTable dt_Body = constructionListBL.GetPDFData(model);


            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 35, 35, 35, 50);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                Font font_Header = CreateJapaneseFontHeader();
                Font font = CreateJapaneseFont();
                Font font_Color = CreateJapaneseFont_Color();

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
                PdfPTable table = new PdfPTable(8);
                table.HorizontalAlignment = 1;
                table.TotalWidth = 525f;
                table.LockedWidth = true;
                float[] widths = new float[] { 50f, 90f, 195f, 30f, 30f, 30f, 50f, 50f};
                table.SetWidths(widths);
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                //Cell no 1
                PdfPCell cell = new PdfPCell();

                //Cell
                cell = new PdfPCell(new Phrase("工事別明細表", font_Header));
                cell.Colspan = 2;
                cell.Rowspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("工事名 : " + model.prjName, font_Header));
                cell.Colspan = 6;
                cell.VerticalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(f_Date + "～" + t_Date, font));
                cell.Colspan = 9;
                cell.BorderWidthTop = 0;
                cell.VerticalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("1 ページ", font_Header));
                cell.Colspan = 2;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.MinimumHeight = 35;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                table.AddCell(new PdfPCell(new Phrase("月/日", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                table.AddCell(new PdfPCell(new Phrase("社員名", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                table.AddCell(new PdfPCell(new Phrase("作業名", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                table.AddCell(new PdfPCell(new Phrase("就業時間帯", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, Colspan = 2 });
                table.AddCell(new PdfPCell(new Phrase("人工", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                table.AddCell(new PdfPCell(new Phrase("時間外", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                table.AddCell(new PdfPCell(new Phrase("深夜", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                
                //for (int i = 0; i < dt_Body.Rows.Count; i++)
                //{
                //    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["japan_day"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                //    if (dt_Body.Rows[i]["project_name"].ToString() == "休")
                //        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_name"].ToString(), font_Color)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                //    else
                //        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                //    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["working_hour"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                //    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["J_Basic"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                //    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["Col_5"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                //    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["Col_6"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                //    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["Col_7"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                //    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["Col_8"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                //    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["Col_9"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                //}

                //pdfDoc.Add(table);

                pdfWriter.CloseStream = false;
                pdfDoc.Close();

                byte[] bytes = myMemoryStream.ToArray();
                // Write out PDF from memory stream.                
                string FolderName = Server.MapPath("/output/staff/");

                string name = "project";
                string fileName = userbl.GetPDF(name);
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

        public Font CreateJapaneseFontHeader()
        {
            string font_folder = Server.MapPath("~/fonts/");
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFT, 13, Font.BOLD);
            return font;
        }

        public Font CreateJapaneseFont()
        {
            string font_folder = Server.MapPath("~/fonts/");
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFT);
            return font;
        }

        public Font CreateJapaneseFont_Color()
        {
            string font_folder = Server.MapPath("~/fonts/");
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFT, 11);
            font.Color = BaseColor.RED;
            return font;
        }

        public Stream ChangeToStream(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}