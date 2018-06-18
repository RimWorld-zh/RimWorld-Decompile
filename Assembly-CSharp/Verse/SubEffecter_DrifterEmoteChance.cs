using System;

namespace Verse
{
	// Token: 0x02000979 RID: 2425
	public class SubEffecter_DrifterEmoteChance : SubEffecter_DrifterEmote
	{
		// Token: 0x06003686 RID: 13958 RVA: 0x001D107F File Offset: 0x001CF47F
		public SubEffecter_DrifterEmoteChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003687 RID: 13959 RVA: 0x001D108C File Offset: 0x001CF48C
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			float chancePerTick = this.def.chancePerTick;
			if (Rand.Value < chancePerTick)
			{
				base.MakeMote(A);
			}
		}
	}
}
