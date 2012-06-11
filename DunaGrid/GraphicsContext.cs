using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DunaGrid
{
    /// <summary>
    /// trida pro cachovani GDI+ objektu
    /// tohle je spis takovy nacrt - asi to casem bude vypadat uplne jinak
    /// </summary>
    public class GraphicsContext
    {
        protected Graphics graphics;
        protected Font font = null;
        protected static Font SystemFont = SystemFonts.DefaultFont; //tohle je silena brzda, hned si to nacachovat a uz to nikdy nevolat :))

        public Graphics Graphics
        {
            get
            {
                return this.graphics;
            }

            set
            {
                this.graphics = value;
            }
        }

        public Font Font
        {
            get
            {
                if (this.font == null)
                {
                    return GraphicsContext.SystemFont;
                }
                else
                {
                    return this.font;
                }
            }

            set
            {
                this.font = value;
            }
        }
    }
}
