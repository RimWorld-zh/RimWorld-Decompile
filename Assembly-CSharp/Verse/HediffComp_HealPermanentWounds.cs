using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	public class HediffComp_HealPermanentWounds : HediffComp
	{
		private int ticksToHeal;

		[CompilerGenerated]
		private static Func<Hediff, bool> <>f__mg$cache0;

		public HediffComp_HealPermanentWounds()
		{
		}

		public HediffCompProperties_HealPermanentWounds Props
		{
			get
			{
				return (HediffCompProperties_HealPermanentWounds)this.props;
			}
		}

		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ResetTicksToHeal();
		}

		private void ResetTicksToHeal()
		{
			this.ticksToHeal = Rand.Range(15, 30) * 60000;
		}

		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToHeal--;
			if (this.ticksToHeal <= 0)
			{
				this.TryHealRandomPermanentWound();
				this.ResetTicksToHeal();
			}
		}

		private void TryHealRandomPermanentWound()
		{
			IEnumerable<Hediff> hediffs = base.Pawn.health.hediffSet.hediffs;
			if (HediffComp_HealPermanentWounds.<>f__mg$cache0 == null)
			{
				HediffComp_HealPermanentWounds.<>f__mg$cache0 = new Func<Hediff, bool>(HediffUtility.IsPermanent);
			}
			Hediff hediff;
			if (!hediffs.Where(HediffComp_HealPermanentWounds.<>f__mg$cache0).TryRandomElement(out hediff))
			{
				return;
			}
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

		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		public override string CompDebugString()
		{
			return "ticksToHeal: " + this.ticksToHeal;
		}
	}
}
