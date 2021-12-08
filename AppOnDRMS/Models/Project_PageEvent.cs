using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AppOnDRMS.Models
{
    public class Project_PageEvent : PdfPageEventHelper
    {
        public DataTable dt_staffName { get; set; }
        public Font font_Header { get; set; }
        public string prjName { get; set; }
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

            PdfPTable table = new PdfPTable(8);
            table.HorizontalAlignment = 1;
            table.TotalWidth = 525f;
            table.LockedWidth = true;
            float[] widths = new float[] { 65f, 100f, 150f, 40f, 40f, 60f, 40f, 30f };
            table.SetWidths(widths);
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            //Cell no 1
            PdfPCell cell = new PdfPCell();

            //Cell
            cell = new PdfPCell(new Phrase("工事別明細表", font_Header));
            cell.Colspan = 2;
            cell.Rowspan = 2;
            cell.BorderWidthRight = 0;
            cell.BorderWidthBottom = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("工事名 : " + prjName, font_Header));
            cell.Colspan = 6;
            cell.MinimumHeight = 35;
            cell.BorderWidthBottom = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(f_Date + "～" + t_Date, font_Normal));
            cell.Colspan = 3;
            cell.BorderWidthTop = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthBottom = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(page_Number + "ページ", font_Normal));
            cell.Colspan = 3;
            cell.BorderWidthTop = 0;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthBottom = 0;
            cell.MinimumHeight = 35;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);

            float cellHeight = pdfDoc.TopMargin;
            table.WriteSelectedRows(0, -1, 35, 807, pdfWriter.DirectContent);
        }
    }
}