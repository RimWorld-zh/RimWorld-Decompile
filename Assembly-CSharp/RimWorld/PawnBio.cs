using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E0 RID: 1248
	[CaseInsensitiveXMLParsing]
	public class PawnBio
	{
		// Token: 0x04000CFE RID: 3326
		public GenderPossibility gender;

		// Token: 0x04000CFF RID: 3327
		public NameTriple name;

		// Token: 0x04000D00 RID: 3328
		public Backstory childhood;

		// Token: 0x04000D01 RID: 3329
		public Backstory adulthood;

		// Token: 0x04000D02 RID: 3330
		public bool pirateKing = false;

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001646 RID: 5702 RVA: 0x000C5C84 File Offset: 0x000C4084
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

		// Token: 0x06001647 RID: 5703 RVA: 0x000C5CBE File Offset: 0x000C40BE
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

		// Token: 0x06001648 RID: 5704 RVA: 0x000C5CF0 File Offset: 0x000C40F0
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

		// Token: 0x06001649 RID: 5705 RVA: 0x000C5D78 File Offset: 0x000C4178
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

		// Token: 0x0600164A RID: 5706 RVA: 0x000C5DA4 File Offset: 0x000C41A4
		public override string ToString()
		{
			return "PawnBio(" + this.name + ")";
		}
	}
}
