using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003CB RID: 971
	public class SymbolResolver_Hives : SymbolResolver
	{
		// Token: 0x04000A35 RID: 2613
		private static readonly IntRange DefaultHivesCountRange = new IntRange(1, 3);

		// Token: 0x060010BB RID: 4283 RVA: 0x0008E680 File Offset: 0x0008CA80
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && this.TryFindFirstHivePos(rp.rect, out intVec);
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0008E6C4 File Offset: 0x0008CAC4
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

		// Token: 0x060010BD RID: 4285 RVA: 0x0008E7B0 File Offset: 0x0008CBB0
		private bool TryFindFirstHivePos(CellRect rect, out IntVec3 pos)
		{
			Map map = BaseGen.globalSettings.map;
			return (from mc in rect.Cells
			where mc.Standable(map)
			select mc).TryRandomElement(out pos);
		}
	}
}
