using System;

namespace Verse
{
	public abstract class Name : IExposable
	{
		protected Name()
		{
		}

		public abstract string ToStringFull { get; }

		public abstract string ToStringShort { get; }

		public abstract bool IsValid { get; }

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

		public abstract bool ConfusinglySimilarTo(Name other);

		public abstract void ExposeData();

		public abstract bool Numerical { get; }
	}
}
