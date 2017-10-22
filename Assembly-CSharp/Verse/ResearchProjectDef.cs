using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class ResearchProjectDef : Def
	{
		public TechLevel techLevel;

		[MustTranslate]
		private string descriptionDiscovered;

		public float baseCost = 100f;

		public List<ResearchProjectDef> prerequisites;

		public List<ResearchProjectDef> requiredByThis;

		private List<ResearchMod> researchMods;

		public ThingDef requiredResearchBuilding;

		public List<ThingDef> requiredResearchFacilities;

		[NoTranslate]
		public List<string> tags;

		public ResearchTabDef tab;

		public float researchViewX = 1f;

		public float researchViewY = 1f;

		private float x = 1f;

		private float y = 1f;

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

		public string DescriptionDiscovered
		{
			get
			{
				if (this.descriptionDiscovered != null)
				{
					return this.descriptionDiscovered;
				}
				return base.description;
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
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (this.techLevel == TechLevel.Undefined)
			{
				yield return "techLevel is Undefined";
			}
			if (this.ResearchViewX < 0.0 || this.ResearchViewY < 0.0)
			{
				yield return "researchViewX and/or researchViewY not set";
			}
			List<ResearchProjectDef> rpDefs = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < rpDefs.Count; i++)
			{
				if (rpDefs[i] != this && rpDefs[i].tab == this.tab && rpDefs[i].ResearchViewX == this.ResearchViewX && rpDefs[i].ResearchViewY == this.ResearchViewY)
				{
					yield return "same research view coords and tab as " + rpDefs[i] + ": " + this.ResearchViewX + ", " + this.ResearchViewY + "(" + this.tab + ")";
				}
			}
		}

		public float CostFactor(TechLevel researcherTechLevel)
		{
			if ((int)researcherTechLevel >= (int)this.techLevel)
			{
				return 1f;
			}
			int num = this.techLevel - researcherTechLevel;
			return (float)(1.0 + (float)num);
		}

		public bool HasTag(string tag)
		{
			if (this.tags == null)
			{
				return false;
			}
			return this.tags.Contains(tag);
		}

		public bool CanBeResearchedAt(Building_ResearchBench bench, bool ignoreResearchBenchPowerStatus)
		{
			if (this.requiredResearchBuilding != null && bench.def != this.requiredResearchBuilding)
			{
				return false;
			}
			if (!ignoreResearchBenchPowerStatus)
			{
				CompPowerTrader comp = bench.GetComp<CompPowerTrader>();
				if (comp != null && !comp.PowerOn)
				{
					return false;
				}
			}
			if (!this.requiredResearchFacilities.NullOrEmpty())
			{
				CompAffectedByFacilities affectedByFacilities = bench.TryGetComp<CompAffectedByFacilities>();
				if (affectedByFacilities == null)
				{
					return false;
				}
				List<Thing> linkedFacilitiesListForReading = affectedByFacilities.LinkedFacilitiesListForReading;
				int i;
				for (i = 0; i < this.requiredResearchFacilities.Count; i++)
				{
					if (linkedFacilitiesListForReading.Find((Predicate<Thing>)((Thing x) => x.def == this.requiredResearchFacilities[i] && affectedByFacilities.IsFacilityActive(x))) == null)
					{
						return false;
					}
				}
			}
			return true;
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
						Log.Error("Exception applying research mod for project " + this + ": " + ex.ToString());
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
			List<ResearchProjectDef>.Enumerator enumerator = DefDatabase<ResearchProjectDef>.AllDefsListForReading.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ResearchProjectDef current = enumerator.Current;
					current.x = current.researchViewX;
					current.y = current.researchViewY;
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			int num = 0;
			while (true)
			{
				bool flag = false;
				List<ResearchProjectDef>.Enumerator enumerator2 = DefDatabase<ResearchProjectDef>.AllDefsListForReading.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						ResearchProjectDef current2 = enumerator2.Current;
						List<ResearchProjectDef>.Enumerator enumerator3 = DefDatabase<ResearchProjectDef>.AllDefsListForReading.GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								ResearchProjectDef current3 = enumerator3.Current;
								if (current2 != current3 && current2.tab == current3.tab)
								{
									bool flag2 = Mathf.Abs(current2.x - current3.x) < 0.5;
									bool flag3 = Mathf.Abs(current2.y - current3.y) < 0.25;
									if (flag2 && flag3)
									{
										flag = true;
										if (current2.x <= current3.x)
										{
											current2.x -= 0.1f;
											current3.x += 0.1f;
										}
										else
										{
											current2.x += 0.1f;
											current3.x -= 0.1f;
										}
										if (current2.y <= current3.y)
										{
											current2.y -= 0.1f;
											current3.y += 0.1f;
										}
										else
										{
											current2.y += 0.1f;
											current3.y -= 0.1f;
										}
										current2.x += 0.001f;
										current2.y += 0.001f;
										current3.x -= 0.001f;
										current3.y -= 0.001f;
										ResearchProjectDef.ClampInCoordinateLimits(current2);
										ResearchProjectDef.ClampInCoordinateLimits(current3);
									}
								}
							}
						}
						finally
						{
							((IDisposable)(object)enumerator3).Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
				if (flag)
				{
					num++;
					if (num > 200)
						break;
					continue;
				}
				return;
			}
			Log.Error("Couldn't relax research project coordinates apart after " + 200 + " passes.");
		}

		private static void ClampInCoordinateLimits(ResearchProjectDef rp)
		{
			if (rp.x < 0.0)
			{
				rp.x = 0f;
			}
			if (rp.y < 0.0)
			{
				rp.y = 0f;
			}
			if (rp.y > 6.5)
			{
				rp.y = 6.5f;
			}
		}
	}
}
