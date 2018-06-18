using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FA9 RID: 4009
	public class PrefsData
	{
		// Token: 0x060060D2 RID: 24786 RVA: 0x0030E486 File Offset: 0x0030C886
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

		// Token: 0x04003F4E RID: 16206
		public float volumeGame = 0.8f;

		// Token: 0x04003F4F RID: 16207
		public float volumeMusic = 0.4f;

		// Token: 0x04003F50 RID: 16208
		public float volumeAmbient = 1f;

		// Token: 0x04003F51 RID: 16209
		public float uiScale = 1f;

		// Token: 0x04003F52 RID: 16210
		public bool customCursorEnabled = true;

		// Token: 0x04003F53 RID: 16211
		public bool hatsOnlyOnMap = false;

		// Token: 0x04003F54 RID: 16212
		public bool plantWindSway = true;

		// Token: 0x04003F55 RID: 16213
		public bool showRealtimeClock = false;

		// Token: 0x04003F56 RID: 16214
		public AnimalNameDisplayMode animalNameMode = AnimalNameDisplayMode.None;

		// Token: 0x04003F57 RID: 16215
		public bool adaptiveTrainingEnabled = true;

		// Token: 0x04003F58 RID: 16216
		public List<string> preferredNames = new List<string>();

		// Token: 0x04003F59 RID: 16217
		public bool resourceReadoutCategorized = false;

		// Token: 0x04003F5A RID: 16218
		public bool runInBackground = false;

		// Token: 0x04003F5B RID: 16219
		public bool edgeScreenScroll = true;

		// Token: 0x04003F5C RID: 16220
		public TemperatureDisplayMode temperatureMode = TemperatureDisplayMode.Celsius;

		// Token: 0x04003F5D RID: 16221
		public float autosaveIntervalDays = 1f;

		// Token: 0x04003F5E RID: 16222
		public bool testMapSizes = false;

		// Token: 0x04003F5F RID: 16223
		public int maxNumberOfPlayerHomes = 1;

		// Token: 0x04003F60 RID: 16224
		public bool pauseOnLoad = false;

		// Token: 0x04003F61 RID: 16225
		public bool pauseOnUrgentLetter = false;

		// Token: 0x04003F62 RID: 16226
		public bool devMode = false;

		// Token: 0x04003F63 RID: 16227
		public string langFolderName = "unknown";

		// Token: 0x04003F64 RID: 16228
		public bool logVerbose = false;

		// Token: 0x04003F65 RID: 16229
		public bool pauseOnError = false;

		// Token: 0x04003F66 RID: 16230
		public bool resetModsConfigOnCrash = true;
	}
}
