using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C69 RID: 3177
	public class PlaceWorker_ReportWorkSpeedPenalties : PlaceWorker
	{
		// Token: 0x060045C6 RID: 17862 RVA: 0x0024C4B4 File Offset: 0x0024A8B4
		public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				bool flag = StatPart_WorkTableOutdoors.Applies(thingDef, map, loc);
				bool flag2 = StatPart_WorkTableTemperature.Applies(thingDef, map, loc);
				if (flag || flag2)
				{
					string text = "WillGetWorkSpeedPenalty".Translate(new object[]
					{
						def.label
					}).CapitalizeFirst() + ": ";
					string text2 = "";
					if (flag)
					{
						text2 += "Outdoors".Translate();
					}
					if (flag2)
					{
						if (!text2.NullOrEmpty())
						{
							text2 += ", ";
						}
						text2 += "BadTemperature".Translate();
					}
					text += text2.CapitalizeFirst();
					text += ".";
					Messages.Message(text, new TargetInfo(loc, map, false), MessageTypeDefOf.CautionInput, false);
				}
			}
		}
	}
}
