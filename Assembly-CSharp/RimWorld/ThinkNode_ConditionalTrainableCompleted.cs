using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C5 RID: 453
	public class ThinkNode_ConditionalTrainableCompleted : ThinkNode_Conditional
	{
		// Token: 0x06000941 RID: 2369 RVA: 0x00056080 File Offset: 0x00054480
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalTrainableCompleted thinkNode_ConditionalTrainableCompleted = (ThinkNode_ConditionalTrainableCompleted)base.DeepCopy(resolve);
			thinkNode_ConditionalTrainableCompleted.trainable = this.trainable;
			return thinkNode_ConditionalTrainableCompleted;
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x000560B0 File Offset: 0x000544B0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.training != null && pawn.training.HasLearned(this.trainable);
		}

		// Token: 0x040003DF RID: 991
		private TrainableDef trainable;
	}
}
