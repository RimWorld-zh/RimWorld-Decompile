using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C5 RID: 453
	public class ThinkNode_ConditionalTrainableCompleted : ThinkNode_Conditional
	{
		// Token: 0x040003DE RID: 990
		private TrainableDef trainable;

		// Token: 0x0600093E RID: 2366 RVA: 0x00056090 File Offset: 0x00054490
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalTrainableCompleted thinkNode_ConditionalTrainableCompleted = (ThinkNode_ConditionalTrainableCompleted)base.DeepCopy(resolve);
			thinkNode_ConditionalTrainableCompleted.trainable = this.trainable;
			return thinkNode_ConditionalTrainableCompleted;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x000560C0 File Offset: 0x000544C0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.training != null && pawn.training.HasLearned(this.trainable);
		}
	}
}
