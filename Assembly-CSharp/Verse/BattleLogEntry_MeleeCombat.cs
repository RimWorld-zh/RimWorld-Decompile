using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BC0 RID: 3008
	public class BattleLogEntry_MeleeCombat : LogEntry_DamageResult
	{
		// Token: 0x04002C9A RID: 11418
		private RulePackDef ruleDef;

		// Token: 0x04002C9B RID: 11419
		private Pawn initiator;

		// Token: 0x04002C9C RID: 11420
		private Pawn recipientPawn;

		// Token: 0x04002C9D RID: 11421
		private ThingDef recipientThing;

		// Token: 0x04002C9E RID: 11422
		private ImplementOwnerTypeDef implementType;

		// Token: 0x04002C9F RID: 11423
		private ThingDef ownerEquipmentDef;

		// Token: 0x04002CA0 RID: 11424
		private HediffDef ownerHediffDef;

		// Token: 0x04002CA1 RID: 11425
		private string toolLabel;

		// Token: 0x04002CA2 RID: 11426
		public bool alwaysShowInCompact;

		// Token: 0x04002CA3 RID: 11427
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChanceOnMiss = 0.5f;

		// Token: 0x06004145 RID: 16709 RVA: 0x002277A8 File Offset: 0x00225BA8
		public BattleLogEntry_MeleeCombat() : base(null)
		{
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x002277B4 File Offset: 0x00225BB4
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
		// (get) Token: 0x06004147 RID: 16711 RVA: 0x00227864 File Offset: 0x00225C64
		private string InitiatorName
		{
			get
			{
				return (this.initiator == null) ? "null" : this.initiator.LabelShort;
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06004148 RID: 16712 RVA: 0x0022789C File Offset: 0x00225C9C
		private string RecipientName
		{
			get
			{
				return (this.recipientPawn == null) ? "null" : this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06004149 RID: 16713 RVA: 0x002278D4 File Offset: 0x00225CD4
		// (set) Token: 0x0600414A RID: 16714 RVA: 0x002278EF File Offset: 0x00225CEF
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

		// Token: 0x0600414B RID: 16715 RVA: 0x00227900 File Offset: 0x00225D00
		public override bool Concerns(Thing t)
		{
			return t == this.initiator || t == this.recipientPawn;
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x00227930 File Offset: 0x00225D30
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

		// Token: 0x0600414D RID: 16717 RVA: 0x0022795C File Offset: 0x00225D5C
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

		// Token: 0x0600414E RID: 16718 RVA: 0x002279C8 File Offset: 0x00225DC8
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

		// Token: 0x0600414F RID: 16719 RVA: 0x00227A68 File Offset: 0x00225E68
		protected override BodyDef DamagedBody()
		{
			return (this.recipientPawn == null) ? null : this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x00227AA0 File Offset: 0x00225EA0
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

		// Token: 0x06004151 RID: 16721 RVA: 0x00227C38 File Offset: 0x00226038
		public override bool ShowInCompactView()
		{
			return this.alwaysShowInCompact || Rand.ChanceSeeded(BattleLogEntry_MeleeCombat.DisplayChanceOnMiss, this.logID);
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x00227C70 File Offset: 0x00226070
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

		// Token: 0x06004153 RID: 16723 RVA: 0x00227D1C File Offset: 0x0022611C
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
