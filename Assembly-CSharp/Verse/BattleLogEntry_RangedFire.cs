using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BC0 RID: 3008
	public class BattleLogEntry_RangedFire : LogEntry
	{
		// Token: 0x04002C9D RID: 11421
		private Pawn initiatorPawn;

		// Token: 0x04002C9E RID: 11422
		private ThingDef initiatorThing;

		// Token: 0x04002C9F RID: 11423
		private Pawn recipientPawn;

		// Token: 0x04002CA0 RID: 11424
		private ThingDef recipientThing;

		// Token: 0x04002CA1 RID: 11425
		private ThingDef weaponDef;

		// Token: 0x04002CA2 RID: 11426
		private ThingDef projectileDef;

		// Token: 0x04002CA3 RID: 11427
		private bool burst;

		// Token: 0x04002CA4 RID: 11428
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChance = 0.25f;

		// Token: 0x06004155 RID: 16725 RVA: 0x00227BE8 File Offset: 0x00225FE8
		public BattleLogEntry_RangedFire() : base(null)
		{
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x00227BF4 File Offset: 0x00225FF4
		public BattleLogEntry_RangedFire(Thing initiator, Thing target, ThingDef weaponDef, ThingDef projectileDef, bool burst) : base(null)
		{
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			if (target is Pawn)
			{
				this.recipientPawn = (target as Pawn);
			}
			else if (target != null)
			{
				this.recipientThing = target.def;
			}
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
			this.burst = burst;
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06004157 RID: 16727 RVA: 0x00227C7C File Offset: 0x0022607C
		private string InitiatorName
		{
			get
			{
				return (this.initiatorPawn == null) ? "null" : this.initiatorPawn.LabelShort;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06004158 RID: 16728 RVA: 0x00227CB4 File Offset: 0x002260B4
		private string RecipientName
		{
			get
			{
				return (this.recipientPawn == null) ? "null" : this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x00227CEC File Offset: 0x002260EC
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x00227D1C File Offset: 0x0022611C
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
			yield break;
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x00227D48 File Offset: 0x00226148
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

		// Token: 0x0600415C RID: 16732 RVA: 0x00227DB0 File Offset: 0x002261B0
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.initiatorPawn == null && this.initiatorThing == null)
			{
				Log.ErrorOnce("BattleLogEntry_RangedFire has a null initiator.", 60465709, false);
			}
			if (this.weaponDef != null && this.weaponDef.Verbs[0].rangedFireRulepack != null)
			{
				result.Includes.Add(this.weaponDef.Verbs[0].rangedFireRulepack);
			}
			else
			{
				result.Includes.Add(RulePackDefOf.Combat_RangedFire);
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
			result.Rules.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("WEAPON", this.weaponDef, this.projectileDef));
			result.Constants["BURST"] = this.burst.ToString();
			return result;
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x00227F80 File Offset: 0x00226380
		public override bool ShowInCompactView()
		{
			return Rand.ChanceSeeded(BattleLogEntry_RangedFire.DisplayChance, this.logID);
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x00227FA8 File Offset: 0x002263A8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Values.Look<bool>(ref this.burst, "burst", false, false);
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x00228030 File Offset: 0x00226430
		public override string ToString()
		{
			return "BattleLogEntry_RangedFire: " + this.InitiatorName + "->" + this.RecipientName;
		}
	}
}
