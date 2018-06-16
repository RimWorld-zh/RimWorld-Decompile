using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBD RID: 3517
	public static class AudioSourceMaker
	{
		// Token: 0x06004E77 RID: 20087 RVA: 0x0028F6EC File Offset: 0x0028DAEC
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

		// Token: 0x0400343B RID: 13371
		private const AudioRolloffMode WorldRolloffMode = AudioRolloffMode.Linear;
	}
}
