using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D15 RID: 3349
	public class HediffComp_HealPermanentWounds : HediffComp
	{
		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x060049BC RID: 18876 RVA: 0x00268EAC File Offset: 0x002672AC
		public HediffCompProperties_HealPermanentWounds Props
		{
			get
			{
				return (HediffCompProperties_HealPermanentWounds)this.props;
			}
		}

		// Token: 0x060049BD RID: 18877 RVA: 0x00268ECC File Offset: 0x002672CC
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ResetTicksToHeal();
		}

		// Token: 0x060049BE RID: 18878 RVA: 0x00268EDB File Offset: 0x002672DB
		private void ResetTicksToHeal()
		{
			this.ticksToHeal = Rand.Range(15, 30) * 60000;
		}

		// Token: 0x060049BF RID: 18879 RVA: 0x00268EF3 File Offset: 0x002672F3
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToHeal--;
			if (this.ticksToHeal <= 0)
			{
				this.TryHealRandomPermanentWound();
				this.ResetTicksToHeal();
			}
		}

		// Token: 0x060049C0 RID: 18880 RVA: 0x00268F20 File Offset: 0x00267320
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

		// Token: 0x060049C1 RID: 18881 RVA: 0x00268FDC File Offset: 0x002673DC
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		// Token: 0x060049C2 RID: 18882 RVA: 0x00268FF4 File Offset: 0x002673F4
		public override string CompDebugString()
		{
			return "ticksToHeal: " + this.ticksToHeal;
		}

		// Token: 0x04003200 RID: 12800
		private int ticksToHeal = 0;
	}
}
