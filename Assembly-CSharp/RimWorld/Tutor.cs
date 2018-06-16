using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D5 RID: 2261
	public class Tutor : IExposable
	{
		// Token: 0x060033B9 RID: 13241 RVA: 0x001B9EF8 File Offset: 0x001B82F8
		public void ExposeData()
		{
			Scribe_Deep.Look<ActiveLessonHandler>(ref this.activeLesson, "activeLesson", new object[0]);
			Scribe_Deep.Look<LearningReadout>(ref this.learningReadout, "learningReadout", new object[0]);
			Scribe_Deep.Look<TutorialState>(ref this.tutorialState, "tutorialState", new object[0]);
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x001B9F48 File Offset: 0x001B8348
		internal void TutorUpdate()
		{
			this.activeLesson.ActiveLessonUpdate();
			this.learningReadout.LearningReadoutUpdate();
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x001B9F61 File Offset: 0x001B8361
		internal void TutorOnGUI()
		{
			this.activeLesson.ActiveLessonOnGUI();
			this.learningReadout.LearningReadoutOnGUI();
		}

		// Token: 0x04001BC3 RID: 7107
		public ActiveLessonHandler activeLesson = new ActiveLessonHandler();

		// Token: 0x04001BC4 RID: 7108
		public LearningReadout learningReadout = new LearningReadout();

		// Token: 0x04001BC5 RID: 7109
		public TutorialState tutorialState = new TutorialState();
	}
}
