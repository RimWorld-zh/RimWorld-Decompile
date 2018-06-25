using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D3 RID: 2259
	public class Tutor : IExposable
	{
		// Token: 0x04001BC1 RID: 7105
		public ActiveLessonHandler activeLesson = new ActiveLessonHandler();

		// Token: 0x04001BC2 RID: 7106
		public LearningReadout learningReadout = new LearningReadout();

		// Token: 0x04001BC3 RID: 7107
		public TutorialState tutorialState = new TutorialState();

		// Token: 0x060033B8 RID: 13240 RVA: 0x001BA2E8 File Offset: 0x001B86E8
		public void ExposeData()
		{
			Scribe_Deep.Look<ActiveLessonHandler>(ref this.activeLesson, "activeLesson", new object[0]);
			Scribe_Deep.Look<LearningReadout>(ref this.learningReadout, "learningReadout", new object[0]);
			Scribe_Deep.Look<TutorialState>(ref this.tutorialState, "tutorialState", new object[0]);
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x001BA338 File Offset: 0x001B8738
		internal void TutorUpdate()
		{
			this.activeLesson.ActiveLessonUpdate();
			this.learningReadout.LearningReadoutUpdate();
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x001BA351 File Offset: 0x001B8751
		internal void TutorOnGUI()
		{
			this.activeLesson.ActiveLessonOnGUI();
			this.learningReadout.LearningReadoutOnGUI();
		}
	}
}
