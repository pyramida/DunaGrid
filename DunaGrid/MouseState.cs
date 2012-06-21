using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DunaGrid
{
    public struct MouseState
    {
        public bool left_down;
        public MouseAction mouse_action;
        public Point last_location;
        public Point last_last_location;
        public object parameters;
        public bool block_left;
        public bool block_right;


        public MouseState(bool left_down=false, MouseAction mouse_action = MouseAction.none)
        {
            this.left_down = left_down;
            this.mouse_action = mouse_action;
            this.last_location = Point.Empty;
            this.last_last_location = this.last_location;
            this.parameters = null;
            this.block_left = false;
            this.block_right = false;
        }

        public void setLastLocation(Point mouse_position)
        {
            this.last_last_location = this.last_location;
            this.last_location = mouse_position;
        }

        public int getDeltaX()
        {
            return this.last_location.X - this.last_last_location.X;
        }

        public int getDeltaY()
        {
            return this.last_location.Y - this.last_last_location.Y;
        }
    }

    public enum MouseAction
    {
        none,
        changeColumnWidth,
        changeRowHeight,
        changeRowSelectorWidth
    }
}
