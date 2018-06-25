using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BBD RID: 3005
	public class BattleLogEntry_Event : LogEntry
	{
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

		// Token: 0x06004130 RID: 16688 RVA: 0x00226A54 File Offset: 0x00224E54
		public BattleLogEntry_Event() : base(null)
		{
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x00226A60 File Offset: 0x00224E60
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
		// (get) Token: 0x06004132 RID: 16690 RVA: 0x00226AD8 File Offset: 0x00224ED8
		private string SubjectName
		{
			get
			{
				return (this.subjectPawn == null) ? "null" : this.subjectPawn.LabelShort;
			}
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x00226B10 File Offset: 0x00224F10
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiatorPawn;
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x00226B40 File Offset: 0x00224F40
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

		// Token: 0x06004135 RID: 16693 RVA: 0x00226B6C File Offset: 0x00224F6C
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

		// Token: 0x06004136 RID: 16694 RVA: 0x00226BC4 File Offset: 0x00224FC4
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

		// Token: 0x06004137 RID: 16695 RVA: 0x00226CA8 File Offset: 0x002250A8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.eventDef, "eventDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x00226D10 File Offset: 0x00225110
		public override string ToString()
		{
			return this.eventDef.defName + ": " + this.subjectPawn;
		}
	}
}
