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
using System.Web.Hosting;

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
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFT,11);
            return font;
        }
        public Font CreateJapaneseFont_Color()
        {
            string font_folder = Server.MapPath("~/fonts/");
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFT,11);
            font.Color = BaseColor.RED;
            return font;
        }
        [HttpPost]
        public ActionResult Employee(DateDifferenceModel model)
        {
            UserLoginModel login_Model = new UserLoginModel();
            login_Model.member_id = model.Radio_Value;
            string staff_Name = user_bl.GetUser(login_Model);
            DataTable dt_staffName = JsonConvert.DeserializeObject<DataTable>(staff_Name);

            DateTime from_Date = new DateTime(model.From_Date.Year, model.From_Date.Month, model.From_Date.Day);
            string f_Date = user_bl.GetTextDateJapan(from_Date);

            DateTime to_Date = new DateTime(model.To_Date.Year, model.To_Date.Month, model.To_Date.Day);
            string t_Date = user_bl.GetTextDateJapan(to_Date);

            DataTable dt_Body = user_bl.GetStaff_PDFData(model);


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
                PdfPTable table = new PdfPTable(9);
                table.HorizontalAlignment = 1;
                table.TotalWidth = 525f;
                table.LockedWidth = true;
                float[] widths = new float[] { 50f, 180f, 75f, 30f, 30f, 30f, 50f, 50f, 30f };
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
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("《社員別明細表》", font_Header));
                cell.Colspan = 3;
                cell.BorderWidthRight = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthBottom = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("1 ページ", font_Header));
                cell.Colspan = 2;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthBottom = 0;
                cell.MinimumHeight = 35;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
               
                cell = new PdfPCell(new Phrase(f_Date+ "～"+t_Date,font));
                cell.Colspan = 9;
                cell.BorderWidthTop = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                table.AddCell(new PdfPCell(new Phrase("月/日", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight=20f});
                table.AddCell(new PdfPCell(new Phrase("工事名", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });                
                table.AddCell(new PdfPCell(new Phrase("就業時間", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                table.AddCell(new PdfPCell(new Phrase("基本", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                table.AddCell(new PdfPCell(new Phrase("時外", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                table.AddCell(new PdfPCell(new Phrase("深夜", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                table.AddCell(new PdfPCell(new Phrase("所定外", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f } );
                table.AddCell(new PdfPCell(new Phrase("法定休", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f } );
                table.AddCell(new PdfPCell(new Phrase("休深", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f } );

                int work_day_count = 0;
                int work_day_hour = 0;
                for(int i=0;i<dt_Body.Rows.Count;i++)
                {
                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["japan_day"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                    if(dt_Body.Rows[i]["project_name"].ToString() == "休")
                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_name"].ToString(), font_Color)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                    else
                    {
                        table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        work_day_count++;
                        work_day_hour = work_day_hour + 8;
                    }
                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["working_hour"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                    table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["J_Basic"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                    table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                    table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                    table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                    table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                    table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                }

                //Total footer
                cell = new PdfPCell(new Phrase("合計", font)); 
                cell.BorderWidthRight = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell); //1st cell
                cell = new PdfPCell(new Phrase(" ", font));
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0;
                table.AddCell(cell);//2nd cell
                cell = new PdfPCell(new Phrase("出勤："+ work_day_count + " 日 ", font));
                cell.BorderWidthLeft = 0;
                cell.MinimumHeight = 20f;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);//3rd cell
                cell = new PdfPCell(new Phrase(work_day_hour.ToString(), font));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);//4th cell

                table.AddCell(new Phrase(" ", font)); //5th cell
                table.AddCell(new Phrase(" ", font));//6th cell
                table.AddCell(new Phrase(" ", font));//7th cell
                table.AddCell(new Phrase(" ", font));//8th cell
                table.AddCell(new Phrase(" ", font));//9th cell

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
        public class PageEventHeader : PdfPageEventHelper
        {
           public DataTable dt_staffName { get; set; }
           public Font font_Header { get; set; }
           public string f_Date { get; set; }
           public string t_Date { get; set; }
           public Font font_Normal { get; set; }
            int page_Number = 0;

            public override void OnEndPage(PdfWriter pdfWriter, Document pdfDoc)
            {
                page_Number = page_Number + 1;
                //Add border to page
                PdfContentByte content = pdfWriter.DirectContent;
                Rectangle rectangle = new Rectangle(pdfDoc.PageSize);
                rectangle.Left += pdfDoc.LeftMargin;
                rectangle.Right -= pdfDoc.RightMargin;
                rectangle.Top -= pdfDoc.TopMargin;
                rectangle.Bottom += pdfDoc.BottomMargin;
                content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                content.Stroke();

                PdfPTable table = new PdfPTable(9);
                table.HorizontalAlignment = 1;
                table.TotalWidth = 525f;
                table.LockedWidth = true;
                float[] widths = new float[] { 50f, 180f, 75f, 30f, 30f, 30f, 50f, 50f, 30f };
                table.SetWidths(widths);
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;
               
                //Cell no 1
                PdfPCell cell = new PdfPCell();
                //Cell
                cell = new PdfPCell(new Phrase("社員名：" + dt_staffName.Rows[0]["member_name"].ToString() + "", font_Header));
                cell.Colspan = 4;
                cell.BorderWidthRight = 0;
                cell.BorderWidthBottom = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("《社員別明細表》", font_Header));
                cell.Colspan = 3;
                cell.BorderWidthRight = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthBottom = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(page_Number+"ページ", font_Header));
                cell.Colspan = 2;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthBottom = 0;
                cell.MinimumHeight = 35;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(f_Date + "～" + t_Date,font_Normal));
                cell.Colspan = 9;
                cell.BorderWidthTop = 0;
                cell.BorderWidthBottom = 0;
                cell.MinimumHeight = 35;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                float cellHeight = pdfDoc.TopMargin;
                table.WriteSelectedRows(0, -1, 35, 807, pdfWriter.DirectContent);
            }
        }
       
        [HttpPost]
        public ActionResult EmployeeList(DateDifferenceModel model)
        {
            UserLoginModel login_Model = new UserLoginModel();
            login_Model.member_id = model.Radio_Value;
            string staff_Name = user_bl.GetUser(login_Model);
            DataTable dt_staffName = JsonConvert.DeserializeObject<DataTable>(staff_Name);

            DateTime from_Date = new DateTime(model.From_Date.Year, model.From_Date.Month, model.From_Date.Day);
            string f_Date = user_bl.GetTextDateJapan(from_Date);

            DateTime to_Date = new DateTime(model.To_Date.Year, model.To_Date.Month, model.To_Date.Day);
            string t_Date = user_bl.GetTextDateJapan(to_Date);

            DataTable dt_Body = user_bl.GetStaff_PDFData(model);

            Font font_Header = CreateJapaneseFontHeader();
            Font font = CreateJapaneseFont();
            Font font_Color = CreateJapaneseFont_Color();

            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                using (Document pdfDoc = new Document(PageSize.A4, 35, 35, 100, 62))
                {
                    var header = new PageEventHeader();
                    using (PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream))
                    {
                        pdfDoc.Open();

                        pdfWriter.PageEvent = header;
                        header.dt_staffName = dt_staffName;
                        header.font_Header = font_Header;
                        header.f_Date = f_Date;
                        header.t_Date = t_Date;
                        header.font_Normal = font;

                        ////Add border to page
                        //PdfContentByte content = pdfWriter.DirectContent;
                        //Rectangle rectangle = new Rectangle(pdfDoc.PageSize);
                        //rectangle.Left += pdfDoc.LeftMargin;
                        //rectangle.Right -= pdfDoc.RightMargin;
                        //rectangle.Top -= pdfDoc.TopMargin;
                        //rectangle.Bottom += pdfDoc.BottomMargin;
                        //content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                        //content.Stroke();

                        //Table
                        PdfPTable table = new PdfPTable(9);
                        table.HorizontalAlignment = 1;
                        table.TotalWidth = 525f;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 50f, 180f, 75f, 30f, 30f, 30f, 50f, 50f, 30f };
                        table.SetWidths(widths);
                        table.SpacingBefore = 20f;
                        table.SpacingAfter = 30f;

                        //Cell no 1
                        PdfPCell cell = new PdfPCell();

                        ////Cell
                        //cell = new PdfPCell(new Phrase("社員名：" + dt_staffName.Rows[0]["member_name"].ToString() + "", font_Header));
                        //cell.Colspan = 4;
                        //cell.BorderWidthRight = 0;
                        //cell.BorderWidthBottom = 0;
                        //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //table.AddCell(cell);
                        //cell = new PdfPCell(new Phrase("《社員別明細表》", font_Header));
                        //cell.Colspan = 3;
                        //cell.BorderWidthRight = 0;
                        //cell.BorderWidthLeft = 0;
                        //cell.BorderWidthBottom = 0;
                        //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //table.AddCell(cell);
                        //cell = new PdfPCell(new Phrase("1 ページ", font_Header));
                        //cell.Colspan = 2;
                        //cell.BorderWidthLeft = 0;
                        //cell.BorderWidthBottom = 0;
                        //cell.MinimumHeight = 35;
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //table.AddCell(cell);

                        //cell = new PdfPCell(new Phrase(f_Date + "～" + t_Date, font));
                        //cell.Colspan = 9;
                        //cell.BorderWidthTop = 0;
                        //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //table.AddCell(cell);

                        table.AddCell(new PdfPCell(new Phrase("月/日", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        table.AddCell(new PdfPCell(new Phrase("工事名", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        table.AddCell(new PdfPCell(new Phrase("就業時間", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        table.AddCell(new PdfPCell(new Phrase("基本", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        table.AddCell(new PdfPCell(new Phrase("時外", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        table.AddCell(new PdfPCell(new Phrase("深夜", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        table.AddCell(new PdfPCell(new Phrase("所定外", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        table.AddCell(new PdfPCell(new Phrase("法定休", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        table.AddCell(new PdfPCell(new Phrase("休深", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });

                        int work_day_count = 0;
                        int work_day_hour = 0;
                        for (int i = 0; i < dt_Body.Rows.Count; i++)
                        {
                            table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["japan_day"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                            if (dt_Body.Rows[i]["project_name"].ToString() == "休")
                                table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_name"].ToString(), font_Color)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                            else
                            {
                                table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                                work_day_count++;
                                work_day_hour = work_day_hour + 8;
                            }
                            table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["working_hour"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                            table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["J_Basic"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                            table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                            table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                            table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                            table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                            table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f });
                        }

                        //Total footer
                        cell = new PdfPCell(new Phrase("合計", font));
                        cell.BorderWidthRight = 0;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell); //1st cell
                        cell = new PdfPCell(new Phrase(" ", font));
                        cell.BorderWidthLeft = 0;
                        cell.BorderWidthRight = 0;
                        table.AddCell(cell);//2nd cell
                        cell = new PdfPCell(new Phrase("出勤：" + work_day_count + " 日 ", font));
                        cell.BorderWidthLeft = 0;
                        cell.MinimumHeight = 20f;
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);//3rd cell
                        cell = new PdfPCell(new Phrase(work_day_hour.ToString(), font));
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);//4th cell

                        table.AddCell(new Phrase(" ", font)); //5th cell
                        table.AddCell(new Phrase(" ", font));//6th cell
                        table.AddCell(new Phrase(" ", font));//7th cell
                        table.AddCell(new Phrase(" ", font));//8th cell
                        table.AddCell(new Phrase(" ", font));//9th cell

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
                }
            }
            return View();
        }
    }
}