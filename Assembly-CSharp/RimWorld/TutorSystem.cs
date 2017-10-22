using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class TutorSystem
	{
		public static bool TutorialMode
		{
			get
			{
				return Find.Storyteller != null && Find.Storyteller.def != null && Find.Storyteller.def.tutorialMode;
			}
		}

		public static bool AdaptiveTrainingEnabled
		{
			get
			{
				return (byte)(Prefs.AdaptiveTrainingEnabled ? ((Find.Storyteller == null || Find.Storyteller.def == null || !Find.Storyteller.def.disableAdaptiveTraining) ? 1 : 0) : 0) != 0;
			}
		}

		public static void Notify_Event(string eventTag, IntVec3 cell)
		{
			TutorSystem.Notify_Event(new EventPack(eventTag, cell));
		}

		public static void Notify_Event(EventPack ep)
		{
			if (TutorSystem.TutorialMode)
			{
				if (DebugViewSettings.logTutor)
				{
					Log.Message("Notify_Event: " + ep);
				}
				if (Current.Game != null)
				{
					if (Find.ActiveLesson.Current != null)
					{
						Find.ActiveLesson.Current.Notify_Event(ep);
					}
					using (IEnumerator<InstructionDef> enumerator = DefDatabase<InstructionDef>.AllDefs.GetEnumerator())
					{
						InstructionDef current;
						while (true)
						{
							if (enumerator.MoveNext())
							{
								current = enumerator.Current;
								if (current.eventTagInitiate == ep.Tag)
								{
									if (TutorSystem.TutorialMode)
										break;
									if (!current.tutorialModeOnly)
										break;
								}
								continue;
							}
							return;
						}
						Find.ActiveLesson.Activate(current);
					}
				}
			}
		}

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
					Log.Message("AllowAction: " + ep);
				}
				if (ep.Cells != null && ep.Cells.Count() == 1)
				{
					result = TutorSystem.AllowAction(new EventPack(ep.Tag, ep.Cells.First()));
				}
				else
				{
					if (Find.ActiveLesson.Current != null)
					{
						AcceptanceReport acceptanceReport = Find.ActiveLesson.Current.AllowAction(ep);
						if (!acceptanceReport.Accepted)
						{
							string text = acceptanceReport.Reason.NullOrEmpty() ? Find.ActiveLesson.Current.DefaultRejectInputMessage : acceptanceReport.Reason;
							Messages.Message(text, MessageTypeDefOf.RejectInput);
							result = false;
							goto IL_00e7;
						}
					}
					result = true;
				}
			}
			goto IL_00e7;
			IL_00e7:
			return result;
		}
	}
}
