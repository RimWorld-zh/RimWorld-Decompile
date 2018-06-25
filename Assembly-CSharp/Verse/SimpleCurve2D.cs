using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EFC RID: 3836
	public class SimpleCurve2D : IEnumerable<CurveColumn>, IEnumerable
	{
		// Token: 0x04003CBD RID: 15549
		private List<CurveColumn> columns = new List<CurveColumn>();

		// Token: 0x06005BC4 RID: 23492 RVA: 0x002EBF48 File Offset: 0x002EA348
		public float Evaluate(float x, float y)
		{
			float result;
			if (this.columns.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve2D with no columns.", false);
				result = 0f;
			}
			else if (x <= this.columns[0].x)
			{
				result = this.columns[0].y.Evaluate(y);
			}
			else if (x >= this.columns[this.columns.Count - 1].x)
			{
				result = this.columns[this.columns.Count - 1].y.Evaluate(y);
			}
			else
			{
				CurveColumn curveColumn = this.columns[0];
				CurveColumn curveColumn2 = this.columns[this.columns.Count - 1];
				for (int i = 0; i < this.columns.Count; i++)
				{
					if (x <= this.columns[i].x)
					{
						curveColumn2 = this.columns[i];
						if (i > 0)
						{
							curveColumn = this.columns[i - 1];
						}
						break;
					}
				}
				float t = (x - curveColumn.x) / (curveColumn2.x - curveColumn.x);
				result = Mathf.Lerp(curveColumn.y.Evaluate(y), curveColumn2.y.Evaluate(y), t);
			}
			return result;
		}

		// Token: 0x06005BC5 RID: 23493 RVA: 0x002EC0E1 File Offset: 0x002EA4E1
		public void Add(CurveColumn newColumn)
		{
			this.columns.Add(newColumn);
		}

		// Token: 0x06005BC6 RID: 23494 RVA: 0x002EC0F0 File Offset: 0x002EA4F0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06005BC7 RID: 23495 RVA: 0x002EC10C File Offset: 0x002EA50C
		public IEnumerator<CurveColumn> GetEnumerator()
		{
			foreach (CurveColumn column in this.columns)
			{
				yield return column;
			}
			yield break;
		}

		// Token: 0x06005BC8 RID: 23496 RVA: 0x002EC130 File Offset: 0x002EA530
		public IEnumerable<string> ConfigErrors(string prefix)
		{
			for (int i = 0; i < this.columns.Count - 1; i++)
			{
				if (this.columns[i + 1].x < this.columns[i].x)
				{
					yield return prefix + ": columns are out of order";
					break;
				}
			}
			yield break;
		}
	}
}
