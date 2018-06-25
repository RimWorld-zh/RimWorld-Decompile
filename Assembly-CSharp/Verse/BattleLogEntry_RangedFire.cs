using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BC1 RID: 3009
	public class BattleLogEntry_RangedFire : LogEntry
	{
		// Token: 0x04002CA4 RID: 11428
		private Pawn initiatorPawn;

		// Token: 0x04002CA5 RID: 11429
		private ThingDef initiatorThing;

		// Token: 0x04002CA6 RID: 11430
		private Pawn recipientPawn;

		// Token: 0x04002CA7 RID: 11431
		private ThingDef recipientThing;

		// Token: 0x04002CA8 RID: 11432
		private ThingDef weaponDef;

		// Token: 0x04002CA9 RID: 11433
		private ThingDef projectileDef;

		// Token: 0x04002CAA RID: 11434
		private bool burst;

		// Token: 0x04002CAB RID: 11435
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChance = 0.25f;

		// Token: 0x06004155 RID: 16725 RVA: 0x00227EC8 File Offset: 0x002262C8
		public BattleLogEntry_RangedFire() : base(null)
		{
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x00227ED4 File Offset: 0x002262D4
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
		// (get) Token: 0x06004157 RID: 16727 RVA: 0x00227F5C File Offset: 0x0022635C
		private string InitiatorName
		{
			get
			{
				return (this.initiatorPawn == null) ? "null" : this.initiatorPawn.LabelShort;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06004158 RID: 16728 RVA: 0x00227F94 File Offset: 0x00226394
		private string RecipientName
		{
			get
			{
				return (this.recipientPawn == null) ? "null" : this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x00227FCC File Offset: 0x002263CC
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x00227FFC File Offset: 0x002263FC
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

		// Token: 0x0600415B RID: 16731 RVA: 0x00228028 File Offset: 0x00226428
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

		// Token: 0x0600415C RID: 16732 RVA: 0x00228090 File Offset: 0x00226490
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

		// Token: 0x0600415D RID: 16733 RVA: 0x00228260 File Offset: 0x00226660
		public override bool ShowInCompactView()
		{
			return Rand.ChanceSeeded(BattleLogEntry_RangedFire.DisplayChance, this.logID);
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x00228288 File Offset: 0x00226688
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

		// Token: 0x0600415F RID: 16735 RVA: 0x00228310 File Offset: 0x00226710
		public override string ToString()
		{
			return "BattleLogEntry_RangedFire: " + this.InitiatorName + "->" + this.RecipientName;
		}
	}
}
