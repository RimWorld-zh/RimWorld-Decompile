using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse
{
	public static class GenDefDatabase
	{
		public static Def GetDef(Type defType, string defName, bool errorOnFail = true)
		{
			return (Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamed", new object[]
			{
				defName,
				errorOnFail
			});
		}

		public static Def GetDefSilentFail(Type type, string targetDefName, bool specialCaseForSoundDefs = true)
		{
			Def result;
			if (specialCaseForSoundDefs && type == typeof(SoundDef))
			{
				result = SoundDef.Named(targetDefName);
			}
			else
			{
				result = (Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), type, "GetNamedSilentFail", new object[]
				{
					targetDefName
				});
			}
			return result;
		}

		public static IEnumerable<Type> AllDefTypesWithDatabases()
		{
			foreach (Type defType in typeof(Def).AllSubclasses())
			{
				if (!defType.IsAbstract)
				{
					if (defType != typeof(Def))
					{
						bool foundNonAbstractAncestor = false;
						Type parent = defType.BaseType;
						while (parent != null && parent != typeof(Def))
						{
							if (!parent.IsAbstract)
							{
								foundNonAbstractAncestor = true;
								break;
							}
							parent = parent.BaseType;
						}
						if (!foundNonAbstractAncestor)
						{
							yield return defType;
						}
					}
				}
			}
			yield break;
		}

		public static IEnumerable<T> DefsToGoInDatabase<T>(ModContentPack mod)
		{
			return mod.AllDefs.OfType<T>();
		}

		[CompilerGenerated]
		private sealed class <AllDefTypesWithDatabases>c__Iterator0 : IEnumerable, IEnumerable<Type>, IEnumerator, IDisposable, IEnumerator<Type>
		{
			internal IEnumerator<Type> $locvar0;

			internal Type <defType>__1;

			internal bool <foundNonAbstractAncestor>__2;

			internal Type <parent>__2;

			internal Type $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllDefTypesWithDatabases>c__Iterator0()
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
					enumerator = typeof(Def).AllSubclasses().GetEnumerator();
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
					}
					while (enumerator.MoveNext())
					{
						defType = enumerator.Current;
						if (!defType.IsAbstract)
						{
							if (defType != typeof(Def))
							{
								foundNonAbstractAncestor = false;
								parent = defType.BaseType;
								while (parent != null && parent != typeof(Def))
								{
									if (!parent.IsAbstract)
									{
										foundNonAbstractAncestor = true;
										break;
									}
									parent = parent.BaseType;
								}
								if (!foundNonAbstractAncestor)
								{
									this.$current = defType;
									if (!this.$disposing)
									{
										this.$PC = 1;
									}
									flag = true;
									return true;
								}
							}
						}
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			Type IEnumerator<Type>.Current
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
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
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
				return this.System.Collections.Generic.IEnumerable<System.Type>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Type> IEnumerable<Type>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new GenDefDatabase.<AllDefTypesWithDatabases>c__Iterator0();
			}
		}
	}
}
