using System;

namespace Verse
{
	// Token: 0x02000D18 RID: 3352
	public class HediffComp_RecoveryThought : HediffComp
	{
		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x060049E4 RID: 18916 RVA: 0x0026A980 File Offset: 0x00268D80
		public HediffCompProperties_RecoveryThought Props
		{
			get
			{
				return (HediffCompProperties_RecoveryThought)this.props;
			}
		}

		// Token: 0x060049E5 RID: 18917 RVA: 0x0026A9A0 File Offset: 0x00268DA0
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
