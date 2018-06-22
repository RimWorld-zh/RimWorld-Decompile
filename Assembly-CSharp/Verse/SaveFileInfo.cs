using System;
using System.IO;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D8E RID: 3470
	public struct SaveFileInfo
	{
		// Token: 0x06004DAE RID: 19886 RVA: 0x00289636 File Offset: 0x00287A36
		public SaveFileInfo(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
			this.gameVersion = ScribeMetaHeaderUtility.GameVersionOf(fileInfo);
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06004DAF RID: 19887 RVA: 0x0028964C File Offset: 0x00287A4C
		public bool Valid
		{
			get
			{
				return this.gameVersion != null;
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06004DB0 RID: 19888 RVA: 0x00289670 File Offset: 0x00287A70
		public FileInfo FileInfo
		{
			get
			{
				return this.fileInfo;
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x06004DB1 RID: 19889 RVA: 0x0028968C File Offset: 0x00287A8C
		public string GameVersion
		{
			get
			{
				string result;
				if (!this.Valid)
				{
					result = "???";
				}
				else
				{
					result = this.gameVersion;
				}
				return result;
			}
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x06004DB2 RID: 19890 RVA: 0x002896C0 File Offset: 0x00287AC0
		public Color VersionColor
		{
			get
			{
				Color result;
				if (!this.Valid)
				{
					result = Color.red;
				}
				else if (VersionControl.MajorFromVersionString(this.gameVersion) != VersionControl.CurrentMajor || VersionControl.MinorFromVersionString(this.gameVersion) != VersionControl.CurrentMinor)
				{
					if (BackCompatibility.IsSaveCompatibleWith(this.gameVersion))
					{
						result = Color.yellow;
					}
					else
					{
						result = Color.red;
					}
				}
				else if (VersionControl.BuildFromVersionString(this.gameVersion) != VersionControl.CurrentBuild)
				{
					result = Color.yellow;
				}
				else
				{
					result = SaveFileInfo.UnimportantTextColor;
				}
				return result;
			}
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x06004DB3 RID: 19891 RVA: 0x00289764 File Offset: 0x00287B64
		public TipSignal CompatibilityTip
		{
			get
			{
				TipSignal result;
				if (!this.Valid)
				{
					result = "SaveIsUnknownFormat".Translate();
				}
				else if (VersionControl.MajorFromVersionString(this.gameVersion) != VersionControl.CurrentMajor || VersionControl.MinorFromVersionString(this.gameVersion) != VersionControl.CurrentMinor)
				{
					result = "SaveIsFromDifferentGameVersion".Translate(new object[]
					{
						VersionControl.CurrentVersionString,
						this.gameVersion
					});
				}
				else if (VersionControl.BuildFromVersionString(this.gameVersion) != VersionControl.CurrentBuild)
				{
					result = "SaveIsFromDifferentGameBuild".Translate(new object[]
					{
						VersionControl.CurrentVersionString,
						this.gameVersion
					});
				}
				else
				{
					result = "SaveIsFromThisGameBuild".Translate();
				}
				return result;
			}
		}

		// Token: 0x040033D4 RID: 13268
		private FileInfo fileInfo;

		// Token: 0x040033D5 RID: 13269
		private string gameVersion;

		// Token: 0x040033D6 RID: 13270
		public static readonly Color UnimportantTextColor = new Color(1f, 1f, 1f, 0.5f);
	}
}
