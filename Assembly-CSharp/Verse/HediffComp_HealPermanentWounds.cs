using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D12 RID: 3346
	public class HediffComp_HealPermanentWounds : HediffComp
	{
		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x060049CD RID: 18893 RVA: 0x0026A2E0 File Offset: 0x002686E0
		public HediffCompProperties_HealPermanentWounds Props
		{
			get
			{
				return (HediffCompProperties_HealPermanentWounds)this.props;
			}
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x0026A300 File Offset: 0x00268700
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ResetTicksToHeal();
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x0026A30F File Offset: 0x0026870F
		private void ResetTicksToHeal()
		{
			this.ticksToHeal = Rand.Range(15, 30) * 60000;
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x0026A327 File Offset: 0x00268727
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToHeal--;
			if (this.ticksToHeal <= 0)
			{
				this.TryHealRandomPermanentWound();
				this.ResetTicksToHeal();
			}
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x0026A354 File Offset: 0x00268754
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

		// Token: 0x060049D2 RID: 18898 RVA: 0x0026A410 File Offset: 0x00268810
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x0026A428 File Offset: 0x00268828
		public override string CompDebugString()
		{
			return "ticksToHeal: " + this.ticksToHeal;
		}

		// Token: 0x0400320B RID: 12811
		private int ticksToHeal = 0;
	}
}
