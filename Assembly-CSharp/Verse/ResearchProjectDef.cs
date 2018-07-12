using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class ResearchProjectDef : Def
	{
		public float baseCost = 100f;

		public List<ResearchProjectDef> prerequisites = null;

		public TechLevel techLevel = TechLevel.Undefined;

		public List<ResearchProjectDef> requiredByThis = null;

		private List<ResearchMod> researchMods = null;

		public ThingDef requiredResearchBuilding = null;

		public List<ThingDef> requiredResearchFacilities = null;

		public List<ResearchProjectTagDef> tags = null;

		public ResearchTabDef tab;

		public float researchViewX = 1f;

		public float researchViewY = 1f;

		[MustTranslate]
		public string discoveredLetterTitle;

		[MustTranslate]
		public string discoveredLetterText;

		[Unsaved]
		private float x = 1f;

		[Unsaved]
		private float y = 1f;

		[Unsaved]
		private bool positionModified = false;

		public const TechLevel MaxEffectiveTechLevel = TechLevel.Industrial;

		private const float ResearchCostFactorPerTechLevelDiff = 0.5f;

		public ResearchProjectDef()
		{
		}

		public float ResearchViewX
		{
			get
			{
				return this.x;
			}
		}

		public float ResearchViewY
		{
			get
			{
				return this.y;
			}
		}

		public float CostApparent
		{
			get
			{
				return this.baseCost * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		public float ProgressReal
		{
			get
			{
				return Find.ResearchManager.GetProgress(this);
			}
		}

		public float ProgressApparent
		{
			get
			{
				return this.ProgressReal * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		public float ProgressPercent
		{
			get
			{
				return Find.ResearchManager.GetProgress(this) / this.baseCost;
			}
		}

		public bool IsFinished
		{
			get
			{
				return this.ProgressReal >= this.baseCost;
			}
		}

		public bool CanStartNow
		{
			get
			{
				return !this.IsFinished && this.PrerequisitesCompleted && (this.requiredResearchBuilding == null || this.PlayerHasAnyAppropriateResearchBench);
			}
		}

		public bool PrerequisitesCompleted
		{
			get
			{
				if (this.prerequisites != null)
				{
					for (int i = 0; i < this.prerequisites.Count; i++)
					{
						if (!this.prerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		private bool PlayerHasAnyAppropriateResearchBench
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Building> allBuildingsColonist = maps[i].listerBuildings.allBuildingsColonist;
					for (int j = 0; j < allBuildingsColonist.Count; j++)
					{
						Building_ResearchBench building_ResearchBench = allBuildingsColonist[j] as Building_ResearchBench;
						if (building_ResearchBench != null && this.CanBeResearchedAt(building_ResearchBench, true))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public override void ResolveReferences()
		{
			if (this.tab == null)
			{
				this.tab = ResearchTabDefOf.Main;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.techLevel == TechLevel.Undefined)
			{
				yield return "techLevel is Undefined";
			}
			if (this.ResearchViewX < 0f || this.ResearchViewY < 0f)
			{
				yield return "researchViewX and/or researchViewY not set";
			}
			List<ResearchProjectDef> rpDefs = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < rpDefs.Count; i++)
			{
				if (rpDefs[i] != this && rpDefs[i].tab == this.tab && rpDefs[i].ResearchViewX == this.ResearchViewX && rpDefs[i].ResearchViewY == this.ResearchViewY)
				{
					yield return string.Concat(new object[]
					{
						"same research view coords and tab as ",
						rpDefs[i],
						": ",
						this.ResearchViewX,
						", ",
						this.ResearchViewY,
						"(",
						this.tab,
						")"
					});
				}
			}
			yield break;
		}

		public float CostFactor(TechLevel researcherTechLevel)
		{
			TechLevel techLevel = (TechLevel)Mathf.Min((int)this.techLevel, 4);
			float result;
			if (researcherTechLevel >= techLevel)
			{
				result = 1f;
			}
			else
			{
				int num = (int)(techLevel - researcherTechLevel);
				result = 1f + (float)num * 0.5f;
			}
			return result;
		}

		public bool HasTag(ResearchProjectTagDef tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		public bool CanBeResearchedAt(Building_ResearchBench bench, bool ignoreResearchBenchPowerStatus)
		{
			bool result;
			if (this.requiredResearchBuilding != null && bench.def != this.requiredResearchBuilding)
			{
				result = false;
			}
			else
			{
				if (!ignoreResearchBenchPowerStatus)
				{
					CompPowerTrader comp = bench.GetComp<CompPowerTrader>();
					if (comp != null && !comp.PowerOn)
					{
						return false;
					}
				}
				if (!this.requiredResearchFacilities.NullOrEmpty<ThingDef>())
				{
					ResearchProjectDef.<CanBeResearchedAt>c__AnonStorey2 <CanBeResearchedAt>c__AnonStorey = new ResearchProjectDef.<CanBeResearchedAt>c__AnonStorey2();
					<CanBeResearchedAt>c__AnonStorey.$this = this;
					<CanBeResearchedAt>c__AnonStorey.affectedByFacilities = bench.TryGetComp<CompAffectedByFacilities>();
					if (<CanBeResearchedAt>c__AnonStorey.affectedByFacilities == null)
					{
						return false;
					}
					List<Thing> linkedFacilitiesListForReading = <CanBeResearchedAt>c__AnonStorey.affectedByFacilities.LinkedFacilitiesListForReading;
					int i;
					for (i = 0; i < this.requiredResearchFacilities.Count; i++)
					{
						if (linkedFacilitiesListForReading.Find((Thing x) => x.def == <CanBeResearchedAt>c__AnonStorey.$this.requiredResearchFacilities[i] && <CanBeResearchedAt>c__AnonStorey.affectedByFacilities.IsFacilityActive(x)) == null)
						{
							return false;
						}
					}
				}
				result = true;
			}
			return result;
		}

		public void ReapplyAllMods()
		{
			if (this.researchMods != null)
			{
				for (int i = 0; i < this.researchMods.Count; i++)
				{
					try
					{
						this.researchMods[i].Apply();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception applying research mod for project ",
							this,
							": ",
							ex.ToString()
						}), false);
					}
				}
			}
		}

		public static ResearchProjectDef Named(string defName)
		{
			return DefDatabase<ResearchProjectDef>.GetNamed(defName, true);
		}

		public static void GenerateNonOverlappingCoordinates()
		{
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
			{
				researchProjectDef.x = researchProjectDef.researchViewX;
				researchProjectDef.y = researchProjectDef.researchViewY;
			}
			int num = 0;
			for (;;)
			{
				bool flag = false;
				foreach (ResearchProjectDef researchProjectDef2 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
				{
					foreach (ResearchProjectDef researchProjectDef3 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
					{
						if (researchProjectDef2 != researchProjectDef3 && researchProjectDef2.tab == researchProjectDef3.tab)
						{
							bool flag2 = Mathf.Abs(researchProjectDef2.x - researchProjectDef3.x) < 0.5f;
							bool flag3 = Mathf.Abs(researchProjectDef2.y - researchProjectDef3.y) < 0.25f;
							if (flag2 && flag3)
							{
								flag = true;
								if (researchProjectDef2.x <= researchProjectDef3.x)
								{
									researchProjectDef2.x -= 0.1f;
									researchProjectDef3.x += 0.1f;
								}
								else
								{
									researchProjectDef2.x += 0.1f;
									researchProjectDef3.x -= 0.1f;
								}
								if (researchProjectDef2.y <= researchProjectDef3.y)
								{
									researchProjectDef2.y -= 0.1f;
									researchProjectDef3.y += 0.1f;
								}
								else
								{
									researchProjectDef2.y += 0.1f;
									researchProjectDef3.y -= 0.1f;
								}
								researchProjectDef2.x += 0.001f;
								researchProjectDef2.y += 0.001f;
								researchProjectDef3.x -= 0.001f;
								researchProjectDef3.y -= 0.001f;
								ResearchProjectDef.ClampInCoordinateLimits(researchProjectDef2);
								ResearchProjectDef.ClampInCoordinateLimits(researchProjectDef3);
							}
						}
					}
				}
				if (!flag)
				{
					break;
				}
				num++;
				if (num > 200)
				{
					goto Block_4;
				}
			}
			return;
			Block_4:
			Log.Error("Couldn't relax research project coordinates apart after " + 200 + " passes.", false);
		}

		private static void ClampInCoordinateLimits(ResearchProjectDef rp)
		{
			if (rp.x < 0f)
			{
				rp.x = 0f;
			}
			if (rp.y < 0f)
			{
				rp.y = 0f;
			}
			if (rp.y > 6.5f)
			{
				rp.y = 6.5f;
			}
		}

		public void Debug_ApplyPositionDelta(Vector2 delta)
		{
			this.x += delta.x;
			this.y += delta.y;
			this.positionModified = true;
		}

		public bool Debug_IsPositionModified()
		{
			return this.positionModified;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <e>__1;

			internal List<ResearchProjectDef> <rpDefs>__0;

			internal int <i>__2;

			internal ResearchProjectDef $this;

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
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<ConfigErrors>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_EF;
				case 3u:
					goto IL_138;
				case 4u:
					IL_274:
					i++;
					goto IL_283;
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
						e = enumerator.Current;
						this.$current = e;
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
				if (this.techLevel == TechLevel.Undefined)
				{
					this.$current = "techLevel is Undefined";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_EF:
				if (base.ResearchViewX < 0f || base.ResearchViewY < 0f)
				{
					this.$current = "researchViewX and/or researchViewY not set";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_138:
				rpDefs = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
				i = 0;
				IL_283:
				if (i >= rpDefs.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (rpDefs[i] != this && rpDefs[i].tab == this.tab && rpDefs[i].ResearchViewX == base.ResearchViewX && rpDefs[i].ResearchViewY == base.ResearchViewY)
					{
						this.$current = string.Concat(new object[]
						{
							"same research view coords and tab as ",
							rpDefs[i],
							": ",
							base.ResearchViewX,
							", ",
							base.ResearchViewY,
							"(",
							this.tab,
							")"
						});
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						return true;
					}
					goto IL_274;
				}
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ResearchProjectDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new ResearchProjectDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <CanBeResearchedAt>c__AnonStorey2
		{
			internal CompAffectedByFacilities affectedByFacilities;

			internal ResearchProjectDef $this;

			public <CanBeResearchedAt>c__AnonStorey2()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <CanBeResearchedAt>c__AnonStorey1
		{
			internal int i;

			internal ResearchProjectDef.<CanBeResearchedAt>c__AnonStorey2 <>f__ref$2;

			public <CanBeResearchedAt>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return x.def == this.<>f__ref$2.$this.requiredResearchFacilities[this.i] && this.<>f__ref$2.affectedByFacilities.IsFacilityActive(x);
			}
		}
	}
}
