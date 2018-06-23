using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B1 RID: 2225
	public class ActiveLessonHandler : IExposable
	{
		// Token: 0x04001B7D RID: 7037
		private Lesson activeLesson;

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060032E2 RID: 13026 RVA: 0x001B6814 File Offset: 0x001B4C14
		public Lesson Current
		{
			get
			{
				return this.activeLesson;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x060032E3 RID: 13027 RVA: 0x001B6830 File Offset: 0x001B4C30
		public bool ActiveLessonVisible
		{
			get
			{
				return this.activeLesson != null && !Find.WindowStack.WindowsPreventDrawTutor;
			}
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x001B6860 File Offset: 0x001B4C60
		public void ExposeData()
		{
			Scribe_Deep.Look<Lesson>(ref this.activeLesson, "activeLesson", new object[0]);
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x001B687C File Offset: 0x001B4C7C
		public void Activate(InstructionDef id)
		{
			Lesson_Instruction lesson_Instruction = this.activeLesson as Lesson_Instruction;
			if (lesson_Instruction == null || id != lesson_Instruction.def)
			{
				Lesson_Instruction lesson_Instruction2 = (Lesson_Instruction)Activator.CreateInstance(id.instructionClass);
				lesson_Instruction2.def = id;
				this.activeLesson = lesson_Instruction2;
				this.activeLesson.OnActivated();
			}
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x001B68D8 File Offset: 0x001B4CD8
		public void Activate(Lesson lesson)
		{
			Lesson_Note lesson_Note = lesson as Lesson_Note;
			if (lesson_Note != null && this.activeLesson != null)
			{
				lesson_Note.doFadeIn = false;
			}
			this.activeLesson = lesson;
			this.activeLesson.OnActivated();
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x001B6918 File Offset: 0x001B4D18
		public void Deactivate()
		{
			Lesson lesson = this.activeLesson;
			this.activeLesson = null;
			if (lesson != null)
			{
				lesson.PostDeactivated();
			}
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x001B6940 File Offset: 0x001B4D40
		public void ActiveLessonOnGUI()
		{
			if (Time.timeSinceLevelLoad >= 0.01f && this.ActiveLessonVisible)
			{
				this.activeLesson.LessonOnGUI();
			}
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x001B696D File Offset: 0x001B4D6D
		public void ActiveLessonUpdate()
		{
			if (Time.timeSinceLevelLoad >= 0.01f && this.ActiveLessonVisible)
			{
				this.activeLesson.LessonUpdate();
			}
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x001B699A File Offset: 0x001B4D9A
		public void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.Current != null)
			{
				this.Current.Notify_KnowledgeDemonstrated(conc);
			}
		}
	}
}
