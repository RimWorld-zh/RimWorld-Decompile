using System;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_OutdoorsCampfire : SymbolResolver
	{
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			IntVec3 intVec = default(IntVec3);
			if (!this.TryFindSpawnCell(rp.rect, out intVec))
			{
				return false;
			}
			return true;
		}

		public override void Resolve(ResolveParams rp)
		{
			IntVec3 loc = default(IntVec3);
			if (this.TryFindSpawnCell(rp.rect, out loc))
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Campfire, null);
				thing.SetFaction(rp.faction, null);
				GenSpawn.Spawn(thing, loc, BaseGen.globalSettings.map);
			}
		}

		private bool TryFindSpawnCell(CellRect rect, out IntVec3 result)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rect, (Predicate<IntVec3>)((IntVec3 c) => c.Standable(map) && !c.Roofed(map) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(c, map) && c.GetFirstItem(map) == null && !GenSpawn.WouldWipeAnythingWith(c, Rot4.North, ThingDefOf.Campfire, map, (Thing x) => x.def.category == ThingCategory.Building)), out result);
		}
	}
}
