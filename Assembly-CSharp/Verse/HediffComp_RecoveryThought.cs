using System;

namespace Verse
{
	// Token: 0x02000D19 RID: 3353
	public class HediffComp_RecoveryThought : HediffComp
	{
		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x060049E4 RID: 18916 RVA: 0x0026AC60 File Offset: 0x00269060
		public HediffCompProperties_RecoveryThought Props
		{
			get
			{
				return (HediffCompProperties_RecoveryThought)this.props;
			}
		}

		// Token: 0x060049E5 RID: 18917 RVA: 0x0026AC80 File Offset: 0x00269080
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
