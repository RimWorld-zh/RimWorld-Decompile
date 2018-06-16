using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BC1 RID: 3009
	public class BattleLogEntry_MeleeCombat : LogEntry_DamageResult
	{
		// Token: 0x0600413E RID: 16702 RVA: 0x00226C70 File Offset: 0x00225070
		public BattleLogEntry_MeleeCombat() : base(null)
		{
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x00226C7C File Offset: 0x0022507C
		public BattleLogEntry_MeleeCombat(RulePackDef ruleDef, bool alwaysShowInCompact, Pawn initiator, Thing recipient, ImplementOwnerTypeDef implementType, string toolLabel, ThingDef ownerEquipmentDef = null, HediffDef ownerHediffDef = null, LogEntryDef def = null) : base(def)
		{
			this.ruleDef = ruleDef;
			this.alwaysShowInCompact = alwaysShowInCompact;
			this.initiator = initiator;
			this.implementType = implementType;
			this.ownerEquipmentDef = ownerEquipmentDef;
			this.ownerHediffDef = ownerHediffDef;
			this.toolLabel = toolLabel;
			if (recipient is Pawn)
			{
				this.recipientPawn = (recipient as Pawn);
			}
			else if (recipient != null)
			{
				this.recipientThing = recipient.def;
			}
			if (ownerEquipmentDef != null && ownerHediffDef != null)
			{
				Log.ErrorOnce(string.Format("Combat log owned by both equipment {0} and hediff {1}, may produce unexpected results", ownerEquipmentDef.label, ownerHediffDef.label), 96474669, false);
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06004140 RID: 16704 RVA: 0x00226D2C File Offset: 0x0022512C
		private string InitiatorName
		{
			get
			{
				return (this.initiator == null) ? "null" : this.initiator.LabelShort;
			}
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06004141 RID: 16705 RVA: 0x00226D64 File Offset: 0x00225164
		private string RecipientName
		{
			get
			{
				return (this.recipientPawn == null) ? "null" : this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06004142 RID: 16706 RVA: 0x00226D9C File Offset: 0x0022519C
		// (set) Token: 0x06004143 RID: 16707 RVA: 0x00226DB7 File Offset: 0x002251B7
		public RulePackDef RuleDef
		{
			get
			{
				return this.ruleDef;
			}
			set
			{
				this.ruleDef = value;
				base.ResetCache();
			}
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x00226DC8 File Offset: 0x002251C8
		public override bool Concerns(Thing t)
		{
			return t == this.initiator || t == this.recipientPawn;
		}

		// Token: 0x06004145 RID: 16709 RVA: 0x00226DF8 File Offset: 0x002251F8
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiator != null)
			{
				yield return this.initiator;
			}
			if (this.recipientPawn != null)
			{
				yield return this.recipientPawn;
			}
			yield break;
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x00226E24 File Offset: 0x00225224
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator && this.recipientPawn != null)
			{
				CameraJumper.TryJumpAndSelect(this.recipientPawn);
			}
			else if (pov == this.recipientPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiator);
			}
			else if (this.recipientPawn != null)
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x00226E90 File Offset: 0x00225290
		public override Texture2D IconFromPOV(Thing pov)
		{
			Texture2D result;
			if (this.damagedParts.NullOrEmpty<BodyPartRecord>())
			{
				result = this.def.iconMissTex;
			}
			else if (this.deflected)
			{
				result = this.def.iconMissTex;
			}
			else if (pov == null || pov == this.recipientPawn)
			{
				result = this.def.iconDamagedTex;
			}
			else if (pov == this.initiator)
			{
				result = this.def.iconDamagedFromInstigatorTex;
			}
			else
			{
				result = this.def.iconDamagedTex;
			}
			return result;
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x00226F30 File Offset: 0x00225330
		protected override BodyDef DamagedBody()
		{
			return (this.recipientPawn == null) ? null : this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06004149 RID: 16713 RVA: 0x00226F68 File Offset: 0x00225368
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, result.Constants));
			if (this.recipientPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants));
			}
			else if (this.recipientThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT", this.recipientThing));
			}
			result.Includes.Add(this.ruleDef);
			if (!this.toolLabel.NullOrEmpty())
			{
				result.Rules.Add(new Rule_String("TOOL_label", this.toolLabel));
			}
			if (this.implementType != null && !this.implementType.implementOwnerRuleName.NullOrEmpty())
			{
				if (this.ownerEquipmentDef != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef(this.implementType.implementOwnerRuleName, this.ownerEquipmentDef));
				}
				else if (this.ownerHediffDef != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef(this.implementType.implementOwnerRuleName, this.ownerHediffDef));
				}
			}
			if (this.implementType != null && !this.implementType.implementOwnerTypeValue.NullOrEmpty())
			{
				result.Constants["IMPLEMENTOWNER_type"] = this.implementType.implementOwnerTypeValue;
			}
			return result;
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x00227100 File Offset: 0x00225500
		public override bool ShowInCompactView()
		{
			return this.alwaysShowInCompact || Rand.ChanceSeeded(BattleLogEntry_MeleeCombat.DisplayChanceOnMiss, this.logID);
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x00227138 File Offset: 0x00225538
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
			Scribe_Values.Look<bool>(ref this.alwaysShowInCompact, "alwaysShowInCompact", false, false);
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ImplementOwnerTypeDef>(ref this.implementType, "implementType");
			Scribe_Defs.Look<ThingDef>(ref this.ownerEquipmentDef, "ownerDef");
			Scribe_Collections.Look<BodyPartRecord>(ref this.damagedParts, "damagedParts", LookMode.Def, new object[0]);
			Scribe_Collections.Look<bool>(ref this.damagedPartsDestroyed, "damagedPartsDestroyed", LookMode.Value, new object[0]);
			Scribe_Values.Look<string>(ref this.toolLabel, "toolLabel", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				BackCompatibility.BattleLogEntry_MeleeCombat_LoadingVars(this);
			}
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x00227214 File Offset: 0x00225614
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.ruleDef.defName,
				": ",
				this.InitiatorName,
				"->",
				this.RecipientName
			});
		}

		// Token: 0x04002C8E RID: 11406
		private RulePackDef ruleDef;

		// Token: 0x04002C8F RID: 11407
		private Pawn initiator;

		// Token: 0x04002C90 RID: 11408
		private Pawn recipientPawn;

		// Token: 0x04002C91 RID: 11409
		private ThingDef recipientThing;

		// Token: 0x04002C92 RID: 11410
		private ImplementOwnerTypeDef implementType;

		// Token: 0x04002C93 RID: 11411
		private ThingDef ownerEquipmentDef;

		// Token: 0x04002C94 RID: 11412
		private HediffDef ownerHediffDef;

		// Token: 0x04002C95 RID: 11413
		private string toolLabel;

		// Token: 0x04002C96 RID: 11414
		public bool alwaysShowInCompact;

		// Token: 0x04002C97 RID: 11415
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChanceOnMiss = 0.5f;
	}
}
