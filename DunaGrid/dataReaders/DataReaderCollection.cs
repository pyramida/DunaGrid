using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DunaGrid.dataReaders
{
    public class DataReaderCollection : List<IDataReader>
    {
        /// <summary>
        /// vrati datareader podle typu datasource
        /// </summary>
        /// <param name="data_souce"></param>
        /// <returns></returns>
        public IDataReader GetByType(object data_souce)
        {
            foreach (IDataReader dr in this)
            {
                if (dr.IsReadable(data_souce))
                {
                    return dr;
                }
            }

            return null;
        }
    }
}
