using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C7 RID: 2247
	public class Instruction_LearnConcept : Lesson_Instruction
	{
		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06003358 RID: 13144 RVA: 0x001B8DF8 File Offset: 0x001B71F8
		protected override float ProgressPercent
		{
			get
			{
				return PlayerKnowledgeDatabase.GetKnowledge(this.def.concept);
			}
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x001B8E1D File Offset: 0x001B721D
		public override void OnActivated()
		{
			PlayerKnowledgeDatabase.SetKnowledge(this.def.concept, 0f);
			base.OnActivated();
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x001B8E3B File Offset: 0x001B723B
		public override void LessonUpdate()
		{
			base.LessonUpdate();
			if (PlayerKnowledgeDatabase.IsComplete(this.def.concept))
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
