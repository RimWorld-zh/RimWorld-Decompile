using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B9 RID: 2489
	public class StatPart_RoomStat : StatPart
	{
		// Token: 0x060037B6 RID: 14262 RVA: 0x001DA750 File Offset: 0x001D8B50
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

		// Token: 0x060037B7 RID: 14263 RVA: 0x001DA794 File Offset: 0x001D8B94
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_Passable);
				if (room != null)
				{
					string labelCap;
					if (!this.customLabel.NullOrEmpty())
					{
						labelCap = this.customLabel;
					}
					else
					{
						labelCap = this.roomStat.LabelCap;
					}
					return labelCap + ": x" + room.GetStat(this.roomStat).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x040023BF RID: 9151
		private RoomStatDef roomStat = null;

		// Token: 0x040023C0 RID: 9152
		[MustTranslate]
		private string customLabel = null;
	}
}
