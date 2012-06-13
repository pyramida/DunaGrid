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
        protected int width = 200;
        protected int minwidth = 20;
        protected string column_name = null;
        protected bool visible = true;
        protected bool read_only = false;
        protected DunaGrid parent = null;
        protected int datasource_column_index = 0;
        protected bool fill_column = false;

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                if (value >= this.minwidth)
                {
                    this.width = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Zadana hodnota je nizsi nez minimalni povolena sirka");
                }
            }
        }

        public bool Elastic
        {
            get
            {
                return this.fill_column;
            }

            set
            {
                this.fill_column = value;
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

        public int DataSourceColumnIndex
        {
            get
            {
                return this.datasource_column_index;
            }

            set
            {
                this.datasource_column_index = value;
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
            g.Graphics.Clear(Color.DarkGray);
            g.Graphics.DrawString(this.HeadText, g.Font, Brushes.Black, new PointF(3, 3));
        }

        /// <summary>
        /// vykresli pozadi pod bunkou
        /// </summary>
        /// <param name="g"></param>
        /// <param name="render_state"></param>
        public void renderCellBackground(GraphicsContext g, CellRenderState render_state = CellRenderState.Normal)
        {
            
        }

        /// <summary>
        /// urci zda se string vejde do bunky
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected bool fitStringToCell(string text)
        {
            return true; //soucasna implementace je jen zkusebni TODO: dodelat
        }


        public int MinimalWidth
        {
            get
            {
                return this.minwidth;
            }
            set
            {
                if (value > 0)
                {
                    minwidth = value;
                    if (this.width < this.minwidth)
                    {
                        this.width = value;
                    }
                }
            }
        }
    }
}
