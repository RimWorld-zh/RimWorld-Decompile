using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BC0 RID: 3008
	public class BattleLogEntry_ExplosionImpact : LogEntry_DamageResult
	{
		// Token: 0x06004132 RID: 16690 RVA: 0x0022660C File Offset: 0x00224A0C
		public BattleLogEntry_ExplosionImpact() : base(null)
		{
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x00226618 File Offset: 0x00224A18
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

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06004134 RID: 16692 RVA: 0x002266A0 File Offset: 0x00224AA0
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

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06004135 RID: 16693 RVA: 0x002266F4 File Offset: 0x00224AF4
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

		// Token: 0x06004136 RID: 16694 RVA: 0x00226748 File Offset: 0x00224B48
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x00226778 File Offset: 0x00224B78
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

		// Token: 0x06004138 RID: 16696 RVA: 0x002267A4 File Offset: 0x00224BA4
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

		// Token: 0x06004139 RID: 16697 RVA: 0x0022680C File Offset: 0x00224C0C
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

		// Token: 0x0600413A RID: 16698 RVA: 0x00226870 File Offset: 0x00224C70
		protected override BodyDef DamagedBody()
		{
			return (this.recipientPawn == null) ? null : this.recipientPawn.RaceProps.body;
		}

		// Token: 0x0600413B RID: 16699 RVA: 0x002268A8 File Offset: 0x00224CA8
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

		// Token: 0x0600413C RID: 16700 RVA: 0x00226A3C File Offset: 0x00224E3C
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
			Scribe_Collections.Look<BodyPartRecord>(ref this.damagedParts, "damagedParts", LookMode.Def, new object[0]);
			Scribe_Collections.Look<bool>(ref this.damagedPartsDestroyed, "damagedPartsDestroyed", LookMode.Value, new object[0]);
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x00226AF0 File Offset: 0x00224EF0
		public override string ToString()
		{
			return "BattleLogEntry_ExplosionImpact: " + this.InitiatorName + "->" + this.RecipientName;
		}

		// Token: 0x04002C87 RID: 11399
		private Pawn initiatorPawn;

		// Token: 0x04002C88 RID: 11400
		private ThingDef initiatorThing;

		// Token: 0x04002C89 RID: 11401
		private Pawn recipientPawn;

		// Token: 0x04002C8A RID: 11402
		private ThingDef recipientThing;

		// Token: 0x04002C8B RID: 11403
		private ThingDef weaponDef;

		// Token: 0x04002C8C RID: 11404
		private ThingDef projectileDef;

		// Token: 0x04002C8D RID: 11405
		private DamageDef damageDef;
	}
}
