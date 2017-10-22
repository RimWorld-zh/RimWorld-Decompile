using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	public class Scenario : IExposable, WorkshopUploadable
	{
		public string name;

		public string summary;

		public string description;

		internal ScenPart_PlayerFaction playerFaction;

		internal List<ScenPart> parts = new List<ScenPart>();

		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		private ScenarioCategory categoryInt = ScenarioCategory.Undefined;

		public string fileName;

		private WorkshopItemHook workshopHookInt;

		private string tempUploadDir;

		public bool enabled = true;

		public const int NameMaxLength = 55;

		public const int SummaryMaxLength = 300;

		public const int DescriptionMaxLength = 1000;

		public FileInfo File
		{
			get
			{
				return new FileInfo(GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		public IEnumerable<ScenPart> AllParts
		{
			get
			{
				yield return (ScenPart)this.playerFaction;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public ScenarioCategory Category
		{
			get
			{
				if (this.categoryInt == ScenarioCategory.Undefined)
				{
					Log.Error("Category is Undefined on Scenario " + this);
				}
				return this.categoryInt;
			}
			set
			{
				this.categoryInt = value;
			}
		}

		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", (string)null, false);
			Scribe_Values.Look<string>(ref this.summary, "summary", (string)null, false);
			Scribe_Values.Look<string>(ref this.description, "description", (string)null, false);
			Scribe_Values.Look<PublishedFileId_t>(ref this.publishedFileIdInt, "publishedFileId", PublishedFileId_t.Invalid, false);
			Scribe_Deep.Look<ScenPart_PlayerFaction>(ref this.playerFaction, "playerFaction", new object[0]);
			Scribe_Collections.Look<ScenPart>(ref this.parts, "parts", LookMode.Deep, new object[0]);
		}

		public IEnumerable<string> ConfigErrors()
		{
			if (this.name.NullOrEmpty())
			{
				yield return "no title";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.parts.NullOrEmpty())
			{
				yield return "no parts";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.playerFaction == null)
			{
				yield return "no playerFaction";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			foreach (ScenPart allPart in this.AllParts)
			{
				using (IEnumerator<string> enumerator2 = allPart.ConfigErrors().GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						string e = enumerator2.Current;
						yield return e;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_01ce:
			/*Error near IL_01cf: Unexpected return in MoveNext()*/;
		}

		public string GetFullInformationText()
		{
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.description);
				stringBuilder.AppendLine();
				foreach (ScenPart allPart in this.AllParts)
				{
					allPart.summarized = false;
				}
				foreach (ScenPart item in from p in this.AllParts
				orderby p.def.summaryPriority descending, p.def.defName
				where p.visible
				select p)
				{
					string text = item.Summary(this);
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text);
					}
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
			catch (Exception ex)
			{
				Log.ErrorOnce("Exception in Scenario.GetFullInformationText():\n" + ex.ToString(), 10395878);
				return "Cannot read data.";
			}
		}

		public string GetSummary()
		{
			return this.summary;
		}

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

		public void PreConfigure()
		{
			foreach (ScenPart allPart in this.AllParts)
			{
				allPart.PreConfigure();
			}
		}

		public Page GetFirstConfigPage()
		{
			List<Page> list = new List<Page>();
			list.Add(new Page_SelectStoryteller());
			list.Add(new Page_CreateWorldParams());
			list.Add(new Page_SelectLandingSite());
			foreach (Page item in this.parts.SelectMany((Func<ScenPart, IEnumerable<Page>>)((ScenPart p) => p.GetConfigPages())))
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
				page2.nextAct = (Action)delegate
				{
					PageUtility.InitGameStart();
				};
			}
			return page;
		}

		public bool AllowPlayerStartingPawn(Pawn pawn)
		{
			foreach (ScenPart allPart in this.AllParts)
			{
				if (!allPart.AllowPlayerStartingPawn(pawn))
				{
					return false;
				}
			}
			return true;
		}

		public void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context)
		{
			foreach (ScenPart allPart in this.AllParts)
			{
				allPart.Notify_PawnGenerated(pawn, context);
			}
		}

		public void Notify_PawnDied(Corpse corpse)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnDied(corpse);
			}
		}

		public void PostWorldGenerate()
		{
			foreach (ScenPart allPart in this.AllParts)
			{
				allPart.PostWorldGenerate();
			}
		}

		public void PreMapGenerate()
		{
			foreach (ScenPart allPart in this.AllParts)
			{
				allPart.PreMapGenerate();
			}
		}

		public void GenerateIntoMap(Map map)
		{
			foreach (ScenPart allPart in this.AllParts)
			{
				allPart.GenerateIntoMap(map);
			}
		}

		public void PostMapGenerate(Map map)
		{
			foreach (ScenPart allPart in this.AllParts)
			{
				allPart.PostMapGenerate(map);
			}
		}

		public void PostGameStart()
		{
			foreach (ScenPart allPart in this.AllParts)
			{
				allPart.PostGameStart();
			}
		}

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

		public void TickScenario()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Tick();
			}
		}

		public void RemovePart(ScenPart part)
		{
			if (!this.parts.Contains(part))
			{
				Log.Error("Cannot remove: " + part);
			}
			this.parts.Remove(part);
		}

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
				switch (dir)
				{
				case ReorderDirection.Up:
				{
					result = ((byte)((num != 0) ? ((num <= 0 || this.parts[num - 1].def.PlayerAddRemovable) ? 1 : 0) : 0) != 0);
					break;
				}
				case ReorderDirection.Down:
				{
					result = (num != this.parts.Count - 1);
					break;
				}
				default:
				{
					throw new NotImplementedException();
				}
				}
			}
			return result;
		}

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

		public bool CanToUploadToWorkshop()
		{
			return (byte)((this.Category != ScenarioCategory.FromDef) ? (this.TryUploadReport().Accepted ? ((!this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser) ? 1 : 0) : 0) : 0) != 0;
		}

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
			string str = Path.Combine(this.tempUploadDir, this.name);
			str += ".rsc";
			GameDataSaveLoader.SaveScenario(this, str);
		}

		public AcceptanceReport TryUploadReport()
		{
			return (this.name != null && this.name.Length >= 3 && this.summary != null && this.summary.Length >= 3 && this.description != null && this.description.Length >= 3) ? AcceptanceReport.WasAccepted : "TextFieldsMustBeFilled".Translate();
		}

		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			this.publishedFileIdInt = newPfid;
			if (this.Category == ScenarioCategory.CustomLocal && !this.fileName.NullOrEmpty())
			{
				GameDataSaveLoader.SaveScenario(this, GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		public string GetWorkshopName()
		{
			return this.name;
		}

		public string GetWorkshopDescription()
		{
			return this.GetFullInformationText();
		}

		public string GetWorkshopPreviewImagePath()
		{
			return GenFilePaths.ScenarioPreviewImagePath;
		}

		public IList<string> GetWorkshopTags()
		{
			List<string> list = new List<string>();
			list.Add("Scenario");
			return list;
		}

		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return new DirectoryInfo(this.tempUploadDir);
		}

		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		public override string ToString()
		{
			return this.name.NullOrEmpty() ? "LabellessScenario" : this.name;
		}

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
	}
}
