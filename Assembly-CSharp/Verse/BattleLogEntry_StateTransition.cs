using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BC4 RID: 3012
	public class BattleLogEntry_StateTransition : LogEntry
	{
		// Token: 0x06004168 RID: 16744 RVA: 0x00228264 File Offset: 0x00226664
		public BattleLogEntry_StateTransition() : base(null)
		{
		}

		// Token: 0x06004169 RID: 16745 RVA: 0x00228270 File Offset: 0x00226670
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

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x0600416A RID: 16746 RVA: 0x002282F8 File Offset: 0x002266F8
		private string SubjectName
		{
			get
			{
				return (this.subjectPawn == null) ? "null" : this.subjectPawn.LabelShort;
			}
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x00228330 File Offset: 0x00226730
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiator;
		}

		// Token: 0x0600416C RID: 16748 RVA: 0x00228360 File Offset: 0x00226760
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

		// Token: 0x0600416D RID: 16749 RVA: 0x0022838C File Offset: 0x0022678C
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

		// Token: 0x0600416E RID: 16750 RVA: 0x002283E4 File Offset: 0x002267E4
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

		// Token: 0x0600416F RID: 16751 RVA: 0x00228464 File Offset: 0x00226864
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

		// Token: 0x06004170 RID: 16752 RVA: 0x00228598 File Offset: 0x00226998
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

		// Token: 0x06004171 RID: 16753 RVA: 0x00228620 File Offset: 0x00226A20
		public override string ToString()
		{
			return this.transitionDef.defName + ": " + this.subjectPawn;
		}

		// Token: 0x04002CAB RID: 11435
		private RulePackDef transitionDef;

		// Token: 0x04002CAC RID: 11436
		private Pawn subjectPawn;

		// Token: 0x04002CAD RID: 11437
		private ThingDef subjectThing;

		// Token: 0x04002CAE RID: 11438
		private Pawn initiator;

		// Token: 0x04002CAF RID: 11439
		private HediffDef culpritHediffDef;

		// Token: 0x04002CB0 RID: 11440
		private BodyPartRecord culpritHediffTargetPart;

		// Token: 0x04002CB1 RID: 11441
		private BodyPartRecord culpritTargetPart;
	}
}
