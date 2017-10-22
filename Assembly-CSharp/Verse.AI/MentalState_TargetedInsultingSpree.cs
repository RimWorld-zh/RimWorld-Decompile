using RimWorld;
using System.Collections.Generic;

namespace Verse.AI
{
	public class MentalState_TargetedInsultingSpree : MentalState_InsultingSpree
	{
		private static List<Pawn> candidates = new List<Pawn>();

		public override string InspectLine
		{
			get
			{
				return string.Format(base.def.baseInspectLine, base.target.LabelShort);
			}
		}

		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return base.insultedTargetAtLeastOnce;
			}
		}

		public override void MentalStateTick()
		{
			if (base.target != null && (!base.target.Spawned || !base.pawn.CanReach((Thing)base.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn)))
			{
				Pawn target = base.target;
				if (!this.TryFindNewTarget())
				{
					base.RecoverFromState();
				}
				else
				{
					Messages.Message("MessageTargetedInsultingSpreeChangedTarget".Translate(base.pawn.LabelShort, target.Label, base.target.Label).AdjustedFor(base.pawn), (Thing)base.pawn, MessageTypeDefOf.NegativeEvent);
					base.MentalStateTick();
				}
			}
			else if (base.target == null || !InsultingSpreeMentalStateUtility.CanChaseAndInsult(base.pawn, base.target, false, false))
			{
				base.RecoverFromState();
			}
			else
			{
				base.MentalStateTick();
			}
		}

		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.TryFindNewTarget();
		}

		private bool TryFindNewTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(base.pawn, MentalState_TargetedInsultingSpree.candidates, false);
			bool result = ((IEnumerable<Pawn>)MentalState_TargetedInsultingSpree.candidates).TryRandomElement<Pawn>(out base.target);
			MentalState_TargetedInsultingSpree.candidates.Clear();
			return result;
		}

		public override void PostEnd()
		{
			base.PostEnd();
			if (base.target != null && PawnUtility.ShouldSendNotificationAbout(base.pawn))
			{
				Messages.Message("MessageNoLongerOnTargetedInsultingSpree".Translate(base.pawn.NameStringShort, base.target.Label), (Thing)base.pawn, MessageTypeDefOf.SituationResolved);
			}
		}

		public override string GetBeginLetterText()
		{
			string result;
			if (base.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.");
				result = "";
			}
			else
			{
				result = string.Format(base.def.beginLetter, base.pawn.Label, base.target.Label).AdjustedFor(base.pawn).CapitalizeFirst();
			}
			return result;
		}
	}
}
