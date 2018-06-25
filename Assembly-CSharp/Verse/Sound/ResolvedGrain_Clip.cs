using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B7A RID: 2938
	public class ResolvedGrain_Clip : ResolvedGrain
	{
		// Token: 0x04002AED RID: 10989
		public AudioClip clip;

		// Token: 0x06004007 RID: 16391 RVA: 0x0021BB78 File Offset: 0x00219F78
		public ResolvedGrain_Clip(AudioClip clip)
		{
			this.clip = clip;
			this.duration = clip.length;
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x0021BB94 File Offset: 0x00219F94
		public override string ToString()
		{
			return "Clip:" + this.clip.name;
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x0021BBC0 File Offset: 0x00219FC0
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

		// Token: 0x0600400A RID: 16394 RVA: 0x0021BC08 File Offset: 0x0021A008
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
