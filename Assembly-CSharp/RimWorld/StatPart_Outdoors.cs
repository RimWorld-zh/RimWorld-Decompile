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
			string result;
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					string str = (!this.ConsideredOutdoors(req)) ? "Indoors".Translate() : "Outdoors".Translate();
					result = str + ": x" + this.OutdoorsFactor(req).ToStringPercent();
					goto IL_006f;
				}
			}
			result = (string)null;
			goto IL_006f;
			IL_006f:
			return result;
		}

		private float OutdoorsFactor(StatRequest req)
		{
			return (!this.ConsideredOutdoors(req)) ? this.factorIndoors : this.factorOutdoors;
		}

		private bool ConsideredOutdoors(StatRequest req)
		{
			bool result;
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					result = ((byte)(room.OutdoorsForWork ? 1 : ((req.HasThing && req.Thing.Spawned && !req.Thing.Map.roofGrid.Roofed(req.Thing.Position)) ? 1 : 0)) != 0);
					goto IL_008f;
				}
			}
			result = false;
			goto IL_008f;
			IL_008f:
			return result;
		}
	}
}
