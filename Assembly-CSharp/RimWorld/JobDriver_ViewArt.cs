using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200005D RID: 93
	public class JobDriver_ViewArt : JobDriver_VisitJoyThing
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0001D80C File Offset: 0x0001BC0C
		private Thing ArtThing
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0001D838 File Offset: 0x0001BC38
		protected override void WaitTickAction()
		{
			float num = this.ArtThing.GetStatValue(StatDefOf.Beauty, true) / this.ArtThing.def.GetStatValueAbstract(StatDefOf.Beauty, null);
			float num2 = (num <= 0f) ? 0f : num;
			this.pawn.GainComfortFromCellIfPossible();
			Pawn pawn = this.pawn;
			float extraJoyGainFactor = num2;
			JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, (Building)this.ArtThing);
		}
	}
}
