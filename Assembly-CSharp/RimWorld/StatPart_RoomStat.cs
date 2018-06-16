using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B9 RID: 2489
	public class StatPart_RoomStat : StatPart
	{
		// Token: 0x060037B4 RID: 14260 RVA: 0x001DA67C File Offset: 0x001D8A7C
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

		// Token: 0x060037B5 RID: 14261 RVA: 0x001DA6C0 File Offset: 0x001D8AC0
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
