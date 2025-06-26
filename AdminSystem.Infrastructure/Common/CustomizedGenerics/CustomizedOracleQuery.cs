using AdminSystem.Infrastructure.Persistence.Context;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Common.CustomizedGenerics
{
    public class CustomizedOracleQuery
    {
        public static T GetById<T>(string tableName, string columnName, int value)
        {
            tableName = tableName.Trim();
            columnName = columnName.Trim();
            using var con = new OracleConnection(Connection.ConnectionString());

            string sql = $"SELECT * FROM {tableName} WHERE  {columnName} = {value}";
            var rows = con.QuerySingle<T>(sql);
            return rows;

        }
        public static List<T> GetByIdList<T>(string tableName, string columnName, int value)
        {
            tableName = tableName.Trim();
            columnName = columnName.Trim();
            using var con = new OracleConnection(Connection.ConnectionString());

            string sql = $"SELECT * FROM {tableName} WHERE  {columnName} = {value}";
            var rows = con.Query<T>(sql).ToList();
            return rows;

        }


        public static List<T> GetAll<T>(string tableName)
        {
            tableName = tableName.Trim();

            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                string sql = $"SELECT * FROM {tableName}";
                var rows = con.Query<T>(sql).ToList();
                return rows;
            }

        }

        public static T MaxNumber<T>(string tableName, string maxcolumnName, string columnName, int value)
        {
            tableName = tableName.Trim();
            maxcolumnName = maxcolumnName.Trim();
            columnName = columnName.Trim();
            using var con = new OracleConnection(Connection.ConnectionString());

            string sql = $" SELECT ISNULL(MAX({maxcolumnName}), 0) AS {maxcolumnName} FROM {tableName}  WHERE  {columnName} = {value}";
            var rows = con.QuerySingle<T>(sql);
            return rows;

        }



        public static T MaxNumberForTwoIntParemeter<T>(string tableName, string maxcolumnName, string columnName, int value, string columnName2, int value2)
        {
            tableName = tableName.Trim();
            maxcolumnName = maxcolumnName.Trim();
            columnName = columnName.Trim();
            columnName2 = columnName2.Trim();
            using var con = new OracleConnection(Connection.ConnectionString());

            string sql = $" SELECT ISNULL(MAX({maxcolumnName}),0) AS {maxcolumnName} FROM {tableName}  WHERE  {columnName} = {value} AND {columnName2}={value2}";
            var rows = con.QuerySingle<T>(sql);
            return rows;

        }

        public static T MaxNumberForTwoStringParemeter<T>(string tableName, string maxcolumnName, string columnName, int value, string columnName2, string value2)
        {
            tableName = tableName.Trim();
            maxcolumnName = maxcolumnName.Trim();
            columnName = columnName.Trim();
            columnName2 = columnName2.Trim();
            using var con = new OracleConnection(Connection.ConnectionString());
            string sql = $" SELECT ISNULL(MAX({maxcolumnName}), 0) AS {maxcolumnName} FROM {tableName}  WHERE  {columnName} = {value} AND {columnName2}='{value2}' ";
            var rows = con.QuerySingle<T>(sql);
            return rows;

        }


        public static T MaxNumberForThreeParemeter<T>(string tableName, string maxcolumnName, string columnName, int value, string columnName2, int value2, string columnName3, string value3)
        {
            tableName = tableName.Trim();
            maxcolumnName = maxcolumnName.Trim();
            columnName = columnName.Trim();
            columnName2 = columnName2.Trim();
            columnName3 = columnName3.Trim();
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                string sql = $" SELECT ISNULL(MAX({maxcolumnName}), 0) AS {maxcolumnName} FROM {tableName} WHERE  {columnName} = {value} AND {columnName2}={value2} AND {columnName3} = '{value3}' ";
                var rows = con.QuerySingle<T>(sql);
                return rows;
            }
        }

        public static T SumForUniquePriceAndQty<T>(string tableName, string maxcolumnName, string columnName, int value, string columnName2, int value2, string columnName3, int value3)
        {
            tableName = tableName.Trim();
            maxcolumnName = maxcolumnName.Trim();
            columnName = columnName.Trim();
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                string sql = $" SELECT  ISNULL(SUM({maxcolumnName}), 0) AS {maxcolumnName} FROM {tableName}  WHERE  {columnName} = {value} AND {columnName2}={value2} AND {columnName3} = {value3} ";
                var rows = con.QuerySingle<T>(sql);
                return rows;
            }
        }
        public static List<T> IsExist<T>(string tableName, string columnName, string value)
        {
            tableName = tableName.Trim();
            columnName = columnName.Trim();
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                string sql = $"SELECT * FROM {tableName} WHERE  {columnName} = '{value}'";
                var rows = con.Query<T>(sql).ToList();
                return rows;
            }

        }
        public static List<T> IsExist<T>(string tableName, string columnName, string password, string value, string pass)
        {
            tableName = tableName.Trim();
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                string sql = $"SELECT * FROM {tableName} WHERE  {columnName} = '{value}' AND {password}='{pass}'";
                var rows = con.Query<T>(sql).ToList();
                return rows;
            }

        }
        public static List<T> IsExistForTwoCondition<T>(string tableName, string columnName1, string value1, string columnName2, int value2)
        {
            tableName = tableName.Trim();
            columnName1 = columnName1.Trim();
            columnName2 = columnName2.Trim();
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                string sql = $"SELECT * FROM {tableName} WHERE {columnName1} = '{value1}' AND {columnName2}={value2} ";
                var rows = con.Query<T>(sql).ToList();
                return rows;
            }

        }

        internal static bool IsExistForThreeCondition<T>(string v1, string v2, string buyerName1, string v3, int comcod, string buyerName2, string v4, int buyerSupplier)
        {
            throw new NotImplementedException();
        }


        public static List<T> IsExistForThreeCondition<T>(string tableName, string columnName1, string value1, string columnName2, int value2, string columnName3, int value3)
        {
            tableName = tableName.Trim();
            columnName1 = columnName1.Trim();
            columnName2 = columnName2.Trim();
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                string sql = $"SELECT * FROM {tableName} WHERE {columnName1} = '{value1}' AND {columnName2}={value2} AND {columnName3}={value3} ";
                var rows = con.Query<T>(sql).ToList();
                return rows;
            }

        }
        public static List<T> IsExistForFourCondition<T>(string tableName, string columnName1, string value1, string columnName2, int value2, string columnName3, int value3, string columnName4, int value4)
        {
            tableName = tableName.Trim();
            columnName1 = columnName1.Trim();
            columnName2 = columnName2.Trim();
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                string sql = $"SELECT * FROM {tableName} WHERE {columnName1} = '{value1}' AND {columnName2}={value2} AND {columnName3}={value3} AND {columnName4}={value4} ";
                var rows = con.Query<T>(sql).ToList();
                return rows;
            }

        }
        public static List<T> IsExistForFiveCondition<T>(string tableName, string columnName1, string value1, string columnName2, int value2, string columnName3, int value3, string columnName4, int value4, string columnName5, int value5)
        {
            tableName = tableName.Trim();
            columnName1 = columnName1.Trim();
            columnName2 = columnName2.Trim();
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                string sql = $"SELECT * FROM {tableName} WHERE {columnName1} = '{value1}' AND {columnName2}={value2} AND {columnName3}={value3} AND {columnName4}={value4} AND {columnName5}={value5}";
                var rows = con.Query<T>(sql).ToList();
                return rows;
            }

        }
        public static List<T> IsExistTwoStringColoum<T>(string tableName, string columnName1, string value1, string columnName2, string value2)
        {
            tableName = tableName.Trim();
            columnName1 = columnName1.Trim();
            columnName2 = columnName2.Trim();
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                string sql = $"SELECT * FROM {tableName} WHERE {columnName1} = '{value1}' AND {columnName2}='{value2}'";
                var rows = con.Query<T>(sql).ToList();
                return rows;
            }

        }

        public static List<T> GetAllByCompId<T>(string tableName, string columnName, int value)
        {
            tableName = tableName.Trim();
            columnName = columnName.Trim();
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                string sql = $"SELECT * FROM {tableName} WHERE {columnName} = {value}";
                var rows = con.Query<T>(sql).ToList();
                return rows;
            }

        }
        public static List<T> GetAllActiveByCompId<T>(string tableName, string columnName, int value, string columnName2, int value2)
        {
            tableName = tableName.Trim();
            columnName = columnName.Trim();
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                string sql = $"SELECT * FROM {tableName} WHERE {columnName} = {value} and {columnName2}={value2}";
                var rows = con.Query<T>(sql).ToList();
                return rows;
            }

        }

        public static string ConvertDateTimeToString(string dateFieldName)
        {
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                var _cvtDate = dateFieldName;
                if (_cvtDate != null)
                {
                    var cvtdate = dateFieldName.Split("/");
                    var cvtyear = cvtdate[2].PadLeft(4, '0');
                    var cvtmonth = cvtdate[0].PadLeft(2, '0');
                    var cvtday = cvtdate[1].PadLeft(2, '0');
                    if (cvtday.Length < 2)
                    {
                        cvtday = "0" + cvtday;
                    }
                    else
                    {
                        cvtday = cvtdate[1].PadLeft(2, '0');
                    }
                    if (cvtmonth.Length < 2)
                    {
                        cvtmonth = "0" + cvtmonth;
                    }
                    else
                    {
                        cvtmonth = cvtdate[0].PadLeft(2, '0');
                    }

                    _cvtDate = $"{cvtyear}{cvtmonth}{cvtday}";
                }
                return _cvtDate;
            }
        }
        public static string ConvertDateTimeYyyyMonthDay(string dateFieldName)
        {
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                var _cvtDate = dateFieldName;
                if (_cvtDate != null)
                {
                    var cvtdate = dateFieldName.Split("/");
                    var cvtyear = cvtdate[2].PadLeft(4, '0');
                    var cvtday = cvtdate[0].PadLeft(2, '0');
                    var cvtmonth = cvtdate[1].PadLeft(2, '0');

                    if (cvtday.Length < 2)
                    {
                        cvtday = "0" + cvtday;
                    }
                    else
                    {
                        cvtday = cvtdate[1].PadLeft(2, '0');
                    }
                    if (cvtmonth.Length < 2)
                    {
                        cvtmonth = "0" + cvtmonth;
                    }
                    else
                    {
                        cvtmonth = cvtdate[0].PadLeft(2, '0');
                    }

                    _cvtDate = $"{cvtyear}{cvtday}{cvtmonth}";
                }
                return _cvtDate;
            }
        }
        public static string ConvertDateYyyyMonthDay(string dateFieldName)
        {
            using (var con = new OracleConnection(Connection.ConnectionString()))
            {
                var _cvtDate = dateFieldName;
                if (_cvtDate != null)
                {
                    var cvtdate = dateFieldName.Split("-");
                    var cvtyear = cvtdate[2].PadLeft(4, '0');
                    var cvtday = cvtdate[0].PadLeft(2, '0');
                    var cvtmonth = cvtdate[1].PadLeft(2, '0');

                    if (cvtday.Length < 2)
                    {
                        cvtday = "0" + cvtday;
                    }
                    else
                    {
                        cvtday = cvtdate[1].PadLeft(2, '0');
                    }
                    if (cvtmonth.Length < 2)
                    {
                        cvtmonth = "0" + cvtmonth;
                    }
                    else
                    {
                        cvtmonth = cvtdate[0].PadLeft(2, '0');
                    }

                    _cvtDate = $"{cvtyear}{cvtday}{cvtmonth}";
                }
                return _cvtDate;
            }
        }
    }
}