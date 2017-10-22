using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JoyGiver
	{
		public JoyGiverDef def;

		private static List<Thing> tmpCandidates = new List<Thing>();

		public virtual float GetChance(Pawn pawn)
		{
			return this.def.baseChance;
		}

		protected virtual List<Thing> GetSearchSet(Pawn pawn)
		{
			List<Thing> result;
			if (this.def.thingDefs == null)
			{
				JoyGiver.tmpCandidates.Clear();
				result = JoyGiver.tmpCandidates;
			}
			else if (this.def.thingDefs.Count == 1)
			{
				result = pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[0]);
			}
			else
			{
				JoyGiver.tmpCandidates.Clear();
				for (int i = 0; i < this.def.thingDefs.Count; i++)
				{
					JoyGiver.tmpCandidates.AddRange(pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[i]));
				}
				result = JoyGiver.tmpCandidates;
			}
			return result;
		}

		public abstract Job TryGiveJob(Pawn pawn);

		public virtual Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return null;
		}

		public virtual Job TryGiveJobInPartyArea(Pawn pawn, IntVec3 partySpot)
		{
			return null;
		}

		public PawnCapacityDef MissingRequiredCapacity(Pawn pawn)
		{
			int num = 0;
			PawnCapacityDef result;
			while (true)
			{
				if (num < this.def.requiredCapacities.Count)
				{
					if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[num]))
					{
						result = this.def.requiredCapacities[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
