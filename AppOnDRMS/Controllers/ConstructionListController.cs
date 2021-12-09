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
using AppOnDRMS.Models;

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
            //Change Japanese Calendar
            ConstructionListBL constructionListBL = new ConstructionListBL();
            DateTime from_Date = new DateTime(model.startDate.Year, model.startDate.Month, model.startDate.Day);
            string f_Date = userbl.GetTextDateJapan(from_Date);
            DateTime to_Date = new DateTime(model.endDate.Year, model.endDate.Month, model.endDate.Day);
            string t_Date = userbl.GetTextDateJapan(to_Date);

            //get pdf body data
            DataTable dt_Body = constructionListBL.GetPDFData(model);

            //Header, body and color font create
            PDF_Font font_Class = new PDF_Font();
            string font_folder = Server.MapPath("~/fonts/");
            Font font_Header = font_Class.CreateJapaneseFontHeader(font_folder);
            Font font = font_Class.CreateJapaneseFont(font_folder);
            Font font_Color = font_Class.CreateJapaneseFont_Color(font_folder);

            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                using (Document pdfDoc = new Document(PageSize.A4, 35, 35, 100, 62))
                {
                    //calling for pdf header and page number
                    var header = new Project_PageEvent();
                    using (PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream))
                    {
                        pdfDoc.Open();
                        //for pdf header 
                        pdfWriter.PageEvent = header;
                        header.prjName = model.prjName;
                        header.font_Header = font_Header;
                        header.f_Date = f_Date;
                        header.t_Date = t_Date;
                        header.font_Normal = font;

                        //Table
                        PdfPTable table = new PdfPTable(8);
                        table.HorizontalAlignment = 1;
                        table.TotalWidth = 525f;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 65f, 100f, 150f, 40f, 40f, 60f, 40f, 30f };
                        table.SetWidths(widths);
                        table.SpacingBefore = 20f;
                        table.SpacingAfter = 30f;

                        table.AddCell(new PdfPCell(new Phrase("月/日", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) });
                        table.AddCell(new PdfPCell(new Phrase("社員名", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) });
                        table.AddCell(new PdfPCell(new Phrase("作業名", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) });
                        table.AddCell(new PdfPCell(new Phrase("就業時間帯", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, Colspan = 2, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) });
                        table.AddCell(new PdfPCell(new Phrase("人工", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) });
                        table.AddCell(new PdfPCell(new Phrase("時間外", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) });
                        table.AddCell(new PdfPCell(new Phrase("深夜", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) });

                        string work_date = string.Empty;
                        string project_work_name = string.Empty;
                        string att_start_time = string.Empty;
                        string att_end_time = string.Empty;
                        string artificial = string.Empty;
                        int total_time = 0; int count = 0;
                        for (int i = 0; i < dt_Body.Rows.Count; i++)
                        {
                            if (work_date != dt_Body.Rows[i]["work_date"].ToString())
                            {
                                work_date = dt_Body.Rows[i]["work_date"].ToString();
                                count = 1;
                            }
                            else
                                count += 1;
                            if (dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                                table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                            else
                            {
                                if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["work_date"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0 });
                                else
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["work_date"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                            }

                            if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                table.AddCell(new PdfPCell(new Phrase(count + ": " + dt_Body.Rows[i]["member_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                            else
                                table.AddCell(new PdfPCell(new Phrase(count + ": " + dt_Body.Rows[i]["member_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthTop = 0 });

                            if (dt_Body.Rows[i]["project_work_name"].ToString() == project_work_name && dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                            {
                                if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                    table.AddCell(new PdfPCell(new Phrase("〃", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                                else
                                    table.AddCell(new PdfPCell(new Phrase("〃", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthTop = 0 });
                            }
                            else
                            {
                                project_work_name = dt_Body.Rows[i]["project_work_name"].ToString();
                                if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_work_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                                else
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_work_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                            }

                            if (dt_Body.Rows[i]["att_start_time"].ToString() == att_start_time && dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                            {
                                if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                    table.AddCell(new PdfPCell(new Phrase("〃", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                                else
                                    table.AddCell(new PdfPCell(new Phrase("〃", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthTop = 0 });
                            }
                            else
                            {
                                att_start_time = dt_Body.Rows[i]["att_start_time"].ToString();
                                if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_start_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                                else
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_start_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthTop = 0 });
                            }

                            if (dt_Body.Rows[i]["att_end_time"].ToString() == att_end_time && dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                            {
                                if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                    table.AddCell(new PdfPCell(new Phrase("〃", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                                else
                                    table.AddCell(new PdfPCell(new Phrase("〃", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthTop = 0 });
                            }
                            else
                            {
                                att_end_time = dt_Body.Rows[i]["att_end_time"].ToString();
                                if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_end_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                                else
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_end_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthTop = 0 });
                            }

                            if (dt_Body.Rows[i]["artificial"].ToString() == "1.0")
                                total_time += 1;

                            if (dt_Body.Rows[i]["artificial"].ToString() == artificial && dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                            {
                                if(artificial == "1.0")
                                {
                                    if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                        table.AddCell(new PdfPCell(new Phrase("〃", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                                    else
                                        table.AddCell(new PdfPCell(new Phrase("〃", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthTop = 0 });
                                }
                                else
                                if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["artificial"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                                else
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["artificial"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthTop = 0 });
                            }
                            else
                            {
                                artificial = dt_Body.Rows[i]["artificial"].ToString();
                                if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["artificial"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                                else
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["artificial"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthTop = 0 });
                            }

                            if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["off_hours"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                            else
                                table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["off_hours"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthTop = 0 });

                            if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["midnight"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                            else
                                table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["midnight"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthTop = 0 });
                        }

                        //Cell
                        PdfPCell cell = new PdfPCell();
                        //Total footer
                        cell = new PdfPCell(new Phrase("合計", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) };
                        cell.Colspan = 5;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);
                        cell = new PdfPCell(new Phrase(total_time + " 人工", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) };
                        cell.BorderWidthLeft = 0;
                        cell.MinimumHeight = 20f;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);
                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) });
                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) });

                        pdfDoc.Add(table);
                        pdfWriter.CloseStream = false;
                        pdfDoc.Close();

                        byte[] bytes = myMemoryStream.ToArray();
                        // Write out PDF from memory stream.                
                        string FolderName = Server.MapPath("~/output/project/");

                        string name = "project";
                        string fileName = userbl.GetPDF(name);
                        using (FileStream fs = new FileStream(Server.MapPath(Path.Combine("~/output/project/", fileName)), FileMode.OpenOrCreate, FileAccess.Write))
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
                }

                ////Add border to page
                //PdfContentByte content = pdfWriter.DirectContent;
                //Rectangle rectangle = new Rectangle(pdfDoc.PageSize);
                //rectangle.Left += pdfDoc.LeftMargin;
                //rectangle.Right -= pdfDoc.RightMargin;
                //rectangle.Top -= pdfDoc.TopMargin;
                //rectangle.Bottom += pdfDoc.BottomMargin;
                //content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                //content.Stroke();

            }
            return View();
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