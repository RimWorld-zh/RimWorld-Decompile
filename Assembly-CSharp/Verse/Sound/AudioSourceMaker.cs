using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBB RID: 3515
	public static class AudioSourceMaker
	{
		// Token: 0x04003444 RID: 13380
		private const AudioRolloffMode WorldRolloffMode = AudioRolloffMode.Linear;

		// Token: 0x06004E8E RID: 20110 RVA: 0x00290DA8 File Offset: 0x0028F1A8
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
	}
}
