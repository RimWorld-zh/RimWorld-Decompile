using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBC RID: 3516
	public static class AudioSourceMaker
	{
		// Token: 0x0400344B RID: 13387
		private const AudioRolloffMode WorldRolloffMode = AudioRolloffMode.Linear;

		// Token: 0x06004E8E RID: 20110 RVA: 0x00291088 File Offset: 0x0028F488
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
