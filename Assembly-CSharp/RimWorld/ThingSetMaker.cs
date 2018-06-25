using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E4 RID: 740
	public abstract class ThingSetMaker
	{
		// Token: 0x040007A7 RID: 1959
		public ThingSetMakerParams fixedParams;

		// Token: 0x040007A8 RID: 1960
		public static List<List<Thing>> thingsBeingGeneratedNow = new List<List<Thing>>();

		// Token: 0x06000C1F RID: 3103 RVA: 0x0006BB14 File Offset: 0x00069F14
		static ThingSetMaker()
		{
			Gen.EnsureAllFieldsNullable(typeof(ThingSetMakerParams));
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0006BB38 File Offset: 0x00069F38
		public List<Thing> Generate()
		{
			return this.Generate(default(ThingSetMakerParams));
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0006BB5C File Offset: 0x00069F5C
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

		// Token: 0x06000C23 RID: 3107 RVA: 0x0006BC18 File Offset: 0x0006A018
		public bool CanGenerate(ThingSetMakerParams parms)
		{
			ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
			return this.CanGenerateSub(parms2);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0006BC3C File Offset: 0x0006A03C
		protected virtual bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return true;
		}

		// Token: 0x06000C25 RID: 3109
		protected abstract void Generate(ThingSetMakerParams parms, List<Thing> outThings);

		// Token: 0x06000C26 RID: 3110 RVA: 0x0006BC54 File Offset: 0x0006A054
		public IEnumerable<ThingDef> AllGeneratableThingsDebug()
		{
			return this.AllGeneratableThingsDebug(default(ThingSetMakerParams));
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0006BC78 File Offset: 0x0006A078
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

		// Token: 0x06000C28 RID: 3112
		protected abstract IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms);

		// Token: 0x06000C29 RID: 3113 RVA: 0x0006BCAC File Offset: 0x0006A0AC
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

		// Token: 0x06000C2A RID: 3114 RVA: 0x0006BDD0 File Offset: 0x0006A1D0
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

		// Token: 0x06000C2B RID: 3115 RVA: 0x0006BE48 File Offset: 0x0006A248
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

		// Token: 0x06000C2C RID: 3116 RVA: 0x0006BEA0 File Offset: 0x0006A2A0
		private ThingSetMakerParams ApplyFixedParams(ThingSetMakerParams parms)
		{
			ThingSetMakerParams result = this.fixedParams;
			Gen.ReplaceNullFields<ThingSetMakerParams>(ref result, parms);
			return result;
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0006BEC5 File Offset: 0x0006A2C5
		public virtual void ResolveReferences()
		{
			if (this.fixedParams.filter != null)
			{
				this.fixedParams.filter.ResolveReferences();
			}
		}
	}
}
