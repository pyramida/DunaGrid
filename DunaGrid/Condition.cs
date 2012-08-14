using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.columns;
using DunaGrid.rows;
using System.Text.RegularExpressions;

namespace DunaGrid
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
        /// Zpracuje retezec na podminku
        /// </summary>
        /// <param name="string_condition">text podminky</param>
        /// <param name="columns">sloupce gridu, pro ktery plati podminka</param>
        public Condition(string string_condition, ColumnCollection columns)
        {
            this.left_value = null;
            this.right_value = null;
            this.compare_operator = Operators.not_equal;

            this.ParseString(string_condition, columns);
        }

        /// <summary>
        /// rozparsuje podminku ve stringu
        /// nepodporuje slozene podminky!
        /// </summary>
        /// <param name="condition_string">text, ktery chci parsovat</param>
        /// <param name="grid_columns">kolekce sloupcu</param>
        private void ParseString(string condition_string, ColumnCollection grid_columns)
        {
            this.left_value = null;
            this.right_value = null;

            //slovnik urcujici, ktery znak znamena jaky operator
            Dictionary<string, Operators> operatory = new Dictionary<string, Operators>();
            operatory.Add("<>", Operators.not_equal);
            operatory.Add(">=", Operators.greater_than | Operators.equal);
            operatory.Add(">", Operators.greater_than);
            operatory.Add("<=", Operators.lower_than | Operators.equal);
            operatory.Add("<", Operators.lower_than);
            operatory.Add("=", Operators.equal);
            operatory.Add(" LIKE ", Operators.like); //tady jsou mezery nutnosti
            operatory.Add(" REGEXP ", Operators.regexp); //tady taky

            Dictionary<string, string> replacements = new Dictionary<string, string>();

            //odstrani stringy aby se nemotali v parsovani a nahradi je referencnim stringem
            condition_string = ReplaceStrings(condition_string, out replacements);


            //zmena operatoru na vsechny znaky velke
            condition_string = Regex.Replace(condition_string, "([Ll][Ii][Kk][Ee])", "LIKE");
            condition_string = Regex.Replace(condition_string, "([Rr][Ee][Gg][Ee][Xx][Pp])", "REGEXP");

            string[] rozrezany = condition_string.Split(operatory.Keys.ToArray<string>(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < rozrezany.Length; i++)
            {
                rozrezany[i] = rozrezany[i].Trim();
            }

            if (rozrezany.Length == 2)
            {
                Operators op = 0;
                //mame levou a pravou stranu rovnice, urcime operator
                foreach (KeyValuePair<string, Operators> pair in operatory)
                {
                    if (condition_string.Contains(pair.Key.Trim()))
                    {
                        op = pair.Value;
                        break;
                    }
                }

                this.compare_operator = op; //nastavi operator

                this.left_value = this.ParseColumn(rozrezany[0], grid_columns, replacements);
                this.right_value = this.ParseColumn(rozrezany[1], grid_columns, replacements);
            }
        }

        private string ReplaceRefs(string s, Dictionary<string, string> strings)
        {
                foreach (KeyValuePair<string, string> pair in strings)
                {
                    s = s.Replace(pair.Key, pair.Value);
                }

                return s;
        }

        private object ParseColumn(string t, ColumnCollection grid_columns, Dictionary<string, string> replacements)
        {
            string col_name;
            t = ReplaceRefs(t, replacements);
            if (this.IsColumn(t, grid_columns, out col_name))
            {
                return grid_columns[col_name];
            }
            else
            {
                if (t[0] == '\'' && t[t.Length - 1] == '\'')
                {
                    t = CutFirstAndLastLetter(t);
                }
                return t;
            }
        }

        private string ReplaceStrings(string s, out Dictionary<string, string> dictionary)
        {
            //MatchCollection matches = Regex.Matches(s, , RegexOptions.IgnoreCase);
            string[] patterns = { @"'([^']*)'", @"\[([^']*)\]" };
            dictionary = new Dictionary<string, string>();
            
            int i = 0;

            foreach(string pattern in patterns)
            {
                MatchCollection matches = Regex.Matches(s, pattern, RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    Group group = match.Groups[0];
                    string key = "$" + i + "$";
                    dictionary.Add(key, CutFirstAndLastLetter(group.Value));
                    s = s.Replace(group.Value, key);
                    i++;
                }
            }

            return s;
        }

        private string CutFirstAndLastLetter(string s)
        {
            return s.Substring(1, s.Length - 2);
        }

        private bool IsColumn(string t, ColumnCollection grid_columns, out string column_name)
        {
            bool zavorky = false;

            //sloupce s nazvem o vice slovech 
            if (t[0] == '[' && t[t.Length - 1] == ']')
            {
                t = CutFirstAndLastLetter(t);
                zavorky = true;
            }

            if (grid_columns.Contains(t))
            {
                column_name = t;
                return true;
            }
            else
            {
                if (zavorky)
                {
                    throw new ConditionException("Neexistujici sloupec '"+t+"'");
                }
                else
                {
                    column_name = null;
                }
                return false;
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
            if (!this.IsComparable(left, right))
            {
                return false;
            }
            else
            {
                switch (compare_operator)
                {
                    case Operators.equal:
                        if (left.Equals(right)) return true;
                        break;

                    case Operators.not_equal:
                        if (!left.Equals(right)) return true;
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
            if (data is IColumn)
            {
                return radek[((IColumn)data).Name];
            }
            else
            {
                return data;
            }
        }

        private bool IsComparable(object firts, object second)
        {
            return true;
        }
    }

    public class ConditionException : Exception
    {
        public ConditionException(string msg) : base(msg)
        {
            
        }
    }
}
