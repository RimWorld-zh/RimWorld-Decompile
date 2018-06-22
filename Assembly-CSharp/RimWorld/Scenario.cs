using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Steamworks;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02000650 RID: 1616
	public class Scenario : IExposable, WorkshopUploadable
	{
		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x0011CE14 File Offset: 0x0011B214
		public FileInfo File
		{
			get
			{
				return new FileInfo(GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06002195 RID: 8597 RVA: 0x0011CE3C File Offset: 0x0011B23C
		public IEnumerable<ScenPart> AllParts
		{
			get
			{
				yield return this.playerFaction;
				for (int i = 0; i < this.parts.Count; i++)
				{
					yield return this.parts[i];
				}
				yield break;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06002196 RID: 8598 RVA: 0x0011CE68 File Offset: 0x0011B268
		// (set) Token: 0x06002197 RID: 8599 RVA: 0x0011CE9F File Offset: 0x0011B29F
		public ScenarioCategory Category
		{
			get
			{
				if (this.categoryInt == ScenarioCategory.Undefined)
				{
					Log.Error("Category is Undefined on Scenario " + this, false);
				}
				return this.categoryInt;
			}
			set
			{
				this.categoryInt = value;
			}
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x0011CEAC File Offset: 0x0011B2AC
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<string>(ref this.summary, "summary", null, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
			Scribe_Values.Look<PublishedFileId_t>(ref this.publishedFileIdInt, "publishedFileId", PublishedFileId_t.Invalid, false);
			Scribe_Deep.Look<ScenPart_PlayerFaction>(ref this.playerFaction, "playerFaction", new object[0]);
			Scribe_Collections.Look<ScenPart>(ref this.parts, "parts", LookMode.Deep, new object[0]);
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x0011CF34 File Offset: 0x0011B334
		public IEnumerable<string> ConfigErrors()
		{
			if (this.name.NullOrEmpty())
			{
				yield return "no title";
			}
			if (this.parts.NullOrEmpty<ScenPart>())
			{
				yield return "no parts";
			}
			if (this.playerFaction == null)
			{
				yield return "no playerFaction";
			}
			foreach (ScenPart part in this.AllParts)
			{
				foreach (string e in part.ConfigErrors())
				{
					yield return e;
				}
			}
			yield break;
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x0011CF60 File Offset: 0x0011B360
		public string GetFullInformationText()
		{
			string result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.description);
				stringBuilder.AppendLine();
				foreach (ScenPart scenPart in this.AllParts)
				{
					scenPart.summarized = false;
				}
				foreach (ScenPart scenPart2 in from p in this.AllParts
				orderby p.def.summaryPriority descending, p.def.defName
				where p.visible
				select p)
				{
					string text = scenPart2.Summary(this);
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text);
					}
				}
				result = stringBuilder.ToString().TrimEndNewlines();
			}
			catch (Exception ex)
			{
				Log.ErrorOnce("Exception in Scenario.GetFullInformationText():\n" + ex.ToString(), 10395878, false);
				result = "Cannot read data.";
			}
			return result;
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x0011D114 File Offset: 0x0011B514
		public string GetSummary()
		{
			return this.summary;
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x0011D130 File Offset: 0x0011B530
		public Scenario CopyForEditing()
		{
			Scenario scenario = new Scenario();
			scenario.name = this.name;
			scenario.summary = this.summary;
			scenario.description = this.description;
			scenario.playerFaction = (ScenPart_PlayerFaction)this.playerFaction.CopyForEditing();
			scenario.parts.AddRange(from p in this.parts
			select p.CopyForEditing());
			scenario.categoryInt = ScenarioCategory.CustomLocal;
			return scenario;
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x0011D1C0 File Offset: 0x0011B5C0
		public void PreConfigure()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreConfigure();
			}
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x0011D21C File Offset: 0x0011B61C
		public Page GetFirstConfigPage()
		{
			List<Page> list = new List<Page>();
			list.Add(new Page_SelectStoryteller());
			list.Add(new Page_CreateWorldParams());
			list.Add(new Page_SelectLandingSite());
			foreach (Page item in this.parts.SelectMany((ScenPart p) => p.GetConfigPages()))
			{
				list.Add(item);
			}
			Page page = PageUtility.StitchedPages(list);
			if (page != null)
			{
				Page page2 = page;
				while (page2.next != null)
				{
					page2 = page2.next;
				}
				page2.nextAct = delegate()
				{
					PageUtility.InitGameStart();
				};
			}
			return page;
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x0011D320 File Offset: 0x0011B720
		public bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				if (!scenPart.AllowPlayerStartingPawn(pawn, tryingToRedress, req))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x0011D394 File Offset: 0x0011B794
		public void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_NewPawnGenerating(pawn, context);
			}
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x0011D3F4 File Offset: 0x0011B7F4
		public void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_PawnGenerated(pawn, context, redressed);
			}
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x0011D454 File Offset: 0x0011B854
		public void Notify_PawnDied(Corpse corpse)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnDied(corpse);
			}
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x0011D494 File Offset: 0x0011B894
		public void PostWorldGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostWorldGenerate();
			}
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x0011D4F0 File Offset: 0x0011B8F0
		public void PreMapGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreMapGenerate();
			}
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x0011D54C File Offset: 0x0011B94C
		public void GenerateIntoMap(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.GenerateIntoMap(map);
			}
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x0011D5AC File Offset: 0x0011B9AC
		public void PostMapGenerate(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostMapGenerate(map);
			}
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x0011D60C File Offset: 0x0011BA0C
		public void PostGameStart()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostGameStart();
			}
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x0011D668 File Offset: 0x0011BA68
		public float GetStatFactor(StatDef stat)
		{
			float num = 1f;
			for (int i = 0; i < this.parts.Count; i++)
			{
				ScenPart_StatFactor scenPart_StatFactor = this.parts[i] as ScenPart_StatFactor;
				if (scenPart_StatFactor != null)
				{
					num *= scenPart_StatFactor.GetStatFactor(stat);
				}
			}
			return num;
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x0011D6C4 File Offset: 0x0011BAC4
		public void TickScenario()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Tick();
			}
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x0011D701 File Offset: 0x0011BB01
		public void RemovePart(ScenPart part)
		{
			if (!this.parts.Contains(part))
			{
				Log.Error("Cannot remove: " + part, false);
			}
			this.parts.Remove(part);
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x0011D734 File Offset: 0x0011BB34
		public bool CanReorder(ScenPart part, ReorderDirection dir)
		{
			bool result;
			if (!part.def.PlayerAddRemovable)
			{
				result = false;
			}
			else
			{
				int num = this.parts.IndexOf(part);
				if (dir == ReorderDirection.Up)
				{
					result = (num != 0 && (num <= 0 || this.parts[num - 1].def.PlayerAddRemovable));
				}
				else
				{
					if (dir != ReorderDirection.Down)
					{
						throw new NotImplementedException();
					}
					result = (num != this.parts.Count - 1);
				}
			}
			return result;
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x0011D7D4 File Offset: 0x0011BBD4
		public void Reorder(ScenPart part, ReorderDirection dir)
		{
			int num = this.parts.IndexOf(part);
			this.parts.RemoveAt(num);
			if (dir == ReorderDirection.Up)
			{
				this.parts.Insert(num - 1, part);
			}
			if (dir == ReorderDirection.Down)
			{
				this.parts.Insert(num + 1, part);
			}
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x0011D828 File Offset: 0x0011BC28
		public bool CanToUploadToWorkshop()
		{
			return this.Category != ScenarioCategory.FromDef && this.TryUploadReport().Accepted && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x0011D884 File Offset: 0x0011BC84
		public void PrepareForWorkshopUpload()
		{
			string path = this.name + Rand.RangeInclusive(100, 999).ToString();
			this.tempUploadDir = Path.Combine(GenFilePaths.TempFolderPath, path);
			DirectoryInfo directoryInfo = new DirectoryInfo(this.tempUploadDir);
			if (directoryInfo.Exists)
			{
				directoryInfo.Delete();
			}
			directoryInfo.Create();
			string text = Path.Combine(this.tempUploadDir, this.name);
			text += ".rsc";
			GameDataSaveLoader.SaveScenario(this, text);
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x0011D914 File Offset: 0x0011BD14
		public AcceptanceReport TryUploadReport()
		{
			AcceptanceReport result;
			if (this.name == null || this.name.Length < 3 || this.summary == null || this.summary.Length < 3 || this.description == null || this.description.Length < 3)
			{
				result = "TextFieldsMustBeFilled".Translate();
			}
			else
			{
				result = AcceptanceReport.WasAccepted;
			}
			return result;
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x0011D998 File Offset: 0x0011BD98
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x0011D9B3 File Offset: 0x0011BDB3
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			this.publishedFileIdInt = newPfid;
			if (this.Category == ScenarioCategory.CustomLocal && !this.fileName.NullOrEmpty())
			{
				GameDataSaveLoader.SaveScenario(this, GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x0011D9EC File Offset: 0x0011BDEC
		public string GetWorkshopName()
		{
			return this.name;
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x0011DA08 File Offset: 0x0011BE08
		public string GetWorkshopDescription()
		{
			return this.GetFullInformationText();
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x0011DA24 File Offset: 0x0011BE24
		public string GetWorkshopPreviewImagePath()
		{
			return GenFilePaths.ScenarioPreviewImagePath;
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x0011DA40 File Offset: 0x0011BE40
		public IList<string> GetWorkshopTags()
		{
			return new List<string>
			{
				"Scenario"
			};
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x0011DA68 File Offset: 0x0011BE68
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return new DirectoryInfo(this.tempUploadDir);
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x0011DA88 File Offset: 0x0011BE88
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x0011DABC File Offset: 0x0011BEBC
		public override string ToString()
		{
			return this.name.NullOrEmpty() ? "LabellessScenario" : this.name;
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x0011DAF4 File Offset: 0x0011BEF4
		public override int GetHashCode()
		{
			int num = 6126121;
			if (this.name != null)
			{
				num ^= this.name.GetHashCode();
			}
			if (this.summary != null)
			{
				num ^= this.summary.GetHashCode();
			}
			if (this.description != null)
			{
				num ^= this.description.GetHashCode();
			}
			return num ^ this.publishedFileIdInt.GetHashCode();
		}

		// Token: 0x04001314 RID: 4884
		[MustTranslate]
		public string name;

		// Token: 0x04001315 RID: 4885
		[MustTranslate]
		public string summary;

		// Token: 0x04001316 RID: 4886
		[MustTranslate]
		public string description;

		// Token: 0x04001317 RID: 4887
		internal ScenPart_PlayerFaction playerFaction;

		// Token: 0x04001318 RID: 4888
		internal List<ScenPart> parts = new List<ScenPart>();

		// Token: 0x04001319 RID: 4889
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x0400131A RID: 4890
		private ScenarioCategory categoryInt = ScenarioCategory.Undefined;

		// Token: 0x0400131B RID: 4891
		[NoTranslate]
		public string fileName;

		// Token: 0x0400131C RID: 4892
		private WorkshopItemHook workshopHookInt;

		// Token: 0x0400131D RID: 4893
		[NoTranslate]
		private string tempUploadDir;

		// Token: 0x0400131E RID: 4894
		public bool enabled = true;

		// Token: 0x0400131F RID: 4895
		public const int NameMaxLength = 55;

		// Token: 0x04001320 RID: 4896
		public const int SummaryMaxLength = 300;

		// Token: 0x04001321 RID: 4897
		public const int DescriptionMaxLength = 1000;
	}
}
