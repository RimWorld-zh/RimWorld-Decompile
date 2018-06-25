using System;

namespace Verse.Sound
{
	// Token: 0x02000B7B RID: 2939
	public class ResolvedGrain_Silence : ResolvedGrain
	{
		// Token: 0x04002AEE RID: 10990
		public AudioGrain_Silence sourceGrain;

		// Token: 0x0600400B RID: 16395 RVA: 0x0021BC40 File Offset: 0x0021A040
		public ResolvedGrain_Silence(AudioGrain_Silence sourceGrain)
		{
			this.sourceGrain = sourceGrain;
			this.duration = sourceGrain.durationRange.RandomInRange;
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x0021BC64 File Offset: 0x0021A064
		public override string ToString()
		{
			return "Silence";
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x0021BC80 File Offset: 0x0021A080
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

		// Token: 0x0600400E RID: 16398 RVA: 0x0021BCC4 File Offset: 0x0021A0C4
		public override int GetHashCode()
		{
			return this.sourceGrain.GetHashCode();
		}
	}
}
