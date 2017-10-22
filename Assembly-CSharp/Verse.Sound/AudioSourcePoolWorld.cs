using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	public class AudioSourcePoolWorld
	{
		private List<AudioSource> sourcesWorld = new List<AudioSource>();

		private const int NumSourcesWorld = 32;

		public AudioSourcePoolWorld()
		{
			GameObject gameObject = new GameObject("OneShotSourcesWorldContainer")
			{
				transform = 
				{
					position = Vector3.zero
				}
			};
			for (int i = 0; i < 32; i++)
			{
				GameObject go = new GameObject("OneShotSource_" + i.ToString())
				{
					transform = 
					{
						parent = gameObject.transform,
						localPosition = Vector3.zero
					}
				};
				this.sourcesWorld.Add(AudioSourceMaker.NewAudioSourceOn(go));
			}
		}

		public AudioSource GetSourceWorld()
		{
			foreach (AudioSource item in this.sourcesWorld)
			{
				if (!item.isPlaying)
				{
					SoundFilterUtility.DisableAllFiltersOn(item);
					return item;
				}
			}
			return null;
		}
	}
}
