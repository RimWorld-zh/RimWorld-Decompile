using Verse;

namespace RimWorld
{
	public class StatPart_RoomStat : StatPart
	{
		private RoomStatDef roomStat = null;

		private string customLabel = (string)null;

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
			string result;
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_Passable);
				if (room != null)
				{
					string str = this.customLabel.NullOrEmpty() ? this.roomStat.LabelCap : this.customLabel;
					result = str + ": x" + room.GetStat(this.roomStat).ToStringPercent();
					goto IL_0075;
				}
			}
			result = (string)null;
			goto IL_0075;
			IL_0075:
			return result;
		}
	}
}
