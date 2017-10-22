using Verse;

namespace RimWorld
{
	public class StatPart_WorkTableOutdoors : StatPart
	{
		public const float WorkRateFactor = 0.8f;

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				val *= 0.8f;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			return (!req.HasThing || !StatPart_WorkTableOutdoors.Applies(req.Thing)) ? null : ("Outdoors".Translate() + ": x" + 0.8f.ToStringPercent());
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
