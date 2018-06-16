using System;

namespace Verse
{
	// Token: 0x02000979 RID: 2425
	public class SubEffecter_DrifterEmoteChance : SubEffecter_DrifterEmote
	{
		// Token: 0x06003684 RID: 13956 RVA: 0x001D0FB7 File Offset: 0x001CF3B7
		public SubEffecter_DrifterEmoteChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x001D0FC4 File Offset: 0x001CF3C4
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
