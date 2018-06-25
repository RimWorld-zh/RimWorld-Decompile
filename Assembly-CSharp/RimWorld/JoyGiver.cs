using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JoyGiver
	{
		public JoyGiverDef def;

		private static List<Thing> tmpCandidates = new List<Thing>();

		protected JoyGiver()
		{
		}

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
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return this.def.requiredCapacities[i];
				}
			}
			return null;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static JoyGiver()
		{
		}
	}
}
