using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D16 RID: 3350
	public class HediffComp_HealPermanentWounds : HediffComp
	{
		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x060049BE RID: 18878 RVA: 0x00268ED4 File Offset: 0x002672D4
		public HediffCompProperties_HealPermanentWounds Props
		{
			get
			{
				return (HediffCompProperties_HealPermanentWounds)this.props;
			}
		}

		// Token: 0x060049BF RID: 18879 RVA: 0x00268EF4 File Offset: 0x002672F4
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ResetTicksToHeal();
		}

		// Token: 0x060049C0 RID: 18880 RVA: 0x00268F03 File Offset: 0x00267303
		private void ResetTicksToHeal()
		{
			this.ticksToHeal = Rand.Range(15, 30) * 60000;
		}

		// Token: 0x060049C1 RID: 18881 RVA: 0x00268F1B File Offset: 0x0026731B
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToHeal--;
			if (this.ticksToHeal <= 0)
			{
				this.TryHealRandomPermanentWound();
				this.ResetTicksToHeal();
			}
		}

		// Token: 0x060049C2 RID: 18882 RVA: 0x00268F48 File Offset: 0x00267348
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

		// Token: 0x060049C3 RID: 18883 RVA: 0x00269004 File Offset: 0x00267404
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		// Token: 0x060049C4 RID: 18884 RVA: 0x0026901C File Offset: 0x0026741C
		public override string CompDebugString()
		{
			return "ticksToHeal: " + this.ticksToHeal;
		}

		// Token: 0x04003202 RID: 12802
		private int ticksToHeal = 0;
	}
}
