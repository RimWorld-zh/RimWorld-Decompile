using Verse;

namespace RimWorld
{
	public class StatPart_WorkTableTemperature : StatPart
	{
		public const float WorkRateFactor = 0.6f;

		public const float MinTemp = 5f;

		public const float MaxTemp = 35f;

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				val *= 0.6f;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			return (!req.HasThing || !StatPart_WorkTableTemperature.Applies(req.Thing)) ? null : ("BadTemperature".Translate().CapitalizeFirst() + ": x" + 0.6f.ToStringPercent());
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
				result = (temperatureForCell < 5.0 || temperatureForCell > 35.0);
			}
			return result;
		}
	}
}
