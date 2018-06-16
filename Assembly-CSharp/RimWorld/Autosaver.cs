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
	// Token: 0x020008F6 RID: 2294
	public sealed class Autosaver
	{
		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x060034FF RID: 13567 RVA: 0x001C4B70 File Offset: 0x001C2F70
		private float AutosaveIntervalDays
		{
			get
			{
				float num = Prefs.AutosaveIntervalDays;
				if (Current.Game.Info.permadeathMode && num > 1f)
				{
					num = 1f;
				}
				return num;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06003500 RID: 13568 RVA: 0x001C4BB4 File Offset: 0x001C2FB4
		private int AutosaveIntervalTicks
		{
			get
			{
				return Mathf.RoundToInt(this.AutosaveIntervalDays * 60000f);
			}
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x001C4BDC File Offset: 0x001C2FDC
		public void AutosaverTick()
		{
			this.ticksSinceSave++;
			if (this.ticksSinceSave >= this.AutosaveIntervalTicks)
			{
				LongEventHandler.QueueLongEvent(new Action(this.DoAutosave), "Autosaving", false, null);
				this.ticksSinceSave = 0;
			}
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x001C4C2C File Offset: 0x001C302C
		public void DoAutosave()
		{
			string fileName;
			if (Current.Game.Info.permadeathMode)
			{
				fileName = Current.Game.Info.permadeathModeUniqueName;
			}
			else
			{
				fileName = this.NewAutosaveFileName();
			}
			GameDataSaveLoader.SaveGame(fileName);
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x001C4C70 File Offset: 0x001C3070
		private void DoMemoryCleanup()
		{
			Profiler.BeginSample("UnloadUnusedAssets");
			MemoryUtility.UnloadUnusedUnityAssets();
			Profiler.EndSample();
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x001C4C88 File Offset: 0x001C3088
		private string NewAutosaveFileName()
		{
			string text = (from name in this.AutoSaveNames()
			where !SaveGameFilesUtility.SavedGameNamedExists(name)
			select name).FirstOrDefault<string>();
			string result;
			if (text != null)
			{
				result = text;
			}
			else
			{
				string text2 = this.AutoSaveNames().MinBy((string name) => new FileInfo(GenFilePaths.FilePathForSavedGame(name)).LastWriteTime);
				result = text2;
			}
			return result;
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x001C4D04 File Offset: 0x001C3104
		private IEnumerable<string> AutoSaveNames()
		{
			for (int i = 1; i <= 5; i++)
			{
				yield return "Autosave-" + i;
			}
			yield break;
		}

		// Token: 0x04001C9E RID: 7326
		private int ticksSinceSave = 0;

		// Token: 0x04001C9F RID: 7327
		private const int NumAutosaves = 5;

		// Token: 0x04001CA0 RID: 7328
		public const float MaxPermadeathModeAutosaveInterval = 1f;
	}
}
