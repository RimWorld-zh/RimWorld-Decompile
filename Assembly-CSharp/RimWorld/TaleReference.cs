using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000665 RID: 1637
	public class TaleReference : IExposable
	{
		// Token: 0x0600223F RID: 8767 RVA: 0x0012282E File Offset: 0x00120C2E
		public TaleReference()
		{
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x00122837 File Offset: 0x00120C37
		public TaleReference(Tale tale)
		{
			this.tale = tale;
			this.seed = Rand.Range(0, int.MaxValue);
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06002241 RID: 8769 RVA: 0x00122858 File Offset: 0x00120C58
		public static TaleReference Taleless
		{
			get
			{
				return new TaleReference(null);
			}
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x00122873 File Offset: 0x00120C73
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.seed, "seed", 0, false);
			Scribe_References.Look<Tale>(ref this.tale, "tale", false);
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x00122899 File Offset: 0x00120C99
		public void ReferenceDestroyed()
		{
			if (this.tale != null)
			{
				this.tale.Notify_ReferenceDestroyed();
				this.tale = null;
			}
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x001228BC File Offset: 0x00120CBC
		public string GenerateText(TextGenerationPurpose purpose, RulePackDef extraInclude)
		{
			return TaleTextGenerator.GenerateTextFromTale(purpose, this.tale, this.seed, extraInclude);
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x001228E4 File Offset: 0x00120CE4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"TaleReference(tale=",
				(this.tale != null) ? this.tale.ToString() : "null",
				", seed=",
				this.seed,
				")"
			});
		}

		// Token: 0x04001374 RID: 4980
		private Tale tale;

		// Token: 0x04001375 RID: 4981
		private int seed;
	}
}
