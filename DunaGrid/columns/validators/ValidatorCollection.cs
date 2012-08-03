using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DunaGrid.columns.validators
{
    public class ValidatorCollection : List<IValidator>, IValidator
    {
        public bool GetResult(object value)
        {
            foreach (IValidator validator in this)
            {
                if (!validator.GetResult(value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
