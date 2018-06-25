using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public static class PawnNameDatabaseSolid
	{
		private static Dictionary<GenderPossibility, List<NameTriple>> solidNames = new Dictionary<GenderPossibility, List<NameTriple>>();

		private const float PreferredNameChance = 0.5f;

		static PawnNameDatabaseSolid()
		{
			IEnumerator enumerator = Enum.GetValues(typeof(GenderPossibility)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					GenderPossibility key = (GenderPossibility)obj;
					PawnNameDatabaseSolid.solidNames.Add(key, new List<NameTriple>());
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		public static void AddPlayerContentName(NameTriple newName, GenderPossibility genderPos)
		{
			PawnNameDatabaseSolid.solidNames[genderPos].Add(newName);
		}

		public static List<NameTriple> GetListForGender(GenderPossibility gp)
		{
			return PawnNameDatabaseSolid.solidNames[gp];
		}

		public static IEnumerable<NameTriple> AllNames()
		{
			foreach (KeyValuePair<GenderPossibility, List<NameTriple>> kvp in PawnNameDatabaseSolid.solidNames)
			{
				foreach (NameTriple name in kvp.Value)
				{
					yield return name;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <AllNames>c__Iterator0 : IEnumerable, IEnumerable<NameTriple>, IEnumerator, IDisposable, IEnumerator<NameTriple>
		{
			internal Dictionary<GenderPossibility, List<NameTriple>>.Enumerator $locvar0;

			internal KeyValuePair<GenderPossibility, List<NameTriple>> <kvp>__1;

			internal List<NameTriple>.Enumerator $locvar1;

			internal NameTriple <name>__2;

			internal NameTriple $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllNames>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = PawnNameDatabaseSolid.solidNames.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						Block_4:
						try
						{
							switch (num)
							{
							}
							if (enumerator2.MoveNext())
							{
								name = enumerator2.Current;
								this.$current = name;
								if (!this.$disposing)
								{
									this.$PC = 1;
								}
								flag = true;
								return true;
							}
						}
						finally
						{
							if (!flag)
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
						break;
					}
					if (enumerator.MoveNext())
					{
						kvp = enumerator.Current;
						enumerator2 = kvp.Value.GetEnumerator();
						num = 4294967293u;
						goto Block_4;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				this.$PC = -1;
				return false;
			}

			NameTriple IEnumerator<NameTriple>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
						try
						{
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.NameTriple>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<NameTriple> IEnumerable<NameTriple>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new PawnNameDatabaseSolid.<AllNames>c__Iterator0();
			}
		}
	}
}
