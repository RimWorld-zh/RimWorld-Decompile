using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BC3 RID: 3011
	public class BattleLogEntry_RangedImpact : LogEntry_DamageResult
	{
		// Token: 0x0600415A RID: 16730 RVA: 0x00227994 File Offset: 0x00225D94
		public BattleLogEntry_RangedImpact() : base(null)
		{
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x002279A0 File Offset: 0x00225DA0
		public BattleLogEntry_RangedImpact(Thing initiator, Thing recipient, Thing originalTarget, ThingDef weaponDef, ThingDef projectileDef, ThingDef coverDef) : base(null)
		{
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			if (recipient is Pawn)
			{
				this.recipientPawn = (recipient as Pawn);
			}
			else if (recipient != null)
			{
				this.recipientThing = recipient.def;
			}
			if (originalTarget is Pawn)
			{
				this.originalTargetPawn = (originalTarget as Pawn);
				this.originalTargetMobile = (!this.originalTargetPawn.Downed && !this.originalTargetPawn.Dead && this.originalTargetPawn.Awake());
			}
			else if (originalTarget != null)
			{
				this.originalTargetThing = originalTarget.def;
			}
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
			this.coverDef = coverDef;
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x0600415C RID: 16732 RVA: 0x00227A90 File Offset: 0x00225E90
		private string InitiatorName
		{
			get
			{
				string result;
				if (this.initiatorPawn != null)
				{
					result = this.initiatorPawn.LabelShort;
				}
				else if (this.initiatorThing != null)
				{
					result = this.initiatorThing.defName;
				}
				else
				{
					result = "null";
				}
				return result;
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x0600415D RID: 16733 RVA: 0x00227AE4 File Offset: 0x00225EE4
		private string RecipientName
		{
			get
			{
				string result;
				if (this.recipientPawn != null)
				{
					result = this.recipientPawn.LabelShort;
				}
				else if (this.recipientThing != null)
				{
					result = this.recipientThing.defName;
				}
				else
				{
					result = "null";
				}
				return result;
			}
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x00227B38 File Offset: 0x00225F38
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn || t == this.originalTargetPawn;
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x00227B74 File Offset: 0x00225F74
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiatorPawn != null)
			{
				yield return this.initiatorPawn;
			}
			if (this.recipientPawn != null)
			{
				yield return this.recipientPawn;
			}
			if (this.originalTargetPawn != null)
			{
				yield return this.originalTargetPawn;
			}
			yield break;
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x00227BA0 File Offset: 0x00225FA0
		public override void ClickedFromPOV(Thing pov)
		{
			if (this.recipientPawn != null)
			{
				if (pov == this.initiatorPawn)
				{
					CameraJumper.TryJumpAndSelect(this.recipientPawn);
				}
				else
				{
					if (pov != this.recipientPawn)
					{
						throw new NotImplementedException();
					}
					CameraJumper.TryJumpAndSelect(this.initiatorPawn);
				}
			}
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x00227C08 File Offset: 0x00226008
		public override Texture2D IconFromPOV(Thing pov)
		{
			Texture2D result;
			if (this.damagedParts.NullOrEmpty<BodyPartRecord>())
			{
				result = null;
			}
			else if (this.deflected)
			{
				result = null;
			}
			else if (pov == null || pov == this.recipientPawn)
			{
				result = LogEntry.Blood;
			}
			else if (pov == this.initiatorPawn)
			{
				result = LogEntry.BloodTarget;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x00227C7C File Offset: 0x0022607C
		protected override BodyDef DamagedBody()
		{
			return (this.recipientPawn == null) ? null : this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x00227CB4 File Offset: 0x002260B4
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.recipientPawn != null || this.recipientThing != null)
			{
				result.Includes.Add((!this.deflected) ? RulePackDefOf.Combat_RangedDamage : RulePackDefOf.Combat_RangedDeflect);
			}
			else
			{
				result.Includes.Add(RulePackDefOf.Combat_RangedMiss);
			}
			if (this.initiatorPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiatorPawn, result.Constants));
			}
			else if (this.initiatorThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", this.initiatorThing));
			}
			else
			{
				result.Constants["INITIATOR_missing"] = "True";
			}
			if (this.recipientPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants));
			}
			else if (this.recipientThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT", this.recipientThing));
			}
			else
			{
				result.Constants["RECIPIENT_missing"] = "True";
			}
			if (this.originalTargetPawn != this.recipientPawn || this.originalTargetThing != this.recipientThing)
			{
				if (this.originalTargetPawn != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForPawn("ORIGINALTARGET", this.originalTargetPawn, result.Constants));
					result.Constants["ORIGINALTARGET_mobile"] = this.originalTargetMobile.ToString();
				}
				else if (this.originalTargetThing != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef("ORIGINALTARGET", this.originalTargetThing));
				}
				else
				{
					result.Constants["ORIGINALTARGET_missing"] = "True";
				}
			}
			result.Rules.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("WEAPON", this.weaponDef, this.projectileDef));
			result.Constants["COVER_missing"] = ((this.coverDef == null) ? "True" : "False");
			if (this.coverDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("COVER", this.coverDef));
			}
			return result;
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x00227F40 File Offset: 0x00226340
		public override bool ShowInCompactView()
		{
			if (!this.deflected)
			{
				if (this.recipientPawn != null)
				{
					return true;
				}
				if (this.originalTargetThing != null && this.originalTargetThing == this.recipientThing)
				{
					return true;
				}
			}
			int num = 1;
			if (this.weaponDef != null && !this.weaponDef.Verbs.NullOrEmpty<VerbProperties>())
			{
				num = this.weaponDef.Verbs[0].burstShotCount;
			}
			return Rand.ChanceSeeded(BattleLogEntry_RangedImpact.DisplayChanceOnMiss / (float)num, this.logID);
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x00227FE4 File Offset: 0x002263E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_References.Look<Pawn>(ref this.originalTargetPawn, "originalTargetPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.originalTargetThing, "originalTargetThing");
			Scribe_Values.Look<bool>(ref this.originalTargetMobile, "originalTargetMobile", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Defs.Look<ThingDef>(ref this.coverDef, "coverDef");
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x002280A0 File Offset: 0x002264A0
		public override string ToString()
		{
			return "BattleLogEntry_RangedImpact: " + this.InitiatorName + "->" + this.RecipientName;
		}

		// Token: 0x04002CA0 RID: 11424
		private Pawn initiatorPawn;

		// Token: 0x04002CA1 RID: 11425
		private ThingDef initiatorThing;

		// Token: 0x04002CA2 RID: 11426
		private Pawn recipientPawn;

		// Token: 0x04002CA3 RID: 11427
		private ThingDef recipientThing;

		// Token: 0x04002CA4 RID: 11428
		private Pawn originalTargetPawn;

		// Token: 0x04002CA5 RID: 11429
		private ThingDef originalTargetThing;

		// Token: 0x04002CA6 RID: 11430
		private bool originalTargetMobile;

		// Token: 0x04002CA7 RID: 11431
		private ThingDef weaponDef;

		// Token: 0x04002CA8 RID: 11432
		private ThingDef projectileDef;

		// Token: 0x04002CA9 RID: 11433
		private ThingDef coverDef;

		// Token: 0x04002CAA RID: 11434
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChanceOnMiss = 0.25f;
	}
}
