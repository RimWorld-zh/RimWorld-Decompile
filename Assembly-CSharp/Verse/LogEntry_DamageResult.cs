using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BCD RID: 3021
	public abstract class LogEntry_DamageResult : LogEntry
	{
		// Token: 0x060041C5 RID: 16837 RVA: 0x00225D0D File Offset: 0x0022410D
		public LogEntry_DamageResult(LogEntryDef def = null) : base(def)
		{
		}

		// Token: 0x060041C6 RID: 16838 RVA: 0x00225D17 File Offset: 0x00224117
		public void FillTargets(List<BodyPartRecord> recipientParts, List<bool> recipientPartsDestroyed, bool deflected)
		{
			this.damagedParts = recipientParts;
			this.damagedPartsDestroyed = recipientPartsDestroyed;
			this.deflected = deflected;
			base.ResetCache();
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x00225D38 File Offset: 0x00224138
		protected virtual BodyDef DamagedBody()
		{
			return null;
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x00225D50 File Offset: 0x00224150
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", this.DamagedBody(), this.damagedParts, this.damagedPartsDestroyed, result.Constants));
			result.Constants.Add("deflected", this.deflected.ToString());
			return result;
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x00225DC0 File Offset: 0x002241C0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<BodyPartRecord>(ref this.damagedParts, "damagedParts", LookMode.BodyPart, new object[0]);
			Scribe_Collections.Look<bool>(ref this.damagedPartsDestroyed, "damagedPartsDestroyed", LookMode.Value, new object[0]);
			Scribe_Values.Look<bool>(ref this.deflected, "deflected", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.damagedParts != null)
				{
					for (int i = this.damagedParts.Count - 1; i >= 0; i--)
					{
						if (this.damagedParts[i] == null)
						{
							this.damagedParts.RemoveAt(i);
							if (i < this.damagedPartsDestroyed.Count)
							{
								this.damagedPartsDestroyed.RemoveAt(i);
							}
						}
					}
				}
			}
		}

		// Token: 0x04002CEC RID: 11500
		protected List<BodyPartRecord> damagedParts;

		// Token: 0x04002CED RID: 11501
		protected List<bool> damagedPartsDestroyed;

		// Token: 0x04002CEE RID: 11502
		protected bool deflected;
	}
}
