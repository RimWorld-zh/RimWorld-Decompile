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
	// Token: 0x02000654 RID: 1620
	public class Scenario : IExposable, WorkshopUploadable
	{
		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x0600219C RID: 8604 RVA: 0x0011CD14 File Offset: 0x0011B114
		public FileInfo File
		{
			get
			{
				return new FileInfo(GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x0600219D RID: 8605 RVA: 0x0011CD3C File Offset: 0x0011B13C
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
		// (get) Token: 0x0600219E RID: 8606 RVA: 0x0011CD68 File Offset: 0x0011B168
		// (set) Token: 0x0600219F RID: 8607 RVA: 0x0011CD9F File Offset: 0x0011B19F
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

		// Token: 0x060021A0 RID: 8608 RVA: 0x0011CDAC File Offset: 0x0011B1AC
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<string>(ref this.summary, "summary", null, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
			Scribe_Values.Look<PublishedFileId_t>(ref this.publishedFileIdInt, "publishedFileId", PublishedFileId_t.Invalid, false);
			Scribe_Deep.Look<ScenPart_PlayerFaction>(ref this.playerFaction, "playerFaction", new object[0]);
			Scribe_Collections.Look<ScenPart>(ref this.parts, "parts", LookMode.Deep, new object[0]);
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x0011CE34 File Offset: 0x0011B234
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

		// Token: 0x060021A2 RID: 8610 RVA: 0x0011CE60 File Offset: 0x0011B260
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

		// Token: 0x060021A3 RID: 8611 RVA: 0x0011D014 File Offset: 0x0011B414
		public string GetSummary()
		{
			return this.summary;
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x0011D030 File Offset: 0x0011B430
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

		// Token: 0x060021A5 RID: 8613 RVA: 0x0011D0C0 File Offset: 0x0011B4C0
		public void PreConfigure()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreConfigure();
			}
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x0011D11C File Offset: 0x0011B51C
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

		// Token: 0x060021A7 RID: 8615 RVA: 0x0011D220 File Offset: 0x0011B620
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

		// Token: 0x060021A8 RID: 8616 RVA: 0x0011D294 File Offset: 0x0011B694
		public void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_NewPawnGenerating(pawn, context);
			}
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x0011D2F4 File Offset: 0x0011B6F4
		public void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_PawnGenerated(pawn, context, redressed);
			}
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x0011D354 File Offset: 0x0011B754
		public void Notify_PawnDied(Corpse corpse)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnDied(corpse);
			}
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x0011D394 File Offset: 0x0011B794
		public void PostWorldGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostWorldGenerate();
			}
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x0011D3F0 File Offset: 0x0011B7F0
		public void PreMapGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreMapGenerate();
			}
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x0011D44C File Offset: 0x0011B84C
		public void GenerateIntoMap(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.GenerateIntoMap(map);
			}
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x0011D4AC File Offset: 0x0011B8AC
		public void PostMapGenerate(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostMapGenerate(map);
			}
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x0011D50C File Offset: 0x0011B90C
		public void PostGameStart()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostGameStart();
			}
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x0011D568 File Offset: 0x0011B968
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

		// Token: 0x060021B1 RID: 8625 RVA: 0x0011D5C4 File Offset: 0x0011B9C4
		public void TickScenario()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Tick();
			}
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x0011D601 File Offset: 0x0011BA01
		public void RemovePart(ScenPart part)
		{
			if (!this.parts.Contains(part))
			{
				Log.Error("Cannot remove: " + part, false);
			}
			this.parts.Remove(part);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x0011D634 File Offset: 0x0011BA34
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

		// Token: 0x060021B4 RID: 8628 RVA: 0x0011D6D4 File Offset: 0x0011BAD4
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

		// Token: 0x060021B5 RID: 8629 RVA: 0x0011D728 File Offset: 0x0011BB28
		public bool CanToUploadToWorkshop()
		{
			return this.Category != ScenarioCategory.FromDef && this.TryUploadReport().Accepted && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x0011D784 File Offset: 0x0011BB84
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

		// Token: 0x060021B7 RID: 8631 RVA: 0x0011D814 File Offset: 0x0011BC14
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

		// Token: 0x060021B8 RID: 8632 RVA: 0x0011D898 File Offset: 0x0011BC98
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x0011D8B3 File Offset: 0x0011BCB3
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			this.publishedFileIdInt = newPfid;
			if (this.Category == ScenarioCategory.CustomLocal && !this.fileName.NullOrEmpty())
			{
				GameDataSaveLoader.SaveScenario(this, GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x0011D8EC File Offset: 0x0011BCEC
		public string GetWorkshopName()
		{
			return this.name;
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x0011D908 File Offset: 0x0011BD08
		public string GetWorkshopDescription()
		{
			return this.GetFullInformationText();
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x0011D924 File Offset: 0x0011BD24
		public string GetWorkshopPreviewImagePath()
		{
			return GenFilePaths.ScenarioPreviewImagePath;
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x0011D940 File Offset: 0x0011BD40
		public IList<string> GetWorkshopTags()
		{
			return new List<string>
			{
				"Scenario"
			};
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x0011D968 File Offset: 0x0011BD68
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return new DirectoryInfo(this.tempUploadDir);
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x0011D988 File Offset: 0x0011BD88
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x0011D9BC File Offset: 0x0011BDBC
		public override string ToString()
		{
			return this.name.NullOrEmpty() ? "LabellessScenario" : this.name;
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x0011D9F4 File Offset: 0x0011BDF4
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

		// Token: 0x04001317 RID: 4887
		[MustTranslate]
		public string name;

		// Token: 0x04001318 RID: 4888
		[MustTranslate]
		public string summary;

		// Token: 0x04001319 RID: 4889
		[MustTranslate]
		public string description;

		// Token: 0x0400131A RID: 4890
		internal ScenPart_PlayerFaction playerFaction;

		// Token: 0x0400131B RID: 4891
		internal List<ScenPart> parts = new List<ScenPart>();

		// Token: 0x0400131C RID: 4892
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x0400131D RID: 4893
		private ScenarioCategory categoryInt = ScenarioCategory.Undefined;

		// Token: 0x0400131E RID: 4894
		[NoTranslate]
		public string fileName;

		// Token: 0x0400131F RID: 4895
		private WorkshopItemHook workshopHookInt;

		// Token: 0x04001320 RID: 4896
		[NoTranslate]
		private string tempUploadDir;

		// Token: 0x04001321 RID: 4897
		public bool enabled = true;

		// Token: 0x04001322 RID: 4898
		public const int NameMaxLength = 55;

		// Token: 0x04001323 RID: 4899
		public const int SummaryMaxLength = 300;

		// Token: 0x04001324 RID: 4900
		public const int DescriptionMaxLength = 1000;
	}
}
