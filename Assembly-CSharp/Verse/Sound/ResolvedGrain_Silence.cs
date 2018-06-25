using System;

namespace Verse.Sound
{
	// Token: 0x02000B7C RID: 2940
	public class ResolvedGrain_Silence : ResolvedGrain
	{
		// Token: 0x04002AF5 RID: 10997
		public AudioGrain_Silence sourceGrain;

		// Token: 0x0600400B RID: 16395 RVA: 0x0021BF20 File Offset: 0x0021A320
		public ResolvedGrain_Silence(AudioGrain_Silence sourceGrain)
		{
			this.sourceGrain = sourceGrain;
			this.duration = sourceGrain.durationRange.RandomInRange;
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x0021BF44 File Offset: 0x0021A344
		public override string ToString()
		{
			return "Silence";
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x0021BF60 File Offset: 0x0021A360
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

		// Token: 0x0600400E RID: 16398 RVA: 0x0021BFA4 File Offset: 0x0021A3A4
		public override int GetHashCode()
		{
			return this.sourceGrain.GetHashCode();
		}
	}
}
