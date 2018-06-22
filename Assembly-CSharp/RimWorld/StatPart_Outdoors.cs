using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B0 RID: 2480
	public class StatPart_Outdoors : StatPart
	{
		// Token: 0x0600379A RID: 14234 RVA: 0x001DA20B File Offset: 0x001D860B
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.OutdoorsFactor(req);
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x001DA21C File Offset: 0x001D861C
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

		// Token: 0x0600379C RID: 14236 RVA: 0x001DA29C File Offset: 0x001D869C
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

		// Token: 0x0600379D RID: 14237 RVA: 0x001DA2D0 File Offset: 0x001D86D0
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

		// Token: 0x040023AA RID: 9130
		private float factorIndoors = 1f;

		// Token: 0x040023AB RID: 9131
		private float factorOutdoors = 1f;
	}
}
