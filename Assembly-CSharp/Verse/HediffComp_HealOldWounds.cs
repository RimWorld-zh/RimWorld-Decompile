using RimWorld;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Verse
{
	public class HediffComp_HealOldWounds : HediffComp
	{
		private int ticksToHeal;

		[CompilerGenerated]
		private static Func<Hediff, bool> _003C_003Ef__mg_0024cache0;

		public HediffCompProperties_HealOldWounds Props
		{
			get
			{
				return (HediffCompProperties_HealOldWounds)base.props;
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
				this.TryHealRandomOldWound();
				this.ResetTicksToHeal();
			}
		}

		private void TryHealRandomOldWound()
		{
			Hediff hediff = default(Hediff);
			if (base.Pawn.health.hediffSet.hediffs.Where(HediffUtility.IsOld).TryRandomElement<Hediff>(out hediff))
			{
				hediff.Severity = 0f;
				if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
				{
					Messages.Message("MessageOldWoundHealed".Translate(base.parent.Label, base.Pawn.LabelShort, hediff.Label), MessageTypeDefOf.PositiveEvent);
				}
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
