using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D1 RID: 2257
	public static class PlayerKnowledgeDatabase
	{
		// Token: 0x04001BC5 RID: 7109
		private static PlayerKnowledgeDatabase.ConceptKnowledge data;

		// Token: 0x060033AD RID: 13229 RVA: 0x001BA1D3 File Offset: 0x001B85D3
		static PlayerKnowledgeDatabase()
		{
			PlayerKnowledgeDatabase.ReloadAndRebind();
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x001BA1DC File Offset: 0x001B85DC
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

		// Token: 0x060033AF RID: 13231 RVA: 0x001BA284 File Offset: 0x001B8684
		public static void ResetPersistent()
		{
			FileInfo fileInfo = new FileInfo(GenFilePaths.ConceptKnowledgeFilePath);
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
			PlayerKnowledgeDatabase.data = new PlayerKnowledgeDatabase.ConceptKnowledge();
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x001BA2B8 File Offset: 0x001B86B8
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

		// Token: 0x060033B1 RID: 13233 RVA: 0x001BA34C File Offset: 0x001B874C
		public static float GetKnowledge(ConceptDef def)
		{
			return PlayerKnowledgeDatabase.data.knowledge[def];
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x001BA374 File Offset: 0x001B8774
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

		// Token: 0x060033B3 RID: 13235 RVA: 0x001BA3C8 File Offset: 0x001B87C8
		public static bool IsComplete(ConceptDef conc)
		{
			return PlayerKnowledgeDatabase.data.knowledge[conc] > 0.999f;
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x001BA3F4 File Offset: 0x001B87F4
		private static void NewlyLearned(ConceptDef conc)
		{
			TutorSystem.Notify_Event("ConceptLearned-" + conc.defName);
			if (Find.Tutor != null)
			{
				Find.Tutor.learningReadout.Notify_ConceptNewlyLearned(conc);
			}
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x001BA42C File Offset: 0x001B882C
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

		// Token: 0x020008D2 RID: 2258
		private class ConceptKnowledge
		{
			// Token: 0x04001BC6 RID: 7110
			public Dictionary<ConceptDef, float> knowledge = new Dictionary<ConceptDef, float>();

			// Token: 0x060033B6 RID: 13238 RVA: 0x001BA518 File Offset: 0x001B8918
			public ConceptKnowledge()
			{
				foreach (ConceptDef key in DefDatabase<ConceptDef>.AllDefs)
				{
					this.knowledge.Add(key, 0f);
				}
			}
		}
	}
}
