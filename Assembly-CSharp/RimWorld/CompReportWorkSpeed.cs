using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000730 RID: 1840
	public class CompReportWorkSpeed : ThingComp
	{
		// Token: 0x06002895 RID: 10389 RVA: 0x0015A888 File Offset: 0x00158C88
		public override string CompInspectStringExtra()
		{
			bool flag = StatPart_WorkTableOutdoors.Applies(this.parent.def, this.parent.Map, this.parent.Position);
			bool flag2 = StatPart_WorkTableTemperature.Applies(this.parent);
			bool flag3 = StatPart_WorkTableUnpowered.Applies(this.parent);
			string result;
			if (flag || flag2 || flag3)
			{
				string text = "WorkSpeedPenalty".Translate() + ": ";
				string text2 = "";
				if (flag)
				{
					text2 += "Outdoors".Translate().ToLower();
				}
				if (flag2)
				{
					if (!text2.NullOrEmpty())
					{
						text2 += ", ";
					}
					text2 += "BadTemperature".Translate().ToLower();
				}
				if (flag3)
				{
					if (!text2.NullOrEmpty())
					{
						text2 += ", ";
					}
					text2 += "NoPower".Translate().ToLower();
				}
				text += text2.CapitalizeFirst();
				result = text;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
