using System;
using System.IO;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D90 RID: 3472
	public struct SaveFileInfo
	{
		// Token: 0x040033D4 RID: 13268
		private FileInfo fileInfo;

		// Token: 0x040033D5 RID: 13269
		private string gameVersion;

		// Token: 0x040033D6 RID: 13270
		public static readonly Color UnimportantTextColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x06004DB2 RID: 19890 RVA: 0x00289762 File Offset: 0x00287B62
		public SaveFileInfo(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
			this.gameVersion = ScribeMetaHeaderUtility.GameVersionOf(fileInfo);
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06004DB3 RID: 19891 RVA: 0x00289778 File Offset: 0x00287B78
		public bool Valid
		{
			get
			{
				return this.gameVersion != null;
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06004DB4 RID: 19892 RVA: 0x0028979C File Offset: 0x00287B9C
		public FileInfo FileInfo
		{
			get
			{
				return this.fileInfo;
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06004DB5 RID: 19893 RVA: 0x002897B8 File Offset: 0x00287BB8
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
		// (get) Token: 0x06004DB6 RID: 19894 RVA: 0x002897EC File Offset: 0x00287BEC
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
		// (get) Token: 0x06004DB7 RID: 19895 RVA: 0x00289890 File Offset: 0x00287C90
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
