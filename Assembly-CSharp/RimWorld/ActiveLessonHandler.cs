using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008B3 RID: 2227
	public class ActiveLessonHandler : IExposable
	{
		// Token: 0x04001B7D RID: 7037
		private Lesson activeLesson;

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060032E6 RID: 13030 RVA: 0x001B6954 File Offset: 0x001B4D54
		public Lesson Current
		{
			get
			{
				return this.activeLesson;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x060032E7 RID: 13031 RVA: 0x001B6970 File Offset: 0x001B4D70
		public bool ActiveLessonVisible
		{
			get
			{
				return this.activeLesson != null && !Find.WindowStack.WindowsPreventDrawTutor;
			}
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x001B69A0 File Offset: 0x001B4DA0
		public void ExposeData()
		{
			Scribe_Deep.Look<Lesson>(ref this.activeLesson, "activeLesson", new object[0]);
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x001B69BC File Offset: 0x001B4DBC
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

		// Token: 0x060032EA RID: 13034 RVA: 0x001B6A18 File Offset: 0x001B4E18
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

		// Token: 0x060032EB RID: 13035 RVA: 0x001B6A58 File Offset: 0x001B4E58
		public void Deactivate()
		{
			Lesson lesson = this.activeLesson;
			this.activeLesson = null;
			if (lesson != null)
			{
				lesson.PostDeactivated();
			}
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x001B6A80 File Offset: 0x001B4E80
		public void ActiveLessonOnGUI()
		{
			if (Time.timeSinceLevelLoad >= 0.01f && this.ActiveLessonVisible)
			{
				this.activeLesson.LessonOnGUI();
			}
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x001B6AAD File Offset: 0x001B4EAD
		public void ActiveLessonUpdate()
		{
			if (Time.timeSinceLevelLoad >= 0.01f && this.ActiveLessonVisible)
			{
				this.activeLesson.LessonUpdate();
			}
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x001B6ADA File Offset: 0x001B4EDA
		public void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.Current != null)
			{
				this.Current.Notify_KnowledgeDemonstrated(conc);
			}
		}
	}
}
