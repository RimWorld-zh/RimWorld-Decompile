using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B7 RID: 2487
	public class StatPart_RoomStat : StatPart
	{
		// Token: 0x040023BA RID: 9146
		private RoomStatDef roomStat = null;

		// Token: 0x040023BB RID: 9147
		[MustTranslate]
		private string customLabel = null;

		// Token: 0x040023BC RID: 9148
		[Unsaved]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel = null;

		// Token: 0x060037B3 RID: 14259 RVA: 0x001DAA58 File Offset: 0x001D8E58
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x001DAA68 File Offset: 0x001D8E68
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

		// Token: 0x060037B5 RID: 14261 RVA: 0x001DAAAC File Offset: 0x001D8EAC
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
