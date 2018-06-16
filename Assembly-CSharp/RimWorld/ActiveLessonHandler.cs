using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B5 RID: 2229
	public class ActiveLessonHandler : IExposable
	{
		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060032E7 RID: 13031 RVA: 0x001B6564 File Offset: 0x001B4964
		public Lesson Current
		{
			get
			{
				return this.activeLesson;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060032E8 RID: 13032 RVA: 0x001B6580 File Offset: 0x001B4980
		public bool ActiveLessonVisible
		{
			get
			{
				return this.activeLesson != null && !Find.WindowStack.WindowsPreventDrawTutor;
			}
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x001B65B0 File Offset: 0x001B49B0
		public void ExposeData()
		{
			Scribe_Deep.Look<Lesson>(ref this.activeLesson, "activeLesson", new object[0]);
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x001B65CC File Offset: 0x001B49CC
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

		// Token: 0x060032EB RID: 13035 RVA: 0x001B6628 File Offset: 0x001B4A28
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

		// Token: 0x060032EC RID: 13036 RVA: 0x001B6668 File Offset: 0x001B4A68
		public void Deactivate()
		{
			Lesson lesson = this.activeLesson;
			this.activeLesson = null;
			if (lesson != null)
			{
				lesson.PostDeactivated();
			}
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x001B6690 File Offset: 0x001B4A90
		public void ActiveLessonOnGUI()
		{
			if (Time.timeSinceLevelLoad >= 0.01f && this.ActiveLessonVisible)
			{
				this.activeLesson.LessonOnGUI();
			}
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x001B66BD File Offset: 0x001B4ABD
		public void ActiveLessonUpdate()
		{
			if (Time.timeSinceLevelLoad >= 0.01f && this.ActiveLessonVisible)
			{
				this.activeLesson.LessonUpdate();
			}
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x001B66EA File Offset: 0x001B4AEA
		public void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.Current != null)
			{
				this.Current.Notify_KnowledgeDemonstrated(conc);
			}
		}

		// Token: 0x04001B7F RID: 7039
		private Lesson activeLesson;
	}
}
