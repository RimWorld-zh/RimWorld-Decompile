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
		private static Func<ConceptDef, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ConceptDef, float> <>f__am$cache1;

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
				return 100f - LessonAutoActivator.SecondsSinceLesson * 0.111111112f;
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
					Log.Error("Unknown need", false);
					break;
				}
				LessonAutoActivator.opportunities[conc] = value;
				if (opp >= OpportunityType.Important || Find.Tutor.learningReadout.ActiveConceptsCount < 4)
				{
					LessonAutoActivator.TryInitiateLesson(conc);
				}
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
					if (Time.timeSinceLevelLoad < 8f || Find.WindowStack.SecondsSinceClosedGameStartDialog < 8f || Find.TickManager.NotPlaying)
					{
						return;
					}
				}
				for (int i = LessonAutoActivator.alertingConcepts.Count - 1; i >= 0; i--)
				{
					if (PlayerKnowledgeDatabase.IsComplete(LessonAutoActivator.alertingConcepts[i]))
					{
						LessonAutoActivator.alertingConcepts.RemoveAt(i);
					}
				}
				if (Time.frameCount % 15 == 0 && Find.ActiveLesson.Current == null)
				{
					for (int j = 0; j < DefDatabase<ConceptDef>.AllDefsListForReading.Count; j++)
					{
						ConceptDef conceptDef = DefDatabase<ConceptDef>.AllDefsListForReading[j];
						if (!PlayerKnowledgeDatabase.IsComplete(conceptDef))
						{
							float num = PlayerKnowledgeDatabase.GetKnowledge(conceptDef);
							num -= 0.00015f * Time.deltaTime * 15f;
							if (num < 0f)
							{
								num = 0f;
							}
							PlayerKnowledgeDatabase.SetKnowledge(conceptDef, num);
							if (conceptDef.opportunityDecays)
							{
								float num2 = LessonAutoActivator.GetOpportunity(conceptDef);
								num2 -= 0.4f * Time.deltaTime * 15f;
								if (num2 < 0f)
								{
									num2 = 0f;
								}
								LessonAutoActivator.opportunities[conceptDef] = num2;
							}
						}
					}
					if (Find.Tutor.learningReadout.ActiveConceptsCount < 3)
					{
						ConceptDef conceptDef2 = LessonAutoActivator.MostDesiredConcept();
						if (conceptDef2 != null)
						{
							float desire = LessonAutoActivator.GetDesire(conceptDef2);
							if (desire > 0.1f && LessonAutoActivator.RelaxDesire < desire)
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
			int i = 0;
			while (i < allDefsListForReading.Count)
			{
				ConceptDef conceptDef = allDefsListForReading[i];
				float desire = LessonAutoActivator.GetDesire(conceptDef);
				if (desire > num)
				{
					if (!conceptDef.needsOpportunity || LessonAutoActivator.GetOpportunity(conceptDef) >= 0.1f)
					{
						if (PlayerKnowledgeDatabase.GetKnowledge(conceptDef) <= 0.15f)
						{
							num = desire;
							result = conceptDef;
						}
					}
				}
				IL_72:
				i++;
				continue;
				goto IL_72;
			}
			return result;
		}

		private static float GetDesire(ConceptDef conc)
		{
			float result;
			if (PlayerKnowledgeDatabase.IsComplete(conc))
			{
				result = 0f;
			}
			else if (Find.Tutor.learningReadout.IsActive(conc))
			{
				result = 0f;
			}
			else if (Current.ProgramState != conc.gameMode)
			{
				result = 0f;
			}
			else if (conc.needsOpportunity && LessonAutoActivator.GetOpportunity(conc) < 0.1f)
			{
				result = 0f;
			}
			else
			{
				float num = 0f;
				num += conc.priority;
				num += LessonAutoActivator.GetOpportunity(conc) / 100f * 60f;
				num *= 1f - PlayerKnowledgeDatabase.GetKnowledge(conc);
				result = num;
			}
			return result;
		}

		private static float GetOpportunity(ConceptDef conc)
		{
			float num;
			float result;
			if (LessonAutoActivator.opportunities.TryGetValue(conc, out num))
			{
				result = num;
			}
			else
			{
				LessonAutoActivator.opportunities[conc] = 0f;
				result = 0f;
			}
			return result;
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
			foreach (ConceptDef conceptDef in from co in DefDatabase<ConceptDef>.AllDefs
			orderby LessonAutoActivator.GetDesire(co) descending
			select co)
			{
				if (PlayerKnowledgeDatabase.IsComplete(conceptDef))
				{
					stringBuilder.AppendLine(conceptDef.defName + " complete");
				}
				else
				{
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						conceptDef.defName,
						"\n   know ",
						PlayerKnowledgeDatabase.GetKnowledge(conceptDef).ToString("F3"),
						"\n   need ",
						LessonAutoActivator.opportunities[conceptDef].ToString("F3"),
						"\n   des ",
						LessonAutoActivator.GetDesire(conceptDef).ToString("F3")
					}));
				}
			}
			return stringBuilder.ToString();
		}

		public static void DebugForceInitiateBestLessonNow()
		{
			LessonAutoActivator.TryInitiateLesson((from def in DefDatabase<ConceptDef>.AllDefs
			orderby LessonAutoActivator.GetDesire(def) descending
			select def).First<ConceptDef>());
		}

		// Note: this type is marked as 'beforefieldinit'.
		static LessonAutoActivator()
		{
		}

		[CompilerGenerated]
		private static float <DebugString>m__0(ConceptDef co)
		{
			return LessonAutoActivator.GetDesire(co);
		}

		[CompilerGenerated]
		private static float <DebugForceInitiateBestLessonNow>m__1(ConceptDef def)
		{
			return LessonAutoActivator.GetDesire(def);
		}
	}
}
