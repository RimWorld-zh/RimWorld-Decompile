using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E2 RID: 738
	public abstract class ThingSetMaker
	{
		// Token: 0x06000C1C RID: 3100 RVA: 0x0006B9BC File Offset: 0x00069DBC
		static ThingSetMaker()
		{
			Gen.EnsureAllFieldsNullable(typeof(ThingSetMakerParams));
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0006B9E0 File Offset: 0x00069DE0
		public List<Thing> Generate()
		{
			return this.Generate(default(ThingSetMakerParams));
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0006BA04 File Offset: 0x00069E04
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

		// Token: 0x06000C20 RID: 3104 RVA: 0x0006BAC0 File Offset: 0x00069EC0
		public bool CanGenerate(ThingSetMakerParams parms)
		{
			ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
			return this.CanGenerateSub(parms2);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0006BAE4 File Offset: 0x00069EE4
		protected virtual bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return true;
		}

		// Token: 0x06000C22 RID: 3106
		protected abstract void Generate(ThingSetMakerParams parms, List<Thing> outThings);

		// Token: 0x06000C23 RID: 3107 RVA: 0x0006BAFC File Offset: 0x00069EFC
		public IEnumerable<ThingDef> AllGeneratableThingsDebug()
		{
			return this.AllGeneratableThingsDebug(default(ThingSetMakerParams));
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0006BB20 File Offset: 0x00069F20
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

		// Token: 0x06000C25 RID: 3109
		protected abstract IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms);

		// Token: 0x06000C26 RID: 3110 RVA: 0x0006BB54 File Offset: 0x00069F54
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

		// Token: 0x06000C27 RID: 3111 RVA: 0x0006BC78 File Offset: 0x0006A078
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

		// Token: 0x06000C28 RID: 3112 RVA: 0x0006BCF0 File Offset: 0x0006A0F0
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

		// Token: 0x06000C29 RID: 3113 RVA: 0x0006BD48 File Offset: 0x0006A148
		private ThingSetMakerParams ApplyFixedParams(ThingSetMakerParams parms)
		{
			ThingSetMakerParams result = this.fixedParams;
			Gen.ReplaceNullFields<ThingSetMakerParams>(ref result, parms);
			return result;
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0006BD6D File Offset: 0x0006A16D
		public virtual void ResolveReferences()
		{
			if (this.fixedParams.filter != null)
			{
				this.fixedParams.filter.ResolveReferences();
			}
		}

		// Token: 0x040007A4 RID: 1956
		public ThingSetMakerParams fixedParams;

		// Token: 0x040007A5 RID: 1957
		public static List<List<Thing>> thingsBeingGeneratedNow = new List<List<Thing>>();
	}
}
