using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse.Grammar;

namespace Verse
{
	public static class PlayLogEntryUtility
	{
		public static IEnumerable<Rule> RulesForOptionalWeapon(string prefix, ThingDef weaponDef, ThingDef projectileDef)
		{
			if (weaponDef != null)
			{
				foreach (Rule rule in GrammarUtility.RulesForDef(prefix, weaponDef))
				{
					yield return rule;
				}
				ThingDef projectile = projectileDef;
				if (projectile == null && !weaponDef.Verbs.NullOrEmpty<VerbProperties>())
				{
					projectile = weaponDef.Verbs[0].defaultProjectile;
				}
				if (projectile != null)
				{
					foreach (Rule rule2 in GrammarUtility.RulesForDef(prefix + "_projectile", projectile))
					{
						yield return rule2;
					}
				}
			}
			yield break;
		}

		public static IEnumerable<Rule> RulesForDamagedParts(string prefix, BodyDef body, List<BodyPartRecord> bodyParts, List<bool> bodyPartsDestroyed, Dictionary<string, string> constants)
		{
			if (bodyParts != null)
			{
				int destroyedIndex = 0;
				int damagedIndex = 0;
				for (int i = 0; i < bodyParts.Count; i++)
				{
					yield return new Rule_String(string.Format(prefix + "{0}_label", i), bodyParts[i].Label);
					constants[string.Format(prefix + "{0}_destroyed", i)] = bodyPartsDestroyed[i].ToString();
					if (bodyPartsDestroyed[i])
					{
						yield return new Rule_String(string.Format(prefix + "_destroyed{0}_label", destroyedIndex), bodyParts[i].Label);
						constants[string.Format("{0}_destroyed{1}_outside", prefix, destroyedIndex)] = (bodyParts[i].depth == BodyPartDepth.Outside).ToString();
						destroyedIndex++;
					}
					else
					{
						yield return new Rule_String(string.Format(prefix + "_damaged{0}_label", damagedIndex), bodyParts[i].Label);
						constants[string.Format("{0}_damaged{1}_outside", prefix, damagedIndex)] = (bodyParts[i].depth == BodyPartDepth.Outside).ToString();
						damagedIndex++;
					}
				}
				constants[prefix + "_count"] = bodyParts.Count.ToString();
				constants[prefix + "_destroyed_count"] = destroyedIndex.ToString();
				constants[prefix + "_damaged_count"] = damagedIndex.ToString();
			}
			else
			{
				constants[prefix + "_count"] = "0";
				constants[prefix + "_destroyed_count"] = "0";
				constants[prefix + "_damaged_count"] = "0";
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <RulesForOptionalWeapon>c__Iterator0 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal ThingDef weaponDef;

			internal string prefix;

			internal IEnumerator<Rule> $locvar0;

			internal Rule <rule>__1;

			internal ThingDef projectileDef;

			internal ThingDef <projectile>__2;

			internal IEnumerator<Rule> $locvar1;

			internal Rule <rule>__3;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RulesForOptionalWeapon>c__Iterator0()
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
					if (weaponDef == null)
					{
						goto IL_1B5;
					}
					enumerator = GrammarUtility.RulesForDef(prefix, weaponDef).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_141;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						rule = enumerator.Current;
						this.$current = rule;
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
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				projectile = projectileDef;
				if (projectile == null && !weaponDef.Verbs.NullOrEmpty<VerbProperties>())
				{
					projectile = weaponDef.Verbs[0].defaultProjectile;
				}
				if (projectile == null)
				{
					goto IL_1B5;
				}
				enumerator2 = GrammarUtility.RulesForDef(prefix + "_projectile", projectile).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_141:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						rule2 = enumerator2.Current;
						this.$current = rule2;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				IL_1B5:
				this.$PC = -1;
				return false;
			}

			Rule IEnumerator<Rule>.Current
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
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
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
				return this.System.Collections.Generic.IEnumerable<Verse.Grammar.Rule>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PlayLogEntryUtility.<RulesForOptionalWeapon>c__Iterator0 <RulesForOptionalWeapon>c__Iterator = new PlayLogEntryUtility.<RulesForOptionalWeapon>c__Iterator0();
				<RulesForOptionalWeapon>c__Iterator.weaponDef = weaponDef;
				<RulesForOptionalWeapon>c__Iterator.prefix = prefix;
				<RulesForOptionalWeapon>c__Iterator.projectileDef = projectileDef;
				return <RulesForOptionalWeapon>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <RulesForDamagedParts>c__Iterator1 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal List<BodyPartRecord> bodyParts;

			internal int <destroyedIndex>__1;

			internal int <damagedIndex>__1;

			internal int <i>__2;

			internal string prefix;

			internal List<bool> bodyPartsDestroyed;

			internal Dictionary<string, string> constants;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RulesForDamagedParts>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (bodyParts != null)
					{
						destroyedIndex = 0;
						damagedIndex = 0;
						i = 0;
						goto IL_276;
					}
					constants[prefix + "_count"] = "0";
					constants[prefix + "_destroyed_count"] = "0";
					constants[prefix + "_damaged_count"] = "0";
					goto IL_37E;
				case 1u:
					constants[string.Format(prefix + "{0}_destroyed", i)] = bodyPartsDestroyed[i].ToString();
					if (bodyPartsDestroyed[i])
					{
						this.$current = new Rule_String(string.Format(prefix + "_destroyed{0}_label", destroyedIndex), bodyParts[i].Label);
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					this.$current = new Rule_String(string.Format(prefix + "_damaged{0}_label", damagedIndex), bodyParts[i].Label);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 2u:
					constants[string.Format("{0}_destroyed{1}_outside", prefix, destroyedIndex)] = (bodyParts[i].depth == BodyPartDepth.Outside).ToString();
					destroyedIndex++;
					break;
				case 3u:
					constants[string.Format("{0}_damaged{1}_outside", prefix, damagedIndex)] = (bodyParts[i].depth == BodyPartDepth.Outside).ToString();
					damagedIndex++;
					break;
				default:
					return false;
				}
				i++;
				IL_276:
				if (i < bodyParts.Count)
				{
					this.$current = new Rule_String(string.Format(prefix + "{0}_label", i), bodyParts[i].Label);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				constants[prefix + "_count"] = bodyParts.Count.ToString();
				constants[prefix + "_destroyed_count"] = destroyedIndex.ToString();
				constants[prefix + "_damaged_count"] = damagedIndex.ToString();
				IL_37E:
				this.$PC = -1;
				return false;
			}

			Rule IEnumerator<Rule>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Grammar.Rule>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PlayLogEntryUtility.<RulesForDamagedParts>c__Iterator1 <RulesForDamagedParts>c__Iterator = new PlayLogEntryUtility.<RulesForDamagedParts>c__Iterator1();
				<RulesForDamagedParts>c__Iterator.bodyParts = bodyParts;
				<RulesForDamagedParts>c__Iterator.prefix = prefix;
				<RulesForDamagedParts>c__Iterator.bodyPartsDestroyed = bodyPartsDestroyed;
				<RulesForDamagedParts>c__Iterator.constants = constants;
				return <RulesForDamagedParts>c__Iterator;
			}
		}
	}
}
