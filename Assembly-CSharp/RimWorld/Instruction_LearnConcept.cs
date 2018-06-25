using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C5 RID: 2245
	public class Instruction_LearnConcept : Lesson_Instruction
	{
		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06003355 RID: 13141 RVA: 0x001B9120 File Offset: 0x001B7520
		protected override float ProgressPercent
		{
			get
			{
				return PlayerKnowledgeDatabase.GetKnowledge(this.def.concept);
			}
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x001B9145 File Offset: 0x001B7545
		public override void OnActivated()
		{
			PlayerKnowledgeDatabase.SetKnowledge(this.def.concept, 0f);
			base.OnActivated();
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x001B9163 File Offset: 0x001B7563
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
