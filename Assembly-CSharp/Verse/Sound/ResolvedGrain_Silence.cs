using System;

namespace Verse.Sound
{
	// Token: 0x02000B7D RID: 2941
	public class ResolvedGrain_Silence : ResolvedGrain
	{
		// Token: 0x06004006 RID: 16390 RVA: 0x0021B4C8 File Offset: 0x002198C8
		public ResolvedGrain_Silence(AudioGrain_Silence sourceGrain)
		{
			this.sourceGrain = sourceGrain;
			this.duration = sourceGrain.durationRange.RandomInRange;
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x0021B4EC File Offset: 0x002198EC
		public override string ToString()
		{
			return "Silence";
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x0021B508 File Offset: 0x00219908
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

		// Token: 0x06004009 RID: 16393 RVA: 0x0021B54C File Offset: 0x0021994C
		public override int GetHashCode()
		{
			return this.sourceGrain.GetHashCode();
		}

		// Token: 0x04002AE9 RID: 10985
		public AudioGrain_Silence sourceGrain;
	}
}
