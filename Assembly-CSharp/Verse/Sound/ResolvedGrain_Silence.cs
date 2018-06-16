using System;

namespace Verse.Sound
{
	// Token: 0x02000B7D RID: 2941
	public class ResolvedGrain_Silence : ResolvedGrain
	{
		// Token: 0x06004004 RID: 16388 RVA: 0x0021B3F4 File Offset: 0x002197F4
		public ResolvedGrain_Silence(AudioGrain_Silence sourceGrain)
		{
			this.sourceGrain = sourceGrain;
			this.duration = sourceGrain.durationRange.RandomInRange;
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x0021B418 File Offset: 0x00219818
		public override string ToString()
		{
			return "Silence";
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x0021B434 File Offset: 0x00219834
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

		// Token: 0x06004007 RID: 16391 RVA: 0x0021B478 File Offset: 0x00219878
		public override int GetHashCode()
		{
			return this.sourceGrain.GetHashCode();
		}

		// Token: 0x04002AE9 RID: 10985
		public AudioGrain_Silence sourceGrain;
	}
}
