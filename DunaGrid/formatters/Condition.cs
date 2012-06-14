using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.columns;
using DunaGrid.rows;

namespace DunaGrid.formatters
{
    /// <summary>
    /// TODO: podpora pro datumy zatim chybi!
    /// take neni implementovan operator LIKE a REGEXP
    /// </summary>
    public struct Condition : ICondition
    {
        /// <summary>
        /// sloupec na ktery se podminka vztahuje
        /// </summary>
        private object left_value;

        /// <summary>
        /// rovnostni operator z vyctovehi typu
        /// </summary>
        /// <seealso cref="Operators"/>
        private Operators compare_operator;

        /// <summary>
        /// hodnota, ktera se porovnava
        /// </summary>
        private object right_value;

        /// <summary>
        /// standardni konstruktor
        /// </summary>
        /// <param name="col">sloupec, ke kteremu se podminka vztahuje</param>
        /// <param name="compare_operator">rovnostni operator</param>
        /// <param name="value">jedna strana rovnice</param>
        public Condition(IColumn col, Operators compare_operator, object value)
        {
            this.left_value = col;
            this.compare_operator = compare_operator;
            this.right_value = value;
        }

        /// <summary>
        /// Zpracuje retezec na podminku
        /// </summary>
        /// <param name="string_condition">text podminky</param>
        /// <param name="columns">sloupce gridu, pro ktery plati podminka</param>
        public Condition(string string_condition, ColumnCollection columns)
        {
            this.left_value = null;
            this.right_value = null;
            this.compare_operator = Operators.not_equal;

            this.parseString(string_condition, columns);
        }

        /// <summary>
        /// rozparsuje podminku ve stringu
        /// nepodporuje slozene podminky!
        /// </summary>
        /// <param name="s">text, ktery chci parsovat</param>
        /// <param name="grid_columns">kolekce sloupcu</param>
        private void parseString(string s, ColumnCollection grid_columns)
        {
            //slovnik urcujici, ktery znak znamena jaky operator
            Dictionary<string, Operators> operatory = new Dictionary<string, Operators>();
            operatory.Add("==", Operators.equal);
            operatory.Add("!=", Operators.not_equal);
            operatory.Add(">=", Operators.greater_than | Operators.equal);
            operatory.Add(">", Operators.greater_than);
            operatory.Add("<=", Operators.lower_than | Operators.equal);
            operatory.Add("<", Operators.lower_than);
            operatory.Add(" LIKE ", Operators.like); //tady jsou mezery nutnosti
            operatory.Add(" REGEXP ", Operators.regexp); //tady taky

            string[] rozrezany = s.Split(operatory.Keys.ToArray<string>(), StringSplitOptions.RemoveEmptyEntries);

            if (rozrezany.Length == 2)
            {
                Operators op=0;
                //mame levou a pravou stranu rovnice, urcime operator
                foreach (KeyValuePair<string, Operators> pair in operatory)
                {
                    if (s.Contains(pair.Key))
                    {
                        op = pair.Value;
                        break;
                    }
                }

                this.compare_operator = op; //nastavi operator

                //nyni zparsuje jednotlive strany rovnice
                this.left_value = this.parseType(rozrezany[0], grid_columns);

                this.right_value = this.parseType(rozrezany[1], grid_columns);
            }
            else
            {
                throw new FormatException("retezec podminky neodpovida formatu");
            }
        }

        private object parseType(string text, ColumnCollection cols)
        {
            text = text.Trim(); //orizneme potencialni mezery atd.

            if (text[0] == '\'' && text[text.Length - 1] == '\'')
            {
                //je to retezec
                return text.Substring(1, text.Length - 2);
            }
            else
            {
                //ted je to bud sloupec nebo cislo
                double Num;
                System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("en");
                bool isNum = double.TryParse(text, System.Globalization.NumberStyles.AllowDecimalPoint, culture, out Num);

                if (isNum)
                {
                    //jde o cislo
                    return Num;
                }
                else
                {
                    //mel by to byt nazev sloupce
                    if (cols.Contains(text))
                    {
                        return cols[text];
                    }
                    else
                    {
                        //neplatny nazev sloupce
                        throw new FormatException("neplatny nazev sloupce '" + text + "'");
                    }
                }
            }
        }

        /// <summary>
        /// vyhodnoti podminku a vrati vysledek
        /// </summary>
        public bool getResult(IRow radek)
        {
            //zrusi odkazy na sloupce a pripravi si pro vyhodnoceni realnou hodnotu z radku
            object left = this.tryParseColumn(this.left_value, radek);
            object right = this.tryParseColumn(this.right_value, radek);

            //pokud se nerovnaji datove typy tak si hodnoty nemohou byt rovny
            if (left.GetType() != right.GetType())
            {
                return false;
            }
            else
            {
                switch (compare_operator)
                {
                    case Operators.equal:
                        if (left == right) return true;
                        break;

                    case Operators.not_equal:
                        if (left != right) return true;
                        break;

                    case Operators.greater_than:
                        if (left.GetType() == typeof(double))
                        {
                            if ((double)left > (double)right) return true;
                        }
                        break;

                    case Operators.lower_than:
                        if (left.GetType() == typeof(double))
                        {
                            if ((double)left < (double)right) return true;
                        }
                        break;

                    case Operators.like:
                        //TODO: implementovat operator LIKE
                        break;

                    case Operators.regexp:
                        //TODO: implementovat operator REGEXP
                        break;
                }

                return false;
            }
        }

        private object tryParseColumn(object data, IRow radek)
        {
            if (data.GetType() == typeof(IColumn))
            {
                return radek[((IColumn)data).Name];
            }
            else
            {
                return this.left_value;
            }
        }
    }
}
