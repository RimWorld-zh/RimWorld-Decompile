using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C3 RID: 2243
	public class Instruction_LearnConcept : Lesson_Instruction
	{
		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06003351 RID: 13137 RVA: 0x001B8FE0 File Offset: 0x001B73E0
		protected override float ProgressPercent
		{
			get
			{
				return PlayerKnowledgeDatabase.GetKnowledge(this.def.concept);
			}
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x001B9005 File Offset: 0x001B7405
		public override void OnActivated()
		{
			PlayerKnowledgeDatabase.SetKnowledge(this.def.concept, 0f);
			base.OnActivated();
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x001B9023 File Offset: 0x001B7423
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
