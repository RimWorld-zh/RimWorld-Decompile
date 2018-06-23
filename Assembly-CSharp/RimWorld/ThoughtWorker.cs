using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020001EF RID: 495
	public abstract class ThoughtWorker
	{
		// Token: 0x040003E7 RID: 999
		public ThoughtDef def;

		// Token: 0x060009A3 RID: 2467 RVA: 0x0005721C File Offset: 0x0005561C
		public ThoughtState CurrentState(Pawn p)
		{
			return this.PostProcessedState(this.CurrentStateInternal(p));
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00057240 File Offset: 0x00055640
		public ThoughtState CurrentSocialState(Pawn p, Pawn otherPawn)
		{
			return this.PostProcessedState(this.CurrentSocialStateInternal(p, otherPawn));
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00057264 File Offset: 0x00055664
		private ThoughtState PostProcessedState(ThoughtState state)
		{
			if (this.def.invert)
			{
				if (state.Active)
				{
					state = ThoughtState.Inactive;
				}
				else
				{
					state = ThoughtState.ActiveAtStage(0);
				}
			}
			return state;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x000572AC File Offset: 0x000556AC
		protected virtual ThoughtState CurrentStateInternal(Pawn p)
		{
			throw new NotImplementedException(this.def.defName + " (normal)");
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x000572C9 File Offset: 0x000556C9
		protected virtual ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			throw new NotImplementedException(this.def.defName + " (social)");
		}
	}
}
