using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BusinessLogic.Healper
{
    public static class DataSetExtensions
    {
        public static List<Dictionary<string, object>> ToDictionaryList(this DataTable table)
        {
            if (table == null)
                return null;
            return table.Rows.Cast<DataRow>()
                .Select(r => table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName.Trim(), c => r[c.ToString().Trim()]))
                .ToList();
        }

        public static Dictionary<string, List<Dictionary<string, object>>> ToDictionary(this DataSet set)
        {
            if (set == null)
                return null;
            return set.Tables.Cast<DataTable>()
                .ToDictionary(t => t.TableName, t => t.ToDictionaryList());
        }
    }
}
