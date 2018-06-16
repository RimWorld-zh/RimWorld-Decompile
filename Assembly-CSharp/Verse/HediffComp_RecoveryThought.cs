using System;

namespace Verse
{
	// Token: 0x02000D1A RID: 3354
	public class HediffComp_RecoveryThought : HediffComp
	{
		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x060049D2 RID: 18898 RVA: 0x00269498 File Offset: 0x00267898
		public HediffCompProperties_RecoveryThought Props
		{
			get
			{
				return (HediffCompProperties_RecoveryThought)this.props;
			}
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x002694B8 File Offset: 0x002678B8
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
