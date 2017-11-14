using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public static class DebugTables
	{
		public static void MakeTablesDialog<T>(IEnumerable<T> dataSources, params TableDataGetter<T>[] getters)
		{
			List<TableDataGetter<T>> list = getters.ToList();
			int num = dataSources.Count() + 1;
			int count = list.Count;
			string[,] array = new string[count, num];
			int num2 = 0;
			foreach (TableDataGetter<T> tableDataGetter in getters)
			{
				string[,] array2 = array;
				int num3 = num2;
				string label = tableDataGetter.label;
				array2[num3, 0] = label;
				num2++;
			}
			int num4 = 1;
			foreach (T dataSource in dataSources)
			{
				for (int j = 0; j < count; j++)
				{
					string[,] array3 = array;
					int num5 = j;
					int num6 = num4;
					string text = list[j].getter(dataSource);
					array3[num5, num6] = text;
				}
				num4++;
			}
			Find.WindowStack.Add(new Dialog_DebugTables(array));
		}

		public static void MakeTablesDialog<TColumn, TRow>(IEnumerable<TColumn> colValues, Func<TColumn, string> colLabelFormatter, IEnumerable<TRow> rowValues, Func<TRow, string> rowLabelFormatter, Func<TColumn, TRow, string> func, string tlLabel = "")
		{
			int num = colValues.Count() + 1;
			int num2 = rowValues.Count() + 1;
			string[,] array = new string[num, num2];
			array[0, 0] = tlLabel;
			int num3 = 1;
			foreach (TColumn colValue in colValues)
			{
				string[,] array2 = array;
				int num4 = num3;
				string text = colLabelFormatter(colValue);
				array2[num4, 0] = text;
				num3++;
			}
			int num5 = 1;
			foreach (TRow rowValue in rowValues)
			{
				string[,] array3 = array;
				int num6 = num5;
				string text2 = rowLabelFormatter(rowValue);
				array3[0, num6] = text2;
				num5++;
			}
			int num7 = 1;
			foreach (TRow rowValue2 in rowValues)
			{
				int num8 = 1;
				foreach (TColumn colValue2 in colValues)
				{
					string[,] array4 = array;
					int num9 = num8;
					int num10 = num7;
					string text3 = func(colValue2, rowValue2);
					array4[num9, num10] = text3;
					num8++;
				}
				num7++;
			}
			Find.WindowStack.Add(new Dialog_DebugTables(array));
		}
	}
}
