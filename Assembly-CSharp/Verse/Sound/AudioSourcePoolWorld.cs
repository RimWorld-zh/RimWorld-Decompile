using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBF RID: 3519
	public class AudioSourcePoolWorld
	{
		// Token: 0x04003451 RID: 13393
		private List<AudioSource> sourcesWorld = new List<AudioSource>();

		// Token: 0x04003452 RID: 13394
		private const int NumSourcesWorld = 32;

		// Token: 0x06004E93 RID: 20115 RVA: 0x00291280 File Offset: 0x0028F680
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

		// Token: 0x06004E94 RID: 20116 RVA: 0x00291320 File Offset: 0x0028F720
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
	}
}
