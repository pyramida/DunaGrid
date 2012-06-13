using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DunaGrid.columns
{
    public interface IColumn
    {
        int Width { get; set; } //sirka bunky
        int MinimalWidth { get; set; } //minimalni sirka bunky
        string HeadText { get; set; } //text, co se vypise v hlavicce sloupce
        bool Visible { get; set; } //je sloupec viditelny?
        bool ReadOnly { get; set; } //je sloupec jenom pro cteni?
        DunaGrid ParentGrid { get; set; } //odkaz na grid, ve kterem se sloupec nachazi
        int DataSourceColumnIndex { set; get; } //cislo sloupce v datovem zdroji (od nuly)
        bool Elastic { set; get; }

        void renderHead(GraphicsContext g, ColumnContext context);
        void renderCell(GraphicsContext g, object value, CellRenderState render_state = CellRenderState.Normal);
        void renderCellBackground(GraphicsContext g, CellRenderState render_state = CellRenderState.Normal);
    }
}
