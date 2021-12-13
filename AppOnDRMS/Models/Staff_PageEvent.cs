using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AppOnDRMS.Models
{
    public class Staff_PageEvent : PdfPageEventHelper
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
            cell = new PdfPCell(new Phrase(page_Number + "ページ", font_Header));
            cell.Colspan = 2;
            cell.PaddingRight = 10;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthBottom = 0;
            cell.MinimumHeight = 35;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(f_Date + "～" + t_Date, font_Normal));
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
}