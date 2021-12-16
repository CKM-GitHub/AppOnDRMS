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
            rectangle.BorderWidthBottom = 2f;
            rectangle.BorderWidthLeft = 2f;
            rectangle.BorderWidthRight = 2f;
            content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            content.SetLineWidth(2);
            //content.EoFill();
            content.Stroke();

            PdfPTable mainTable = new PdfPTable(1);
            mainTable.HorizontalAlignment = 1;
            mainTable.TotalWidth = 527f;
            float[] widths_ = new float[] { 527 };
            mainTable.SetWidths(widths_);

            //Cell no 1
            PdfPCell cell_main = new PdfPCell();
            //Cell
            cell_main = new PdfPCell(new Phrase(" "));
            cell_main.PaddingLeft = 1;
            cell_main.BorderWidthLeft = 2f;
            cell_main.BorderWidthRight = 2f;
            cell_main.BorderWidthTop = 2f;
            cell_main.BorderWidthBottom = 0f;
            cell_main.MinimumHeight = 85;
            mainTable.AddCell(cell_main);

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
            cell.BorderWidthLeft = 0.3f;
            cell.BorderWidthRight = 1f;
            cell.BorderWidthTop = 0.3f;
            cell.BorderWidthBottom = 0.3f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("工事名 : " + prjName, font_Header));
            cell.Colspan = 6;
            cell.PaddingLeft = 10;
            cell.MinimumHeight = 32.5f;
            cell.BorderWidthBottom = 0;
            cell.BorderWidthTop = 0.3f;
            cell.BorderWidthRight = 0.3f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(f_Date + "～" + t_Date, font_Normal));
            cell.Colspan = 3;
            cell.PaddingLeft = 10;
            cell.BorderWidthTop = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthBottom = 0.3f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(page_Number + "ページ", font_Normal));
            cell.Colspan = 3;
            cell.PaddingRight = 10;
            cell.BorderWidthTop = 0;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthBottom = 0.3f;
            cell.BorderWidthRight = 0.3f;
            cell.MinimumHeight = 32.5f;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);

            table.AddCell(new PdfPCell(new Phrase("月/日", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 2f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("社員名", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 2f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("作業名", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 2f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("就業時間帯", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, Colspan = 2, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 2f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("人工", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 2f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("時間外", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 2f, PaddingBottom = 5f });
            table.AddCell(new PdfPCell(new Phrase("深夜", font_Normal)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, FixedHeight = 20f, BackgroundColor = new iTextSharp.text.BaseColor(228, 246, 248), BorderWidthLeft = 0.3f, BorderWidthRight = 1f, BorderWidthBottom = 0.3f, BorderWidthTop = 2f, PaddingBottom = 5f });

            table.WriteSelectedRows(0, -1, 35, 807, pdfWriter.DirectContent);
            mainTable.WriteSelectedRows(0, -1, 34, 807, pdfWriter.DirectContent);
            mainTable.AddCell(table);
        }
    }
}