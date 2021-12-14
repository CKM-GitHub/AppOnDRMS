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
using AppOnDRMS.Models;

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
            //get staff Name
            UserLoginModel login_Model = new UserLoginModel();
            login_Model.member_id = model.Radio_Value;
            string staff_Name = user_bl.GetUser(login_Model);
            DataTable dt_staffName = JsonConvert.DeserializeObject<DataTable>(staff_Name);

            //Change Japanese Calendar
            DateTime from_Date = new DateTime(model.From_Date.Year, model.From_Date.Month, model.From_Date.Day);
            string f_Date = user_bl.GetTextDateJapan(from_Date);

            DateTime to_Date = new DateTime(model.To_Date.Year, model.To_Date.Month, model.To_Date.Day);
            string t_Date = user_bl.GetTextDateJapan(to_Date);

            //get pdf body from D_WorkingHistory
            DataTable dt_Body = user_bl.GetStaff_PDFData(model);

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
                    var header = new Staff_PageEvent();
                    using (PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, myMemoryStream))
                    {
                        pdfDoc.Open();
                        //for pdf header 
                        pdfWriter.PageEvent = header;
                        header.dt_staffName = dt_staffName;
                        header.font_Header = font_Header;
                        header.f_Date = f_Date;
                        header.t_Date = t_Date;
                        header.font_Normal = font;

                        //Table
                        PdfPTable table = new PdfPTable(9);
                        table.HorizontalAlignment = 1;
                        table.TotalWidth = 525f;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 56f, 180f, 75f, 30f, 28f, 28f, 50f, 50f, 28f };
                        table.SetWidths(widths);
                        table.SpacingBefore = 20f;
                        table.SpacingAfter = 30f;

                        int work_day_count = 0;
                        int work_day_hour = 0;
                        for (int i = 0; i < dt_Body.Rows.Count; i++)
                        {
                            table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["japan_day"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f,BorderWidthLeft = 2f,BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingLeft = 5f, PaddingBottom = 5f });
                            if (dt_Body.Rows[i]["project_name"].ToString() == "休")
                                table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_name"].ToString(), font_Color)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                            else
                            {
                                table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["project_name"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                                work_day_count++;
                                work_day_hour = work_day_hour + 8;
                            }
                            table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["working_hour"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                            table.AddCell(new PdfPCell(new Phrase(dt_Body.Rows[i]["J_Basic"].ToString(), font)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                            table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                            table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                            table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                            table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                            table.AddCell(new PdfPCell(new Phrase("", font)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 2f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                        }

                        //float a = font_Class.CalculatePdfPTableHeight(table);
                        int pg_count = table.Rows.Count / 33;
                        int last_pg = table.Rows.Count % 33;
                        //if(last_pg != 0)
                        //{
                            while (last_pg <= 28)
                            {
                                table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = 20f, BorderWidthLeft = 2f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                                table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                                table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                                table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                                table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                                table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                                table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                                table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                                table.AddCell(new PdfPCell(new Phrase("", font)) { FixedHeight = 20f, BorderWidthLeft = 0.3f, BorderWidthRight = 2f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
                                last_pg += 1;
                            }
                        //}

                        //Cell
                        PdfPCell cell = new PdfPCell();
                        //Total footer
                        cell = new PdfPCell(new Phrase("合計", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) };
                        cell.BorderWidthRight = 0;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.PaddingBottom = 5f;
                        cell.BorderWidthLeft = 2f;
                        table.AddCell(cell); //1st cell
                        cell = new PdfPCell(new Phrase(" ", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) };
                        cell.BorderWidthLeft = 0;
                        cell.BorderWidthRight = 0;
                        cell.PaddingBottom = 5f;
                        table.AddCell(cell);//2nd cell
                        cell = new PdfPCell(new Phrase("出勤：" + work_day_count + " 日 ", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) };
                        cell.BorderWidthLeft = 0;
                        cell.MinimumHeight = 20f;
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.PaddingBottom = 5f;
                        table.AddCell(cell);//3rd cell
                        cell = new PdfPCell(new Phrase(work_day_hour.ToString(), font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248) };
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.PaddingBottom = 5f;
                        table.AddCell(cell);//4th cell

                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), PaddingBottom = 5f }); //5th cell
                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), PaddingBottom = 5f });//6th cell
                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), PaddingBottom = 5f });//7th cell
                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), PaddingBottom = 5f });//8th cell
                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), PaddingBottom = 5f, BorderWidthRight = 2f });//9th cell

                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { Colspan = 9, MinimumHeight = 20, BorderWidthLeft = 2f, BorderWidthRight = 2f, BorderWidthBottom = 0 });
                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { Colspan = 9, MinimumHeight = 20, BorderWidthLeft = 2f, BorderWidthRight = 2f, BorderWidthBottom = 0, BorderWidthTop = 0 });
                        table.AddCell(new PdfPCell(new Phrase(" ", font)) { Colspan = 9, MinimumHeight = 20, BorderWidthLeft = 2f, BorderWidthRight = 2f, BorderWidthBottom = 2f, BorderWidthTop = 0 });

                        pdfDoc.Add(table);

                        pdfWriter.CloseStream = false;
                        pdfDoc.Close();

                        byte[] bytes = myMemoryStream.ToArray();
                        // Write out PDF from memory stream.                
                        string FolderName = Server.MapPath("~/output/staff/");

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