using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class PrefsData
	{
		public float volumeGame = 0.8f;

		public float volumeMusic = 0.4f;

		public float volumeAmbient = 1f;

		public float uiScale = 1f;

		public bool customCursorEnabled = true;

		public bool hatsOnlyOnMap = false;

		public bool plantWindSway = true;

		public bool showRealtimeClock = false;

		public AnimalNameDisplayMode animalNameMode = AnimalNameDisplayMode.None;

		public bool adaptiveTrainingEnabled = true;

		public List<string> preferredNames = new List<string>();

		public bool resourceReadoutCategorized = false;

		public bool runInBackground = false;

		public bool edgeScreenScroll = true;

		public TemperatureDisplayMode temperatureMode = TemperatureDisplayMode.Celsius;

		public float autosaveIntervalDays = 1f;

		public bool testMapSizes = false;

		public int maxNumberOfPlayerHomes = 1;

		public bool pauseOnLoad = false;

		public bool pauseOnUrgentLetter = false;

		public bool devMode = false;

		public string langFolderName = "unknown";

		public bool logVerbose = false;

		public bool pauseOnError = false;

		public bool resetModsConfigOnCrash = true;

		public PrefsData()
		{
		}

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
