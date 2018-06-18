using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBC RID: 3516
	public static class AudioSourceMaker
	{
		// Token: 0x06004E75 RID: 20085 RVA: 0x0028F6CC File Offset: 0x0028DACC
		public static AudioSource NewAudioSourceOn(GameObject go)
		{
			AudioSource result;
			if (go.GetComponent<AudioSource>() != null)
			{
				Log.Warning("Adding audio source on " + go + " that already has one.", false);
				result = go.GetComponent<AudioSource>();
			}
			else
			{
				AudioSource audioSource = go.AddComponent<AudioSource>();
				audioSource.rolloffMode = AudioRolloffMode.Linear;
				audioSource.dopplerLevel = 0f;
				audioSource.playOnAwake = false;
				result = audioSource;
			}
			return result;
		}

		// Token: 0x04003439 RID: 13369
		private const AudioRolloffMode WorldRolloffMode = AudioRolloffMode.Linear;
	}
}
