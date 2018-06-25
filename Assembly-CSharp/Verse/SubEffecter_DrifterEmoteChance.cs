using System;

namespace Verse
{
	// Token: 0x02000977 RID: 2423
	public class SubEffecter_DrifterEmoteChance : SubEffecter_DrifterEmote
	{
		// Token: 0x06003683 RID: 13955 RVA: 0x001D13A7 File Offset: 0x001CF7A7
		public SubEffecter_DrifterEmoteChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x001D13B4 File Offset: 0x001CF7B4
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
