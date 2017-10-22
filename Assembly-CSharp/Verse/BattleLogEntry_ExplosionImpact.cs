using RimWorld;
using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	public class BattleLogEntry_ExplosionImpact : LogEntry, IDamageResultLog
	{
		private Pawn initiatorPawn;

		private ThingDef initiatorThing;

		private Pawn recipientPawn;

		private ThingDef recipientThing;

		private ThingDef weaponDef;

		private ThingDef projectileDef;

		private List<BodyPartDef> damagedParts;

		private List<bool> damagedPartsDestroyed;

		private string InitiatorName
		{
			get
			{
				return (this.initiatorPawn == null) ? ((this.initiatorThing == null) ? "null" : this.initiatorThing.defName) : this.initiatorPawn.NameStringShort;
			}
		}

		private string RecipientName
		{
			get
			{
				return (this.recipientPawn == null) ? ((this.recipientThing == null) ? "null" : this.recipientThing.defName) : this.recipientPawn.NameStringShort;
			}
		}

		public BattleLogEntry_ExplosionImpact()
		{
		}

		public BattleLogEntry_ExplosionImpact(Thing initiator, Thing recipient, ThingDef weaponDef, ThingDef projectileDef)
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
		}

		public void FillTargets(List<BodyPartDef> recipientParts, List<bool> recipientPartsDestroyed)
		{
			this.damagedParts = recipientParts;
			this.damagedPartsDestroyed = recipientPartsDestroyed;
		}

		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		public override void ClickedFromPOV(Thing pov)
		{
			if (this.recipientPawn != null)
			{
				if (pov != this.initiatorPawn)
				{
					if (pov == this.recipientPawn)
					{
						CameraJumper.TryJumpAndSelect((Thing)this.initiatorPawn);
						return;
					}
					throw new NotImplementedException();
				}
				CameraJumper.TryJumpAndSelect((Thing)this.recipientPawn);
			}
		}

		public override string ToGameStringFromPOV(Thing pov)
		{
			Rand.PushState();
			Rand.Seed = base.randSeed;
			List<Rule> list = new List<Rule>();
			list.AddRange(RulePackDefOf.Combat_ExplosionImpact.Rules);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (this.initiatorPawn != null)
			{
				list.AddRange(GrammarUtility.RulesForPawn("initiator", this.initiatorPawn));
			}
			else if (this.initiatorThing != null)
			{
				list.AddRange(GrammarUtility.RulesForDef("initiator", this.initiatorThing));
			}
			else
			{
				dictionary["initiator_missing"] = "True";
			}
			if (this.recipientPawn != null)
			{
				list.AddRange(GrammarUtility.RulesForPawn("recipient", this.recipientPawn));
			}
			else if (this.recipientThing != null)
			{
				list.AddRange(GrammarUtility.RulesForDef("recipient", this.recipientThing));
			}
			else
			{
				dictionary["recipient_missing"] = "True";
			}
			list.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("weapon", this.weaponDef, this.projectileDef));
			if (this.projectileDef != null && this.projectileDef.projectile.damageDef != null && this.projectileDef.projectile.damageDef.combatLogRules != null)
			{
				list.AddRange(this.projectileDef.projectile.damageDef.combatLogRules.Rules);
			}
			list.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", this.damagedParts, this.damagedPartsDestroyed, dictionary));
			string result = GrammarResolver.Resolve("logentry", list, dictionary, "ranged explosion");
			Rand.PopState();
			return result;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Collections.Look<BodyPartDef>(ref this.damagedParts, "damagedParts", LookMode.Def, new object[0]);
			Scribe_Collections.Look<bool>(ref this.damagedPartsDestroyed, "damagedPartsDestroyed", LookMode.Value, new object[0]);
		}

		public override string ToString()
		{
			return "BattleLogEntry_ExplosionImpact: " + this.InitiatorName + "->" + this.RecipientName;
		}
	}
}
