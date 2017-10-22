using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	public class AudioSourcePoolWorld
	{
		private const int NumSourcesWorld = 32;

		private List<AudioSource> sourcesWorld = new List<AudioSource>();

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
			List<AudioSource>.Enumerator enumerator = this.sourcesWorld.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					AudioSource current = enumerator.Current;
					if (!current.isPlaying)
					{
						SoundFilterUtility.DisableAllFiltersOn(current);
						return current;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return null;
		}
	}
}
