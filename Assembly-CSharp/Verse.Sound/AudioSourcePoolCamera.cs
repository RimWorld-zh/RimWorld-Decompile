using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	public class AudioSourcePoolCamera
	{
		public GameObject cameraSourcesContainer;

		private List<AudioSource> sourcesCamera = new List<AudioSource>();

		private const int NumSourcesCamera = 16;

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

		public AudioSource GetSourceCamera()
		{
			int num = 0;
			AudioSource result;
			while (true)
			{
				if (num < this.sourcesCamera.Count)
				{
					AudioSource audioSource = this.sourcesCamera[num];
					if (!audioSource.isPlaying)
					{
						audioSource.clip = null;
						SoundFilterUtility.DisableAllFiltersOn(audioSource);
						result = audioSource;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
