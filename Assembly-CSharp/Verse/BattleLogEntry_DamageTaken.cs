using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BBE RID: 3006
	public class BattleLogEntry_DamageTaken : LogEntry_DamageResult
	{
		// Token: 0x06004120 RID: 16672 RVA: 0x00225F5E File Offset: 0x0022435E
		public BattleLogEntry_DamageTaken() : base(null)
		{
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x00225F68 File Offset: 0x00224368
		public BattleLogEntry_DamageTaken(Pawn recipient, RulePackDef ruleDef, Pawn initiator = null) : base(null)
		{
			this.initiatorPawn = initiator;
			this.recipientPawn = recipient;
			this.ruleDef = ruleDef;
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06004122 RID: 16674 RVA: 0x00225F88 File Offset: 0x00224388
		private string RecipientName
		{
			get
			{
				return (this.recipientPawn == null) ? "null" : this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x00225FC0 File Offset: 0x002243C0
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x00225FF0 File Offset: 0x002243F0
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

		// Token: 0x06004125 RID: 16677 RVA: 0x0022601A File Offset: 0x0022441A
		public override void ClickedFromPOV(Thing pov)
		{
			CameraJumper.TryJumpAndSelect(this.recipientPawn);
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x00226030 File Offset: 0x00224430
		public override Texture2D IconFromPOV(Thing pov)
		{
			return LogEntry.Blood;
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x0022604C File Offset: 0x0022444C
		protected override BodyDef DamagedBody()
		{
			return (this.recipientPawn == null) ? null : this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x00226084 File Offset: 0x00224484
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.recipientPawn == null)
			{
				Log.ErrorOnce("BattleLogEntry_DamageTaken has a null recipient.", 60465709, false);
			}
			result.Includes.Add(this.ruleDef);
			result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants));
			return result;
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x002260F1 File Offset: 0x002244F1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x0022612C File Offset: 0x0022452C
		public override string ToString()
		{
			return "BattleLogEntry_DamageTaken: " + this.RecipientName;
		}

		// Token: 0x04002C7F RID: 11391
		private Pawn initiatorPawn;

		// Token: 0x04002C80 RID: 11392
		private Pawn recipientPawn;

		// Token: 0x04002C81 RID: 11393
		private RulePackDef ruleDef;
	}
}
