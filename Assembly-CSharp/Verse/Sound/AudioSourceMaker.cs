using System;
using UnityEngine;

namespace Verse.Sound
{
	public static class AudioSourceMaker
	{
		private const AudioRolloffMode WorldRolloffMode = AudioRolloffMode.Linear;

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
