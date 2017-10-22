using Verse;

namespace RimWorld
{
	public class PlaceWorker_ReportWorkSpeedPenalties : PlaceWorker
	{
		public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				bool flag = StatPart_WorkTableOutdoors.Applies(thingDef, map, loc);
				bool flag2 = StatPart_WorkTableTemperature.Applies(thingDef, map, loc);
				if (!flag && !flag2)
					return;
				string str = "WillGetWorkSpeedPenalty".Translate(def.label).CapitalizeFirst() + ": ";
				if (flag)
				{
					str += "Outdoors".Translate().ToLower();
				}
				if (flag2)
				{
					if (flag)
					{
						str += ", ";
					}
					str += "BadTemperature".Translate().ToLower();
				}
				str += ".";
				Messages.Message(str, MessageSound.Negative);
			}
		}
	}
}
