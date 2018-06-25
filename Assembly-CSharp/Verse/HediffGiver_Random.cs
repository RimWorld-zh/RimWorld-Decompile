using System;

namespace Verse
{
	// Token: 0x02000D38 RID: 3384
	public class HediffGiver_Random : HediffGiver
	{
		// Token: 0x0400325E RID: 12894
		public float mtbDays = 0f;

		// Token: 0x06004A90 RID: 19088 RVA: 0x0026E68E File Offset: 0x0026CA8E
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (Rand.MTBEventOccurs(this.mtbDays, 60000f, 60f))
			{
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
			}
		}
	}
}
