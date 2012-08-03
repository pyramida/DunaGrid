using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DunaGrid.columns.validators
{
    public class NotNullValidator : IValidator
    {
        public bool GetResult(object value)
        {
            if (value.ToString() == "")
            {
                if (value.ToString() == "")
                {
                    return false;
                }
            }
            else if (value == null)
            {
                return false;
            }

            return true;
        }
    }
}
