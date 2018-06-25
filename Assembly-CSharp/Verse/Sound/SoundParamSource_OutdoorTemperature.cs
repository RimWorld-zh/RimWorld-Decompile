using System;

namespace Verse.Sound
{
	// Token: 0x02000B8A RID: 2954
	public class SoundParamSource_OutdoorTemperature : SoundParamSource
	{
		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06004038 RID: 16440 RVA: 0x0021D030 File Offset: 0x0021B430
		public override string Label
		{
			get
			{
				return "Outdoor temperature";
			}
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x0021D04C File Offset: 0x0021B44C
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
