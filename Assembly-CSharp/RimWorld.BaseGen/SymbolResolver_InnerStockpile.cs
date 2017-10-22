using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_InnerStockpile : SymbolResolver
	{
		private const int DefaultSize = 3;

		public override void Resolve(ResolveParams rp)
		{
			int? innerStockpileSize = rp.innerStockpileSize;
			CellRect rect = default(CellRect);
			if (innerStockpileSize.HasValue)
			{
				if (!this.TryFindPerfectPlaceThenBest(rp.rect, rp.innerStockpileSize.Value, out rect))
					return;
			}
			else if (rp.stockpileConcreteContents != null)
			{
				int num = Mathf.CeilToInt(Mathf.Sqrt((float)rp.stockpileConcreteContents.Count));
				int num2 = default(int);
				if (!this.TryFindRandomInnerRect(rp.rect, num, out rect, num * num, out num2))
				{
					rect = rp.rect;
				}
			}
			else if (!this.TryFindPerfectPlaceThenBest(rp.rect, 3, out rect))
				return;
			ResolveParams resolveParams = rp;
			resolveParams.rect = rect;
			BaseGen.symbolStack.Push("stockpile", resolveParams);
		}

		private bool TryFindPerfectPlaceThenBest(CellRect outerRect, int size, out CellRect rect)
		{
			int num = default(int);
			bool result;
			if (!this.TryFindRandomInnerRect(outerRect, size, out rect, size * size, out num))
			{
				if (num == 0)
				{
					result = false;
					goto IL_0043;
				}
				int num2 = default(int);
				if (!this.TryFindRandomInnerRect(outerRect, size, out rect, num, out num2))
				{
					result = false;
					goto IL_0043;
				}
			}
			result = true;
			goto IL_0043;
			IL_0043:
			return result;
		}

		private bool TryFindRandomInnerRect(CellRect outerRect, int size, out CellRect rect, int minValidCells, out int maxValidCellsFound)
		{
			Map map = BaseGen.globalSettings.map;
			size = Mathf.Min(size, Mathf.Min(outerRect.Width, outerRect.Height));
			int maxValidCellsFoundLocal = 0;
			bool result = outerRect.TryFindRandomInnerRect(new IntVec2(size, size), out rect, (Predicate<CellRect>)delegate(CellRect x)
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
