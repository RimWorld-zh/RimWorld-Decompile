using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordToil_DefendAndExpandHive : LordToil
	{
		public float distToHiveToAttack = 10f;

		private LordToilData_DefendAndExpandHive Data
		{
			get
			{
				return (LordToilData_DefendAndExpandHive)base.data;
			}
		}

		public LordToil_DefendAndExpandHive()
		{
			base.data = new LordToilData_DefendAndExpandHive();
		}

		public override void UpdateAllDuties()
		{
			this.FilterOutUnspawnedHives();
			for (int i = 0; i < base.lord.ownedPawns.Count; i++)
			{
				Hive hiveFor = this.GetHiveFor(base.lord.ownedPawns[i]);
				PawnDuty duty = new PawnDuty(DutyDefOf.DefendAndExpandHive, hiveFor, this.distToHiveToAttack);
				base.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		private void FilterOutUnspawnedHives()
		{
			this.Data.assignedHives.RemoveAll((KeyValuePair<Pawn, Hive> x) => x.Value == null || !x.Value.Spawned);
		}

		private Hive GetHiveFor(Pawn pawn)
		{
			Hive hive = default(Hive);
			if (this.Data.assignedHives.TryGetValue(pawn, out hive))
			{
				return hive;
			}
			hive = this.FindClosestHive(pawn);
			if (hive != null)
			{
				this.Data.assignedHives.Add(pawn, hive);
			}
			return hive;
		}

		private Hive FindClosestHive(Pawn pawn)
		{
			return (Hive)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Hive), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 30f, (Thing x) => x.Faction == pawn.Faction, null, 0, 30, false, RegionType.Set_Passable, false);
		}
	}
}
