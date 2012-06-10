using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DunaGrid
{
    public interface IColumn
    {
        int Height { get; set; }
        int Width { get; set; }
        string HeadText { get; set; }
        bool Visible { get; set; }
        bool ReadOnly { get; set; }

        void renderHead(Graphics g);
        void renderCell(Graphics g, object value);
    }
}
