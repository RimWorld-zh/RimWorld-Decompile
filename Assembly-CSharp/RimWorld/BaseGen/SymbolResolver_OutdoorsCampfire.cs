using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003B5 RID: 949
	public class SymbolResolver_OutdoorsCampfire : SymbolResolver
	{
		// Token: 0x06001075 RID: 4213 RVA: 0x0008B408 File Offset: 0x00089808
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindSpawnCell(rp.rect, out intVec);
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x0008B44C File Offset: 0x0008984C
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

		// Token: 0x06001077 RID: 4215 RVA: 0x0008B4A0 File Offset: 0x000898A0
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
