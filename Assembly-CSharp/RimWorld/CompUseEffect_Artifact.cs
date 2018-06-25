using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000760 RID: 1888
	public class CompUseEffect_Artifact : CompUseEffect
	{
		// Token: 0x060029B9 RID: 10681 RVA: 0x00162304 File Offset: 0x00160704
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(usedBy.MapHeld);
			usedBy.records.Increment(RecordDefOf.ArtifactsActivated);
		}
	}
}
