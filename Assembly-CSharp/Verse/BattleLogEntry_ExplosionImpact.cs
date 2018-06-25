using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BBF RID: 3007
	public class BattleLogEntry_ExplosionImpact : LogEntry_DamageResult
	{
		// Token: 0x04002C93 RID: 11411
		private Pawn initiatorPawn;

		// Token: 0x04002C94 RID: 11412
		private ThingDef initiatorThing;

		// Token: 0x04002C95 RID: 11413
		private Pawn recipientPawn;

		// Token: 0x04002C96 RID: 11414
		private ThingDef recipientThing;

		// Token: 0x04002C97 RID: 11415
		private ThingDef weaponDef;

		// Token: 0x04002C98 RID: 11416
		private ThingDef projectileDef;

		// Token: 0x04002C99 RID: 11417
		private DamageDef damageDef;

		// Token: 0x06004139 RID: 16697 RVA: 0x00227170 File Offset: 0x00225570
		public BattleLogEntry_ExplosionImpact() : base(null)
		{
		}

		// Token: 0x0600413A RID: 16698 RVA: 0x0022717C File Offset: 0x0022557C
		public BattleLogEntry_ExplosionImpact(Thing initiator, Thing recipient, ThingDef weaponDef, ThingDef projectileDef, DamageDef damageDef) : base(null)
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
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
			this.damageDef = damageDef;
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x0600413B RID: 16699 RVA: 0x00227204 File Offset: 0x00225604
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

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x0600413C RID: 16700 RVA: 0x00227258 File Offset: 0x00225658
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

		// Token: 0x0600413D RID: 16701 RVA: 0x002272AC File Offset: 0x002256AC
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x002272DC File Offset: 0x002256DC
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

		// Token: 0x0600413F RID: 16703 RVA: 0x00227308 File Offset: 0x00225708
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

		// Token: 0x06004140 RID: 16704 RVA: 0x00227370 File Offset: 0x00225770
		public override Texture2D IconFromPOV(Thing pov)
		{
			Texture2D result;
			if (this.damagedParts.NullOrEmpty<BodyPartRecord>())
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

		// Token: 0x06004141 RID: 16705 RVA: 0x002273D4 File Offset: 0x002257D4
		protected override BodyDef DamagedBody()
		{
			return (this.recipientPawn == null) ? null : this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x0022740C File Offset: 0x0022580C
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Includes.Add(RulePackDefOf.Combat_ExplosionImpact);
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
			if (this.projectileDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("PROJECTILE", this.projectileDef));
			}
			if (this.damageDef != null && this.damageDef.combatLogRules != null)
			{
				result.Includes.Add(this.damageDef.combatLogRules);
			}
			return result;
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x002275A0 File Offset: 0x002259A0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Defs.Look<DamageDef>(ref this.damageDef, "damageDef");
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x00227628 File Offset: 0x00225A28
		public override string ToString()
		{
			return "BattleLogEntry_ExplosionImpact: " + this.InitiatorName + "->" + this.RecipientName;
		}
	}
}
