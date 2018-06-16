using System;

namespace Verse
{
	// Token: 0x02000D4A RID: 3402
	public abstract class Name : IExposable
	{
		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06004ADB RID: 19163
		public abstract string ToStringFull { get; }

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06004ADC RID: 19164
		public abstract string ToStringShort { get; }

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06004ADD RID: 19165
		public abstract bool IsValid { get; }

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06004ADE RID: 19166 RVA: 0x002705E0 File Offset: 0x0026E9E0
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

		// Token: 0x06004ADF RID: 19167
		public abstract bool ConfusinglySimilarTo(Name other);

		// Token: 0x06004AE0 RID: 19168
		public abstract void ExposeData();

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06004AE1 RID: 19169
		public abstract bool Numerical { get; }
	}
}
