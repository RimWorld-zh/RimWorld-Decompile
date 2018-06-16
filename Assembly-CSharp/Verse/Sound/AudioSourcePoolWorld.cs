using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DC0 RID: 3520
	public class AudioSourcePoolWorld
	{
		// Token: 0x06004E7C RID: 20092 RVA: 0x0028F8E4 File Offset: 0x0028DCE4
		public AudioSourcePoolWorld()
		{
			GameObject gameObject = new GameObject("OneShotSourcesWorldContainer");
			gameObject.transform.position = Vector3.zero;
			for (int i = 0; i < 32; i++)
			{
				GameObject gameObject2 = new GameObject("OneShotSource_" + i.ToString());
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				this.sourcesWorld.Add(AudioSourceMaker.NewAudioSourceOn(gameObject2));
			}
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x0028F984 File Offset: 0x0028DD84
		public AudioSource GetSourceWorld()
		{
			foreach (AudioSource audioSource in this.sourcesWorld)
			{
				if (!audioSource.isPlaying)
				{
					SoundFilterUtility.DisableAllFiltersOn(audioSource);
					return audioSource;
				}
			}
			return null;
		}

		// Token: 0x04003441 RID: 13377
		private List<AudioSource> sourcesWorld = new List<AudioSource>();

		// Token: 0x04003442 RID: 13378
		private const int NumSourcesWorld = 32;
	}
}
