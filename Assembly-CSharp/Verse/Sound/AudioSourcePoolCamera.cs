using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBE RID: 3518
	public class AudioSourcePoolCamera
	{
		// Token: 0x06004E78 RID: 20088 RVA: 0x0028F790 File Offset: 0x0028DB90
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

		// Token: 0x06004E79 RID: 20089 RVA: 0x0028F860 File Offset: 0x0028DC60
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

		// Token: 0x0400343C RID: 13372
		public GameObject cameraSourcesContainer;

		// Token: 0x0400343D RID: 13373
		private List<AudioSource> sourcesCamera = new List<AudioSource>();

		// Token: 0x0400343E RID: 13374
		private const int NumSourcesCamera = 16;
	}
}
