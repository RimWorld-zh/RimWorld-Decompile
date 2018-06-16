using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000762 RID: 1890
	public class CompUseEffect_Artifact : CompUseEffect
	{
		// Token: 0x060029BA RID: 10682 RVA: 0x00161F48 File Offset: 0x00160348
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(usedBy.MapHeld);
			usedBy.records.Increment(RecordDefOf.ArtifactsActivated);
		}
	}
}
