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
	// Token: 0x02000CC5 RID: 3269
	public class ModMetaData : WorkshopUploadable
	{
		// Token: 0x06004841 RID: 18497 RVA: 0x002606F4 File Offset: 0x0025EAF4
		public ModMetaData(string localAbsPath)
		{
			this.rootDirInt = new DirectoryInfo(localAbsPath);
			this.source = ContentSource.LocalFolder;
			this.Init();
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x00260748 File Offset: 0x0025EB48
		public ModMetaData(WorkshopItem workshopItem)
		{
			this.rootDirInt = workshopItem.Directory;
			this.source = ContentSource.SteamWorkshop;
			this.Init();
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06004843 RID: 18499 RVA: 0x0026079C File Offset: 0x0025EB9C
		public string Identifier
		{
			get
			{
				return this.RootDir.Name;
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06004844 RID: 18500 RVA: 0x002607BC File Offset: 0x0025EBBC
		public DirectoryInfo RootDir
		{
			get
			{
				return this.rootDirInt;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06004845 RID: 18501 RVA: 0x002607D8 File Offset: 0x0025EBD8
		public bool IsCoreMod
		{
			get
			{
				return this.Identifier == ModContentPack.CoreModIdentifier;
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06004846 RID: 18502 RVA: 0x00260800 File Offset: 0x0025EC00
		// (set) Token: 0x06004847 RID: 18503 RVA: 0x0026081B File Offset: 0x0025EC1B
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

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06004848 RID: 18504 RVA: 0x00260828 File Offset: 0x0025EC28
		public bool VersionCompatible
		{
			get
			{
				return this.IsCoreMod || (VersionControl.IsWellFormattedVersionString(this.TargetVersion) && VersionControl.MinorFromVersionString(this.TargetVersion) == VersionControl.CurrentMinor && VersionControl.MajorFromVersionString(this.TargetVersion) == VersionControl.CurrentMajor);
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06004849 RID: 18505 RVA: 0x00260890 File Offset: 0x0025EC90
		// (set) Token: 0x0600484A RID: 18506 RVA: 0x002608B0 File Offset: 0x0025ECB0
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

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x0600484B RID: 18507 RVA: 0x002608C0 File Offset: 0x0025ECC0
		public string Author
		{
			get
			{
				return this.meta.author;
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x0600484C RID: 18508 RVA: 0x002608E0 File Offset: 0x0025ECE0
		public string Url
		{
			get
			{
				return this.meta.url;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x0600484D RID: 18509 RVA: 0x00260900 File Offset: 0x0025ED00
		public string TargetVersion
		{
			get
			{
				return this.meta.targetVersion;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x0600484E RID: 18510 RVA: 0x00260920 File Offset: 0x0025ED20
		public string Description
		{
			get
			{
				return this.meta.description;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x0600484F RID: 18511 RVA: 0x00260940 File Offset: 0x0025ED40
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

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06004850 RID: 18512 RVA: 0x00260998 File Offset: 0x0025ED98
		public ContentSource Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x002609B4 File Offset: 0x0025EDB4
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

		// Token: 0x06004852 RID: 18514 RVA: 0x00260B37 File Offset: 0x0025EF37
		internal void DeleteContent()
		{
			this.rootDirInt.Delete(true);
			ModLister.RebuildModList();
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06004853 RID: 18515 RVA: 0x00260B4C File Offset: 0x0025EF4C
		public bool OnSteamWorkshop
		{
			get
			{
				return this.source == ContentSource.SteamWorkshop;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06004854 RID: 18516 RVA: 0x00260B6C File Offset: 0x0025EF6C
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

		// Token: 0x06004855 RID: 18517 RVA: 0x00260BC4 File Offset: 0x0025EFC4
		public void PrepareForWorkshopUpload()
		{
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x00260BC8 File Offset: 0x0025EFC8
		public bool CanToUploadToWorkshop()
		{
			return !this.IsCoreMod && this.Source == ContentSource.LocalFolder && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x00260C1C File Offset: 0x0025F01C
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x00260C37 File Offset: 0x0025F037
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			if (!(this.publishedFileIdInt == newPfid))
			{
				this.publishedFileIdInt = newPfid;
				File.WriteAllText(this.PublishedFileIdPath, newPfid.ToString());
			}
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x00260C70 File Offset: 0x0025F070
		public string GetWorkshopName()
		{
			return this.Name;
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x00260C8C File Offset: 0x0025F08C
		public string GetWorkshopDescription()
		{
			return this.Description;
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x00260CA8 File Offset: 0x0025F0A8
		public string GetWorkshopPreviewImagePath()
		{
			return this.PreviewImagePath;
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x00260CC4 File Offset: 0x0025F0C4
		public IList<string> GetWorkshopTags()
		{
			return new List<string>
			{
				"Mod"
			};
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x00260CEC File Offset: 0x0025F0EC
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return this.RootDir;
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x00260D08 File Offset: 0x0025F108
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x00260D3C File Offset: 0x0025F13C
		public override int GetHashCode()
		{
			return this.Identifier.GetHashCode();
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x00260D5C File Offset: 0x0025F15C
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

		// Token: 0x06004861 RID: 18529 RVA: 0x00260DA8 File Offset: 0x0025F1A8
		public string ToStringLong()
		{
			return this.Identifier + "(" + this.RootDir.ToString() + ")";
		}

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

		// Token: 0x02000CC6 RID: 3270
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
