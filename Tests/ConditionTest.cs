using DunaGrid;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DunaGrid.columns;
using System.Collections.Generic;

namespace Tests
{
    
    
    /// <summary>
    ///This is a test class for ConditionTest and is intended
    ///to contain all ConditionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConditionTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for parseString
        ///</summary>
        [TestMethod()]
        [DeploymentItem("DunaGrid.dll")]
        public void parseStringTest()
        {
            Condition_Accessor target = new Condition_Accessor(); // TODO: Initialize to an appropriate value
            ColumnCollection grid_columns = new ColumnCollection();
            grid_columns.Add(new TextColumn("Sloupec 1"));
            grid_columns.Add(new TextColumn("Sloupec 2"));
            grid_columns.Add(new NumberColumn("Sloupec 3"));
            grid_columns.Add(new NumberColumn("cislo"));

            // sloupec 1 je string takze ocekavam jeho naparsovani jako retezec
            Check(target, "[Sloupec 1] = 100", grid_columns, grid_columns[0], "100", Operators.equal);
            Check(target, "cislo > 56", grid_columns, grid_columns[3], "56", Operators.greater_than);
            Check(target, "cislo >= '56'", grid_columns, grid_columns[3], "56", Operators.greater_than | Operators.equal);
            Check(target, "[Sloupec 2] LIKE '160%'", grid_columns, grid_columns[1], "160%", Operators.like);
            Check(target, "[Sloupec 2] like '160%'", grid_columns, grid_columns[1], "160%", Operators.like);
            Check(target, "Sloupec 2 like '160%'", grid_columns, grid_columns[1], "160%", Operators.like);
            Check(target, "Sloupec 2 like 'neco = pokus'", grid_columns, grid_columns[1], "neco = pokus", Operators.like);
            Check(target, "Sloupec 1 REGeXp 'neco.*'", grid_columns, grid_columns[0], "neco.*", Operators.regexp);
        }

        private static void CheckColumn(object o, string nazev, string podminka)
        {
            IColumn c = (IColumn)o;
            if (c.Name != nazev)
            {
                Assert.Fail(podminka + " => nebyl rozpoznan jako " + nazev + ", ale jako " + c.Name);
            }
        }

        private static void Check(Condition_Accessor target, string podminka, ColumnCollection cols, object left, object right, Operators op)
        {
            target.ParseString(podminka, cols);

            if (target.compare_operator != op)
            {
                Fail(podminka, "spatne urceny operator");
            }

            if (!left.Equals(target.left_value))
            {
                Fail(podminka, "leva strana spatne urcena");
            }

            if (!right.Equals(target.right_value))
            {
                Fail(podminka, "prava strana spatne urcena");
            }

        }

        private static void Fail(string podminka, string text)
        {
            Assert.Fail("\"" + podminka + "\" => " + text);
        }

        /// <summary>
        ///A test for ReplaceStrings
        ///</summary>
        [TestMethod()]
        [DeploymentItem("DunaGrid.dll")]
        public void ReplaceStringsTest()
        {
            Condition_Accessor target = new Condition_Accessor();
            string s = "neco = 'test text' 'trololoooo' like 'neco'";
            Dictionary<string, string> dictionary = null;
            Dictionary<string, string> dictionaryExpected = new Dictionary<string, string>();
            dictionaryExpected.Add("$0$", "test text");
            dictionaryExpected.Add("$1$", "trololoooo");
            dictionaryExpected.Add("$2$", "neco");

            string expected = "neco = $0$ $1$ like $2$";
            string actual;

            actual = target.ReplaceStrings(s, out dictionary);

            foreach (KeyValuePair<string, string> pair in dictionaryExpected)
            {
                if (pair.Value != dictionary[pair.Key])
                {
                    Assert.Fail("spatny slovnik stringu");
                }
            }

            Assert.AreEqual(expected, actual);
        }
    }
}
