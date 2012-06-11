using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DunaGrid.columns
{
    /// <summary>
    /// predek vsech typu sloupcu
    /// </summary>
    public abstract class AbstractColumn : IColumn
    {
        public const int AUTOSIZE = -1;

        protected int width = AUTOSIZE;
        protected string column_name = null;
        protected bool visible = true;
        protected bool read_only = false;
        protected DunaGrid parent = null;

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                if (value > 0 || value == AUTOSIZE)
                {
                    this.width = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(); //TODO: pridat zpravu. Neco ve smyslu ze hodnota musi byt vetsi nez nula nebo povolena zaporna konstanta
                }
            }
        }

        public DunaGrid ParentGrid
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }

        public string HeadText
        {
            get
            {
                if (this.column_name == null)
                {
                    return "Unnamed column";
                }
                else
                {
                    return this.column_name;
                }
            }
            set
            {
                this.column_name = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                this.visible = value;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this.read_only;
            }
            set
            {
                this.read_only = value;
            }
        }

        abstract public void renderCell(GraphicsContext g, object value, CellRenderState render_state = CellRenderState.Normal);

        /// <summary>
        /// standardni vykresleni hlavicky sloupce
        /// </summary>
        /// <param name="g"></param>
        /// <param name="context"></param>
        public void renderHead(GraphicsContext g, ColumnContext context)
        {
            //TODO: implementovat!
        }

        /// <summary>
        /// vykresli pozadi pod bunkou
        /// </summary>
        /// <param name="g"></param>
        /// <param name="render_state"></param>
        public void renderCellBackground(GraphicsContext g, CellRenderState render_state = CellRenderState.Normal)
        {
            throw new NotImplementedException();
        }
    }
}
