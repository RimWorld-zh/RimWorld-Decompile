using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BE RID: 2494
	public class StatPart_WorkTableOutdoors : StatPart
	{
		// Token: 0x060037C8 RID: 14280 RVA: 0x001DAC22 File Offset: 0x001D9022
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				val *= 0.8f;
			}
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x001DAC4C File Offset: 0x001D904C
		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				result = "Outdoors".Translate() + ": x" + 0.8f.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x001DACA4 File Offset: 0x001D90A4
		public static bool Applies(Thing t)
		{
			return StatPart_WorkTableOutdoors.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x001DACD0 File Offset: 0x001D90D0
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

		// Token: 0x040023C5 RID: 9157
		public const float WorkRateFactor = 0.8f;
	}
}
