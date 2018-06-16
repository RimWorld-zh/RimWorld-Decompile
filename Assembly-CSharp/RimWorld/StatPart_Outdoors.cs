using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B4 RID: 2484
	public class StatPart_Outdoors : StatPart
	{
		// Token: 0x0600379F RID: 14239 RVA: 0x001D9F73 File Offset: 0x001D8373
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.OutdoorsFactor(req);
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x001D9F84 File Offset: 0x001D8384
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					string str;
					if (this.ConsideredOutdoors(req))
					{
						str = "Outdoors".Translate();
					}
					else
					{
						str = "Indoors".Translate();
					}
					return str + ": x" + this.OutdoorsFactor(req).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x060037A1 RID: 14241 RVA: 0x001DA004 File Offset: 0x001D8404
		private float OutdoorsFactor(StatRequest req)
		{
			float result;
			if (this.ConsideredOutdoors(req))
			{
				result = this.factorOutdoors;
			}
			else
			{
				result = this.factorIndoors;
			}
			return result;
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x001DA038 File Offset: 0x001D8438
		private bool ConsideredOutdoors(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					return room.OutdoorsForWork || (req.HasThing && req.Thing.Spawned && !req.Thing.Map.roofGrid.Roofed(req.Thing.Position));
				}
			}
			return false;
		}

		// Token: 0x040023B0 RID: 9136
		private float factorIndoors = 1f;

		// Token: 0x040023B1 RID: 9137
		private float factorOutdoors = 1f;
	}
}
