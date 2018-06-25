using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003B3 RID: 947
	public class SymbolResolver_FirefoamPopper : SymbolResolver
	{
		// Token: 0x0600106E RID: 4206 RVA: 0x0008B194 File Offset: 0x00089594
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindSpawnCell(rp.rect, out intVec);
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0008B1D8 File Offset: 0x000895D8
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 loc;
			if (this.TryFindSpawnCell(rp.rect, out loc))
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.FirefoamPopper, null);
				thing.SetFaction(rp.faction, null);
				GenSpawn.Spawn(thing, loc, BaseGen.globalSettings.map, WipeMode.Vanish);
			}
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x0008B22C File Offset: 0x0008962C
		private bool TryFindSpawnCell(CellRect rect, out IntVec3 result)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rect, delegate(IntVec3 c)
			{
				bool result2;
				if (c.Standable(map) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(c, map) && c.GetFirstItem(map) == null)
				{
					result2 = !GenSpawn.WouldWipeAnythingWith(c, Rot4.North, ThingDefOf.FirefoamPopper, map, (Thing x) => x.def.category == ThingCategory.Building);
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
