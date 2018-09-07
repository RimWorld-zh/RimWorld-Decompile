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
	public class ApparelProperties
	{
		public List<BodyPartGroupDef> bodyPartGroups = new List<BodyPartGroupDef>();

		public List<ApparelLayerDef> layers = new List<ApparelLayerDef>();

		[NoTranslate]
		public string wornGraphicPath = string.Empty;

		[NoTranslate]
		public List<string> tags = new List<string>();

		[NoTranslate]
		public List<string> defaultOutfitTags;

		public float wearPerDay = 0.4f;

		public bool careIfWornByCorpse = true;

		public bool hatRenderedFrontOfFace;

		public bool useDeflectMetalEffect;

		[Unsaved]
		private float cachedHumanBodyCoverage = -1f;

		[Unsaved]
		private BodyPartGroupDef[][] interferingBodyPartGroups;

		private static BodyPartGroupDef[] apparelRelevantGroups;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, IEnumerable<BodyPartGroupDef>> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<BodyPartRecord, string> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<ApparelLayerDef, string> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<BodyPartRecord, IEnumerable<BodyPartGroupDef>> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<BodyPartGroupDef, bool> <>f__am$cache5;

		public ApparelProperties()
		{
		}

		public ApparelLayerDef LastLayer
		{
			get
			{
				if (this.layers.Count > 0)
				{
					return this.layers[this.layers.Count - 1];
				}
				Log.ErrorOnce("Failed to get last layer on apparel item (see your config errors)", 31234937, false);
				return ApparelLayerDefOf.Belt;
			}
		}

		public float HumanBodyCoverage
		{
			get
			{
				if (this.cachedHumanBodyCoverage < 0f)
				{
					this.cachedHumanBodyCoverage = 0f;
					List<BodyPartRecord> allParts = BodyDefOf.Human.AllParts;
					for (int i = 0; i < allParts.Count; i++)
					{
						if (this.CoversBodyPart(allParts[i]))
						{
							this.cachedHumanBodyCoverage += allParts[i].coverageAbs;
						}
					}
				}
				return this.cachedHumanBodyCoverage;
			}
		}

		public static void ResetStaticData()
		{
			ApparelProperties.apparelRelevantGroups = (from td in DefDatabase<ThingDef>.AllDefs
			where td.IsApparel
			select td).SelectMany((ThingDef td) => td.apparel.bodyPartGroups).Distinct<BodyPartGroupDef>().ToArray<BodyPartGroupDef>();
		}

		public IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.layers.NullOrEmpty<ApparelLayerDef>())
			{
				yield return parentDef.defName + " apparel has no layers.";
			}
			yield break;
		}

		public bool CoversBodyPart(BodyPartRecord partRec)
		{
			for (int i = 0; i < partRec.groups.Count; i++)
			{
				if (this.bodyPartGroups.Contains(partRec.groups[i]))
				{
					return true;
				}
			}
			return false;
		}

		public string GetCoveredOuterPartsString(BodyDef body)
		{
			IEnumerable<BodyPartRecord> source = from x in body.AllParts
			where x.depth == BodyPartDepth.Outside && x.groups.Any((BodyPartGroupDef y) => this.bodyPartGroups.Contains(y))
			select x;
			return (from part in source.Distinct<BodyPartRecord>()
			select part.Label).ToCommaList(true).CapitalizeFirst();
		}

		public string GetLayersString()
		{
			return (from layer in this.layers
			select layer.label).ToCommaList(true).CapitalizeFirst();
		}

		public BodyPartGroupDef[] GetInterferingBodyPartGroups(BodyDef body)
		{
			if (this.interferingBodyPartGroups == null || this.interferingBodyPartGroups.Length != DefDatabase<BodyDef>.DefCount)
			{
				this.interferingBodyPartGroups = new BodyPartGroupDef[DefDatabase<BodyDef>.DefCount][];
			}
			if (this.interferingBodyPartGroups[(int)body.index] == null)
			{
				BodyPartRecord[] source = (from part in body.AllParts
				where part.groups.Any((BodyPartGroupDef @group) => this.bodyPartGroups.Contains(@group))
				select part).ToArray<BodyPartRecord>();
				BodyPartGroupDef[] array = (from bpgd in source.SelectMany((BodyPartRecord bpr) => bpr.groups).Distinct<BodyPartGroupDef>()
				where ApparelProperties.apparelRelevantGroups.Contains(bpgd)
				select bpgd).ToArray<BodyPartGroupDef>();
				this.interferingBodyPartGroups[(int)body.index] = array;
			}
			return this.interferingBodyPartGroups[(int)body.index];
		}

		[CompilerGenerated]
		private static bool <ResetStaticData>m__0(ThingDef td)
		{
			return td.IsApparel;
		}

		[CompilerGenerated]
		private static IEnumerable<BodyPartGroupDef> <ResetStaticData>m__1(ThingDef td)
		{
			return td.apparel.bodyPartGroups;
		}

		[CompilerGenerated]
		private bool <GetCoveredOuterPartsString>m__2(BodyPartRecord x)
		{
			return x.depth == BodyPartDepth.Outside && x.groups.Any((BodyPartGroupDef y) => this.bodyPartGroups.Contains(y));
		}

		[CompilerGenerated]
		private static string <GetCoveredOuterPartsString>m__3(BodyPartRecord part)
		{
			return part.Label;
		}

		[CompilerGenerated]
		private static string <GetLayersString>m__4(ApparelLayerDef layer)
		{
			return layer.label;
		}

		[CompilerGenerated]
		private bool <GetInterferingBodyPartGroups>m__5(BodyPartRecord part)
		{
			return part.groups.Any((BodyPartGroupDef group) => this.bodyPartGroups.Contains(group));
		}

		[CompilerGenerated]
		private static IEnumerable<BodyPartGroupDef> <GetInterferingBodyPartGroups>m__6(BodyPartRecord bpr)
		{
			return bpr.groups;
		}

		[CompilerGenerated]
		private static bool <GetInterferingBodyPartGroups>m__7(BodyPartGroupDef bpgd)
		{
			return ApparelProperties.apparelRelevantGroups.Contains(bpgd);
		}

		[CompilerGenerated]
		private bool <GetCoveredOuterPartsString>m__8(BodyPartGroupDef y)
		{
			return this.bodyPartGroups.Contains(y);
		}

		[CompilerGenerated]
		private bool <GetInterferingBodyPartGroups>m__9(BodyPartGroupDef group)
		{
			return this.bodyPartGroups.Contains(group);
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal ThingDef parentDef;

			internal ApparelProperties $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (this.layers.NullOrEmpty<ApparelLayerDef>())
					{
						this.$current = parentDef.defName + " apparel has no layers.";
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ApparelProperties.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new ApparelProperties.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				<ConfigErrors>c__Iterator.parentDef = parentDef;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
