using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	public class BattleLogEntry_StateTransition : LogEntry
	{
		private RulePackDef transitionDef;

		private Pawn subject;

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

		public BattleLogEntry_StateTransition(Pawn subject, RulePackDef transitionDef)
		{
			this.subject = subject;
			this.transitionDef = transitionDef;
		}

		public override bool Concerns(Thing t)
		{
			return t == this.subject;
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
				List<Rule> list = new List<Rule>();
				list.AddRange(GrammarUtility.RulesForPawn("subject", this.subject));
				list.AddRange(this.transitionDef.Rules);
				string text = GrammarResolver.Resolve("logentry", list, null, "state transition");
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
		}

		public override string ToString()
		{
			return this.transitionDef.defName + ": " + this.subject;
		}
	}
}
