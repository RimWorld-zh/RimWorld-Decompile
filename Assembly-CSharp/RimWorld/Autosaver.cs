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
	// Token: 0x020008F4 RID: 2292
	public sealed class Autosaver
	{
		// Token: 0x04001CA2 RID: 7330
		private int ticksSinceSave = 0;

		// Token: 0x04001CA3 RID: 7331
		private const int NumAutosaves = 5;

		// Token: 0x04001CA4 RID: 7332
		public const float MaxPermadeathModeAutosaveInterval = 1f;

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x060034FE RID: 13566 RVA: 0x001C5234 File Offset: 0x001C3634
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

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x060034FF RID: 13567 RVA: 0x001C5278 File Offset: 0x001C3678
		private int AutosaveIntervalTicks
		{
			get
			{
				return Mathf.RoundToInt(this.AutosaveIntervalDays * 60000f);
			}
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x001C52A0 File Offset: 0x001C36A0
		public void AutosaverTick()
		{
			this.ticksSinceSave++;
			if (this.ticksSinceSave >= this.AutosaveIntervalTicks)
			{
				LongEventHandler.QueueLongEvent(new Action(this.DoAutosave), "Autosaving", false, null);
				this.ticksSinceSave = 0;
			}
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x001C52F0 File Offset: 0x001C36F0
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

		// Token: 0x06003502 RID: 13570 RVA: 0x001C5334 File Offset: 0x001C3734
		private void DoMemoryCleanup()
		{
			Profiler.BeginSample("UnloadUnusedAssets");
			MemoryUtility.UnloadUnusedUnityAssets();
			Profiler.EndSample();
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x001C534C File Offset: 0x001C374C
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

		// Token: 0x06003504 RID: 13572 RVA: 0x001C53C8 File Offset: 0x001C37C8
		private IEnumerable<string> AutoSaveNames()
		{
			for (int i = 1; i <= 5; i++)
			{
				yield return "Autosave-" + i;
			}
			yield break;
		}
	}
}
