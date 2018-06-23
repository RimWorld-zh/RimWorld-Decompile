using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000532 RID: 1330
	public class Thought_Dumb : Thought
	{
		// Token: 0x04000E9A RID: 3738
		private int forcedStage = 0;

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x000D810C File Offset: 0x000D650C
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x000D8127 File Offset: 0x000D6527
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x000D8131 File Offset: 0x000D6531
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}
	}
}
