using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BC RID: 2492
	public class StatPart_WorkTableOutdoors : StatPart
	{
		// Token: 0x040023C1 RID: 9153
		public const float WorkRateFactor = 0.9f;

		// Token: 0x060037C6 RID: 14278 RVA: 0x001DAF3A File Offset: 0x001D933A
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				val *= 0.9f;
			}
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x001DAF64 File Offset: 0x001D9364
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

		// Token: 0x060037C8 RID: 14280 RVA: 0x001DAFBC File Offset: 0x001D93BC
		public static bool Applies(Thing t)
		{
			return StatPart_WorkTableOutdoors.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x001DAFE8 File Offset: 0x001D93E8
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
