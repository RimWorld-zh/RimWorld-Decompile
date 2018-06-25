using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B7 RID: 2487
	public class StatPart_RoomStat : StatPart
	{
		// Token: 0x040023C1 RID: 9153
		private RoomStatDef roomStat = null;

		// Token: 0x040023C2 RID: 9154
		[MustTranslate]
		private string customLabel = null;

		// Token: 0x040023C3 RID: 9155
		[Unsaved]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel = null;

		// Token: 0x060037B3 RID: 14259 RVA: 0x001DAD2C File Offset: 0x001D912C
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x001DAD3C File Offset: 0x001D913C
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

		// Token: 0x060037B5 RID: 14261 RVA: 0x001DAD80 File Offset: 0x001D9180
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
	}
}
