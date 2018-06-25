using System;
using Verse;

namespace RimWorld
{
	public class StatPart_WorkTableOutdoors : StatPart
	{
		public const float WorkRateFactor = 0.9f;

		public StatPart_WorkTableOutdoors()
		{
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				val *= 0.9f;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				result = "Outdoors".Translate() + ": x" + 0.9f.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		public static bool Applies(Thing t)
		{
			return StatPart_WorkTableOutdoors.Applies(t.def, t.Map, t.Position);
		}

		public static bool Applies(ThingDef def, Map map, IntVec3 c)
		{
			bool result;
			if (def.building == null || !def.building.workSpeedPenaltyOutdoors)
			{
				result = false;
			}
			else if (map == null)
			{
				result = false;
			}
			else
			{
				Room room = c.GetRoom(map, RegionType.Set_All);
				result = (room != null && room.PsychologicallyOutdoors);
			}
			return result;
		}
	}
}
