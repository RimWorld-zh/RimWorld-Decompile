using System;

namespace Verse.Sound
{
	// Token: 0x02000B8B RID: 2955
	public class SoundParamSource_OutdoorTemperature : SoundParamSource
	{
		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06004033 RID: 16435 RVA: 0x0021C5D8 File Offset: 0x0021A9D8
		public override string Label
		{
			get
			{
				return "Outdoor temperature";
			}
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x0021C5F4 File Offset: 0x0021A9F4
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
