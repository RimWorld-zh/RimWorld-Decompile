using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000760 RID: 1888
	public class CompUseEffect_Artifact : CompUseEffect
	{
		// Token: 0x060029B8 RID: 10680 RVA: 0x00162564 File Offset: 0x00160964
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(usedBy.MapHeld);
			usedBy.records.Increment(RecordDefOf.ArtifactsActivated);
		}
	}
}
