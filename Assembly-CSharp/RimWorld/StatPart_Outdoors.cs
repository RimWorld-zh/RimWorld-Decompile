using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B2 RID: 2482
	public class StatPart_Outdoors : StatPart
	{
		// Token: 0x040023B2 RID: 9138
		private float factorIndoors = 1f;

		// Token: 0x040023B3 RID: 9139
		private float factorOutdoors = 1f;

		// Token: 0x0600379E RID: 14238 RVA: 0x001DA61F File Offset: 0x001D8A1F
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.OutdoorsFactor(req);
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x001DA630 File Offset: 0x001D8A30
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

		// Token: 0x060037A0 RID: 14240 RVA: 0x001DA6B0 File Offset: 0x001D8AB0
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

		// Token: 0x060037A1 RID: 14241 RVA: 0x001DA6E4 File Offset: 0x001D8AE4
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
	}
}
