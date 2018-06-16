using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020001EF RID: 495
	public abstract class ThoughtWorker
	{
		// Token: 0x060009A5 RID: 2469 RVA: 0x000571D8 File Offset: 0x000555D8
		public ThoughtState CurrentState(Pawn p)
		{
			return this.PostProcessedState(this.CurrentStateInternal(p));
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x000571FC File Offset: 0x000555FC
		public ThoughtState CurrentSocialState(Pawn p, Pawn otherPawn)
		{
			return this.PostProcessedState(this.CurrentSocialStateInternal(p, otherPawn));
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00057220 File Offset: 0x00055620
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

		// Token: 0x060009A8 RID: 2472 RVA: 0x00057268 File Offset: 0x00055668
		protected virtual ThoughtState CurrentStateInternal(Pawn p)
		{
			throw new NotImplementedException(this.def.defName + " (normal)");
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00057285 File Offset: 0x00055685
		protected virtual ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			throw new NotImplementedException(this.def.defName + " (social)");
		}

		// Token: 0x040003E9 RID: 1001
		public ThoughtDef def;
	}
}
