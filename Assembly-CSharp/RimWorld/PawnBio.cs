using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E4 RID: 1252
	[CaseInsensitiveXMLParsing]
	public class PawnBio
	{
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x0600164E RID: 5710 RVA: 0x000C5C3C File Offset: 0x000C403C
		public PawnBioType BioType
		{
			get
			{
				PawnBioType result;
				if (this.pirateKing)
				{
					result = PawnBioType.PirateKing;
				}
				else if (this.adulthood != null)
				{
					result = PawnBioType.BackstoryInGame;
				}
				else
				{
					result = PawnBioType.Undefined;
				}
				return result;
			}
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x000C5C76 File Offset: 0x000C4076
		public void PostLoad()
		{
			if (this.childhood != null)
			{
				this.childhood.PostLoad();
			}
			if (this.adulthood != null)
			{
				this.adulthood.PostLoad();
			}
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x000C5CA8 File Offset: 0x000C40A8
		public void ResolveReferences()
		{
			if (this.adulthood.spawnCategories.Count == 1 && this.adulthood.spawnCategories[0] == "Trader")
			{
				this.adulthood.spawnCategories.Add("Civil");
			}
			if (this.childhood != null)
			{
				this.childhood.ResolveReferences();
			}
			if (this.adulthood != null)
			{
				this.adulthood.ResolveReferences();
			}
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x000C5D30 File Offset: 0x000C4130
		public IEnumerable<string> ConfigErrors()
		{
			if (this.childhood != null)
			{
				foreach (string error in this.childhood.ConfigErrors(true))
				{
					yield return string.Concat(new object[]
					{
						this.name,
						", ",
						this.childhood.title,
						": ",
						error
					});
				}
			}
			if (this.adulthood != null)
			{
				foreach (string error2 in this.adulthood.ConfigErrors(false))
				{
					yield return string.Concat(new object[]
					{
						this.name,
						", ",
						this.adulthood.title,
						": ",
						error2
					});
				}
			}
			yield break;
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x000C5D5C File Offset: 0x000C415C
		public override string ToString()
		{
			return "PawnBio(" + this.name + ")";
		}

		// Token: 0x04000D01 RID: 3329
		public GenderPossibility gender;

		// Token: 0x04000D02 RID: 3330
		public NameTriple name;

		// Token: 0x04000D03 RID: 3331
		public Backstory childhood;

		// Token: 0x04000D04 RID: 3332
		public Backstory adulthood;

		// Token: 0x04000D05 RID: 3333
		public bool pirateKing = false;
	}
}
