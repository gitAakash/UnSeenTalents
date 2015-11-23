using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace UnseentalentsApp.Models.Repository
{
    public class Common
    {
        public enum RoleType
        {
            Admin = 1,
            Normaluser = 2,
            premiumuser = 3

        }
        public List<RowNum> GetRowNum()
        {
            List<RowNum> lstatus = new List<RowNum>();
            lstatus.Add(new RowNum { ID = 15, Name = 15, });
            lstatus.Add(new RowNum { ID = 30, Name = 30, });
            lstatus.Add(new RowNum { ID = 45, Name = 45, });
            lstatus.Add(new RowNum { ID = 60, Name = 60, });
            lstatus.Add(new RowNum { ID = 75, Name = 75, });

            return lstatus;
        }

        public class RowNum
        {
            public int Name { get; set; }
            public int ID { get; set; }
        }

        public string GetSqlFilterClauseForColumn(string filterColumnName, string filterOperator, string filterValue, string coloumntype)
        {
            //get the actual database column name
            // filterColumnName = GetDatabaseColumnName(filterColumnName);

            if (filterColumnName == string.Empty) //do not create a SQL clause for a non-database columns Or if user specified a column name different from what we are expecting. THIS WILL PREVENT SQL INJECTION.
            {
                return string.Empty;
            }

            //replace all single quote/s or double quotes with 2 single quotes to prevent any chance of SQL Injection 
            if (coloumntype.Trim() != "date")
            {
                filterValue = filterValue.Replace("'", "''");
            }
            //  filterValue = filterValue.Replace(""","''");

            string sqlClause = string.Empty;
            switch (coloumntype)
            {
                case "date":
                    if (filterOperator == "eq")
                    {
                        sqlClause = filterColumnName + " = " + filterValue + " ";
                    }
                    else if (filterOperator == "neq")
                    {
                        sqlClause = filterColumnName + " <> " + filterValue + " ";
                    }
                    else if (filterOperator == "lt")
                    {
                        sqlClause = filterColumnName + " < " + filterValue + "  ";
                    }
                    else if (filterOperator == "lte")
                    {
                        sqlClause = filterColumnName + " <= " + filterValue + " ";
                    }
                    else if (filterOperator == "gt")
                    {
                        sqlClause = filterColumnName + " > " + filterValue + "  ";
                    }
                    else if (filterOperator == "gte")
                    {
                        sqlClause = filterColumnName + " >= " + filterValue + " ";
                    }
                    break;
                case "string":
                    if (filterOperator == "contains")
                    {
                        sqlClause = filterColumnName + " like '%" + filterValue + "%' ";
                    }
                    else if (filterOperator == "doesnotcontain")
                    {
                        sqlClause = filterColumnName + " not like '%" + filterValue + "%' ";
                    }
                    else if (filterOperator == "startswith")
                    {
                        sqlClause = filterColumnName + " like '" + filterValue + "%' ";
                    }
                    else if (filterOperator == "endswith")
                    {
                        sqlClause = filterColumnName + " like '" + filterValue + "%' ";
                    }
                    else if (filterOperator == "eq")
                    {
                        sqlClause = filterColumnName + " = '" + filterValue + "' ";
                    }
                    else if (filterOperator == "neq")
                    {
                        sqlClause = filterColumnName + " <> '" + filterValue + "' ";
                    }
                    break;
                case "int":
                    if (filterOperator == "eq")
                    {
                        sqlClause = filterColumnName + " = " + filterValue + " ";
                    }
                    else if (filterOperator == "neq")
                    {
                        sqlClause = filterColumnName + " <> " + filterValue + " ";
                    }
                    else if (filterOperator == "lt")
                    {
                        sqlClause = filterColumnName + " < " + filterValue + "  ";
                    }
                    else if (filterOperator == "lte")
                    {
                        sqlClause = filterColumnName + " <= " + filterValue + " ";
                    }
                    else if (filterOperator == "gt")
                    {
                        sqlClause = filterColumnName + " > " + filterValue + "  ";
                    }
                    else if (filterOperator == "gte")
                    {
                        sqlClause = filterColumnName + " >= " + filterValue + " ";
                    }
                    break;
                case "bool":
                    if (filterOperator == "eq" && filterValue == "true")
                    {
                        sqlClause = filterColumnName + " = cast( 1 as bit) ";
                    }
                    else if (filterOperator == "eq" && filterValue == "false")
                    {
                        sqlClause = filterColumnName + " = cast( 0 as bit) ";
                    }


                    break;

                default: //column is not a database column but a template column, so we ignore it
                    break;
            }
            return sqlClause;
        }


        public static string ReadEmailformats(string Filename)
        {
            //ConfigurationManager.AppSettings["ImagePath"] + UniqueFileName
            StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/HtmlTemplates/" + Filename + ""));
            string readFile = reader.ReadToEnd();
            string strEmailBody = "";
            strEmailBody = readFile;
            // strEmailBody = strEmailBody.Replace("$$user$$", username);
            return strEmailBody.ToString();
        }


        public string GenerateRandomNumber()
        {
            string strchar = "abcdefghi@!#$&ABCDEFGH1234567";
            string code = "";
            Random rnd = new Random();

            for (int i = 0; i <= 10; i++)
            {
                int iRandom = rnd.Next(0, strchar.Length - 1);
                code += strchar.Substring(iRandom, 1);
            }
            return code;
        }
    }
    public static class StringHelpers
    {
        public static string ToSeoUrl(this string url)
        { // make the url lowercase 
            string encodedUrl = (url ?? "").ToLower(); // replace & with and 
            encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and"); // remove characters
            encodedUrl = encodedUrl.Replace("'", ""); // remove invalid characters 
            encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-"); // remove duplicates 
            encodedUrl = Regex.Replace(encodedUrl, @"-+", "-"); // trim leading & trailing characters 
            encodedUrl = encodedUrl.Trim('-');
            return encodedUrl;
        }

    }
}