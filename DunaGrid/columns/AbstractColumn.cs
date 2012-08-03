using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DunaGrid.columns.validators;

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
        protected DunaGridView parent = null;
        protected int datasource_column_index = 0;
        protected bool fill_column = false;
        protected string name = "";
        protected float ratio_width = 1;
        protected ValidatorCollection validators = new ValidatorCollection();

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
                    OnWidthChange(new EventArgs());
                }
                /*else
                {
                    throw new ArgumentOutOfRangeException("Zadana hodnota je nizsi nez minimalni povolena sirka");
                }*/
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

        public string Name
        {
            get
            {
                if (this.name == "")
                {
                    return this.HeadText;
                }
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        public DunaGridView ParentGrid
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

        public virtual void renderCell(GraphicsContext g, object value, CellRenderState render_state = CellRenderState.Normal)
        {
            if (render_state == CellRenderState.Selected)
            {
                Rectangle r = new Rectangle(0, 0, (int)g.Graphics.ClipBounds.Width, (int)g.Graphics.ClipBounds.Height);
                g.Graphics.FillRectangle(SystemBrushes.Highlight, r);
            }
        }

        // tohle reseni asi moc budoucnost nema
        public virtual Color GetFontColor(CellRenderState rs)
        {
            if (rs == CellRenderState.Selected)
            {
                return Color.White;
            }
            else
            {
                return Color.Black;
            }
        }

        public abstract DunaGrid.components.editors.AbstractGridEditor GetEditControl();

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
                    this.minwidth = value;
                    if (this.width < this.minwidth)
                    {
                        this.width = value;
                    }
                }
            }
        }


        public float RatioWidth
        {
            get
            {
                return this.ratio_width;
            }
            set
            {
                this.ratio_width = value;
            }
        }

        public event EventHandler WidthChanged;

        protected virtual void OnWidthChange(EventArgs e)
        {
            if (WidthChanged != null)
            {
                WidthChanged(this, e);
            }
        }


        public bool Pinned
        {
            get;
            set;
        }


        public validators.ValidatorCollection Validators
        {
            get { return this.validators; }
        }
    }
}
