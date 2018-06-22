using System;

namespace Verse.Sound
{
	// Token: 0x02000B87 RID: 2951
	public class SoundParamSource_OutdoorTemperature : SoundParamSource
	{
		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06004035 RID: 16437 RVA: 0x0021CC74 File Offset: 0x0021B074
		public override string Label
		{
			get
			{
				return "Outdoor temperature";
			}
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x0021CC90 File Offset: 0x0021B090
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
