using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse
{
	public static class GenTypes
	{
		public static readonly List<string> IgnoredNamespaceNames = new List<string>
		{
			"RimWorld",
			"Verse",
			"Verse.AI",
			"Verse.Sound",
			"Verse.Grammar",
			"RimWorld.Planet",
			"RimWorld.BaseGen"
		};

		[CompilerGenerated]
		private static Func<Type, bool> <>f__am$cache0;

		private static IEnumerable<Assembly> AllActiveAssemblies
		{
			get
			{
				yield return Assembly.GetExecutingAssembly();
				foreach (ModContentPack mod in LoadedModManager.RunningMods)
				{
					for (int i = 0; i < mod.assemblies.loadedAssemblies.Count; i++)
					{
						yield return mod.assemblies.loadedAssemblies[i];
					}
				}
				yield break;
			}
		}

		public static IEnumerable<Type> AllTypes
		{
			get
			{
				foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
				{
					Type[] assemblyTypes = null;
					try
					{
						assemblyTypes = assembly.GetTypes();
					}
					catch (ReflectionTypeLoadException)
					{
						Log.Error("Exception getting types in assembly " + assembly.ToString(), false);
					}
					if (assemblyTypes != null)
					{
						foreach (Type type in assemblyTypes)
						{
							yield return type;
						}
					}
				}
				yield break;
			}
		}

		public static IEnumerable<Type> AllTypesWithAttribute<TAttr>() where TAttr : Attribute
		{
			return GenTypes.AllTypes.Where(new Func<Type, bool>(GenAttribute.HasAttribute<TAttr>));
		}

		public static IEnumerable<Type> AllSubclasses(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType)
			select x;
		}

		public static IEnumerable<Type> AllSubclassesNonAbstract(this Type baseType)
		{
			return from x in GenTypes.AllTypes
			where x.IsSubclassOf(baseType) && !x.IsAbstract
			select x;
		}

		public static IEnumerable<Type> AllLeafSubclasses(this Type baseType)
		{
			return from type in baseType.AllSubclasses()
			where !type.AllSubclasses().Any<Type>()
			select type;
		}

		public static IEnumerable<Type> InstantiableDescendantsAndSelf(this Type baseType)
		{
			if (!baseType.IsAbstract)
			{
				yield return baseType;
			}
			foreach (Type descendant in baseType.AllSubclasses())
			{
				if (!descendant.IsAbstract)
				{
					yield return descendant;
				}
			}
			yield break;
		}

		public static Type GetTypeInAnyAssembly(string typeName)
		{
			Type typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(typeName);
			if (typeInAnyAssemblyRaw != null)
			{
				return typeInAnyAssemblyRaw;
			}
			for (int i = 0; i < GenTypes.IgnoredNamespaceNames.Count; i++)
			{
				string typeName2 = GenTypes.IgnoredNamespaceNames[i] + "." + typeName;
				typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(typeName2);
				if (typeInAnyAssemblyRaw != null)
				{
					return typeInAnyAssemblyRaw;
				}
			}
			return null;
		}

		private static Type GetTypeInAnyAssemblyRaw(string typeName)
		{
			foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
			{
				Type type = assembly.GetType(typeName, false, true);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		public static string GetTypeNameWithoutIgnoredNamespaces(Type type)
		{
			if (type.IsGenericType)
			{
				return type.ToString();
			}
			for (int i = 0; i < GenTypes.IgnoredNamespaceNames.Count; i++)
			{
				if (type.Namespace == GenTypes.IgnoredNamespaceNames[i])
				{
					return type.Name;
				}
			}
			return type.FullName;
		}

		public static bool IsCustomType(Type type)
		{
			string @namespace = type.Namespace;
			return !@namespace.StartsWith("System") && !@namespace.StartsWith("UnityEngine") && !@namespace.StartsWith("Steamworks");
		}

		// Note: this type is marked as 'beforefieldinit'.
		static GenTypes()
		{
		}

		[CompilerGenerated]
		private static bool <AllLeafSubclasses>m__0(Type type)
		{
			return !type.AllSubclasses().Any<Type>();
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Assembly>, IEnumerator, IDisposable, IEnumerator<Assembly>
		{
			internal IEnumerator<ModContentPack> $locvar0;

			internal ModContentPack <mod>__1;

			internal int <i>__2;

			internal Assembly $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
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
					this.$current = Assembly.GetExecutingAssembly();
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					enumerator = LoadedModManager.RunningMods.GetEnumerator();
					num = 4294967293u;
					break;
				case 2u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 2u:
						i++;
						break;
					default:
						goto IL_EC;
					}
					IL_CC:
					if (i < mod.assemblies.loadedAssemblies.Count)
					{
						this.$current = mod.assemblies.loadedAssemblies[i];
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
					IL_EC:
					if (enumerator.MoveNext())
					{
						mod = enumerator.Current;
						i = 0;
						goto IL_CC;
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

			Assembly IEnumerator<Assembly>.Current
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
				case 2u:
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
				return this.System.Collections.Generic.IEnumerable<System.Reflection.Assembly>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Assembly> IEnumerable<Assembly>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new GenTypes.<>c__Iterator0();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<Type>, IEnumerator, IDisposable, IEnumerator<Type>
		{
			internal IEnumerator<Assembly> $locvar0;

			internal Assembly <assembly>__1;

			internal Type[] <assemblyTypes>__2;

			internal Type[] $locvar1;

			internal int $locvar2;

			internal Type <type>__3;

			internal Type $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator1()
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
					enumerator = GenTypes.AllActiveAssemblies.GetEnumerator();
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
						i++;
						goto IL_FC;
					}
					IL_10F:
					while (enumerator.MoveNext())
					{
						assembly = enumerator.Current;
						assemblyTypes = null;
						try
						{
							assemblyTypes = assembly.GetTypes();
						}
						catch (ReflectionTypeLoadException)
						{
							Log.Error("Exception getting types in assembly " + assembly.ToString(), false);
						}
						if (assemblyTypes != null)
						{
							array = assemblyTypes;
							i = 0;
							goto IL_FC;
						}
					}
					goto IL_13F;
					IL_FC:
					if (i >= array.Length)
					{
						goto IL_10F;
					}
					type = array[i];
					this.$current = type;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					flag = true;
					return true;
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
				IL_13F:
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
				return new GenTypes.<>c__Iterator1();
			}
		}

		[CompilerGenerated]
		private sealed class <AllSubclasses>c__AnonStorey3
		{
			internal Type baseType;

			public <AllSubclasses>c__AnonStorey3()
			{
			}

			internal bool <>m__0(Type x)
			{
				return x.IsSubclassOf(this.baseType);
			}
		}

		[CompilerGenerated]
		private sealed class <AllSubclassesNonAbstract>c__AnonStorey4
		{
			internal Type baseType;

			public <AllSubclassesNonAbstract>c__AnonStorey4()
			{
			}

			internal bool <>m__0(Type x)
			{
				return x.IsSubclassOf(this.baseType) && !x.IsAbstract;
			}
		}

		[CompilerGenerated]
		private sealed class <InstantiableDescendantsAndSelf>c__Iterator2 : IEnumerable, IEnumerable<Type>, IEnumerator, IDisposable, IEnumerator<Type>
		{
			internal Type baseType;

			internal IEnumerator<Type> $locvar0;

			internal Type <descendant>__1;

			internal Type $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <InstantiableDescendantsAndSelf>c__Iterator2()
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
					if (!baseType.IsAbstract)
					{
						this.$current = baseType;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_70;
				default:
					return false;
				}
				enumerator = baseType.AllSubclasses().GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_70:
					switch (num)
					{
					}
					while (enumerator.MoveNext())
					{
						descendant = enumerator.Current;
						if (!descendant.IsAbstract)
						{
							this.$current = descendant;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
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
				case 2u:
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
				GenTypes.<InstantiableDescendantsAndSelf>c__Iterator2 <InstantiableDescendantsAndSelf>c__Iterator = new GenTypes.<InstantiableDescendantsAndSelf>c__Iterator2();
				<InstantiableDescendantsAndSelf>c__Iterator.baseType = baseType;
				return <InstantiableDescendantsAndSelf>c__Iterator;
			}
		}
	}
}
