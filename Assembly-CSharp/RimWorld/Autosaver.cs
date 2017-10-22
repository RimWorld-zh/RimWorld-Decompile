#define ENABLE_PROFILER
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;
using Verse.Profile;

namespace RimWorld
{
	public sealed class Autosaver
	{
		private int ticksSinceSave = 0;

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
				LongEventHandler.QueueLongEvent(new Action(this.DoAutosave), "Autosaving", false, null);
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
			Profiler.BeginSample("UnloadUnusedAssets");
			MemoryUtility.UnloadUnusedUnityAssets();
			Profiler.EndSample();
		}

		private string NewAutosaveFileName()
		{
			string text = (from name in this.AutoSaveNames()
			where !SaveGameFilesUtility.SavedGameNamedExists(name)
			select name).FirstOrDefault();
			string result;
			if (text != null)
			{
				result = text;
			}
			else
			{
				string text2 = result = this.AutoSaveNames().MinBy((Func<string, DateTime>)((string name) => new FileInfo(GenFilePaths.FilePathForSavedGame(name)).LastWriteTime));
			}
			return result;
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
