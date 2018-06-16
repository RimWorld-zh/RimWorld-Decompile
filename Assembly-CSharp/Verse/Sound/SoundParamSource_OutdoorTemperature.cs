using System;

namespace Verse.Sound
{
	// Token: 0x02000B8B RID: 2955
	public class SoundParamSource_OutdoorTemperature : SoundParamSource
	{
		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06004031 RID: 16433 RVA: 0x0021C504 File Offset: 0x0021A904
		public override string Label
		{
			get
			{
				return "Outdoor temperature";
			}
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x0021C520 File Offset: 0x0021A920
		public override float ValueFor(Sample samp)
		{
			float result;
			if (Current.ProgramState != ProgramState.Playing)
			{
				result = 0f;
			}
			else if (Find.CurrentMap == null)
			{
				result = 0f;
			}
			else
			{
				result = Find.CurrentMap.mapTemperature.OutdoorTemp;
			}
			return result;
		}
	}
}
