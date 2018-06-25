using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C67 RID: 3175
	public class PlaceWorker_ReportWorkSpeedPenalties : PlaceWorker
	{
		// Token: 0x060045D0 RID: 17872 RVA: 0x0024D938 File Offset: 0x0024BD38
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
