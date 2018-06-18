using System;

namespace Verse
{
	// Token: 0x02000D49 RID: 3401
	public abstract class Name : IExposable
	{
		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06004AD9 RID: 19161
		public abstract string ToStringFull { get; }

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06004ADA RID: 19162
		public abstract string ToStringShort { get; }

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06004ADB RID: 19163
		public abstract bool IsValid { get; }

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06004ADC RID: 19164 RVA: 0x002705B8 File Offset: 0x0026E9B8
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

		// Token: 0x06004ADD RID: 19165
		public abstract bool ConfusinglySimilarTo(Name other);

		// Token: 0x06004ADE RID: 19166
		public abstract void ExposeData();

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06004ADF RID: 19167
		public abstract bool Numerical { get; }
	}
}
