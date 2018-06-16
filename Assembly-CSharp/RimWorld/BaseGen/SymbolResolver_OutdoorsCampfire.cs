using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003B3 RID: 947
	public class SymbolResolver_OutdoorsCampfire : SymbolResolver
	{
		// Token: 0x06001071 RID: 4209 RVA: 0x0008B0CC File Offset: 0x000894CC
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindSpawnCell(rp.rect, out intVec);
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x0008B110 File Offset: 0x00089510
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 loc;
			if (this.TryFindSpawnCell(rp.rect, out loc))
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Campfire, null);
				thing.SetFaction(rp.faction, null);
				GenSpawn.Spawn(thing, loc, BaseGen.globalSettings.map, WipeMode.Vanish);
			}
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x0008B164 File Offset: 0x00089564
		private bool TryFindSpawnCell(CellRect rect, out IntVec3 result)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rect, delegate(IntVec3 c)
			{
				bool result2;
				if (c.Standable(map) && !c.Roofed(map) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(c, map) && c.GetFirstItem(map) == null)
				{
					result2 = !GenSpawn.WouldWipeAnythingWith(c, Rot4.North, ThingDefOf.Campfire, map, (Thing x) => x.def.category == ThingCategory.Building);
				}
				else
				{
					result2 = false;
				}
				return result2;
			}, out result);
		}
	}
}
