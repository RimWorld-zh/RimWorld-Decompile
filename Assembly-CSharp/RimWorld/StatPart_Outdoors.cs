using Verse;

namespace RimWorld
{
	public class StatPart_Outdoors : StatPart
	{
		private float factorIndoors = 1f;

		private float factorOutdoors = 1f;

		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.OutdoorsFactor(req);
		}

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					string str = (!this.ConsideredOutdoors(req)) ? "Indoors".Translate() : "Outdoors".Translate();
					return str + ": x" + this.OutdoorsFactor(req).ToStringPercent();
				}
			}
			return null;
		}

		private float OutdoorsFactor(StatRequest req)
		{
			if (this.ConsideredOutdoors(req))
			{
				return this.factorOutdoors;
			}
			return this.factorIndoors;
		}

		private bool ConsideredOutdoors(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					if (room.OutdoorsForWork)
					{
						return true;
					}
					if (req.HasThing && req.Thing.Spawned && !req.Thing.Map.roofGrid.Roofed(req.Thing.Position))
					{
						return true;
					}
					return false;
				}
			}
			return false;
		}
	}
}
