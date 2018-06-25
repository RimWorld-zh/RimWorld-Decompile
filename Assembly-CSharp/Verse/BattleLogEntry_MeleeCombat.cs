using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BBF RID: 3007
	public class BattleLogEntry_MeleeCombat : LogEntry_DamageResult
	{
		// Token: 0x04002C93 RID: 11411
		private RulePackDef ruleDef;

		// Token: 0x04002C94 RID: 11412
		private Pawn initiator;

		// Token: 0x04002C95 RID: 11413
		private Pawn recipientPawn;

		// Token: 0x04002C96 RID: 11414
		private ThingDef recipientThing;

		// Token: 0x04002C97 RID: 11415
		private ImplementOwnerTypeDef implementType;

		// Token: 0x04002C98 RID: 11416
		private ThingDef ownerEquipmentDef;

		// Token: 0x04002C99 RID: 11417
		private HediffDef ownerHediffDef;

		// Token: 0x04002C9A RID: 11418
		private string toolLabel;

		// Token: 0x04002C9B RID: 11419
		public bool alwaysShowInCompact;

		// Token: 0x04002C9C RID: 11420
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChanceOnMiss = 0.5f;

		// Token: 0x06004145 RID: 16709 RVA: 0x002274C8 File Offset: 0x002258C8
		public BattleLogEntry_MeleeCombat() : base(null)
		{
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x002274D4 File Offset: 0x002258D4
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

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06004147 RID: 16711 RVA: 0x00227584 File Offset: 0x00225984
		private string InitiatorName
		{
			get
			{
				return (this.initiator == null) ? "null" : this.initiator.LabelShort;
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06004148 RID: 16712 RVA: 0x002275BC File Offset: 0x002259BC
		private string RecipientName
		{
			get
			{
				return (this.recipientPawn == null) ? "null" : this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06004149 RID: 16713 RVA: 0x002275F4 File Offset: 0x002259F4
		// (set) Token: 0x0600414A RID: 16714 RVA: 0x0022760F File Offset: 0x00225A0F
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

		// Token: 0x0600414B RID: 16715 RVA: 0x00227620 File Offset: 0x00225A20
		public override bool Concerns(Thing t)
		{
			return t == this.initiator || t == this.recipientPawn;
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x00227650 File Offset: 0x00225A50
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

		// Token: 0x0600414D RID: 16717 RVA: 0x0022767C File Offset: 0x00225A7C
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

		// Token: 0x0600414E RID: 16718 RVA: 0x002276E8 File Offset: 0x00225AE8
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

		// Token: 0x0600414F RID: 16719 RVA: 0x00227788 File Offset: 0x00225B88
		protected override BodyDef DamagedBody()
		{
			return (this.recipientPawn == null) ? null : this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x002277C0 File Offset: 0x00225BC0
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

		// Token: 0x06004151 RID: 16721 RVA: 0x00227958 File Offset: 0x00225D58
		public override bool ShowInCompactView()
		{
			return this.alwaysShowInCompact || Rand.ChanceSeeded(BattleLogEntry_MeleeCombat.DisplayChanceOnMiss, this.logID);
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x00227990 File Offset: 0x00225D90
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
			Scribe_Values.Look<string>(ref this.toolLabel, "toolLabel", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				BackCompatibility.BattleLogEntry_MeleeCombat_LoadingVars(this);
			}
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x00227A3C File Offset: 0x00225E3C
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
	}
}
