using System;
using Verse;

namespace RimWorld
{
	public class StatPart_WorkTableTemperature : StatPart
	{
		public const float WorkRateFactor = 0.7f;

		public const float MinTemp = 9f;

		public const float MaxTemp = 35f;

		public StatPart_WorkTableTemperature()
		{
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				val *= 0.7f;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				result = "BadTemperature".Translate().CapitalizeFirst() + ": x" + 0.7f.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static bool Applies(Thing t)
		{
			return t.Spawned && StatPart_WorkTableTemperature.Applies(t.def, t.Map, t.Position);
		}

		public static bool Applies(ThingDef tDef, Map map, IntVec3 c)
		{
			bool result;
			if (map == null)
			{
				result = false;
			}
			else if (tDef.building == null || !tDef.building.workSpeedPenaltyTemperature)
			{
				result = false;
			}
			else
			{
				float temperatureForCell = GenTemperature.GetTemperatureForCell(c, map);
				result = (temperatureForCell < 9f || temperatureForCell > 35f);
			}
			return result;
		}
	}
}
