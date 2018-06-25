using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FAD RID: 4013
	public class PrefsData
	{
		// Token: 0x04003F63 RID: 16227
		public float volumeGame = 0.8f;

		// Token: 0x04003F64 RID: 16228
		public float volumeMusic = 0.4f;

		// Token: 0x04003F65 RID: 16229
		public float volumeAmbient = 1f;

		// Token: 0x04003F66 RID: 16230
		public float uiScale = 1f;

		// Token: 0x04003F67 RID: 16231
		public bool customCursorEnabled = true;

		// Token: 0x04003F68 RID: 16232
		public bool hatsOnlyOnMap = false;

		// Token: 0x04003F69 RID: 16233
		public bool plantWindSway = true;

		// Token: 0x04003F6A RID: 16234
		public bool showRealtimeClock = false;

		// Token: 0x04003F6B RID: 16235
		public AnimalNameDisplayMode animalNameMode = AnimalNameDisplayMode.None;

		// Token: 0x04003F6C RID: 16236
		public bool adaptiveTrainingEnabled = true;

		// Token: 0x04003F6D RID: 16237
		public List<string> preferredNames = new List<string>();

		// Token: 0x04003F6E RID: 16238
		public bool resourceReadoutCategorized = false;

		// Token: 0x04003F6F RID: 16239
		public bool runInBackground = false;

		// Token: 0x04003F70 RID: 16240
		public bool edgeScreenScroll = true;

		// Token: 0x04003F71 RID: 16241
		public TemperatureDisplayMode temperatureMode = TemperatureDisplayMode.Celsius;

		// Token: 0x04003F72 RID: 16242
		public float autosaveIntervalDays = 1f;

		// Token: 0x04003F73 RID: 16243
		public bool testMapSizes = false;

		// Token: 0x04003F74 RID: 16244
		public int maxNumberOfPlayerHomes = 1;

		// Token: 0x04003F75 RID: 16245
		public bool pauseOnLoad = false;

		// Token: 0x04003F76 RID: 16246
		public bool pauseOnUrgentLetter = false;

		// Token: 0x04003F77 RID: 16247
		public bool devMode = false;

		// Token: 0x04003F78 RID: 16248
		public string langFolderName = "unknown";

		// Token: 0x04003F79 RID: 16249
		public bool logVerbose = false;

		// Token: 0x04003F7A RID: 16250
		public bool pauseOnError = false;

		// Token: 0x04003F7B RID: 16251
		public bool resetModsConfigOnCrash = true;

		// Token: 0x06006105 RID: 24837 RVA: 0x00310BAA File Offset: 0x0030EFAA
		public void Apply()
		{
			if (this.customCursorEnabled)
			{
				CustomCursor.Activate();
			}
			else
			{
				CustomCursor.Deactivate();
			}
			AudioListener.volume = this.volumeGame;
			Application.runInBackground = this.runInBackground;
		}
	}
}
