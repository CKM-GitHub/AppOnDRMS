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
            float[] widths = new float[] { 56f, 180f, 75f, 30f, 28f, 28f, 50f, 50f, 28f };
            table.SetWidths(widths);
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            //Cell no 1
            PdfPCell cell = new PdfPCell();
            //Cell
            cell = new PdfPCell(new Phrase("社員名：" + dt_staffName.Rows[0]["member_name"].ToString() + "", font_Header));
            cell.Colspan = 4;
            cell.PaddingLeft = 10;
            cell.BorderWidthLeft = 2f;
            cell.BorderWidthRight = 0;
            cell.BorderWidthTop = 2f;
            cell.BorderWidthBottom = 0;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("《社員別明細表》", font_Header));
            cell.Colspan = 3;
            cell.BorderWidthRight = 0;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthTop = 2f;
            cell.BorderWidthBottom = 0;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(page_Number + "ページ", font_Header));
            cell.Colspan = 2;
            cell.PaddingRight = 10;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 2f;
            cell.BorderWidthTop = 2f;
            cell.BorderWidthBottom = 0;
            cell.MinimumHeight = 32.5f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(f_Date + "～" + t_Date, font_Normal));
            cell.Colspan = 9;
            cell.PaddingLeft = 10;
            cell.BorderWidthTop = 0;
            cell.BorderWidthBottom = 2f;
            cell.BorderWidthLeft = 2f;
            cell.BorderWidthRight = 2f;
            cell.MinimumHeight = 32.5f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);

            table.AddCell(new PdfPCell(new Phrase("月/日", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 2f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("工事名", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("就業時間", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("基本", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("時外", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("深夜", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("所定外", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("法定休", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 0.3f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("休深", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 2f, BorderWidthBottom = 0.3f, BorderWidthTop = 0.3f, PaddingBottom = 5f });

            float cellHeight = pdfDoc.TopMargin;
            table.WriteSelectedRows(0, -1, 35, 807, pdfWriter.DirectContent);
        }
    }
}