using System;

namespace Verse
{
	// Token: 0x02000D46 RID: 3398
	public abstract class Name : IExposable
	{
		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06004AED RID: 19181
		public abstract string ToStringFull { get; }

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06004AEE RID: 19182
		public abstract string ToStringShort { get; }

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06004AEF RID: 19183
		public abstract bool IsValid { get; }

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06004AF0 RID: 19184 RVA: 0x00271B14 File Offset: 0x0026FF14
		public bool UsedThisGame
		{
			get
			{
				foreach (Name name in NameUseChecker.AllPawnsNamesEverUsed)
				{
					if (name.ConfusinglySimilarTo(this))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06004AF1 RID: 19185
		public abstract bool ConfusinglySimilarTo(Name other);

		// Token: 0x06004AF2 RID: 19186
		public abstract void ExposeData();

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06004AF3 RID: 19187
		public abstract bool Numerical { get; }
	}
}
