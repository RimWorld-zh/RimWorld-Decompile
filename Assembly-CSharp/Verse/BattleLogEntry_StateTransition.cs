using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	public class BattleLogEntry_StateTransition : LogEntry
	{
		private RulePackDef transitionDef;

		private Pawn subject;

		private Pawn initiator;

		private HediffDef culpritHediffDef;

		private BodyPartDef culpritHediffTargetDef;

		private BodyPartDef culpritTargetDef;

		private string SubjectName
		{
			get
			{
				return (this.subject == null) ? "null" : this.subject.NameStringShort;
			}
		}

		public BattleLogEntry_StateTransition()
		{
		}

		public BattleLogEntry_StateTransition(Pawn subject, RulePackDef transitionDef, Pawn initiator, Hediff culpritHediff, BodyPartDef culpritTargetDef)
		{
			this.subject = subject;
			this.transitionDef = transitionDef;
			this.initiator = initiator;
			if (culpritHediff != null)
			{
				this.culpritHediffDef = culpritHediff.def;
				if (culpritHediff.Part != null)
				{
					this.culpritHediffTargetDef = culpritHediff.Part.def;
				}
			}
			this.culpritTargetDef = culpritTargetDef;
		}

		public override bool Concerns(Thing t)
		{
			return t == this.subject || t == this.initiator;
		}

		public override Texture2D IconFromPOV(Thing pov)
		{
			return (pov == null || pov == this.subject) ? LogEntry.Skull : ((pov != this.initiator) ? null : LogEntry.SkullTarget);
		}

		public override string ToGameStringFromPOV(Thing pov)
		{
			string result;
			if (this.subject == null)
			{
				Log.ErrorOnce("BattleLogEntry_StateTransition has a null pawn reference.", 34422);
				result = "[" + this.transitionDef.label + " error: null pawn reference]";
			}
			else
			{
				Rand.PushState();
				Rand.Seed = base.randSeed;
				GrammarRequest request = default(GrammarRequest);
				request.Rules.AddRange(GrammarUtility.RulesForPawn("subject", this.subject));
				request.Includes.Add(this.transitionDef);
				if (this.initiator != null)
				{
					request.Rules.AddRange(GrammarUtility.RulesForPawn("initiator", this.initiator));
				}
				if (this.culpritHediffDef != null)
				{
					request.Rules.AddRange(GrammarUtility.RulesForDef("culpritHediff", this.culpritHediffDef));
				}
				if (this.culpritHediffTargetDef != null)
				{
					request.Rules.AddRange(GrammarUtility.RulesForDef("culpritHediff_target", this.culpritHediffTargetDef));
				}
				if (this.culpritTargetDef != null)
				{
					request.Rules.AddRange(GrammarUtility.RulesForDef("culpritHediff_originaltarget", this.culpritTargetDef));
				}
				string text = GrammarResolver.Resolve("logentry", request, "state transition");
				Rand.PopState();
				result = text;
			}
			return result;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.transitionDef, "transitionDef");
			Scribe_References.Look<Pawn>(ref this.subject, "subject", true);
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_Defs.Look<HediffDef>(ref this.culpritHediffDef, "culpritHediffDef");
			Scribe_Defs.Look<BodyPartDef>(ref this.culpritHediffTargetDef, "culpritHediffTargetDef");
			Scribe_Defs.Look<BodyPartDef>(ref this.culpritTargetDef, "culpritTargetDef");
		}

		public override string ToString()
		{
			return this.transitionDef.defName + ": " + this.subject;
		}
	}
}
