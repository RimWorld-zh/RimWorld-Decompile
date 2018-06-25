using System;

namespace Verse.Sound
{
	// Token: 0x02000B89 RID: 2953
	public class SoundParamSource_OutdoorTemperature : SoundParamSource
	{
		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06004038 RID: 16440 RVA: 0x0021CD50 File Offset: 0x0021B150
		public override string Label
		{
			get
			{
				return "Outdoor temperature";
			}
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x0021CD6C File Offset: 0x0021B16C
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
