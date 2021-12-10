using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AppOnDRMS.Models
{
    public class PDF_Font
    {
        public Font CreateJapaneseFontHeader(string font_folder)
        {
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new iTextSharp.text.Font(baseFT, 13, Font.BOLD);
            return font;
        }
        public Font CreateJapaneseFont(string font_folder)
        {
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new iTextSharp.text.Font(baseFT, 11);
            return font;
        }
        public Font CreateJapaneseFont_Color(string font_folder)
        {
            BaseFont baseFT = BaseFont.CreateFont(font_folder + "SIMSUN.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFT, 11);
            font.Color = BaseColor.RED;
            return font;
        }
        public float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();

                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }
    }
}