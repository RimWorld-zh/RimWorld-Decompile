using System;
using System.IO;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D91 RID: 3473
	public struct SaveFileInfo
	{
		// Token: 0x040033DB RID: 13275
		private FileInfo fileInfo;

		// Token: 0x040033DC RID: 13276
		private string gameVersion;

		// Token: 0x040033DD RID: 13277
		public static readonly Color UnimportantTextColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x06004DB2 RID: 19890 RVA: 0x00289A42 File Offset: 0x00287E42
		public SaveFileInfo(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
			this.gameVersion = ScribeMetaHeaderUtility.GameVersionOf(fileInfo);
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06004DB3 RID: 19891 RVA: 0x00289A58 File Offset: 0x00287E58
		public bool Valid
		{
			get
			{
				return this.gameVersion != null;
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06004DB4 RID: 19892 RVA: 0x00289A7C File Offset: 0x00287E7C
		public FileInfo FileInfo
		{
			get
			{
				return this.fileInfo;
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06004DB5 RID: 19893 RVA: 0x00289A98 File Offset: 0x00287E98
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

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x06004DB6 RID: 19894 RVA: 0x00289ACC File Offset: 0x00287ECC
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

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x06004DB7 RID: 19895 RVA: 0x00289B70 File Offset: 0x00287F70
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
	}
}
