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
                using (Document pdfDoc = new Document(PageSize.A4, 35, 35, 120, 62))
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

                        string work_date = string.Empty;
                        string project_work_name = string.Empty;
                        string att_start_time = string.Empty;
                        string att_end_time = string.Empty;
                        string artificial = string.Empty;
                        string total_time = string.Empty; int count = 0;

                        float line_height = 20f; float tline_height; int row_add = 0;
                        int pg_count = dt_Body.Rows.Count / 33;
                        int last_pg = dt_Body.Rows.Count % 33;

                        if (last_pg <= 29)
                            tline_height = 20f;
                        else if (last_pg == 30)
                            tline_height = 19.4117f;
                        else if (last_pg == 31)
                            tline_height = 18.85f;
                        else
                        {
                            row_add = 33 - last_pg;
                            last_pg = 0;
                            tline_height = 20f;
                        }
                        for (int i = 0; i < dt_Body.Rows.Count; i++)
                        {
                            if (i >= (pg_count * 33))
                                line_height = tline_height;

                            if (work_date != dt_Body.Rows[i]["work_date"].ToString())
                            {
                                work_date = dt_Body.Rows[i]["work_date"].ToString();
                                count = 1;
                            }
                            else
                                count += 1;
                            //col 1  
                            if (dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                                table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthTop = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, PaddingBottom = 5f });
                            else
                            {
                                if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["work_date"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                else
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["work_date"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                            }
                            //col 2
                            if (dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                                table.AddCell(new PdfPCell(new Phrase(count + ": " + dt_Body.Rows[i]["member_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthTop = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, PaddingBottom = 5f });
                            else
                            {
                                if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                    table.AddCell(new PdfPCell(new Phrase(count + ": " + dt_Body.Rows[i]["member_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                else
                                    table.AddCell(new PdfPCell(new Phrase(count + ": " + dt_Body.Rows[i]["member_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                            }
                            //col3
                            if (dt_Body.Rows[i]["project_work_name"].ToString() == project_work_name)
                            {
                                if (dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                                    table.AddCell(new PdfPCell(new Phrase("〃", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0f, PaddingBottom = 5f });
                                else
                                {
                                    if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_work_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                    else
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_work_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                }
                            }
                            else
                            {
                                project_work_name = dt_Body.Rows[i]["project_work_name"].ToString();
                                if (dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_work_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0f, PaddingBottom = 5f });
                                else
                                {
                                    if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_work_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                    else
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_work_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                }
                            }
                            //col 4
                            if (dt_Body.Rows[i]["att_start_time"].ToString() == att_start_time)
                            {
                                if (dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                                    table.AddCell(new PdfPCell(new Phrase("〃", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0f, PaddingBottom = 5f });
                                else
                                {
                                    if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_start_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                    else
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_start_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                }
                            }
                            else
                            {
                                att_start_time = dt_Body.Rows[i]["att_start_time"].ToString();
                                if (dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_start_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0f, PaddingBottom = 5f });
                                else
                                {
                                    if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_start_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                    else
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_start_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                }
                            }
                            //col 5
                            if (dt_Body.Rows[i]["att_end_time"].ToString() == att_end_time)
                            {
                                if (dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                                    table.AddCell(new PdfPCell(new Phrase("〃", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0f, PaddingBottom = 5f });
                                else
                                {
                                    if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_end_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                    else
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_end_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                }
                            }
                            else
                            {
                                att_end_time = dt_Body.Rows[i]["att_end_time"].ToString();
                                if (dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_end_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0f, PaddingBottom = 5f });
                                else
                                {
                                    if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_end_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                    else
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["att_end_time"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                }
                            }
                            //col 6
                            if (dt_Body.Rows[i]["artificial"].ToString() == artificial)
                            {
                                if (dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                                    table.AddCell(new PdfPCell(new Phrase("〃", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0f, PaddingBottom = 5f });
                                else
                                {
                                    if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["artificial"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                    else
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["artificial"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                }
                            }
                            else
                            {
                                artificial = dt_Body.Rows[i]["artificial"].ToString();
                                if (dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["artificial"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0f, PaddingBottom = 5f });
                                else
                                {
                                    if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["artificial"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                    else
                                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["artificial"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                }
                            }
                            //col 7
                            if (dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                                table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["off_hours"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0f, PaddingBottom = 5f });
                            else
                            {
                                if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["off_hours"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                else
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["off_hours"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                            }
                            //col 8
                            if (dt_Body.Rows[i]["work_date"].ToString() == work_date && count != 1)
                                table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["midnight"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0f, PaddingBottom = 5f });
                            else
                            {
                                if (i + 1 < dt_Body.Rows.Count && dt_Body.Rows[i + 1]["work_date"].ToString() == work_date)
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["midnight"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthBottom = 0, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                                else
                                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["midnight"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0f, BorderWidthTop = 0.6f, PaddingBottom = 5f });
                            }
                        }
                        total_time = dt_Body.Rows.Count + ".0";

                        for (int i = 0; i < row_add; i++)
                        {
                            AddNullRoll_To_Table(table, font, line_height);
                        }
                        //int pg_count = table.Rows.Count / 33;
                        //int last_pg = table.Rows.Count % 33;
                        //if (last_pg != 0)
                        //{
                        while (last_pg <= 28)
                        {
                            AddNullRoll_To_Table(table, font, line_height);
                            last_pg += 1;
                        }
                        //}

                        //Cell
                        PdfPCell cell = new PdfPCell();
                        //Total footer
                        cell = new PdfPCell(new Phrase(" 合計", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) };
                        cell.Colspan = 5;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.PaddingBottom = 5f;
                        cell.BorderWidthLeft = 0.3f;
                        cell.BorderWidthRight = 1f;
                        table.AddCell(cell);
                        cell = new PdfPCell(new Phrase(total_time + " 人工", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) };
                        cell.BorderWidthLeft = 0.3f;
                        cell.MinimumHeight = line_height;
                        cell.BorderWidthRight = 1f;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.PaddingBottom = 5f;
                        table.AddCell(cell);
                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthRight = 1f });
                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthRight = 1f });

                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { Colspan = 8, MinimumHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0 });
                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { Colspan = 8, MinimumHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { Colspan = 8, MinimumHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0, BorderWidthTop = 0 });

                        pdfDoc.Add(table);
                        pdfWriter.CloseStream = false;
                        pdfDoc.Close();

                        byte[] bytes = myMemoryStream.ToArray();
                        // Write out PDF from memory stream.
                        if (!Directory.Exists("~/output"))
                            Directory.CreateDirectory(Server.MapPath("~/output"));
                        if (!Directory.Exists("~/output/project"))
                            Directory.CreateDirectory(Server.MapPath("~/output/project"));
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
            }
            return View();
        }

        public static void AddNullRoll_To_Table(PdfPTable table, Font font, float line_height)
        {
            table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = line_height, BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
        }
    }
}