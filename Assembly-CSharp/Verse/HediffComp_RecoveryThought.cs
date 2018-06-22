using System;

namespace Verse
{
	// Token: 0x02000D16 RID: 3350
	public class HediffComp_RecoveryThought : HediffComp
	{
		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x060049E1 RID: 18913 RVA: 0x0026A8A4 File Offset: 0x00268CA4
		public HediffCompProperties_RecoveryThought Props
		{
			get
			{
				return (HediffCompProperties_RecoveryThought)this.props;
			}
		}

		// Token: 0x060049E2 RID: 18914 RVA: 0x0026A8C4 File Offset: 0x00268CC4
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
