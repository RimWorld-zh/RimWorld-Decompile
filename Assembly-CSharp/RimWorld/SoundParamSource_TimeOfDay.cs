using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000275 RID: 629
	public class SoundParamSource_TimeOfDay : SoundParamSource
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x00062198 File Offset: 0x00060598
		public override string Label
		{
			get
			{
				return "Time of day (hour)";
			}
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x000621B4 File Offset: 0x000605B4
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
