using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000534 RID: 1332
	public class Thought_Dumb : Thought
	{
		// Token: 0x04000E9E RID: 3742
		private int forcedStage = 0;

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060018B4 RID: 6324 RVA: 0x000D84C4 File Offset: 0x000D68C4
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x000D84DF File Offset: 0x000D68DF
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x000D84E9 File Offset: 0x000D68E9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}
	}
}
