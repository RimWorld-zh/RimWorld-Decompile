using Verse;

namespace RimWorld
{
	public class StatPart_RoomStat : StatPart
	{
		private RoomStatDef roomStat;

		private string customLabel;

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_Passable);
				if (room != null)
				{
					val *= room.GetStat(this.roomStat);
				}
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_Passable);
				if (room != null)
				{
					string str = this.customLabel.NullOrEmpty() ? this.roomStat.LabelCap : this.customLabel;
					return str + ": x" + room.GetStat(this.roomStat).ToStringPercent();
				}
			}
			return null;
		}
	}
}
