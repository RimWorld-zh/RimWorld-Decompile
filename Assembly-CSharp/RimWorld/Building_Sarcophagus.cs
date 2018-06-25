using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006AF RID: 1711
	public class Building_Sarcophagus : Building_Grave
	{
		// Token: 0x04001443 RID: 5187
		private bool everNonEmpty = false;

		// Token: 0x04001444 RID: 5188
		private bool thisIsFirstBodyEver = false;

		// Token: 0x060024B7 RID: 9399 RVA: 0x0013A91E File Offset: 0x00138D1E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.everNonEmpty, "everNonEmpty", false, false);
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x0013A93C File Offset: 0x00138D3C
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

		// Token: 0x060024B9 RID: 9401 RVA: 0x0013A980 File Offset: 0x00138D80
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
