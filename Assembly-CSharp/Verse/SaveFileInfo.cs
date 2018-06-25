using System;
using System.IO;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public struct SaveFileInfo
	{
		private FileInfo fileInfo;

		private string gameVersion;

		public static readonly Color UnimportantTextColor = new Color(1f, 1f, 1f, 0.5f);

		public SaveFileInfo(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
			this.gameVersion = ScribeMetaHeaderUtility.GameVersionOf(fileInfo);
		}

		public bool Valid
		{
			get
			{
				return this.gameVersion != null;
			}
		}

		public FileInfo FileInfo
		{
			get
			{
				return this.fileInfo;
			}
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static SaveFileInfo()
		{
		}
	}
}
