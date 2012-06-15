using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DunaGrid.formatters
{
    class ConditionFormatter : IFormatter
    {
        protected ICondition condition = null;
        protected Font font = null;
        protected Color fontcolor = Color.Black; //bylo vy vhodne pak nacitat defaultni barvu z gridu
        protected Color backgroundcolor = Color.White; //bylo vy vhodne pak nacitat defaultni barvu z gridu
        protected ContentAlignment align = 0; //0 = default - neresit pri vykreslovani

        public ICondition Condition
        {
            get
            {
                return this.condition;
            }
            set
            {
                this.condition = value;
            }
        }

        public Font Font
        {
            get
            {
                return this.font;
            }
            set
            {
                this.font = value;
            }
        }

        public Color FontColor
        {
            get
            {
                return this.fontcolor;
            }
            set
            {
                this.fontcolor = value;
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return this.backgroundcolor;
            }
            set
            {
                this.backgroundcolor = value;
            }
        }

        public ContentAlignment CellAlign
        {
            get
            {
                return this.align;
            }
            set
            {
                this.align = value;
            }
        }

        public bool isMatch(rows.IRow row)
        {
            return this.condition.getResult(row);
        }
    }
}
