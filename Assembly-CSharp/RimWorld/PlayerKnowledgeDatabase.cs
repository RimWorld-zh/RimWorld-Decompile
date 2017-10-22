using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class PlayerKnowledgeDatabase
	{
		private class ConceptKnowledge
		{
			public Dictionary<ConceptDef, float> knowledge = new Dictionary<ConceptDef, float>();

			public ConceptKnowledge()
			{
				foreach (ConceptDef allDef in DefDatabase<ConceptDef>.AllDefs)
				{
					this.knowledge.Add(allDef, 0f);
				}
			}
		}

		private static ConceptKnowledge data;

		static PlayerKnowledgeDatabase()
		{
			PlayerKnowledgeDatabase.ReloadAndRebind();
		}

		public static void ReloadAndRebind()
		{
			PlayerKnowledgeDatabase.data = DirectXmlLoader.ItemFromXmlFile<ConceptKnowledge>(GenFilePaths.ConceptKnowledgeFilePath, true);
			foreach (ConceptDef allDef in DefDatabase<ConceptDef>.AllDefs)
			{
				if (!PlayerKnowledgeDatabase.data.knowledge.ContainsKey(allDef))
				{
					Log.Warning("Knowledge data was missing key " + allDef + ". Adding it...");
					PlayerKnowledgeDatabase.data.knowledge.Add(allDef, 0f);
				}
			}
		}

		public static void ResetPersistent()
		{
			FileInfo fileInfo = new FileInfo(GenFilePaths.ConceptKnowledgeFilePath);
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
			PlayerKnowledgeDatabase.data = new ConceptKnowledge();
		}

		public static void Save()
		{
			try
			{
				XDocument xDocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(PlayerKnowledgeDatabase.data, typeof(ConceptKnowledge));
				xDocument.Add(content);
				xDocument.Save(GenFilePaths.ConceptKnowledgeFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(GenFilePaths.ConceptKnowledgeFilePath, ex.ToString()));
				Log.Error("Exception saving knowledge database: " + ex);
			}
		}

		public static float GetKnowledge(ConceptDef def)
		{
			return PlayerKnowledgeDatabase.data.knowledge[def];
		}

		public static void SetKnowledge(ConceptDef def, float value)
		{
			float num = PlayerKnowledgeDatabase.data.knowledge[def];
			float num2 = Mathf.Clamp01(value);
			PlayerKnowledgeDatabase.data.knowledge[def] = num2;
			if (num < 0.99900001287460327 && num2 >= 0.99900001287460327)
			{
				PlayerKnowledgeDatabase.NewlyLearned(def);
			}
		}

		public static bool IsComplete(ConceptDef conc)
		{
			return PlayerKnowledgeDatabase.data.knowledge[conc] > 0.99900001287460327;
		}

		private static void NewlyLearned(ConceptDef conc)
		{
			TutorSystem.Notify_Event("ConceptLearned-" + conc.defName);
			if (Find.Tutor != null)
			{
				Find.Tutor.learningReadout.Notify_ConceptNewlyLearned(conc);
			}
		}

		public static void KnowledgeDemonstrated(ConceptDef conc, KnowledgeAmount know)
		{
			float num;
			switch (know)
			{
			case KnowledgeAmount.FrameDisplayed:
			{
				num = 0.002f;
				break;
			}
			case KnowledgeAmount.FrameInteraction:
			{
				num = 0.008f;
				break;
			}
			case KnowledgeAmount.TinyInteraction:
			{
				num = 0.03f;
				break;
			}
			case KnowledgeAmount.SmallInteraction:
			{
				num = 0.1f;
				break;
			}
			case KnowledgeAmount.SpecificInteraction:
			{
				num = 0.4f;
				break;
			}
			case KnowledgeAmount.Total:
			{
				num = 1f;
				break;
			}
			case KnowledgeAmount.NoteClosed:
			{
				num = 0.5f;
				break;
			}
			case KnowledgeAmount.NoteTaught:
			{
				num = 1f;
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
			}
			PlayerKnowledgeDatabase.SetKnowledge(conc, PlayerKnowledgeDatabase.GetKnowledge(conc) + num);
			LessonAutoActivator.Notify_KnowledgeDemonstrated(conc);
			if (Find.ActiveLesson != null)
			{
				Find.ActiveLesson.Notify_KnowledgeDemonstrated(conc);
			}
		}
	}
}
