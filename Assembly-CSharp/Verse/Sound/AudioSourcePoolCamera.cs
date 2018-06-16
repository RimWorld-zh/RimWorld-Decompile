using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBF RID: 3519
	public class AudioSourcePoolCamera
	{
		// Token: 0x06004E7A RID: 20090 RVA: 0x0028F7B0 File Offset: 0x0028DBB0
		public AudioSourcePoolCamera()
		{
			this.cameraSourcesContainer = new GameObject("OneShotSourcesCameraContainer");
			this.cameraSourcesContainer.transform.parent = Find.Camera.transform;
			this.cameraSourcesContainer.transform.localPosition = Vector3.zero;
			for (int i = 0; i < 16; i++)
			{
				AudioSource audioSource = AudioSourceMaker.NewAudioSourceOn(new GameObject("OneShotSourceCamera_" + i.ToString())
				{
					transform = 
					{
						parent = this.cameraSourcesContainer.transform,
						localPosition = Vector3.zero
					}
				});
				audioSource.bypassReverbZones = true;
				this.sourcesCamera.Add(audioSource);
			}
		}

		// Token: 0x06004E7B RID: 20091 RVA: 0x0028F880 File Offset: 0x0028DC80
		public AudioSource GetSourceCamera()
		{
			for (int i = 0; i < this.sourcesCamera.Count; i++)
			{
				AudioSource audioSource = this.sourcesCamera[i];
				if (!audioSource.isPlaying)
				{
					audioSource.clip = null;
					SoundFilterUtility.DisableAllFiltersOn(audioSource);
					return audioSource;
				}
			}
			return null;
		}

		// Token: 0x0400343E RID: 13374
		public GameObject cameraSourcesContainer;

		// Token: 0x0400343F RID: 13375
		private List<AudioSource> sourcesCamera = new List<AudioSource>();

		// Token: 0x04003440 RID: 13376
		private const int NumSourcesCamera = 16;
	}
}
