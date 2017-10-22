using Verse;

namespace RimWorld
{
	public class Instruction_LearnConcept : Lesson_Instruction
	{
		protected override float ProgressPercent
		{
			get
			{
				return PlayerKnowledgeDatabase.GetKnowledge(base.def.concept);
			}
		}

		public override void OnActivated()
		{
			PlayerKnowledgeDatabase.SetKnowledge(base.def.concept, 0f);
			base.OnActivated();
		}

		public override void LessonUpdate()
		{
			base.LessonUpdate();
			if (PlayerKnowledgeDatabase.IsComplete(base.def.concept))
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
