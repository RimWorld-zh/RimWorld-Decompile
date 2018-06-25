using System;
using Verse;

namespace RimWorld
{
	public class Building_Sarcophagus : Building_Grave
	{
		private bool everNonEmpty = false;

		private bool thisIsFirstBodyEver = false;

		public Building_Sarcophagus()
		{
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.everNonEmpty, "everNonEmpty", false, false);
		}

		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			bool result;
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				this.thisIsFirstBodyEver = !this.everNonEmpty;
				this.everNonEmpty = true;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public override void Notify_CorpseBuried(Pawn worker)
		{
			base.Notify_CorpseBuried(worker);
			if (this.thisIsFirstBodyEver && worker.IsColonist && base.Corpse.InnerPawn.def.race.Humanlike && !base.Corpse.everBuriedInSarcophagus)
			{
				base.Corpse.everBuriedInSarcophagus = true;
				foreach (Pawn pawn in base.Map.mapPawns.FreeColonists)
				{
					if (pawn.needs.mood != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowBuriedInSarcophagus, null);
					}
				}
			}
		}
	}
}
