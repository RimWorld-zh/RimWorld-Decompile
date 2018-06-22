using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B7C RID: 2940
	public static class ReverbCustomPresets
	{
		// Token: 0x06004018 RID: 16408 RVA: 0x0021C4C8 File Offset: 0x0021A8C8
		public static void SetupAs(this ReverbSetup setup, AudioReverbPreset preset)
		{
			GameObject gameObject = new GameObject("OneFrameTemp");
			gameObject.AddComponent<AudioSource>();
			AudioReverbFilter audioReverbFilter = gameObject.AddComponent<AudioReverbFilter>();
			audioReverbFilter.reverbPreset = preset;
			setup.dryLevel = audioReverbFilter.dryLevel;
			setup.room = audioReverbFilter.room;
			setup.roomHF = audioReverbFilter.roomHF;
			setup.roomLF = audioReverbFilter.roomLF;
			setup.decayTime = audioReverbFilter.decayTime;
			setup.decayHFRatio = audioReverbFilter.decayHFRatio;
			setup.reflectionsLevel = audioReverbFilter.reflectionsLevel;
			setup.reflectionsDelay = audioReverbFilter.reflectionsDelay;
			setup.reverbLevel = audioReverbFilter.reverbLevel;
			setup.reverbDelay = audioReverbFilter.reverbDelay;
			setup.hfReference = audioReverbFilter.hfReference;
			setup.lfReference = audioReverbFilter.lfReference;
			setup.diffusion = audioReverbFilter.diffusion;
			setup.density = audioReverbFilter.density;
			UnityEngine.Object.Destroy(audioReverbFilter);
		}
	}
}
