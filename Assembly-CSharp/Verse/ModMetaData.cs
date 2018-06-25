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
	// Token: 0x02000CC7 RID: 3271
	public class ModMetaData : WorkshopUploadable
	{
		// Token: 0x040030E9 RID: 12521
		private DirectoryInfo rootDirInt;

		// Token: 0x040030EA RID: 12522
		private ContentSource source;

		// Token: 0x040030EB RID: 12523
		public Texture2D previewImage = null;

		// Token: 0x040030EC RID: 12524
		public bool enabled = true;

		// Token: 0x040030ED RID: 12525
		private ModMetaData.ModMetaDataInternal meta = new ModMetaData.ModMetaDataInternal();

		// Token: 0x040030EE RID: 12526
		private WorkshopItemHook workshopHookInt;

		// Token: 0x040030EF RID: 12527
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x040030F0 RID: 12528
		private const string AboutFolderName = "About";

		// Token: 0x06004844 RID: 18500 RVA: 0x002607D0 File Offset: 0x0025EBD0
		public ModMetaData(string localAbsPath)
		{
			this.rootDirInt = new DirectoryInfo(localAbsPath);
			this.source = ContentSource.LocalFolder;
			this.Init();
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x00260824 File Offset: 0x0025EC24
		public ModMetaData(WorkshopItem workshopItem)
		{
			this.rootDirInt = workshopItem.Directory;
			this.source = ContentSource.SteamWorkshop;
			this.Init();
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06004846 RID: 18502 RVA: 0x00260878 File Offset: 0x0025EC78
		public string Identifier
		{
			get
			{
				return this.RootDir.Name;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06004847 RID: 18503 RVA: 0x00260898 File Offset: 0x0025EC98
		public DirectoryInfo RootDir
		{
			get
			{
				return this.rootDirInt;
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06004848 RID: 18504 RVA: 0x002608B4 File Offset: 0x0025ECB4
		public bool IsCoreMod
		{
			get
			{
				return this.Identifier == ModContentPack.CoreModIdentifier;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06004849 RID: 18505 RVA: 0x002608DC File Offset: 0x0025ECDC
		// (set) Token: 0x0600484A RID: 18506 RVA: 0x002608F7 File Offset: 0x0025ECF7
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

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x0600484B RID: 18507 RVA: 0x00260904 File Offset: 0x0025ED04
		public bool VersionCompatible
		{
			get
			{
				return this.IsCoreMod || (VersionControl.IsWellFormattedVersionString(this.TargetVersion) && VersionControl.MinorFromVersionString(this.TargetVersion) == VersionControl.CurrentMinor && VersionControl.MajorFromVersionString(this.TargetVersion) == VersionControl.CurrentMajor);
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x0600484C RID: 18508 RVA: 0x0026096C File Offset: 0x0025ED6C
		// (set) Token: 0x0600484D RID: 18509 RVA: 0x0026098C File Offset: 0x0025ED8C
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

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x0600484E RID: 18510 RVA: 0x0026099C File Offset: 0x0025ED9C
		public string Author
		{
			get
			{
				return this.meta.author;
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x0600484F RID: 18511 RVA: 0x002609BC File Offset: 0x0025EDBC
		public string Url
		{
			get
			{
				return this.meta.url;
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06004850 RID: 18512 RVA: 0x002609DC File Offset: 0x0025EDDC
		public string TargetVersion
		{
			get
			{
				return this.meta.targetVersion;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06004851 RID: 18513 RVA: 0x002609FC File Offset: 0x0025EDFC
		public string Description
		{
			get
			{
				return this.meta.description;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06004852 RID: 18514 RVA: 0x00260A1C File Offset: 0x0025EE1C
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

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06004853 RID: 18515 RVA: 0x00260A74 File Offset: 0x0025EE74
		public ContentSource Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x06004854 RID: 18516 RVA: 0x00260A90 File Offset: 0x0025EE90
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

		// Token: 0x06004855 RID: 18517 RVA: 0x00260C13 File Offset: 0x0025F013
		internal void DeleteContent()
		{
			this.rootDirInt.Delete(true);
			ModLister.RebuildModList();
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06004856 RID: 18518 RVA: 0x00260C28 File Offset: 0x0025F028
		public bool OnSteamWorkshop
		{
			get
			{
				return this.source == ContentSource.SteamWorkshop;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06004857 RID: 18519 RVA: 0x00260C48 File Offset: 0x0025F048
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

		// Token: 0x06004858 RID: 18520 RVA: 0x00260CA0 File Offset: 0x0025F0A0
		public void PrepareForWorkshopUpload()
		{
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x00260CA4 File Offset: 0x0025F0A4
		public bool CanToUploadToWorkshop()
		{
			return !this.IsCoreMod && this.Source == ContentSource.LocalFolder && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x00260CF8 File Offset: 0x0025F0F8
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x00260D13 File Offset: 0x0025F113
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			if (!(this.publishedFileIdInt == newPfid))
			{
				this.publishedFileIdInt = newPfid;
				File.WriteAllText(this.PublishedFileIdPath, newPfid.ToString());
			}
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x00260D4C File Offset: 0x0025F14C
		public string GetWorkshopName()
		{
			return this.Name;
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x00260D68 File Offset: 0x0025F168
		public string GetWorkshopDescription()
		{
			return this.Description;
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x00260D84 File Offset: 0x0025F184
		public string GetWorkshopPreviewImagePath()
		{
			return this.PreviewImagePath;
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x00260DA0 File Offset: 0x0025F1A0
		public IList<string> GetWorkshopTags()
		{
			return new List<string>
			{
				"Mod"
			};
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x00260DC8 File Offset: 0x0025F1C8
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return this.RootDir;
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x00260DE4 File Offset: 0x0025F1E4
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x00260E18 File Offset: 0x0025F218
		public override int GetHashCode()
		{
			return this.Identifier.GetHashCode();
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x00260E38 File Offset: 0x0025F238
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

		// Token: 0x06004864 RID: 18532 RVA: 0x00260E84 File Offset: 0x0025F284
		public string ToStringLong()
		{
			return this.Identifier + "(" + this.RootDir.ToString() + ")";
		}

		// Token: 0x02000CC8 RID: 3272
		private class ModMetaDataInternal
		{
			// Token: 0x040030F1 RID: 12529
			public string name = "";

			// Token: 0x040030F2 RID: 12530
			public string author = "Anonymous";

			// Token: 0x040030F3 RID: 12531
			public string url = "";

			// Token: 0x040030F4 RID: 12532
			public string targetVersion = "Unknown";

			// Token: 0x040030F5 RID: 12533
			public string description = "No description provided.";
		}
	}
}
