using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using User_BL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

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

        [HttpPost]
        [ValidateInput(false)]
        public FileResult ConstructionList(string ExportData)
        {
            string name = "project";
            string fileName = userbl.GetPDF(name);
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                //StringReader reader = new StringReader(ExportData);
                var pdfData = ChangeToStream(ExportData);
                Document PdfFile = new Document(PageSize.A4, 35, 35, 35, 50);
                PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
                PdfFile.Open();

                Font font = CreateJapaneseFont();

                //Add border to page
                PdfContentByte content = writer.DirectContent;
                Rectangle rectangle = new Rectangle(PdfFile.PageSize);
                rectangle.Left += PdfFile.LeftMargin;
                rectangle.Right -= PdfFile.RightMargin;
                rectangle.Top -= PdfFile.TopMargin;
                rectangle.Bottom += PdfFile.BottomMargin;
                content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                content.Stroke();

                XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, pdfData, System.Text.Encoding.ASCII);
                PdfFile.Close();

                byte[] bytes = stream.ToArray();
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

                return File(stream.ToArray(), "application/pdf", fileName);
            }
        }

        public Font CreateJapaneseFont()
        {
            string font_folder = Server.MapPath("~/fonts/");
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFT);
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