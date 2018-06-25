using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B76 RID: 2934
	public class AudioGrain_Clip : AudioGrain
	{
		// Token: 0x04002AE9 RID: 10985
		public string clipPath = "";

		// Token: 0x06004000 RID: 16384 RVA: 0x0021B654 File Offset: 0x00219A54
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
	}
}
