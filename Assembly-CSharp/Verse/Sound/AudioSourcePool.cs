using System;
using UnityEngine;

namespace Verse.Sound
{
	public class AudioSourcePool
	{
		public AudioSourcePoolCamera sourcePoolCamera;

		public AudioSourcePoolWorld sourcePoolWorld;

		public AudioSourcePool()
		{
			this.sourcePoolCamera = new AudioSourcePoolCamera();
			this.sourcePoolWorld = new AudioSourcePoolWorld();
		}

		public AudioSource GetSource(bool onCamera)
		{
			AudioSource result;
			if (onCamera)
			{
				result = this.sourcePoolCamera.GetSourceCamera();
			}
			else
			{
				result = this.sourcePoolWorld.GetSourceWorld();
			}
			return result;
		}
	}
}
