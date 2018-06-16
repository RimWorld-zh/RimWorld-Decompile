using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B78 RID: 2936
	public class AudioGrain_Clip : AudioGrain
	{
		// Token: 0x06003FF9 RID: 16377 RVA: 0x0021AE08 File Offset: 0x00219208
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			AudioClip clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			if (clip != null)
			{
				yield return new ResolvedGrain_Clip(clip);
			}
			else
			{
				Log.Error("Grain couldn't resolve: Clip not found at " + this.clipPath, false);
			}
			yield break;
		}

		// Token: 0x04002AE4 RID: 10980
		public string clipPath = "";
	}
}
