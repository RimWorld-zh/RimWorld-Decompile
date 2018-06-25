using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003CD RID: 973
	public class SymbolResolver_InnerStockpile : SymbolResolver
	{
		// Token: 0x04000A3A RID: 2618
		private const int DefaultSize = 3;

		// Token: 0x060010C1 RID: 4289 RVA: 0x0008E8E0 File Offset: 0x0008CCE0
		public override void Resolve(ResolveParams rp)
		{
			int? innerStockpileSize = rp.innerStockpileSize;
			CellRect rect;
			if (innerStockpileSize != null)
			{
				if (!this.TryFindPerfectPlaceThenBest(rp.rect, rp.innerStockpileSize.Value, out rect))
				{
					return;
				}
			}
			else if (rp.stockpileConcreteContents != null)
			{
				int num = Mathf.CeilToInt(Mathf.Sqrt((float)rp.stockpileConcreteContents.Count));
				int num2;
				if (!this.TryFindRandomInnerRect(rp.rect, num, out rect, num * num, out num2))
				{
					rect = rp.rect;
				}
			}
			else if (!this.TryFindPerfectPlaceThenBest(rp.rect, 3, out rect))
			{
				return;
			}
			ResolveParams resolveParams = rp;
			resolveParams.rect = rect;
			BaseGen.symbolStack.Push("stockpile", resolveParams);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0008E9B4 File Offset: 0x0008CDB4
		private bool TryFindPerfectPlaceThenBest(CellRect outerRect, int size, out CellRect rect)
		{
			int num;
			if (!this.TryFindRandomInnerRect(outerRect, size, out rect, size * size, out num))
			{
				if (num == 0)
				{
					return false;
				}
				int num2;
				if (!this.TryFindRandomInnerRect(outerRect, size, out rect, num, out num2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0008EA08 File Offset: 0x0008CE08
		private bool TryFindRandomInnerRect(CellRect outerRect, int size, out CellRect rect, int minValidCells, out int maxValidCellsFound)
		{
			Map map = BaseGen.globalSettings.map;
			size = Mathf.Min(size, Mathf.Min(outerRect.Width, outerRect.Height));
			int maxValidCellsFoundLocal = 0;
			bool result = outerRect.TryFindRandomInnerRect(new IntVec2(size, size), out rect, delegate(CellRect x)
			{
				int num = 0;
				CellRect.CellRectIterator iterator = x.GetIterator();
				while (!iterator.Done())
				{
					if (iterator.Current.Standable(map) && iterator.Current.GetFirstItem(map) == null && iterator.Current.GetFirstBuilding(map) == null)
					{
						num++;
					}
					iterator.MoveNext();
				}
				maxValidCellsFoundLocal = Mathf.Max(maxValidCellsFoundLocal, num);
				return num >= minValidCells;
			});
			maxValidCellsFound = maxValidCellsFoundLocal;
			return result;
		}
	}
}
