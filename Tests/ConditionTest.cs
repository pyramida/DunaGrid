using DunaGrid;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DunaGrid.columns;

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

            // sloupec 1 je string takze ocekavam jeho naparsovani jako retezec
            Check(target, "Sloupec 1 == 100", grid_columns, Type.GetType("IColumn"), Type.GetType("System.String"), Operators.equal, "Sloupec 1", "");
            Check(target, "100 == Sloupec 1", grid_columns, Type.GetType("System.String"), Type.GetType("IColumn"), Operators.equal, "", "Sloupec 1");

            // sloupec 3 je ciselny takze ocekavam jeho naparsovani jako integer
            Check(target, "Sloupec 3 == 100", grid_columns, Type.GetType("IColumn"), Type.GetType("System.Int32"), Operators.equal, "Sloupec 3", "");
            Check(target, "100 == Sloupec 3", grid_columns, Type.GetType("System.Int32"), Type.GetType("IColumn"), Operators.equal, "", "Sloupec 3");
        }

        private static void CheckColumn(object o, string nazev, string podminka)
        {
            IColumn c = (IColumn)o;
            if (c.Name != nazev)
            {
                Assert.Fail(podminka + " => nebyl rozpoznan jako " + nazev + ", ale jako " + c.Name);
            }
        }

        private static void Check(Condition_Accessor target, string podminka, ColumnCollection cols, Type left, Type right, Operators op, string left_name, string right_name)
        {
            target.parseString(podminka, cols);

            if (target.compare_operator != op)
            {
                Fail(podminka, "spatne urceny operator");
            }

            if (target.left_value is IColumn)
            {
                CheckColumn(target.left_value, left_name, podminka);
            }
            else
            {
                if (target.left_value.GetType() != left)
                {
                    Fail(podminka, "leva hodnota spatne rozpoznana");
                }
            }

            if (target.right_value is IColumn)
            {
                CheckColumn(target.right_value, right_name, podminka);
            }
            else
            {
                if (target.right_value.GetType() != right)
                {
                    Fail(podminka, "prava hodnota spatne rozpoznana");
                }
            }

        }

        private static void Fail(string podminka, string text)
        {
            Assert.Fail("\"" + podminka + "\" => " + text);
        }
    }
}
