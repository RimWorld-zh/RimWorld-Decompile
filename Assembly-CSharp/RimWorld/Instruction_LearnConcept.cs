using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C7 RID: 2247
	public class Instruction_LearnConcept : Lesson_Instruction
	{
		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06003356 RID: 13142 RVA: 0x001B8D30 File Offset: 0x001B7130
		protected override float ProgressPercent
		{
			get
			{
				return PlayerKnowledgeDatabase.GetKnowledge(this.def.concept);
			}
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x001B8D55 File Offset: 0x001B7155
		public override void OnActivated()
		{
			PlayerKnowledgeDatabase.SetKnowledge(this.def.concept, 0f);
			base.OnActivated();
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x001B8D73 File Offset: 0x001B7173
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
