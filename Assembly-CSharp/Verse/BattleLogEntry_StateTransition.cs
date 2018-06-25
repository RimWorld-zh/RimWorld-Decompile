using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BC3 RID: 3011
	public class BattleLogEntry_StateTransition : LogEntry
	{
		// Token: 0x04002CB7 RID: 11447
		private RulePackDef transitionDef;

		// Token: 0x04002CB8 RID: 11448
		private Pawn subjectPawn;

		// Token: 0x04002CB9 RID: 11449
		private ThingDef subjectThing;

		// Token: 0x04002CBA RID: 11450
		private Pawn initiator;

		// Token: 0x04002CBB RID: 11451
		private HediffDef culpritHediffDef;

		// Token: 0x04002CBC RID: 11452
		private BodyPartRecord culpritHediffTargetPart;

		// Token: 0x04002CBD RID: 11453
		private BodyPartRecord culpritTargetPart;

		// Token: 0x0600416F RID: 16751 RVA: 0x00228D6C File Offset: 0x0022716C
		public BattleLogEntry_StateTransition() : base(null)
		{
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x00228D78 File Offset: 0x00227178
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

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06004171 RID: 16753 RVA: 0x00228E00 File Offset: 0x00227200
		private string SubjectName
		{
			get
			{
				return (this.subjectPawn == null) ? "null" : this.subjectPawn.LabelShort;
			}
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x00228E38 File Offset: 0x00227238
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiator;
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x00228E68 File Offset: 0x00227268
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

		// Token: 0x06004174 RID: 16756 RVA: 0x00228E94 File Offset: 0x00227294
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

		// Token: 0x06004175 RID: 16757 RVA: 0x00228EEC File Offset: 0x002272EC
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

		// Token: 0x06004176 RID: 16758 RVA: 0x00228F6C File Offset: 0x0022736C
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

		// Token: 0x06004177 RID: 16759 RVA: 0x002290A0 File Offset: 0x002274A0
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

		// Token: 0x06004178 RID: 16760 RVA: 0x00229128 File Offset: 0x00227528
		public override string ToString()
		{
			return this.transitionDef.defName + ": " + this.subjectPawn;
		}
	}
}
