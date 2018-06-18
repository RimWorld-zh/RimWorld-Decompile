using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000CC8 RID: 3272
	public class ModMetaData : WorkshopUploadable
	{
		// Token: 0x06004830 RID: 18480 RVA: 0x0025F2DC File Offset: 0x0025D6DC
		public ModMetaData(string localAbsPath)
		{
			this.rootDirInt = new DirectoryInfo(localAbsPath);
			this.source = ContentSource.LocalFolder;
			this.Init();
		}

		// Token: 0x06004831 RID: 18481 RVA: 0x0025F330 File Offset: 0x0025D730
		public ModMetaData(WorkshopItem workshopItem)
		{
			this.rootDirInt = workshopItem.Directory;
			this.source = ContentSource.SteamWorkshop;
			this.Init();
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06004832 RID: 18482 RVA: 0x0025F384 File Offset: 0x0025D784
		public string Identifier
		{
			get
			{
				return this.RootDir.Name;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06004833 RID: 18483 RVA: 0x0025F3A4 File Offset: 0x0025D7A4
		public DirectoryInfo RootDir
		{
			get
			{
				return this.rootDirInt;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06004834 RID: 18484 RVA: 0x0025F3C0 File Offset: 0x0025D7C0
		public bool IsCoreMod
		{
			get
			{
				return this.Identifier == ModContentPack.CoreModIdentifier;
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06004835 RID: 18485 RVA: 0x0025F3E8 File Offset: 0x0025D7E8
		// (set) Token: 0x06004836 RID: 18486 RVA: 0x0025F403 File Offset: 0x0025D803
		public bool Active
		{
			get
			{
				return ModsConfig.IsActive(this);
			}
			set
			{
				ModsConfig.SetActive(this, value);
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06004837 RID: 18487 RVA: 0x0025F410 File Offset: 0x0025D810
		public bool VersionCompatible
		{
			get
			{
				return this.IsCoreMod || (VersionControl.IsWellFormattedVersionString(this.TargetVersion) && VersionControl.MinorFromVersionString(this.TargetVersion) == VersionControl.CurrentMinor && VersionControl.MajorFromVersionString(this.TargetVersion) == VersionControl.CurrentMajor);
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06004838 RID: 18488 RVA: 0x0025F478 File Offset: 0x0025D878
		// (set) Token: 0x06004839 RID: 18489 RVA: 0x0025F498 File Offset: 0x0025D898
		public string Name
		{
			get
			{
				return this.meta.name;
			}
			set
			{
				this.meta.name = value;
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x0600483A RID: 18490 RVA: 0x0025F4A8 File Offset: 0x0025D8A8
		public string Author
		{
			get
			{
				return this.meta.author;
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x0600483B RID: 18491 RVA: 0x0025F4C8 File Offset: 0x0025D8C8
		public string Url
		{
			get
			{
				return this.meta.url;
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x0600483C RID: 18492 RVA: 0x0025F4E8 File Offset: 0x0025D8E8
		public string TargetVersion
		{
			get
			{
				return this.meta.targetVersion;
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x0600483D RID: 18493 RVA: 0x0025F508 File Offset: 0x0025D908
		public string Description
		{
			get
			{
				return this.meta.description;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x0600483E RID: 18494 RVA: 0x0025F528 File Offset: 0x0025D928
		public string PreviewImagePath
		{
			get
			{
				return string.Concat(new object[]
				{
					this.rootDirInt.FullName,
					Path.DirectorySeparatorChar,
					"About",
					Path.DirectorySeparatorChar,
					"Preview.png"
				});
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x0600483F RID: 18495 RVA: 0x0025F580 File Offset: 0x0025D980
		public ContentSource Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x0025F59C File Offset: 0x0025D99C
		private void Init()
		{
			this.meta = DirectXmlLoader.ItemFromXmlFile<ModMetaData.ModMetaDataInternal>(string.Concat(new object[]
			{
				this.RootDir.FullName,
				Path.DirectorySeparatorChar,
				"About",
				Path.DirectorySeparatorChar,
				"About.xml"
			}), true);
			if (this.meta.name.NullOrEmpty())
			{
				if (this.OnSteamWorkshop)
				{
					this.meta.name = "Workshop mod " + this.Identifier;
				}
				else
				{
					this.meta.name = this.Identifier;
				}
			}
			if (!this.IsCoreMod && !this.OnSteamWorkshop && !VersionControl.IsWellFormattedVersionString(this.meta.targetVersion))
			{
				Log.ErrorOnce(string.Concat(new string[]
				{
					"Mod ",
					this.meta.name,
					" has incorrectly formatted target version '",
					this.meta.targetVersion,
					"'. For the current version, write: <targetVersion>",
					VersionControl.CurrentVersionString,
					"</targetVersion>"
				}), this.Identifier.GetHashCode(), false);
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				string url = GenFilePaths.SafeURIForUnityWWWFromPath(this.PreviewImagePath);
				using (WWW www = new WWW(url))
				{
					www.threadPriority = UnityEngine.ThreadPriority.High;
					while (!www.isDone)
					{
						Thread.Sleep(1);
					}
					if (www.error == null)
					{
						this.previewImage = www.textureNonReadable;
					}
				}
			});
			string publishedFileIdPath = this.PublishedFileIdPath;
			if (File.Exists(this.PublishedFileIdPath))
			{
				string s = File.ReadAllText(publishedFileIdPath);
				this.publishedFileIdInt = new PublishedFileId_t(ulong.Parse(s));
			}
		}

		// Token: 0x06004841 RID: 18497 RVA: 0x0025F71F File Offset: 0x0025DB1F
		internal void DeleteContent()
		{
			this.rootDirInt.Delete(true);
			ModLister.RebuildModList();
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06004842 RID: 18498 RVA: 0x0025F734 File Offset: 0x0025DB34
		public bool OnSteamWorkshop
		{
			get
			{
				return this.source == ContentSource.SteamWorkshop;
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06004843 RID: 18499 RVA: 0x0025F754 File Offset: 0x0025DB54
		private string PublishedFileIdPath
		{
			get
			{
				return string.Concat(new object[]
				{
					this.rootDirInt.FullName,
					Path.DirectorySeparatorChar,
					"About",
					Path.DirectorySeparatorChar,
					"PublishedFileId.txt"
				});
			}
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x0025F7AC File Offset: 0x0025DBAC
		public void PrepareForWorkshopUpload()
		{
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x0025F7B0 File Offset: 0x0025DBB0
		public bool CanToUploadToWorkshop()
		{
			return !this.IsCoreMod && this.Source == ContentSource.LocalFolder && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x0025F804 File Offset: 0x0025DC04
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x0025F81F File Offset: 0x0025DC1F
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			if (!(this.publishedFileIdInt == newPfid))
			{
				this.publishedFileIdInt = newPfid;
				File.WriteAllText(this.PublishedFileIdPath, newPfid.ToString());
			}
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x0025F858 File Offset: 0x0025DC58
		public string GetWorkshopName()
		{
			return this.Name;
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x0025F874 File Offset: 0x0025DC74
		public string GetWorkshopDescription()
		{
			return this.Description;
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x0025F890 File Offset: 0x0025DC90
		public string GetWorkshopPreviewImagePath()
		{
			return this.PreviewImagePath;
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x0025F8AC File Offset: 0x0025DCAC
		public IList<string> GetWorkshopTags()
		{
			return new List<string>
			{
				"Mod"
			};
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x0025F8D4 File Offset: 0x0025DCD4
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return this.RootDir;
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x0025F8F0 File Offset: 0x0025DCF0
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x0025F924 File Offset: 0x0025DD24
		public override int GetHashCode()
		{
			return this.Identifier.GetHashCode();
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x0025F944 File Offset: 0x0025DD44
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.Identifier,
				"|",
				this.Name,
				"]"
			});
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x0025F990 File Offset: 0x0025DD90
		public string ToStringLong()
		{
			return this.Identifier + "(" + this.RootDir.ToString() + ")";
		}

		// Token: 0x040030DE RID: 12510
		private DirectoryInfo rootDirInt;

		// Token: 0x040030DF RID: 12511
		private ContentSource source;

		// Token: 0x040030E0 RID: 12512
		public Texture2D previewImage = null;

		// Token: 0x040030E1 RID: 12513
		public bool enabled = true;

		// Token: 0x040030E2 RID: 12514
		private ModMetaData.ModMetaDataInternal meta = new ModMetaData.ModMetaDataInternal();

		// Token: 0x040030E3 RID: 12515
		private WorkshopItemHook workshopHookInt;

		// Token: 0x040030E4 RID: 12516
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x040030E5 RID: 12517
		private const string AboutFolderName = "About";

		// Token: 0x02000CC9 RID: 3273
		private class ModMetaDataInternal
		{
			// Token: 0x040030E6 RID: 12518
			public string name = "";

			// Token: 0x040030E7 RID: 12519
			public string author = "Anonymous";

			// Token: 0x040030E8 RID: 12520
			public string url = "";

			// Token: 0x040030E9 RID: 12521
			public string targetVersion = "Unknown";

			// Token: 0x040030EA RID: 12522
			public string description = "No description provided.";
		}
	}
}
