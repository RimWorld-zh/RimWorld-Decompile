using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003CB RID: 971
	public class SymbolResolver_Hives : SymbolResolver
	{
		// Token: 0x04000A38 RID: 2616
		private static readonly IntRange DefaultHivesCountRange = new IntRange(1, 3);

		// Token: 0x060010BA RID: 4282 RVA: 0x0008E690 File Offset: 0x0008CA90
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindFirstHivePos(rp.rect, out intVec);
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x0008E6D4 File Offset: 0x0008CAD4
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

		// Token: 0x060010BC RID: 4284 RVA: 0x0008E7C0 File Offset: 0x0008CBC0
		private bool TryFindFirstHivePos(CellRect rect, out IntVec3 pos)
		{
			Map map = BaseGen.globalSettings.map;
			return (from mc in rect.Cells
			where mc.Standable(map)
			select mc).TryRandomElement(out pos);
		}
	}
}
