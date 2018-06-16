using System;
using System.IO;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D92 RID: 3474
	public struct SaveFileInfo
	{
		// Token: 0x06004D9B RID: 19867 RVA: 0x002880A6 File Offset: 0x002864A6
		public SaveFileInfo(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
			this.gameVersion = ScribeMetaHeaderUtility.GameVersionOf(fileInfo);
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06004D9C RID: 19868 RVA: 0x002880BC File Offset: 0x002864BC
		public bool Valid
		{
			get
			{
				return this.gameVersion != null;
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06004D9D RID: 19869 RVA: 0x002880E0 File Offset: 0x002864E0
		public FileInfo FileInfo
		{
			get
			{
				return this.fileInfo;
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06004D9E RID: 19870 RVA: 0x002880FC File Offset: 0x002864FC
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
		// (get) Token: 0x06004D9F RID: 19871 RVA: 0x00288130 File Offset: 0x00286530
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
		// (get) Token: 0x06004DA0 RID: 19872 RVA: 0x002881D4 File Offset: 0x002865D4
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

		// Token: 0x040033CB RID: 13259
		private FileInfo fileInfo;

		// Token: 0x040033CC RID: 13260
		private string gameVersion;

		// Token: 0x040033CD RID: 13261
		public static readonly Color UnimportantTextColor = new Color(1f, 1f, 1f, 0.5f);
	}
}
