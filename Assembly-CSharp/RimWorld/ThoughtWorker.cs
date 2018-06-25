using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020001EF RID: 495
	public abstract class ThoughtWorker
	{
		// Token: 0x040003E8 RID: 1000
		public ThoughtDef def;

		// Token: 0x060009A2 RID: 2466 RVA: 0x00057218 File Offset: 0x00055618
		public ThoughtState CurrentState(Pawn p)
		{
			return this.PostProcessedState(this.CurrentStateInternal(p));
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0005723C File Offset: 0x0005563C
		public ThoughtState CurrentSocialState(Pawn p, Pawn otherPawn)
		{
			return this.PostProcessedState(this.CurrentSocialStateInternal(p, otherPawn));
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00057260 File Offset: 0x00055660
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

		// Token: 0x060009A5 RID: 2469 RVA: 0x000572A8 File Offset: 0x000556A8
		protected virtual ThoughtState CurrentStateInternal(Pawn p)
		{
			throw new NotImplementedException(this.def.defName + " (normal)");
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x000572C5 File Offset: 0x000556C5
		protected virtual ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			throw new NotImplementedException(this.def.defName + " (social)");
		}
	}
}
