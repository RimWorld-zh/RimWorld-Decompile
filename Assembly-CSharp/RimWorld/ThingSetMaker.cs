using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public abstract class ThingSetMaker
	{
		public ThingSetMakerParams fixedParams;

		public static List<List<Thing>> thingsBeingGeneratedNow = new List<List<Thing>>();

		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache0;

		static ThingSetMaker()
		{
			Gen.EnsureAllFieldsNullable(typeof(ThingSetMakerParams));
		}

		protected ThingSetMaker()
		{
		}

		public List<Thing> Generate()
		{
			return this.Generate(default(ThingSetMakerParams));
		}

		public List<Thing> Generate(ThingSetMakerParams parms)
		{
			List<Thing> list = new List<Thing>();
			ThingSetMaker.thingsBeingGeneratedNow.Add(list);
			try
			{
				ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
				this.Generate(parms2, list);
				this.PostProcess(list);
			}
			catch (Exception arg)
			{
				Log.Error("Exception while generating thing set: " + arg, false);
				for (int i = list.Count - 1; i >= 0; i--)
				{
					list[i].Destroy(DestroyMode.Vanish);
					list.RemoveAt(i);
				}
			}
			finally
			{
				ThingSetMaker.thingsBeingGeneratedNow.Remove(list);
			}
			return list;
		}

		public bool CanGenerate(ThingSetMakerParams parms)
		{
			ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
			return this.CanGenerateSub(parms2);
		}

		protected virtual bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return true;
		}

		protected abstract void Generate(ThingSetMakerParams parms, List<Thing> outThings);

		public IEnumerable<ThingDef> AllGeneratableThingsDebug()
		{
			return this.AllGeneratableThingsDebug(default(ThingSetMakerParams));
		}

		public IEnumerable<ThingDef> AllGeneratableThingsDebug(ThingSetMakerParams parms)
		{
			if (!this.CanGenerate(parms))
			{
				yield break;
			}
			ThingSetMakerParams parmsToUse = this.ApplyFixedParams(parms);
			foreach (ThingDef t in this.AllGeneratableThingsDebugSub(parmsToUse).Distinct<ThingDef>())
			{
				yield return t;
			}
			yield break;
		}

		protected abstract IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms);

		private void PostProcess(List<Thing> things)
		{
			if (things.RemoveAll((Thing x) => x == null) != 0)
			{
				Log.Error(base.GetType() + " generated null things.", false);
			}
			this.ChangeDeadPawnsToTheirCorpses(things);
			for (int i = things.Count - 1; i >= 0; i--)
			{
				if (things[i].Destroyed)
				{
					Log.Error(base.GetType() + " generated destroyed thing " + things[i].ToStringSafe<Thing>(), false);
					things.RemoveAt(i);
				}
				else if (things[i].stackCount <= 0)
				{
					Log.Error(string.Concat(new object[]
					{
						base.GetType(),
						" generated ",
						things[i].ToStringSafe<Thing>(),
						" with stackCount=",
						things[i].stackCount
					}), false);
					things.RemoveAt(i);
				}
			}
			this.Minify(things);
		}

		private void Minify(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].def.Minifiable)
				{
					int stackCount = things[i].stackCount;
					things[i].stackCount = 1;
					MinifiedThing minifiedThing = things[i].MakeMinified();
					minifiedThing.stackCount = stackCount;
					things[i] = minifiedThing;
				}
			}
		}

		private void ChangeDeadPawnsToTheirCorpses(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].ParentHolder is Corpse)
				{
					things[i] = (Corpse)things[i].ParentHolder;
				}
			}
		}

		private ThingSetMakerParams ApplyFixedParams(ThingSetMakerParams parms)
		{
			ThingSetMakerParams result = this.fixedParams;
			Gen.ReplaceNullFields<ThingSetMakerParams>(ref result, parms);
			return result;
		}

		public virtual void ResolveReferences()
		{
			if (this.fixedParams.filter != null)
			{
				this.fixedParams.filter.ResolveReferences();
			}
		}

		[CompilerGenerated]
		private static bool <PostProcess>m__0(Thing x)
		{
			return x == null;
		}

		[CompilerGenerated]
		private sealed class <AllGeneratableThingsDebug>c__Iterator0 : IEnumerable, IEnumerable<ThingDef>, IEnumerator, IDisposable, IEnumerator<ThingDef>
		{
			internal ThingSetMakerParams parms;

			internal ThingSetMakerParams <parmsToUse>__0;

			internal IEnumerator<ThingDef> $locvar0;

			internal ThingDef <t>__1;

			internal ThingSetMaker $this;

			internal ThingDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllGeneratableThingsDebug>c__Iterator0()
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
					if (!base.CanGenerate(parms))
					{
						return false;
					}
					parmsToUse = base.ApplyFixedParams(parms);
					enumerator = this.AllGeneratableThingsDebugSub(parmsToUse).Distinct<ThingDef>().GetEnumerator();
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
					if (enumerator.MoveNext())
					{
						t = enumerator.Current;
						this.$current = t;
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
				this.$PC = -1;
				return false;
			}

			ThingDef IEnumerator<ThingDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.ThingDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ThingSetMaker.<AllGeneratableThingsDebug>c__Iterator0 <AllGeneratableThingsDebug>c__Iterator = new ThingSetMaker.<AllGeneratableThingsDebug>c__Iterator0();
				<AllGeneratableThingsDebug>c__Iterator.$this = this;
				<AllGeneratableThingsDebug>c__Iterator.parms = parms;
				return <AllGeneratableThingsDebug>c__Iterator;
			}
		}
	}
}
