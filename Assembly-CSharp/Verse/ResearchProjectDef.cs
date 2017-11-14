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
			using (IEnumerator<string> enumerator = base.ConfigErrors().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.techLevel == TechLevel.Undefined)
			{
				yield return "techLevel is Undefined";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!(this.ResearchViewX < 0.0) && !(this.ResearchViewY < 0.0))
			{
				List<ResearchProjectDef> rpDefs = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
				int i = 0;
				while (true)
				{
					if (i < rpDefs.Count)
					{
						if (rpDefs[i] != this && rpDefs[i].tab == this.tab && rpDefs[i].ResearchViewX == this.ResearchViewX && rpDefs[i].ResearchViewY == this.ResearchViewY)
							break;
						i++;
						continue;
					}
					yield break;
				}
				yield return "same research view coords and tab as " + rpDefs[i] + ": " + this.ResearchViewX + ", " + this.ResearchViewY + "(" + this.tab + ")";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			yield return "researchViewX and/or researchViewY not set";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_029c:
			/*Error near IL_029d: Unexpected return in MoveNext()*/;
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
					if (linkedFacilitiesListForReading.Find((Thing x) => x.def == this.requiredResearchFacilities[i] && affectedByFacilities.IsFacilityActive(x)) == null)
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
			foreach (ResearchProjectDef item in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
			{
				item.x = item.researchViewX;
				item.y = item.researchViewY;
			}
			int num = 0;
			while (true)
			{
				bool flag = false;
				foreach (ResearchProjectDef item2 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
				{
					foreach (ResearchProjectDef item3 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
					{
						if (item2 != item3 && item2.tab == item3.tab)
						{
							bool flag2 = Mathf.Abs(item2.x - item3.x) < 0.5;
							bool flag3 = Mathf.Abs(item2.y - item3.y) < 0.25;
							if (flag2 && flag3)
							{
								flag = true;
								if (item2.x <= item3.x)
								{
									item2.x -= 0.1f;
									item3.x += 0.1f;
								}
								else
								{
									item2.x += 0.1f;
									item3.x -= 0.1f;
								}
								if (item2.y <= item3.y)
								{
									item2.y -= 0.1f;
									item3.y += 0.1f;
								}
								else
								{
									item2.y += 0.1f;
									item3.y -= 0.1f;
								}
								item2.x += 0.001f;
								item2.y += 0.001f;
								item3.x -= 0.001f;
								item3.y -= 0.001f;
								ResearchProjectDef.ClampInCoordinateLimits(item2);
								ResearchProjectDef.ClampInCoordinateLimits(item3);
							}
						}
					}
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
