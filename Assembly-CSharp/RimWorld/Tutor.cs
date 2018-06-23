using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D1 RID: 2257
	public class Tutor : IExposable
	{
		// Token: 0x04001BC1 RID: 7105
		public ActiveLessonHandler activeLesson = new ActiveLessonHandler();

		// Token: 0x04001BC2 RID: 7106
		public LearningReadout learningReadout = new LearningReadout();

		// Token: 0x04001BC3 RID: 7107
		public TutorialState tutorialState = new TutorialState();

		// Token: 0x060033B4 RID: 13236 RVA: 0x001BA1A8 File Offset: 0x001B85A8
		public void ExposeData()
		{
			Scribe_Deep.Look<ActiveLessonHandler>(ref this.activeLesson, "activeLesson", new object[0]);
			Scribe_Deep.Look<LearningReadout>(ref this.learningReadout, "learningReadout", new object[0]);
			Scribe_Deep.Look<TutorialState>(ref this.tutorialState, "tutorialState", new object[0]);
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x001BA1F8 File Offset: 0x001B85F8
		internal void TutorUpdate()
		{
			this.activeLesson.ActiveLessonUpdate();
			this.learningReadout.LearningReadoutUpdate();
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x001BA211 File Offset: 0x001B8611
		internal void TutorOnGUI()
		{
			this.activeLesson.ActiveLessonOnGUI();
			this.learningReadout.LearningReadoutOnGUI();
		}
	}
}
