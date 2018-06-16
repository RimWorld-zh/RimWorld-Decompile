using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BBF RID: 3007
	public class BattleLogEntry_Event : LogEntry
	{
		// Token: 0x06004129 RID: 16681 RVA: 0x002261D0 File Offset: 0x002245D0
		public BattleLogEntry_Event() : base(null)
		{
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x002261DC File Offset: 0x002245DC
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

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x0600412B RID: 16683 RVA: 0x00226254 File Offset: 0x00224654
		private string SubjectName
		{
			get
			{
				return (this.subjectPawn == null) ? "null" : this.subjectPawn.LabelShort;
			}
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x0022628C File Offset: 0x0022468C
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiatorPawn;
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x002262BC File Offset: 0x002246BC
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

		// Token: 0x0600412E RID: 16686 RVA: 0x002262E8 File Offset: 0x002246E8
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

		// Token: 0x0600412F RID: 16687 RVA: 0x00226340 File Offset: 0x00224740
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

		// Token: 0x06004130 RID: 16688 RVA: 0x00226424 File Offset: 0x00224824
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.eventDef, "eventDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x0022648C File Offset: 0x0022488C
		public override string ToString()
		{
			return this.eventDef.defName + ": " + this.subjectPawn;
		}

		// Token: 0x04002C82 RID: 11394
		private RulePackDef eventDef;

		// Token: 0x04002C83 RID: 11395
		private Pawn subjectPawn;

		// Token: 0x04002C84 RID: 11396
		private ThingDef subjectThing;

		// Token: 0x04002C85 RID: 11397
		private Pawn initiatorPawn;

		// Token: 0x04002C86 RID: 11398
		private ThingDef initiatorThing;
	}
}
