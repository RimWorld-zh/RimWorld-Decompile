using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D15 RID: 3349
	public class HediffComp_HealPermanentWounds : HediffComp
	{
		// Token: 0x04003212 RID: 12818
		private int ticksToHeal = 0;

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x060049D0 RID: 18896 RVA: 0x0026A69C File Offset: 0x00268A9C
		public HediffCompProperties_HealPermanentWounds Props
		{
			get
			{
				return (HediffCompProperties_HealPermanentWounds)this.props;
			}
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x0026A6BC File Offset: 0x00268ABC
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ResetTicksToHeal();
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x0026A6CB File Offset: 0x00268ACB
		private void ResetTicksToHeal()
		{
			this.ticksToHeal = Rand.Range(15, 30) * 60000;
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x0026A6E3 File Offset: 0x00268AE3
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToHeal--;
			if (this.ticksToHeal <= 0)
			{
				this.TryHealRandomPermanentWound();
				this.ResetTicksToHeal();
			}
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x0026A710 File Offset: 0x00268B10
		private void TryHealRandomPermanentWound()
		{
			Hediff hediff;
			if ((from hd in base.Pawn.health.hediffSet.hediffs
			where hd.IsPermanent()
			select hd).TryRandomElement(out hediff))
			{
				hediff.Severity = 0f;
				if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
				{
					Messages.Message("MessagePermanentWoundHealed".Translate(new object[]
					{
						this.parent.LabelCap,
						base.Pawn.LabelShort,
						hediff.Label
					}), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x0026A7CC File Offset: 0x00268BCC
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x0026A7E4 File Offset: 0x00268BE4
		public override string CompDebugString()
		{
			return "ticksToHeal: " + this.ticksToHeal;
		}
	}
}
