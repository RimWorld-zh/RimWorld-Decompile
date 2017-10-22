using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public static class DebugTables
	{
		public static void MakeTablesDialog<T>(IEnumerable<T> dataSources, params TableDataGetter<T>[] getters)
		{
			List<TableDataGetter<T>> list = ((IEnumerable<TableDataGetter<T>>)getters).ToList<TableDataGetter<T>>();
			int num = dataSources.Count<T>() + 1;
			int count = list.Count;
			string[,] array = new string[count, num];
			int num2 = 0;
			for (int i = 0; i < getters.Length; i++)
			{
				TableDataGetter<T> tableDataGetter = getters[i];
				array[num2, 0] = tableDataGetter.label;
				num2++;
			}
			int num3 = 1;
			foreach (T item in dataSources)
			{
				for (int num4 = 0; num4 < count; num4++)
				{
					array[num4, num3] = list[num4].getter(item);
				}
				num3++;
			}
			Find.WindowStack.Add(new Dialog_DebugTables(array));
		}

		public static void MakeTablesDialog<TColumn, TRow>(IEnumerable<TColumn> colValues, Func<TColumn, string> colLabelFormatter, IEnumerable<TRow> rowValues, Func<TRow, string> rowLabelFormatter, Func<TColumn, TRow, string> func, string tlLabel = "")
		{
			int num = colValues.Count<TColumn>() + 1;
			int num2 = rowValues.Count<TRow>() + 1;
			string[,] array = new string[num, num2];
			array[0, 0] = tlLabel;
			int num3 = 1;
			foreach (TColumn item in colValues)
			{
				array[num3, 0] = colLabelFormatter(item);
				num3++;
			}
			int num4 = 1;
			foreach (TRow item2 in rowValues)
			{
				array[0, num4] = rowLabelFormatter(item2);
				num4++;
			}
			int num5 = 1;
			foreach (TRow item3 in rowValues)
			{
				int num6 = 1;
				foreach (TColumn item4 in colValues)
				{
					array[num6, num5] = func(item4, item3);
					num6++;
				}
				num5++;
			}
			Find.WindowStack.Add(new Dialog_DebugTables(array));
		}
	}
}
