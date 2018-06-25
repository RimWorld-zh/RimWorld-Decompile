using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006AF RID: 1711
	public class Building_Sarcophagus : Building_Grave
	{
		// Token: 0x0400143F RID: 5183
		private bool everNonEmpty = false;

		// Token: 0x04001440 RID: 5184
		private bool thisIsFirstBodyEver = false;

		// Token: 0x060024B8 RID: 9400 RVA: 0x0013A6B6 File Offset: 0x00138AB6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.everNonEmpty, "everNonEmpty", false, false);
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x0013A6D4 File Offset: 0x00138AD4
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

		// Token: 0x060024BA RID: 9402 RVA: 0x0013A718 File Offset: 0x00138B18
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
