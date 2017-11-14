using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Profile;

namespace RimWorld
{
	public sealed class Autosaver
	{
		private int ticksSinceSave;

		private const int NumAutosaves = 5;

		public const float MaxPermadeathModeAutosaveInterval = 1f;

		private float AutosaveIntervalDays
		{
			get
			{
				float num = Prefs.AutosaveIntervalDays;
				if (Current.Game.Info.permadeathMode && num > 1.0)
				{
					num = 1f;
				}
				return num;
			}
		}

		private int AutosaveIntervalTicks
		{
			get
			{
				return Mathf.RoundToInt((float)(this.AutosaveIntervalDays * 60000.0));
			}
		}

		public void AutosaverTick()
		{
			this.ticksSinceSave++;
			if (this.ticksSinceSave >= this.AutosaveIntervalTicks)
			{
				LongEventHandler.QueueLongEvent(this.DoAutosave, "Autosaving", false, null);
				this.ticksSinceSave = 0;
			}
		}

		private void DoAutosave()
		{
			string fileName = (!Current.Game.Info.permadeathMode) ? this.NewAutosaveFileName() : Current.Game.Info.permadeathModeUniqueName;
			GameDataSaveLoader.SaveGame(fileName);
		}

		private void DoMemoryCleanup()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		private string NewAutosaveFileName()
		{
			string text = (from name in this.AutoSaveNames()
			where !SaveGameFilesUtility.SavedGameNamedExists(name)
			select name).FirstOrDefault();
			if (text != null)
			{
				return text;
			}
			return this.AutoSaveNames().MinBy((string name) => new FileInfo(GenFilePaths.FilePathForSavedGame(name)).LastWriteTime);
		}

		private IEnumerable<string> AutoSaveNames()
		{
			int i = 1;
			if (i <= 5)
			{
				yield return "Autosave-" + i;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
