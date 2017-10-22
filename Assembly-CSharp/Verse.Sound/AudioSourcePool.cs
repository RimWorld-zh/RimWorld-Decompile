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
			return (!onCamera) ? this.sourcePoolWorld.GetSourceWorld() : this.sourcePoolCamera.GetSourceCamera();
		}
	}
}
