using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class LessonAutoActivator
	{
		private static Dictionary<ConceptDef, float> opportunities = new Dictionary<ConceptDef, float>();

		private static float timeSinceLastLesson = 10000f;

		private static List<ConceptDef> alertingConcepts = new List<ConceptDef>();

		private const float MapStartGracePeriod = 8f;

		private const float KnowledgeDecayRate = 0.00015f;

		private const float OpportunityDecayRate = 0.4f;

		private const float OpportunityMaxDesireAdd = 60f;

		private const int CheckInterval = 15;

		private const float MaxLessonInterval = 900f;

		[CompilerGenerated]
		private static Func<ConceptDef, float> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Func<ConceptDef, float> _003C_003Ef__mg_0024cache1;

		private static float SecondsSinceLesson
		{
			get
			{
				return LessonAutoActivator.timeSinceLastLesson;
			}
		}

		private static float RelaxDesire
		{
			get
			{
				return (float)(100.0 - LessonAutoActivator.SecondsSinceLesson * 0.1111111119389534);
			}
		}

		public static void Reset()
		{
			LessonAutoActivator.alertingConcepts.Clear();
		}

		public static void TeachOpportunity(ConceptDef conc, OpportunityType opp)
		{
			LessonAutoActivator.TeachOpportunity(conc, null, opp);
		}

		public static void TeachOpportunity(ConceptDef conc, Thing subject, OpportunityType opp)
		{
			if (TutorSystem.AdaptiveTrainingEnabled && !PlayerKnowledgeDatabase.IsComplete(conc))
			{
				float value = 999f;
				switch (opp)
				{
				case OpportunityType.GoodToKnow:
					value = 60f;
					break;
				case OpportunityType.Important:
					value = 80f;
					break;
				case OpportunityType.Critical:
					value = 100f;
					break;
				default:
					Log.Error("Unknown need");
					break;
				}
				LessonAutoActivator.opportunities[conc] = value;
				if ((int)opp < 1 && Find.Tutor.learningReadout.ActiveConceptsCount >= 4)
					return;
				LessonAutoActivator.TryInitiateLesson(conc);
			}
		}

		public static void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (PlayerKnowledgeDatabase.IsComplete(conc))
			{
				LessonAutoActivator.opportunities[conc] = 0f;
			}
		}

		public static void LessonAutoActivatorUpdate()
		{
			if (TutorSystem.AdaptiveTrainingEnabled && Current.Game != null && !Find.Tutor.learningReadout.ShowAllMode)
			{
				LessonAutoActivator.timeSinceLastLesson += RealTime.realDeltaTime;
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (Time.timeSinceLevelLoad < 8.0)
						return;
					if (Find.WindowStack.SecondsSinceClosedGameStartDialog < 8.0)
						return;
					if (Find.TickManager.NotPlaying)
						return;
				}
				for (int num = LessonAutoActivator.alertingConcepts.Count - 1; num >= 0; num--)
				{
					if (PlayerKnowledgeDatabase.IsComplete(LessonAutoActivator.alertingConcepts[num]))
					{
						LessonAutoActivator.alertingConcepts.RemoveAt(num);
					}
				}
				if (Time.frameCount % 15 == 0 && Find.ActiveLesson.Current == null)
				{
					for (int i = 0; i < DefDatabase<ConceptDef>.AllDefsListForReading.Count; i++)
					{
						ConceptDef conceptDef = DefDatabase<ConceptDef>.AllDefsListForReading[i];
						if (!PlayerKnowledgeDatabase.IsComplete(conceptDef))
						{
							float knowledge = PlayerKnowledgeDatabase.GetKnowledge(conceptDef);
							knowledge = (float)(knowledge - 0.0001500000071246177 * Time.deltaTime * 15.0);
							if (knowledge < 0.0)
							{
								knowledge = 0f;
							}
							PlayerKnowledgeDatabase.SetKnowledge(conceptDef, knowledge);
							if (conceptDef.opportunityDecays)
							{
								float opportunity = LessonAutoActivator.GetOpportunity(conceptDef);
								opportunity = (float)(opportunity - 0.40000000596046448 * Time.deltaTime * 15.0);
								if (opportunity < 0.0)
								{
									opportunity = 0f;
								}
								LessonAutoActivator.opportunities[conceptDef] = opportunity;
							}
						}
					}
					if (Find.Tutor.learningReadout.ActiveConceptsCount < 3)
					{
						ConceptDef conceptDef2 = LessonAutoActivator.MostDesiredConcept();
						if (conceptDef2 != null)
						{
							float desire = LessonAutoActivator.GetDesire(conceptDef2);
							if (desire > 0.10000000149011612 && LessonAutoActivator.RelaxDesire < desire)
							{
								LessonAutoActivator.TryInitiateLesson(conceptDef2);
							}
						}
					}
					else
					{
						LessonAutoActivator.SetLastLessonTimeToNow();
					}
				}
			}
		}

		private static ConceptDef MostDesiredConcept()
		{
			float num = -9999f;
			ConceptDef result = null;
			List<ConceptDef> allDefsListForReading = DefDatabase<ConceptDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ConceptDef conceptDef = allDefsListForReading[i];
				float desire = LessonAutoActivator.GetDesire(conceptDef);
				if (desire > num && (!conceptDef.needsOpportunity || !(LessonAutoActivator.GetOpportunity(conceptDef) < 0.10000000149011612)) && !(PlayerKnowledgeDatabase.GetKnowledge(conceptDef) > 0.15000000596046448))
				{
					num = desire;
					result = conceptDef;
				}
			}
			return result;
		}

		private static float GetDesire(ConceptDef conc)
		{
			if (PlayerKnowledgeDatabase.IsComplete(conc))
			{
				return 0f;
			}
			if (Find.Tutor.learningReadout.IsActive(conc))
			{
				return 0f;
			}
			if (Current.ProgramState != conc.gameMode)
			{
				return 0f;
			}
			if (conc.needsOpportunity && LessonAutoActivator.GetOpportunity(conc) < 0.10000000149011612)
			{
				return 0f;
			}
			float num = 0f;
			num += conc.priority;
			num = (float)(num + LessonAutoActivator.GetOpportunity(conc) / 100.0 * 60.0);
			return (float)(num * (1.0 - PlayerKnowledgeDatabase.GetKnowledge(conc)));
		}

		private static float GetOpportunity(ConceptDef conc)
		{
			float result = default(float);
			if (LessonAutoActivator.opportunities.TryGetValue(conc, out result))
			{
				return result;
			}
			LessonAutoActivator.opportunities[conc] = 0f;
			return 0f;
		}

		private static void TryInitiateLesson(ConceptDef conc)
		{
			if (Find.Tutor.learningReadout.TryActivateConcept(conc))
			{
				LessonAutoActivator.SetLastLessonTimeToNow();
			}
		}

		private static void SetLastLessonTimeToNow()
		{
			LessonAutoActivator.timeSinceLastLesson = 0f;
		}

		public static void Notify_TutorialEnding()
		{
			LessonAutoActivator.SetLastLessonTimeToNow();
		}

		public static string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("RelaxDesire: " + LessonAutoActivator.RelaxDesire);
			foreach (ConceptDef item in DefDatabase<ConceptDef>.AllDefs.OrderByDescending(LessonAutoActivator.GetDesire))
			{
				if (PlayerKnowledgeDatabase.IsComplete(item))
				{
					stringBuilder.AppendLine(item.defName + " complete");
				}
				else
				{
					stringBuilder.AppendLine(item.defName + "\n   know " + PlayerKnowledgeDatabase.GetKnowledge(item).ToString("F3") + "\n   need " + LessonAutoActivator.opportunities[item].ToString("F3") + "\n   des " + LessonAutoActivator.GetDesire(item).ToString("F3"));
				}
			}
			return stringBuilder.ToString();
		}

		public static void DebugForceInitiateBestLessonNow()
		{
			LessonAutoActivator.TryInitiateLesson(DefDatabase<ConceptDef>.AllDefs.OrderByDescending(LessonAutoActivator.GetDesire).First());
		}
	}
}
