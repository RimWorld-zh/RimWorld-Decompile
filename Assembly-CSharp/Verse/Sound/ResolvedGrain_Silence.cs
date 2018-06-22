using System;

namespace Verse.Sound
{
	// Token: 0x02000B79 RID: 2937
	public class ResolvedGrain_Silence : ResolvedGrain
	{
		// Token: 0x06004008 RID: 16392 RVA: 0x0021BB64 File Offset: 0x00219F64
		public ResolvedGrain_Silence(AudioGrain_Silence sourceGrain)
		{
			this.sourceGrain = sourceGrain;
			this.duration = sourceGrain.durationRange.RandomInRange;
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x0021BB88 File Offset: 0x00219F88
		public override string ToString()
		{
			return "Silence";
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x0021BBA4 File Offset: 0x00219FA4
		public override bool Equals(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = false;
			}
			else
			{
				ResolvedGrain_Silence resolvedGrain_Silence = obj as ResolvedGrain_Silence;
				result = (resolvedGrain_Silence != null && resolvedGrain_Silence.sourceGrain == this.sourceGrain);
			}
			return result;
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x0021BBE8 File Offset: 0x00219FE8
		public override int GetHashCode()
		{
			return this.sourceGrain.GetHashCode();
		}

		// Token: 0x04002AEE RID: 10990
		public AudioGrain_Silence sourceGrain;
	}
}
