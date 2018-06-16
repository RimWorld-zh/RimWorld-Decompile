using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D6 RID: 2262
	public static class TutorSystem
	{
		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x060033BC RID: 13244 RVA: 0x001B9F7C File Offset: 0x001B837C
		public static bool TutorialMode
		{
			get
			{
				return Find.Storyteller != null && Find.Storyteller.def != null && Find.Storyteller.def.tutorialMode;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x060033BD RID: 13245 RVA: 0x001B9FBC File Offset: 0x001B83BC
		public static bool AdaptiveTrainingEnabled
		{
			get
			{
				return Prefs.AdaptiveTrainingEnabled && (Find.Storyteller == null || Find.Storyteller.def == null || !Find.Storyteller.def.disableAdaptiveTraining);
			}
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x001BA017 File Offset: 0x001B8417
		public static void Notify_Event(string eventTag, IntVec3 cell)
		{
			TutorSystem.Notify_Event(new EventPack(eventTag, cell));
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x001BA028 File Offset: 0x001B8428
		public static void Notify_Event(EventPack ep)
		{
			if (TutorSystem.TutorialMode)
			{
				if (DebugViewSettings.logTutor)
				{
					Log.Message("Notify_Event: " + ep, false);
				}
				if (Current.Game != null)
				{
					Lesson lesson = Find.ActiveLesson.Current;
					if (Find.ActiveLesson.Current != null)
					{
						Find.ActiveLesson.Current.Notify_Event(ep);
					}
					foreach (InstructionDef instructionDef in DefDatabase<InstructionDef>.AllDefs)
					{
						if (instructionDef.eventTagInitiate == ep.Tag && (instructionDef.eventTagInitiateSource == null || (lesson != null && instructionDef.eventTagInitiateSource == lesson.Instruction)) && (TutorSystem.TutorialMode || !instructionDef.tutorialModeOnly))
						{
							Find.ActiveLesson.Activate(instructionDef);
							break;
						}
					}
				}
			}
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x001BA144 File Offset: 0x001B8544
		public static bool AllowAction(EventPack ep)
		{
			bool result;
			if (!TutorSystem.TutorialMode)
			{
				result = true;
			}
			else
			{
				if (DebugViewSettings.logTutor)
				{
					Log.Message("AllowAction: " + ep, false);
				}
				if (ep.Cells != null && ep.Cells.Count<IntVec3>() == 1)
				{
					result = TutorSystem.AllowAction(new EventPack(ep.Tag, ep.Cells.First<IntVec3>()));
				}
				else
				{
					if (Find.ActiveLesson.Current != null)
					{
						AcceptanceReport acceptanceReport = Find.ActiveLesson.Current.AllowAction(ep);
						if (!acceptanceReport.Accepted)
						{
							string text = acceptanceReport.Reason.NullOrEmpty() ? Find.ActiveLesson.Current.DefaultRejectInputMessage : acceptanceReport.Reason;
							Messages.Message(text, MessageTypeDefOf.RejectInput, false);
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}
	}
}
