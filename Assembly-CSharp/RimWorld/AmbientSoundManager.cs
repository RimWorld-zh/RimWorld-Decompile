using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public static class AmbientSoundManager
	{
		private static List<Sustainer> biomeAmbientSustainers = new List<Sustainer>();

		private static Action recreateMapSustainers;

		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		private static bool WorldAmbientSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_Space);
			}
		}

		private static bool AltitudeWindSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_AltitudeWind);
			}
		}

		public static void EnsureWorldAmbientSoundCreated()
		{
			if (!AmbientSoundManager.WorldAmbientSoundCreated)
			{
				SoundDefOf.Ambient_Space.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
			}
		}

		public static void Notify_SwitchedMap()
		{
			LongEventHandler.ExecuteWhenFinished(AmbientSoundManager.recreateMapSustainers);
		}

		private static void RecreateMapSustainers()
		{
			if (!AmbientSoundManager.AltitudeWindSoundCreated)
			{
				SoundDefOf.Ambient_AltitudeWind.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
			}
			SustainerManager sustainerManager = Find.SoundRoot.sustainerManager;
			for (int i = 0; i < AmbientSoundManager.biomeAmbientSustainers.Count; i++)
			{
				Sustainer sustainer = AmbientSoundManager.biomeAmbientSustainers[i];
				if (sustainerManager.AllSustainers.Contains(sustainer) && !sustainer.Ended)
				{
					sustainer.End();
				}
			}
			AmbientSoundManager.biomeAmbientSustainers.Clear();
			if (Find.CurrentMap != null)
			{
				List<SoundDef> soundsAmbient = Find.CurrentMap.Biome.soundsAmbient;
				for (int j = 0; j < soundsAmbient.Count; j++)
				{
					Sustainer item = soundsAmbient[j].TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
					AmbientSoundManager.biomeAmbientSustainers.Add(item);
				}
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static AmbientSoundManager()
		{
			if (AmbientSoundManager.<>f__mg$cache0 == null)
			{
				AmbientSoundManager.<>f__mg$cache0 = new Action(AmbientSoundManager.RecreateMapSustainers);
			}
			AmbientSoundManager.recreateMapSustainers = AmbientSoundManager.<>f__mg$cache0;
		}
	}
}
