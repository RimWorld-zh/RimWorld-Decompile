using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000191 RID: 401
	public abstract class LordToil_HiveRelated : LordToil
	{
		// Token: 0x0600084E RID: 2126 RVA: 0x0004ED38 File Offset: 0x0004D138
		public LordToil_HiveRelated()
		{
			this.data = new LordToil_HiveRelatedData();
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x0004ED4C File Offset: 0x0004D14C
		private LordToil_HiveRelatedData Data
		{
			get
			{
				return (LordToil_HiveRelatedData)this.data;
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0004ED6C File Offset: 0x0004D16C
		protected void FilterOutUnspawnedHives()
		{
			this.Data.assignedHives.RemoveAll((KeyValuePair<Pawn, Hive> x) => x.Value == null || !x.Value.Spawned);
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0004EDA0 File Offset: 0x0004D1A0
		protected Hive GetHiveFor(Pawn pawn)
		{
			Hive hive;
			Hive result;
			if (this.Data.assignedHives.TryGetValue(pawn, out hive))
			{
				result = hive;
			}
			else
			{
				hive = this.FindClosestHive(pawn);
				if (hive != null)
				{
					this.Data.assignedHives.Add(pawn, hive);
				}
				result = hive;
			}
			return result;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0004EDF8 File Offset: 0x0004D1F8
		private Hive FindClosestHive(Pawn pawn)
		{
			return (Hive)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Hive), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 30f, (Thing x) => x.Faction == pawn.Faction, null, 0, 30, false, RegionType.Set_Passable, false);
		}
	}
}
