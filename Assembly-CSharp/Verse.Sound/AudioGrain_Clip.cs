using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	public class AudioGrain_Clip : AudioGrain
	{
		public string clipPath = string.Empty;

		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			AudioClip clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			if ((Object)clip != (Object)null)
			{
				yield return (ResolvedGrain)new ResolvedGrain_Clip(clip);
			}
			else
			{
				Log.Error("Grain couldn't resolve: Clip not found at " + this.clipPath);
			}
		}
	}
}
