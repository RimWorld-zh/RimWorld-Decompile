using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200075E RID: 1886
	public class CompUseEffect_Artifact : CompUseEffect
	{
		// Token: 0x060029B5 RID: 10677 RVA: 0x001621B4 File Offset: 0x001605B4
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(usedBy.MapHeld);
			usedBy.records.Increment(RecordDefOf.ArtifactsActivated);
		}
	}
}
