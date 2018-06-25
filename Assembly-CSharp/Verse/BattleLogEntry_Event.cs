using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BBE RID: 3006
	public class BattleLogEntry_Event : LogEntry
	{
		// Token: 0x04002C8E RID: 11406
		private RulePackDef eventDef;

		// Token: 0x04002C8F RID: 11407
		private Pawn subjectPawn;

		// Token: 0x04002C90 RID: 11408
		private ThingDef subjectThing;

		// Token: 0x04002C91 RID: 11409
		private Pawn initiatorPawn;

		// Token: 0x04002C92 RID: 11410
		private ThingDef initiatorThing;

		// Token: 0x06004130 RID: 16688 RVA: 0x00226D34 File Offset: 0x00225134
		public BattleLogEntry_Event() : base(null)
		{
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x00226D40 File Offset: 0x00225140
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

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06004132 RID: 16690 RVA: 0x00226DB8 File Offset: 0x002251B8
		private string SubjectName
		{
			get
			{
				return (this.subjectPawn == null) ? "null" : this.subjectPawn.LabelShort;
			}
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x00226DF0 File Offset: 0x002251F0
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiatorPawn;
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x00226E20 File Offset: 0x00225220
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

		// Token: 0x06004135 RID: 16693 RVA: 0x00226E4C File Offset: 0x0022524C
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

		// Token: 0x06004136 RID: 16694 RVA: 0x00226EA4 File Offset: 0x002252A4
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

		// Token: 0x06004137 RID: 16695 RVA: 0x00226F88 File Offset: 0x00225388
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.eventDef, "eventDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x00226FF0 File Offset: 0x002253F0
		public override string ToString()
		{
			return this.eventDef.defName + ": " + this.subjectPawn;
		}
	}
}
