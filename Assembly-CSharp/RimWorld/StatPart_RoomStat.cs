using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B5 RID: 2485
	public class StatPart_RoomStat : StatPart
	{
		// Token: 0x060037AF RID: 14255 RVA: 0x001DA918 File Offset: 0x001D8D18
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x001DA928 File Offset: 0x001D8D28
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

		// Token: 0x060037B1 RID: 14257 RVA: 0x001DA96C File Offset: 0x001D8D6C
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

		// Token: 0x040023B9 RID: 9145
		private RoomStatDef roomStat = null;

		// Token: 0x040023BA RID: 9146
		[MustTranslate]
		private string customLabel = null;

		// Token: 0x040023BB RID: 9147
		[Unsaved]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel = null;
	}
}
