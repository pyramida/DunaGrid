using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DunaGrid.columns
{
    /// <summary>
    /// trida prideluje k danemu datovemu typu typ sloupce
    /// </summary>
    public static class ColumnTypeDelegator
    {
        public static IColumn getByType(Type t)
        {
            return new TextColumn(); //jelikoz nemame jiny typ sloupce, tak sem to zatim zbytecne nekomplikoval
        }
    }
}
