using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DunaGrid.columns.validators
{
    public interface IValidator
    {
        /// <summary>
        /// vraci jestli jsou data validni
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool GetResult(object value);
    }
}
