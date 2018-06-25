using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C5 RID: 2245
	public class Instruction_LearnConcept : Lesson_Instruction
	{
		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06003355 RID: 13141 RVA: 0x001B93F4 File Offset: 0x001B77F4
		protected override float ProgressPercent
		{
			get
			{
				return PlayerKnowledgeDatabase.GetKnowledge(this.def.concept);
			}
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x001B9419 File Offset: 0x001B7819
		public override void OnActivated()
		{
			PlayerKnowledgeDatabase.SetKnowledge(this.def.concept, 0f);
			base.OnActivated();
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x001B9437 File Offset: 0x001B7837
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
