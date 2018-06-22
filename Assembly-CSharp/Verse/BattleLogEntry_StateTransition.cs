using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BC0 RID: 3008
	public class BattleLogEntry_StateTransition : LogEntry
	{
		// Token: 0x0600416C RID: 16748 RVA: 0x002289B0 File Offset: 0x00226DB0
		public BattleLogEntry_StateTransition() : base(null)
		{
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x002289BC File Offset: 0x00226DBC
		public BattleLogEntry_StateTransition(Thing subject, RulePackDef transitionDef, Pawn initiator, Hediff culpritHediff, BodyPartRecord culpritTargetDef) : base(null)
		{
			if (subject is Pawn)
			{
				this.subjectPawn = (subject as Pawn);
			}
			else if (subject != null)
			{
				this.subjectThing = subject.def;
			}
			this.transitionDef = transitionDef;
			this.initiator = initiator;
			if (culpritHediff != null)
			{
				this.culpritHediffDef = culpritHediff.def;
				if (culpritHediff.Part != null)
				{
					this.culpritHediffTargetPart = culpritHediff.Part;
				}
			}
			this.culpritTargetPart = culpritTargetDef;
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x0600416E RID: 16750 RVA: 0x00228A44 File Offset: 0x00226E44
		private string SubjectName
		{
			get
			{
				return (this.subjectPawn == null) ? "null" : this.subjectPawn.LabelShort;
			}
		}

		// Token: 0x0600416F RID: 16751 RVA: 0x00228A7C File Offset: 0x00226E7C
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiator;
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x00228AAC File Offset: 0x00226EAC
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiator != null)
			{
				yield return this.initiator;
			}
			if (this.subjectPawn != null)
			{
				yield return this.subjectPawn;
			}
			yield break;
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x00228AD8 File Offset: 0x00226ED8
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.subjectPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiator);
			}
			else
			{
				if (pov != this.initiator)
				{
					throw new NotImplementedException();
				}
				CameraJumper.TryJumpAndSelect(this.subjectPawn);
			}
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x00228B30 File Offset: 0x00226F30
		public override Texture2D IconFromPOV(Thing pov)
		{
			Texture2D result;
			if (pov == null || pov == this.subjectPawn)
			{
				result = ((this.transitionDef != RulePackDefOf.Transition_Downed) ? LogEntry.Skull : LogEntry.Downed);
			}
			else if (pov == this.initiator)
			{
				result = ((this.transitionDef != RulePackDefOf.Transition_Downed) ? LogEntry.SkullTarget : LogEntry.DownedTarget);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x00228BB0 File Offset: 0x00226FB0
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.subjectPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.subjectPawn, result.Constants));
			}
			else if (this.subjectThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("SUBJECT", this.subjectThing));
			}
			result.Includes.Add(this.transitionDef);
			if (this.initiator != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, result.Constants));
			}
			if (this.culpritHediffDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForHediffDef("CULPRITHEDIFF", this.culpritHediffDef, this.culpritHediffTargetPart));
			}
			if (this.culpritHediffTargetPart != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord("CULPRITHEDIFF_target", this.culpritHediffTargetPart));
			}
			if (this.culpritTargetPart != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord("CULPRITHEDIFF_originaltarget", this.culpritTargetPart));
			}
			return result;
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x00228CE4 File Offset: 0x002270E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.transitionDef, "transitionDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_Defs.Look<HediffDef>(ref this.culpritHediffDef, "culpritHediffDef");
			Scribe_BodyParts.Look(ref this.culpritHediffTargetPart, "culpritHediffTargetPart", null);
			Scribe_BodyParts.Look(ref this.culpritTargetPart, "culpritTargetPart", null);
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x00228D6C File Offset: 0x0022716C
		public override string ToString()
		{
			return this.transitionDef.defName + ": " + this.subjectPawn;
		}

		// Token: 0x04002CB0 RID: 11440
		private RulePackDef transitionDef;

		// Token: 0x04002CB1 RID: 11441
		private Pawn subjectPawn;

		// Token: 0x04002CB2 RID: 11442
		private ThingDef subjectThing;

		// Token: 0x04002CB3 RID: 11443
		private Pawn initiator;

		// Token: 0x04002CB4 RID: 11444
		private HediffDef culpritHediffDef;

		// Token: 0x04002CB5 RID: 11445
		private BodyPartRecord culpritHediffTargetPart;

		// Token: 0x04002CB6 RID: 11446
		private BodyPartRecord culpritTargetPart;
	}
}
