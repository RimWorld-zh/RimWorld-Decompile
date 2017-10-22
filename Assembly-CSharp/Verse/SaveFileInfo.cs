using RimWorld;
using System.IO;
using UnityEngine;

namespace Verse
{
	public struct SaveFileInfo
	{
		private FileInfo fileInfo;

		private string gameVersion;

		public static readonly Color UnimportantTextColor = new Color(1f, 1f, 1f, 0.5f);

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
				return this.Valid ? this.gameVersion : "???";
			}
		}

		public Color VersionColor
		{
			get
			{
				return this.Valid ? ((VersionControl.MajorFromVersionString(this.gameVersion) != VersionControl.CurrentMajor || VersionControl.MinorFromVersionString(this.gameVersion) != VersionControl.CurrentMinor) ? ((!BackCompatibility.IsSaveCompatibleWith(this.gameVersion)) ? Color.red : Color.yellow) : ((VersionControl.BuildFromVersionString(this.gameVersion) == VersionControl.CurrentBuild) ? SaveFileInfo.UnimportantTextColor : Color.yellow)) : Color.red;
			}
		}

		public TipSignal CompatibilityTip
		{
			get
			{
				return this.Valid ? ((VersionControl.MajorFromVersionString(this.gameVersion) != VersionControl.CurrentMajor || VersionControl.MinorFromVersionString(this.gameVersion) != VersionControl.CurrentMinor) ? "SaveIsFromDifferentGameVersion".Translate(VersionControl.CurrentVersionString, this.gameVersion) : ((VersionControl.BuildFromVersionString(this.gameVersion) == VersionControl.CurrentBuild) ? "SaveIsFromThisGameBuild".Translate() : "SaveIsFromDifferentGameBuild".Translate(VersionControl.CurrentVersionString, this.gameVersion))) : "SaveIsUnknownFormat".Translate();
			}
		}

		public SaveFileInfo(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
			this.gameVersion = ScribeMetaHeaderUtility.GameVersionOf(fileInfo);
		}
	}
}
