using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B2 RID: 2482
	public class StatPart_Outdoors : StatPart
	{
		// Token: 0x040023AB RID: 9131
		private float factorIndoors = 1f;

		// Token: 0x040023AC RID: 9132
		private float factorOutdoors = 1f;

		// Token: 0x0600379E RID: 14238 RVA: 0x001DA34B File Offset: 0x001D874B
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.OutdoorsFactor(req);
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x001DA35C File Offset: 0x001D875C
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

		// Token: 0x060037A0 RID: 14240 RVA: 0x001DA3DC File Offset: 0x001D87DC
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

		// Token: 0x060037A1 RID: 14241 RVA: 0x001DA410 File Offset: 0x001D8810
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
