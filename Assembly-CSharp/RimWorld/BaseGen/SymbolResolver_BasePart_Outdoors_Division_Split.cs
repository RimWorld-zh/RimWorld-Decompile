using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200039B RID: 923
	public class SymbolResolver_BasePart_Outdoors_Division_Split : SymbolResolver
	{
		// Token: 0x0600101A RID: 4122 RVA: 0x000879F0 File Offset: 0x00085DF0
		public override bool CanResolve(ResolveParams rp)
		{
			int num;
			int num2;
			return base.CanResolve(rp) && (this.TryFindSplitPoint(false, rp.rect, out num, out num2) || this.TryFindSplitPoint(true, rp.rect, out num, out num2));
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00087A50 File Offset: 0x00085E50
		public override void Resolve(ResolveParams rp)
		{
			bool @bool = Rand.Bool;
			int num;
			int num2;
			bool flag;
			if (this.TryFindSplitPoint(@bool, rp.rect, out num, out num2))
			{
				flag = @bool;
			}
			else
			{
				if (!this.TryFindSplitPoint(!@bool, rp.rect, out num, out num2))
				{
					Log.Warning("Could not find split point.", false);
					return;
				}
				flag = !@bool;
			}
			TerrainDef floorDef = rp.pathwayFloorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			ResolveParams resolveParams3;
			ResolveParams resolveParams5;
			if (flag)
			{
				ResolveParams resolveParams = rp;
				resolveParams.rect = new CellRect(rp.rect.minX, rp.rect.minZ + num, rp.rect.Width, num2);
				resolveParams.floorDef = floorDef;
				resolveParams.streetHorizontal = new bool?(true);
				BaseGen.symbolStack.Push("street", resolveParams);
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = new CellRect(rp.rect.minX, rp.rect.minZ, rp.rect.Width, num);
				resolveParams3 = resolveParams2;
				ResolveParams resolveParams4 = rp;
				resolveParams4.rect = new CellRect(rp.rect.minX, rp.rect.minZ + num + num2, rp.rect.Width, rp.rect.Height - num - num2);
				resolveParams5 = resolveParams4;
			}
			else
			{
				ResolveParams resolveParams6 = rp;
				resolveParams6.rect = new CellRect(rp.rect.minX + num, rp.rect.minZ, num2, rp.rect.Height);
				resolveParams6.floorDef = floorDef;
				resolveParams6.streetHorizontal = new bool?(false);
				BaseGen.symbolStack.Push("street", resolveParams6);
				ResolveParams resolveParams7 = rp;
				resolveParams7.rect = new CellRect(rp.rect.minX, rp.rect.minZ, num, rp.rect.Height);
				resolveParams3 = resolveParams7;
				ResolveParams resolveParams8 = rp;
				resolveParams8.rect = new CellRect(rp.rect.minX + num + num2, rp.rect.minZ, rp.rect.Width - num - num2, rp.rect.Height);
				resolveParams5 = resolveParams8;
			}
			if (Rand.Bool)
			{
				BaseGen.symbolStack.Push("basePart_outdoors", resolveParams3);
				BaseGen.symbolStack.Push("basePart_outdoors", resolveParams5);
			}
			else
			{
				BaseGen.symbolStack.Push("basePart_outdoors", resolveParams5);
				BaseGen.symbolStack.Push("basePart_outdoors", resolveParams3);
			}
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00087D08 File Offset: 0x00086108
		private bool TryFindSplitPoint(bool horizontal, CellRect rect, out int splitPoint, out int spaceBetween)
		{
			int num = (!horizontal) ? rect.Width : rect.Height;
			spaceBetween = SymbolResolver_BasePart_Outdoors_Division_Split.SpaceBetweenRange.RandomInRange;
			spaceBetween = Mathf.Min(spaceBetween, num - 10);
			bool result;
			if (spaceBetween < SymbolResolver_BasePart_Outdoors_Division_Split.SpaceBetweenRange.min)
			{
				splitPoint = -1;
				result = false;
			}
			else
			{
				splitPoint = Rand.RangeInclusive(5, num - 5 - spaceBetween);
				result = true;
			}
			return result;
		}

		// Token: 0x04000A07 RID: 2567
		private const int MinLengthAfterSplit = 5;

		// Token: 0x04000A08 RID: 2568
		private static readonly IntRange SpaceBetweenRange = new IntRange(1, 2);
	}
}
