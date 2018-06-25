using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D3 RID: 2259
	public class Tutor : IExposable
	{
		// Token: 0x04001BC7 RID: 7111
		public ActiveLessonHandler activeLesson = new ActiveLessonHandler();

		// Token: 0x04001BC8 RID: 7112
		public LearningReadout learningReadout = new LearningReadout();

		// Token: 0x04001BC9 RID: 7113
		public TutorialState tutorialState = new TutorialState();

		// Token: 0x060033B8 RID: 13240 RVA: 0x001BA5BC File Offset: 0x001B89BC
		public void ExposeData()
		{
			Scribe_Deep.Look<ActiveLessonHandler>(ref this.activeLesson, "activeLesson", new object[0]);
			Scribe_Deep.Look<LearningReadout>(ref this.learningReadout, "learningReadout", new object[0]);
			Scribe_Deep.Look<TutorialState>(ref this.tutorialState, "tutorialState", new object[0]);
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x001BA60C File Offset: 0x001B8A0C
		internal void TutorUpdate()
		{
			this.activeLesson.ActiveLessonUpdate();
			this.learningReadout.LearningReadoutUpdate();
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x001BA625 File Offset: 0x001B8A25
		internal void TutorOnGUI()
		{
			this.activeLesson.ActiveLessonOnGUI();
			this.learningReadout.LearningReadoutOnGUI();
		}
	}
}
