using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BBB RID: 3003
	public class BattleLogEntry_Event : LogEntry
	{
		// Token: 0x0600412D RID: 16685 RVA: 0x00226978 File Offset: 0x00224D78
		public BattleLogEntry_Event() : base(null)
		{
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x00226984 File Offset: 0x00224D84
		public BattleLogEntry_Event(Thing subject, RulePackDef eventDef, Thing initiator) : base(null)
		{
			if (subject is Pawn)
			{
				this.subjectPawn = (subject as Pawn);
			}
			else if (subject != null)
			{
				this.subjectThing = subject.def;
			}
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			this.eventDef = eventDef;
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x0600412F RID: 16687 RVA: 0x002269FC File Offset: 0x00224DFC
		private string SubjectName
		{
			get
			{
				return (this.subjectPawn == null) ? "null" : this.subjectPawn.LabelShort;
			}
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x00226A34 File Offset: 0x00224E34
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiatorPawn;
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x00226A64 File Offset: 0x00224E64
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.subjectPawn != null)
			{
				yield return this.subjectPawn;
			}
			if (this.initiatorPawn != null)
			{
				yield return this.initiatorPawn;
			}
			yield break;
		}

		// Token: 0x06004132 RID: 16690 RVA: 0x00226A90 File Offset: 0x00224E90
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.subjectPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiatorPawn);
			}
			else
			{
				if (pov != this.initiatorPawn)
				{
					throw new NotImplementedException();
				}
				CameraJumper.TryJumpAndSelect(this.subjectPawn);
			}
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x00226AE8 File Offset: 0x00224EE8
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Includes.Add(this.eventDef);
			if (this.subjectPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.subjectPawn, result.Constants));
			}
			else if (this.subjectThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("SUBJECT", this.subjectThing));
			}
			if (this.initiatorPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiatorPawn, result.Constants));
			}
			else if (this.initiatorThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", this.initiatorThing));
			}
			return result;
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x00226BCC File Offset: 0x00224FCC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.eventDef, "eventDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x00226C34 File Offset: 0x00225034
		public override string ToString()
		{
			return this.eventDef.defName + ": " + this.subjectPawn;
		}

		// Token: 0x04002C87 RID: 11399
		private RulePackDef eventDef;

		// Token: 0x04002C88 RID: 11400
		private Pawn subjectPawn;

		// Token: 0x04002C89 RID: 11401
		private ThingDef subjectThing;

		// Token: 0x04002C8A RID: 11402
		private Pawn initiatorPawn;

		// Token: 0x04002C8B RID: 11403
		private ThingDef initiatorThing;
	}
}
