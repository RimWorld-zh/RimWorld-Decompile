using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000534 RID: 1332
	public class Thought_Dumb : Thought
	{
		// Token: 0x04000E9A RID: 3738
		private int forcedStage = 0;

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x000D825C File Offset: 0x000D665C
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x000D8277 File Offset: 0x000D6677
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x000D8281 File Offset: 0x000D6681
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}
	}
}
