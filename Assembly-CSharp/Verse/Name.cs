using System;

namespace Verse
{
	// Token: 0x02000D49 RID: 3401
	public abstract class Name : IExposable
	{
		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06004AF1 RID: 19185
		public abstract string ToStringFull { get; }

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06004AF2 RID: 19186
		public abstract string ToStringShort { get; }

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06004AF3 RID: 19187
		public abstract bool IsValid { get; }

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06004AF4 RID: 19188 RVA: 0x00271F20 File Offset: 0x00270320
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

		// Token: 0x06004AF5 RID: 19189
		public abstract bool ConfusinglySimilarTo(Name other);

		// Token: 0x06004AF6 RID: 19190
		public abstract void ExposeData();

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06004AF7 RID: 19191
		public abstract bool Numerical { get; }
	}
}
