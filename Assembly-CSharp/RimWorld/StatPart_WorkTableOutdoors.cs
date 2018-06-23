using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BA RID: 2490
	public class StatPart_WorkTableOutdoors : StatPart
	{
		// Token: 0x040023C0 RID: 9152
		public const float WorkRateFactor = 0.9f;

		// Token: 0x060037C2 RID: 14274 RVA: 0x001DADFA File Offset: 0x001D91FA
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				val *= 0.9f;
			}
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x001DAE24 File Offset: 0x001D9224
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

		// Token: 0x060037C4 RID: 14276 RVA: 0x001DAE7C File Offset: 0x001D927C
		public static bool Applies(Thing t)
		{
			return StatPart_WorkTableOutdoors.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x001DAEA8 File Offset: 0x001D92A8
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
