using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
namespace BusinessLogic.Healper
{
    public class QueryBuilder
    {
        public string BuildQuerySearch(object obj)
        {

            string SearchWith = ""; string datecondition = ""; string ProjectCondition = "";
            if (obj == null) return "";
            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();
            List<string> propNames = new List<string>();
            foreach (PropertyInfo prp in props)
            {
                if (prp.Name.ToLower() == "RadioSearch".ToLower() || prp.Name == "PageIndex" || prp.Name == "PageSize" || prp.Name == "Type" || prp.Name == "LoginUserId" || prp.Name == "LoginBusinessPartnerId"
                    || prp.Name == "dfromdate" || prp.Name == "dtodate" || prp.Name == "vitemName" || prp.Name == "SearchBy")
                {
                    if (prp.Name.ToLower() == "RadioSearch".ToLower())
                    {
                        System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(prp.Name);
                        try
                        {
                            SearchWith = Convert.ToString(pi.GetValue(obj, null));
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    else if (prp.Name == "dfromdate")
                    {
                        System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(prp.Name);
                        try
                        {
                            if (pi.GetValue(obj) != null)
                            {
                                string date = DateTime.Parse(pi.GetValue(obj, null).ToString()).ToString("dd MMM yyyy", new CultureInfo("en-US"));
                                datecondition = "Tdate between '" + date + "'" + datecondition;
                            }

                        }
                        catch
                        {
                            continue;
                        }

                    }
                    else if (prp.Name == "dtodate")
                    {
                        System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(prp.Name);
                        try
                        {
                            if (pi.GetValue(obj) != null)
                            {
                                string date = DateTime.Parse(pi.GetValue(obj, null).ToString()).ToString("dd MMM yyyy", new CultureInfo("en-US"));
                                datecondition += " AND '" + date + "'";
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                else
                    propNames.Add(prp.Name);

            }
            string commString = String.Join(",", propNames.ToArray()); string WhereCond = string.Empty;
            if (SearchWith != "")
            {
                WhereCond = BuildQuerySearch(obj, SearchWith, commString, datecondition);
            }
            else
            {
                WhereCond = BuildQuery(obj, commString);
            }
            if (ProjectCondition != "")
            {
                WhereCond = WhereCond + ProjectCondition;
            }
            return WhereCond;
        }
        public string BuildQuerySearch(object obj, string SearchWith, string SearchQuery, string DateCondition)
        {
            string dmycheck = "";
            StringBuilder Query = new StringBuilder();
            string[] SearchList = SearchQuery.Split(',');
            if (DateCondition != "")
            {
                Query.Append(DateCondition + " AND ");
            }
            if (SearchWith == "Start with")
            {
                foreach (string search in SearchList)
                {
                    System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(search);
                    try
                    {
                        dmycheck = Convert.ToString(pi.GetValue(obj, null));
                        if ((pi.PropertyType == typeof(int) || pi.PropertyType == typeof(Int16) || pi.PropertyType == typeof(Nullable<Int16>) || pi.PropertyType == typeof(Int32) || pi.PropertyType == typeof(Nullable<Int32>) || pi.PropertyType == typeof(Int64) || pi.PropertyType == typeof(Nullable<Int64>) || pi.PropertyType == typeof(Nullable<Decimal>) || pi.PropertyType == typeof(Decimal)) && Convert.ToDecimal(dmycheck.ToString()) != 0)
                        {
                            Query.Append(search + "=" + dmycheck.ToString() + " AND ");
                        }
                        if (pi.PropertyType == typeof(Nullable<Boolean>))
                        {
                            if (dmycheck.ToString() != null && dmycheck.ToString() != "")
                            {
                                if (dmycheck.ToString().ToLower() == "false")
                                    Query.Append(search + "=" + 0 + " AND ");
                                else
                                    Query.Append(search + "=" + 1 + " AND ");
                            }
                        }
                        if ((pi.PropertyType == typeof(String) || pi.PropertyType == typeof(string)) && dmycheck != "")
                        {
                            Query.Append(search + " LIKE '" + dmycheck.ToString() + "%' AND ");
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            if (SearchWith == "Exact")
            {
                foreach (string search in SearchList)
                {
                    System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(search);
                    try
                    {
                        dmycheck = Convert.ToString(pi.GetValue(obj, null));
                        if ((pi.PropertyType == typeof(int) || pi.PropertyType == typeof(Int16) || pi.PropertyType == typeof(Nullable<Int16>) || pi.PropertyType == typeof(Int32) || pi.PropertyType == typeof(Nullable<Int32>) || pi.PropertyType == typeof(Int64) || pi.PropertyType == typeof(Nullable<Int64>) || pi.PropertyType == typeof(Nullable<Decimal>) || pi.PropertyType == typeof(Decimal)) && Convert.ToDecimal(dmycheck.ToString()) != 0)
                        {

                            Query.Append(search + "=" + dmycheck.ToString() + " AND ");
                        }
                        if (pi.PropertyType == typeof(Nullable<Boolean>))
                        {
                            if (dmycheck.ToString() != null && dmycheck.ToString() != "")
                            {
                                if (dmycheck.ToString().ToLower() == "false")
                                    Query.Append(search + "=" + 0 + " AND ");
                                else
                                    Query.Append(search + "=" + 1 + " AND ");
                            }
                        }
                        if ((pi.PropertyType == typeof(String) || pi.PropertyType == typeof(string)) && dmycheck != "")
                        {
                            Query.Append(search + "='" + dmycheck.ToString() + "' AND ");
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            if (SearchWith == "Contains")
            {
                foreach (string search in SearchList)
                {
                    System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(search);
                    try
                    {
                        dmycheck = Convert.ToString(pi.GetValue(obj, null));
                        if ((pi.PropertyType == typeof(int) || pi.PropertyType == typeof(Int16) || pi.PropertyType == typeof(Nullable<Int16>) || pi.PropertyType == typeof(Int32) || pi.PropertyType == typeof(Nullable<Int32>) || pi.PropertyType == typeof(Int64) || pi.PropertyType == typeof(Nullable<Int64>) || pi.PropertyType == typeof(Nullable<Decimal>) || pi.PropertyType == typeof(Decimal)) && Convert.ToDecimal(dmycheck.ToString()) != 0)
                        {
                            Query.Append(search + "=" + dmycheck.ToString() + " AND ");
                        }
                        if (pi.PropertyType == typeof(Nullable<Boolean>))
                        {
                            if (dmycheck.ToString() != null && dmycheck.ToString() != "")
                            {
                                if (dmycheck.ToString().ToLower() == "false")
                                    Query.Append(search + "=" + 0 + " AND ");
                                else
                                    Query.Append(search + "=" + 1 + " AND ");
                            }
                        }
                        if ((pi.PropertyType == typeof(String) || pi.PropertyType == typeof(string)) && dmycheck != "")
                        {
                            Query.Append(search + " LIKE '%" + dmycheck.ToString() + "%' AND ");
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            if (Query.Length > 4)
            {
                Query.Remove(Query.Length - 4, 3);
                Query.Insert(0, " AND ");
                return Query.ToString();
            }
            else
                return "";
        }
        public string BuildQuery(object obj, string SearchQuery)
        {
            string dmycheck = "";
            StringBuilder Query = new StringBuilder();
            string[] SearchList = SearchQuery.Split(',');
            foreach (string search in SearchList)
            {
                if (search != "Action")
                {
                    System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(search);
                    try
                    {
                        Type columnType = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;
                        dmycheck = Convert.ToString(pi.GetValue(obj, null));
                        if ((columnType == typeof(int) || columnType == typeof(Int16) || columnType == typeof(Int32) || columnType == typeof(Int64) || columnType == typeof(Decimal)) && Convert.ToDecimal(dmycheck.ToString()) != 0)
                        {
                            Query.Append(search + "=" + dmycheck.ToString() + " AND ");
                        }
                        if (columnType == typeof(Nullable<Boolean>))
                        {
                            if (dmycheck.ToString() != null && dmycheck.ToString() != "")
                            {
                                if (dmycheck.ToString().ToLower() == "false")
                                    Query.Append(search + "=" + 0 + " AND ");
                                else
                                    Query.Append(search + "=" + 1 + " AND ");
                            }
                        }
                        if ((columnType == typeof(String) || columnType == typeof(string)) && dmycheck != "")
                        {
                            Query.Append(search + " LIKE '" + dmycheck.ToString() + "%' AND ");
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            if (Query.Length > 4)
            {
                Query.Remove(Query.Length - 4, 3);
                Query.Insert(0, " AND ");
                return Query.ToString();
            }
            else
                return "";
        }

        public string BuildQuerySearchWithItem(object obj)
        {
            string SearchWith = "Contains"; string datecondition = "";
            if (obj == null) return "";
            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();
            List<string> propNames = new List<string>();
            foreach (PropertyInfo prp in props)
            {
                if (prp.Name == "RadioSearch" || prp.Name == "PageIndex" || prp.Name == "PageSize"
                    || prp.Name == "dfromDate" || prp.Name == "dtoDate")
                {
                    if (prp.Name == "RadioSearch")
                    {
                        System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(prp.Name);
                        try
                        {
                            SearchWith = pi.GetValue(obj, null).ToString();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    else if (prp.Name == "dfromDate")
                    {
                        System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(prp.Name);
                        try
                        {
                            string date = DateTime.Parse(pi.GetValue(obj, null).ToString()).ToString("dd MMM yyyy", new CultureInfo("en-US"));
                            datecondition = "dtrndate between '" + date + "'" + datecondition;
                        }
                        catch
                        {
                            continue;
                        }

                    }
                    else if (prp.Name == "dtoDate")
                    {
                        System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(prp.Name);
                        try
                        {
                            string date = DateTime.Parse(pi.GetValue(obj, null).ToString()).ToString("dd MMM yyyy", new CultureInfo("en-US"));
                            datecondition += " AND '" + date + "'";
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                else
                    propNames.Add(prp.Name);
            }
            string commString = String.Join(",", propNames.ToArray()); string WhereCond = string.Empty;
            if (SearchWith != "")
            {
                WhereCond = BuildQuerySearch(obj, SearchWith, commString, datecondition);
            }
            else
            {
                WhereCond = BuildQuery(obj, commString);
            }
            return WhereCond;
        }
    }
}
