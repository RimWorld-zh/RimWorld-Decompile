using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003AB RID: 939
	public class SymbolResolver_EdgeFloor : SymbolResolver
	{
		// Token: 0x06001049 RID: 4169 RVA: 0x0008918C File Offset: 0x0008758C
		public override void Resolve(ResolveParams rp)
		{
			TerrainDef floorDef = rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			ResolveParams resolveParams = rp;
			resolveParams.rect = new CellRect(rp.rect.minX, rp.rect.minZ, rp.rect.Width, 1);
			resolveParams.floorDef = floorDef;
			BaseGen.symbolStack.Push("floor", resolveParams);
			if (rp.rect.Height > 1)
			{
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = new CellRect(rp.rect.minX, rp.rect.maxZ, rp.rect.Width, 1);
				resolveParams2.floorDef = floorDef;
				BaseGen.symbolStack.Push("floor", resolveParams2);
			}
			if (rp.rect.Height > 2)
			{
				ResolveParams resolveParams3 = rp;
				resolveParams3.rect = new CellRect(rp.rect.minX, rp.rect.minZ + 1, 1, rp.rect.Height - 2);
				resolveParams3.floorDef = floorDef;
				BaseGen.symbolStack.Push("floor", resolveParams3);
				if (rp.rect.Width > 1)
				{
					ResolveParams resolveParams4 = rp;
					resolveParams4.rect = new CellRect(rp.rect.maxX, rp.rect.minZ + 1, 1, rp.rect.Height - 2);
					resolveParams4.floorDef = floorDef;
					BaseGen.symbolStack.Push("floor", resolveParams4);
				}
			}
		}
	}
}
