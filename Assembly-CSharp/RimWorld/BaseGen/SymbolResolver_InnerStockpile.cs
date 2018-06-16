using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003CB RID: 971
	public class SymbolResolver_InnerStockpile : SymbolResolver
	{
		// Token: 0x060010BE RID: 4286 RVA: 0x0008E594 File Offset: 0x0008C994
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

		// Token: 0x060010BF RID: 4287 RVA: 0x0008E668 File Offset: 0x0008CA68
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

		// Token: 0x060010C0 RID: 4288 RVA: 0x0008E6BC File Offset: 0x0008CABC
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

		// Token: 0x04000A35 RID: 2613
		private const int DefaultSize = 3;
	}
}
