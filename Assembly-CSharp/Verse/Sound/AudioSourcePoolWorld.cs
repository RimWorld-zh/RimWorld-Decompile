using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBC RID: 3516
	public class AudioSourcePoolWorld
	{
		// Token: 0x06004E8F RID: 20111 RVA: 0x00290E74 File Offset: 0x0028F274
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

		// Token: 0x06004E90 RID: 20112 RVA: 0x00290F14 File Offset: 0x0028F314
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

		// Token: 0x0400344A RID: 13386
		private List<AudioSource> sourcesWorld = new List<AudioSource>();

		// Token: 0x0400344B RID: 13387
		private const int NumSourcesWorld = 32;
	}
}
