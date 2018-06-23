using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B78 RID: 2936
	public class ResolvedGrain_Clip : ResolvedGrain
	{
		// Token: 0x04002AED RID: 10989
		public AudioClip clip;

		// Token: 0x06004004 RID: 16388 RVA: 0x0021BA9C File Offset: 0x00219E9C
		public ResolvedGrain_Clip(AudioClip clip)
		{
			this.clip = clip;
			this.duration = clip.length;
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x0021BAB8 File Offset: 0x00219EB8
		public override string ToString()
		{
			return "Clip:" + this.clip.name;
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x0021BAE4 File Offset: 0x00219EE4
		public override bool Equals(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = false;
			}
			else
			{
				ResolvedGrain_Clip resolvedGrain_Clip = obj as ResolvedGrain_Clip;
				result = (resolvedGrain_Clip != null && resolvedGrain_Clip.clip == this.clip);
			}
			return result;
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x0021BB2C File Offset: 0x00219F2C
		public override int GetHashCode()
		{
			int result;
			if (this.clip == null)
			{
				result = 0;
			}
			else
			{
				result = this.clip.GetHashCode();
			}
			return result;
		}
	}
}
