using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000658 RID: 1624
	public static class AmbientSoundManager
	{
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060021DB RID: 8667 RVA: 0x0011F230 File Offset: 0x0011D630
		private static bool WorldAmbientSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_Space);
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060021DC RID: 8668 RVA: 0x0011F25C File Offset: 0x0011D65C
		private static bool AltitudeWindSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_AltitudeWind);
			}
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x0011F285 File Offset: 0x0011D685
		public static void EnsureWorldAmbientSoundCreated()
		{
			if (!AmbientSoundManager.WorldAmbientSoundCreated)
			{
				SoundDefOf.Ambient_Space.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
			}
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x0011F2A3 File Offset: 0x0011D6A3
		public static void Notify_SwitchedMap()
		{
			LongEventHandler.ExecuteWhenFinished(AmbientSoundManager.recreateMapSustainers);
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x0011F2B0 File Offset: 0x0011D6B0
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

		// Token: 0x060021E0 RID: 8672 RVA: 0x0011F393 File Offset: 0x0011D793
		// Note: this type is marked as 'beforefieldinit'.
		static AmbientSoundManager()
		{
			if (AmbientSoundManager.<>f__mg$cache0 == null)
			{
				AmbientSoundManager.<>f__mg$cache0 = new Action(AmbientSoundManager.RecreateMapSustainers);
			}
			AmbientSoundManager.recreateMapSustainers = AmbientSoundManager.<>f__mg$cache0;
		}

		// Token: 0x0400132F RID: 4911
		private static List<Sustainer> biomeAmbientSustainers = new List<Sustainer>();

		// Token: 0x04001330 RID: 4912
		private static Action recreateMapSustainers;

		// Token: 0x04001331 RID: 4913
		[CompilerGenerated]
		private static Action <>f__mg$cache0;
	}
}
