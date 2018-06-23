using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BC9 RID: 3017
	public abstract class LogEntry_DamageResult : LogEntry
	{
		// Token: 0x04002CF1 RID: 11505
		protected List<BodyPartRecord> damagedParts;

		// Token: 0x04002CF2 RID: 11506
		protected List<bool> damagedPartsDestroyed;

		// Token: 0x04002CF3 RID: 11507
		protected bool deflected;

		// Token: 0x060041C9 RID: 16841 RVA: 0x002264B5 File Offset: 0x002248B5
		public LogEntry_DamageResult(LogEntryDef def = null) : base(def)
		{
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x002264BF File Offset: 0x002248BF
		public void FillTargets(List<BodyPartRecord> recipientParts, List<bool> recipientPartsDestroyed, bool deflected)
		{
			this.damagedParts = recipientParts;
			this.damagedPartsDestroyed = recipientPartsDestroyed;
			this.deflected = deflected;
			base.ResetCache();
		}

		// Token: 0x060041CB RID: 16843 RVA: 0x002264E0 File Offset: 0x002248E0
		protected virtual BodyDef DamagedBody()
		{
			return null;
		}

		// Token: 0x060041CC RID: 16844 RVA: 0x002264F8 File Offset: 0x002248F8
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", this.DamagedBody(), this.damagedParts, this.damagedPartsDestroyed, result.Constants));
			result.Constants.Add("deflected", this.deflected.ToString());
			return result;
		}

		// Token: 0x060041CD RID: 16845 RVA: 0x00226568 File Offset: 0x00224968
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
	}
}
