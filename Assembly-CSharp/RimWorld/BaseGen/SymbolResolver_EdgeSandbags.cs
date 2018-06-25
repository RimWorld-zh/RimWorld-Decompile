using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003AC RID: 940
	public class SymbolResolver_EdgeSandbags : SymbolResolver
	{
		// Token: 0x04000A14 RID: 2580
		private static readonly IntRange LineLengthRange = new IntRange(2, 5);

		// Token: 0x04000A15 RID: 2581
		private static readonly IntRange GapLengthRange = new IntRange(1, 5);

		// Token: 0x0600104B RID: 4171 RVA: 0x00089334 File Offset: 0x00087734
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			int num = 0;
			int num2 = 0;
			int num3 = -1;
			if (rp.rect.EdgeCellsCount < (SymbolResolver_EdgeSandbags.LineLengthRange.max + SymbolResolver_EdgeSandbags.GapLengthRange.max) * 2)
			{
				num = rp.rect.EdgeCellsCount;
			}
			else if (Rand.Bool)
			{
				num = SymbolResolver_EdgeSandbags.LineLengthRange.RandomInRange;
			}
			else
			{
				num2 = SymbolResolver_EdgeSandbags.GapLengthRange.RandomInRange;
			}
			foreach (IntVec3 intVec in rp.rect.EdgeCells)
			{
				num3++;
				if (num2 > 0)
				{
					num2--;
					if (num2 == 0)
					{
						if (num3 == rp.rect.EdgeCellsCount - 2)
						{
							num2 = 1;
						}
						else
						{
							num = SymbolResolver_EdgeSandbags.LineLengthRange.RandomInRange;
						}
					}
				}
				else if (intVec.Standable(map) && !intVec.Roofed(map) && intVec.SupportsStructureType(map, ThingDefOf.Sandbags.terrainAffordanceNeeded))
				{
					if (!GenSpawn.WouldWipeAnythingWith(intVec, Rot4.North, ThingDefOf.Sandbags, map, (Thing x) => x.def.category == ThingCategory.Building || x.def.category == ThingCategory.Item))
					{
						if (num > 0)
						{
							num--;
							if (num == 0)
							{
								num2 = SymbolResolver_EdgeSandbags.GapLengthRange.RandomInRange;
							}
						}
						Thing thing = ThingMaker.MakeThing(ThingDefOf.Sandbags, null);
						thing.SetFaction(rp.faction, null);
						GenSpawn.Spawn(thing, intVec, map, WipeMode.Vanish);
					}
				}
			}
		}
	}
}
