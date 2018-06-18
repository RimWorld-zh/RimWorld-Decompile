using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D3 RID: 2259
	public static class PlayerKnowledgeDatabase
	{
		// Token: 0x060033B0 RID: 13232 RVA: 0x001B9BD7 File Offset: 0x001B7FD7
		static PlayerKnowledgeDatabase()
		{
			PlayerKnowledgeDatabase.ReloadAndRebind();
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x001B9BE0 File Offset: 0x001B7FE0
		public static void ReloadAndRebind()
		{
			PlayerKnowledgeDatabase.data = DirectXmlLoader.ItemFromXmlFile<PlayerKnowledgeDatabase.ConceptKnowledge>(GenFilePaths.ConceptKnowledgeFilePath, true);
			foreach (ConceptDef conceptDef in DefDatabase<ConceptDef>.AllDefs)
			{
				if (!PlayerKnowledgeDatabase.data.knowledge.ContainsKey(conceptDef))
				{
					Log.Warning("Knowledge data was missing key " + conceptDef + ". Adding it...", false);
					PlayerKnowledgeDatabase.data.knowledge.Add(conceptDef, 0f);
				}
			}
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x001B9C88 File Offset: 0x001B8088
		public static void ResetPersistent()
		{
			FileInfo fileInfo = new FileInfo(GenFilePaths.ConceptKnowledgeFilePath);
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
			PlayerKnowledgeDatabase.data = new PlayerKnowledgeDatabase.ConceptKnowledge();
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x001B9CBC File Offset: 0x001B80BC
		public static void Save()
		{
			try
			{
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(PlayerKnowledgeDatabase.data, typeof(PlayerKnowledgeDatabase.ConceptKnowledge));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.ConceptKnowledgeFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(new object[]
				{
					GenFilePaths.ConceptKnowledgeFilePath,
					ex.ToString()
				}));
				Log.Error("Exception saving knowledge database: " + ex, false);
			}
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x001B9D50 File Offset: 0x001B8150
		public static float GetKnowledge(ConceptDef def)
		{
			return PlayerKnowledgeDatabase.data.knowledge[def];
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x001B9D78 File Offset: 0x001B8178
		public static void SetKnowledge(ConceptDef def, float value)
		{
			float num = PlayerKnowledgeDatabase.data.knowledge[def];
			float num2 = Mathf.Clamp01(value);
			PlayerKnowledgeDatabase.data.knowledge[def] = num2;
			if (num < 0.999f && num2 >= 0.999f)
			{
				PlayerKnowledgeDatabase.NewlyLearned(def);
			}
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x001B9DCC File Offset: 0x001B81CC
		public static bool IsComplete(ConceptDef conc)
		{
			return PlayerKnowledgeDatabase.data.knowledge[conc] > 0.999f;
		}

		// Token: 0x060033B7 RID: 13239 RVA: 0x001B9DF8 File Offset: 0x001B81F8
		private static void NewlyLearned(ConceptDef conc)
		{
			TutorSystem.Notify_Event("ConceptLearned-" + conc.defName);
			if (Find.Tutor != null)
			{
				Find.Tutor.learningReadout.Notify_ConceptNewlyLearned(conc);
			}
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x001B9E30 File Offset: 0x001B8230
		public static void KnowledgeDemonstrated(ConceptDef conc, KnowledgeAmount know)
		{
			float num;
			switch (know)
			{
			case KnowledgeAmount.FrameDisplayed:
				num = ((Event.current.type != EventType.Repaint) ? 0f : 0.004f);
				break;
			case KnowledgeAmount.FrameInteraction:
				num = 0.008f;
				break;
			case KnowledgeAmount.TinyInteraction:
				num = 0.03f;
				break;
			case KnowledgeAmount.SmallInteraction:
				num = 0.1f;
				break;
			case KnowledgeAmount.SpecificInteraction:
				num = 0.4f;
				break;
			case KnowledgeAmount.Total:
				num = 1f;
				break;
			case KnowledgeAmount.NoteClosed:
				num = 0.5f;
				break;
			case KnowledgeAmount.NoteTaught:
				num = 1f;
				break;
			default:
				throw new NotImplementedException();
			}
			if (num > 0f)
			{
				PlayerKnowledgeDatabase.SetKnowledge(conc, PlayerKnowledgeDatabase.GetKnowledge(conc) + num);
				LessonAutoActivator.Notify_KnowledgeDemonstrated(conc);
				if (Find.ActiveLesson != null)
				{
					Find.ActiveLesson.Notify_KnowledgeDemonstrated(conc);
				}
			}
		}

		// Token: 0x04001BC1 RID: 7105
		private static PlayerKnowledgeDatabase.ConceptKnowledge data;

		// Token: 0x020008D4 RID: 2260
		private class ConceptKnowledge
		{
			// Token: 0x060033B9 RID: 13241 RVA: 0x001B9F1C File Offset: 0x001B831C
			public ConceptKnowledge()
			{
				foreach (ConceptDef key in DefDatabase<ConceptDef>.AllDefs)
				{
					this.knowledge.Add(key, 0f);
				}
			}

			// Token: 0x04001BC2 RID: 7106
			public Dictionary<ConceptDef, float> knowledge = new Dictionary<ConceptDef, float>();
		}
	}
}
