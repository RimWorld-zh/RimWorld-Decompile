using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B77 RID: 2935
	public class AudioGrain_Clip : AudioGrain
	{
		// Token: 0x04002AF0 RID: 10992
		public string clipPath = "";

		// Token: 0x06004000 RID: 16384 RVA: 0x0021B934 File Offset: 0x00219D34
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
