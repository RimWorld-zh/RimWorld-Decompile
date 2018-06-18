using System;

namespace Verse
{
	// Token: 0x02000D19 RID: 3353
	public class HediffComp_RecoveryThought : HediffComp
	{
		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x060049D0 RID: 18896 RVA: 0x00269470 File Offset: 0x00267870
		public HediffCompProperties_RecoveryThought Props
		{
			get
			{
				return (HediffCompProperties_RecoveryThought)this.props;
			}
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x00269490 File Offset: 0x00267890
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (!base.Pawn.Dead && base.Pawn.needs.mood != null)
			{
				base.Pawn.needs.mood.thoughts.memories.TryGainMemory(this.Props.thought, null);
			}
		}
	}
}
