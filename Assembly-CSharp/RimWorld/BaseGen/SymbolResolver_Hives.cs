using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003C9 RID: 969
	public class SymbolResolver_Hives : SymbolResolver
	{
		// Token: 0x060010B7 RID: 4279 RVA: 0x0008E344 File Offset: 0x0008C744
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindFirstHivePos(rp.rect, out intVec);
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0008E388 File Offset: 0x0008C788
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 loc;
			if (this.TryFindFirstHivePos(rp.rect, out loc))
			{
				int? hivesCount = rp.hivesCount;
				int num = (hivesCount == null) ? SymbolResolver_Hives.DefaultHivesCountRange.RandomInRange : hivesCount.Value;
				Hive hive = (Hive)ThingMaker.MakeThing(ThingDefOf.Hive, null);
				hive.SetFaction(Faction.OfInsects, null);
				if (rp.disableHives != null && rp.disableHives.Value)
				{
					hive.active = false;
				}
				hive = (Hive)GenSpawn.Spawn(hive, loc, BaseGen.globalSettings.map, WipeMode.Vanish);
				for (int i = 0; i < num - 1; i++)
				{
					Hive hive2;
					if (hive.GetComp<CompSpawnerHives>().TrySpawnChildHive(true, out hive2))
					{
						hive = hive2;
					}
				}
			}
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0008E474 File Offset: 0x0008C874
		private bool TryFindFirstHivePos(CellRect rect, out IntVec3 pos)
		{
			Map map = BaseGen.globalSettings.map;
			return (from mc in rect.Cells
			where mc.Standable(map)
			select mc).TryRandomElement(out pos);
		}

		// Token: 0x04000A33 RID: 2611
		private static readonly IntRange DefaultHivesCountRange = new IntRange(1, 3);
	}
}
