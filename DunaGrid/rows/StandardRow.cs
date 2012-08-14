using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.columns;
using DunaGrid.formatters;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using DunaGrid.components.editors;
using DunaGrid.components;

namespace DunaGrid.rows
{
    public class StandardRow : IRow
    {
        public event RowEventHandler CellSelectionChange;
        public event CellEventHandler CellValueChange;
        public event CellEventHandler CellEditStart;
        public event CellEventHandler CellEditEnd;
        public event RowEventHandler RowEditStart;
        public event RowEventHandler RowEditEnd;

        protected Dictionary<string, object> cells_values = new Dictionary<string, object>();
        protected List<string> cells_selects = new List<string>();
        protected IFormatter formatter = null;
        protected RowsCollection parent_collection=null;
        protected int height, index;

        public List<string> ColumnNames
        {
            get
            {
                List<string> names = new List<string>();

                foreach (string name in cells_values.Keys)
                {
                    names.Add(name);
                }

                return names;
            }
        }

        public virtual object this[string columnname]
        {
            get
            {
                return this.cells_values[columnname];
            }
            set
            {
                if (!this.cells_values[columnname].Equals(value))
                {
                    CellEventArgs args = new CellEventArgs();
                    args.OldValue = this.cells_values[columnname];

                    this.cells_values[columnname] = value;

                    args.Value = value;
                    args.Position = new CellPosition(this, parent_collection.Parent.Columns[columnname]);

                    OnCellValueChange(args);
                }
            }
        }

        public RowsCollection parentRowCollection
        {
            set
            {
                this.parent_collection = value;
            }
        }

        public bool Pinned
        {
            get;
            set;
        }

        public virtual int Height
        {
            get
            {
                return this.height;
            }

            set
            {
                if (value > 0)
                {
                    this.height = value;
                    if (parent_collection != null)
                    {
                        this.parent_collection.RowSizeChange(this); //TODO: nahradit udalosti
                    }
                }
            }
        }

        public StandardRow()
        {
            this.Pinned = false;
        }

        public void addCell(string columnname, object value)
        {
            this.cells_values.Add(columnname, value);
        }


        public void render(GraphicsContext g, columns.ColumnCollection visible_columns)
        {
            GraphicsState gs = g.Graphics.Save();

            if (this.formatter == null)
            {
                g.Graphics.Clear(Color.White);
            }
            else
            {
                g.Graphics.Clear(this.formatter.BackgroundColor);
            }

            int row_height = (int)Math.Floor(g.Graphics.ClipBounds.Height);

            foreach (IColumn c in visible_columns)
            {
                g.Graphics.SetClip(new Rectangle(0, 0, c.Width, this.Height));
                CellRenderState rs;
                if (this.IsSelectedCell(c.Name))
                {
                    rs = CellRenderState.Selected;
                }
                else
                {
                    rs = CellRenderState.Normal;
                }

                this.renderCell(g, this.cells_values[c.Name], rs, c);
                g.Graphics.TranslateTransform(c.Width, 0);
            }

            g.Graphics.Restore(gs);
        }

        protected virtual void renderCell(GraphicsContext g, object value, CellRenderState rs, IColumn c)
        {
            c.renderCellBackground(g);
            c.renderCell(g, value, rs);
        }

        protected virtual void OnCellSelectionChange()
        {
            if (this.CellSelectionChange != null)
            {
                CellSelectionChange(this, this.GetEventArgs());
            }
        }

        protected virtual void OnCellValueChange(CellEventArgs cell_args)
        {
            if (this.CellValueChange != null)
            {
                CellValueChange(this, cell_args);
            }
        }

        protected virtual void OnCellEditStart(CellEventArgs cell_args)
        {
            if (this.CellEditStart != null)
            {
                CellEditStart(this, cell_args);
            }
        }

        protected virtual void OnCellEditEnd(CellEventArgs cell_args)
        {
            if (this.CellEditEnd != null)
            {
                CellEditEnd(this, cell_args);
            }
        }

        protected virtual void OnRowEditEnd()
        {
            if (this.RowEditEnd != null)
            {
                RowEditEnd(this, this.GetEventArgs());
            }
        }

        protected virtual void OnRowEditStart()
        {
            Console.WriteLine("Zacala uprava radku " + this.index);
            if (this.RowEditStart != null)
            {
                RowEditStart(this, this.GetEventArgs());
            }
        }

        protected RowEventArgs GetEventArgs()
        {
            RowEventArgs output = new RowEventArgs();

            output.SelectedCells = this.cells_selects;
            output.Index = this.Index;

            return output;
        }

        public bool IsSelectedCell(string col_name)
        {
            return this.cells_selects.Contains(col_name);
        }

        public void SelectCell(string col_name)
        {
            if (!this.IsSelectedCell(col_name))
            {
                this.cells_selects.Add(col_name);
            }

            OnCellSelectionChange();
        }

        public AbstractGridEditor Edit(IColumn c)
        {
            AbstractGridEditor ctr = c.GetEditControl();
            ctr.Width = c.Width - 1; // minus jedna je prave ohraniceni ramecku
            ctr.Height = this.Height;
            ctr.Value = this[c.Name];
            ctr.RowIndex = this.Index;
            ctr.ColumnName = c.Name;
            ctr.CellPadding = c.Padding;

            if (this.parent_collection.StartEditRow(this))
            {
                OnRowEditStart(); //zavola udalost
            }

            return ctr;
        }

        public formatters.IFormatter Formatter
        {
            get
            {
                return this.formatter;
            }
            set
            {
                this.formatter = value;
            }
        }


        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }


        /// <summary>
        /// na smazani
        /// </summary>
        /// <param name="g"></param>
        public void renderRowSelector(GraphicsContext g)
        {
            //g.Graphics.FillRectangle(Brushes.DarkGray, new Rectangle(0, 0, 30, 20));
            g.Graphics.Clear(Color.DarkGray);
        }


        public void SelectCells(List<string> column_names)
        {
            this.cells_selects = column_names;

            OnCellSelectionChange();
        }
    }
}
