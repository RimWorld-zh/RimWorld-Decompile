using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000275 RID: 629
	public class SoundParamSource_TimeOfDay : SoundParamSource
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x000621F4 File Offset: 0x000605F4
		public override string Label
		{
			get
			{
				return "Time of day (hour)";
			}
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00062210 File Offset: 0x00060610
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
