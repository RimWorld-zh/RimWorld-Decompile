using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BC2 RID: 3010
	public class BattleLogEntry_RangedFire : LogEntry
	{
		// Token: 0x0600414E RID: 16718 RVA: 0x002273C0 File Offset: 0x002257C0
		public BattleLogEntry_RangedFire() : base(null)
		{
		}

		// Token: 0x0600414F RID: 16719 RVA: 0x002273CC File Offset: 0x002257CC
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

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06004150 RID: 16720 RVA: 0x00227454 File Offset: 0x00225854
		private string InitiatorName
		{
			get
			{
				return (this.initiatorPawn == null) ? "null" : this.initiatorPawn.LabelShort;
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06004151 RID: 16721 RVA: 0x0022748C File Offset: 0x0022588C
		private string RecipientName
		{
			get
			{
				return (this.recipientPawn == null) ? "null" : this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x002274C4 File Offset: 0x002258C4
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x002274F4 File Offset: 0x002258F4
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

		// Token: 0x06004154 RID: 16724 RVA: 0x00227520 File Offset: 0x00225920
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

		// Token: 0x06004155 RID: 16725 RVA: 0x00227588 File Offset: 0x00225988
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

		// Token: 0x06004156 RID: 16726 RVA: 0x00227758 File Offset: 0x00225B58
		public override bool ShowInCompactView()
		{
			return Rand.ChanceSeeded(BattleLogEntry_RangedFire.DisplayChance, this.logID);
		}

		// Token: 0x06004157 RID: 16727 RVA: 0x00227780 File Offset: 0x00225B80
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

		// Token: 0x06004158 RID: 16728 RVA: 0x00227808 File Offset: 0x00225C08
		public override string ToString()
		{
			return "BattleLogEntry_RangedFire: " + this.InitiatorName + "->" + this.RecipientName;
		}

		// Token: 0x04002C98 RID: 11416
		private Pawn initiatorPawn;

		// Token: 0x04002C99 RID: 11417
		private ThingDef initiatorThing;

		// Token: 0x04002C9A RID: 11418
		private Pawn recipientPawn;

		// Token: 0x04002C9B RID: 11419
		private ThingDef recipientThing;

		// Token: 0x04002C9C RID: 11420
		private ThingDef weaponDef;

		// Token: 0x04002C9D RID: 11421
		private ThingDef projectileDef;

		// Token: 0x04002C9E RID: 11422
		private bool burst;

		// Token: 0x04002C9F RID: 11423
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChance = 0.25f;
	}
}
