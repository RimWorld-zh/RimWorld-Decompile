using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000536 RID: 1334
	public class Thought_Dumb : Thought
	{
		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x000D80AC File Offset: 0x000D64AC
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x000D80C7 File Offset: 0x000D64C7
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x000D80D1 File Offset: 0x000D64D1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}

		// Token: 0x04000E9D RID: 3741
		private int forcedStage = 0;
	}
}
