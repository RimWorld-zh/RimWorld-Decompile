using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000277 RID: 631
	public class SoundParamSource_TimeOfDay : SoundParamSource
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x00062344 File Offset: 0x00060744
		public override string Label
		{
			get
			{
				return "Time of day (hour)";
			}
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00062360 File Offset: 0x00060760
		public override float ValueFor(Sample samp)
		{
			float result;
			if (Find.CurrentMap == null)
			{
				result = 0f;
			}
			else
			{
				result = GenLocalDate.HourFloat(Find.CurrentMap);
			}
			return result;
		}
	}
}
