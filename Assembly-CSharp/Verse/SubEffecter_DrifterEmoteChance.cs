using System;

namespace Verse
{
	// Token: 0x02000975 RID: 2421
	public class SubEffecter_DrifterEmoteChance : SubEffecter_DrifterEmote
	{
		// Token: 0x0600367F RID: 13951 RVA: 0x001D1267 File Offset: 0x001CF667
		public SubEffecter_DrifterEmoteChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x001D1274 File Offset: 0x001CF674
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
