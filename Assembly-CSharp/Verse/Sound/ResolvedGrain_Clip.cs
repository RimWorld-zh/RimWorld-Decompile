using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000B7C RID: 2940
	public class ResolvedGrain_Clip : ResolvedGrain
	{
		// Token: 0x06004002 RID: 16386 RVA: 0x0021B400 File Offset: 0x00219800
		public ResolvedGrain_Clip(AudioClip clip)
		{
			this.clip = clip;
			this.duration = clip.length;
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x0021B41C File Offset: 0x0021981C
		public override string ToString()
		{
			return "Clip:" + this.clip.name;
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x0021B448 File Offset: 0x00219848
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

		// Token: 0x06004005 RID: 16389 RVA: 0x0021B490 File Offset: 0x00219890
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

		// Token: 0x04002AE8 RID: 10984
		public AudioClip clip;
	}
}
