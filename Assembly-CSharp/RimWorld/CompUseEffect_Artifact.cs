using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000762 RID: 1890
	public class CompUseEffect_Artifact : CompUseEffect
	{
		// Token: 0x060029BC RID: 10684 RVA: 0x00161FDC File Offset: 0x001603DC
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(usedBy.MapHeld);
			usedBy.records.Increment(RecordDefOf.ArtifactsActivated);
		}
	}
}
