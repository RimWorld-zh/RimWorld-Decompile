using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B68 RID: 2920
	public class ResearchProjectDef : Def
	{
		// Token: 0x04002AB5 RID: 10933
		public TechLevel techLevel = TechLevel.Undefined;

		// Token: 0x04002AB6 RID: 10934
		[MustTranslate]
		private string descriptionDiscovered = null;

		// Token: 0x04002AB7 RID: 10935
		public float baseCost = 100f;

		// Token: 0x04002AB8 RID: 10936
		public List<ResearchProjectDef> prerequisites = null;

		// Token: 0x04002AB9 RID: 10937
		public List<ResearchProjectDef> requiredByThis = null;

		// Token: 0x04002ABA RID: 10938
		private List<ResearchMod> researchMods = null;

		// Token: 0x04002ABB RID: 10939
		public ThingDef requiredResearchBuilding = null;

		// Token: 0x04002ABC RID: 10940
		public List<ThingDef> requiredResearchFacilities = null;

		// Token: 0x04002ABD RID: 10941
		public List<ResearchProjectTagDef> tags = null;

		// Token: 0x04002ABE RID: 10942
		public ResearchTabDef tab;

		// Token: 0x04002ABF RID: 10943
		public float researchViewX = 1f;

		// Token: 0x04002AC0 RID: 10944
		public float researchViewY = 1f;

		// Token: 0x04002AC1 RID: 10945
		[Unsaved]
		private float x = 1f;

		// Token: 0x04002AC2 RID: 10946
		[Unsaved]
		private float y = 1f;

		// Token: 0x04002AC3 RID: 10947
		[Unsaved]
		private bool positionModified = false;

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06003FC4 RID: 16324 RVA: 0x0021A350 File Offset: 0x00218750
		public float ResearchViewX
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06003FC5 RID: 16325 RVA: 0x0021A36C File Offset: 0x0021876C
		public float ResearchViewY
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06003FC6 RID: 16326 RVA: 0x0021A388 File Offset: 0x00218788
		public float CostApparent
		{
			get
			{
				return this.baseCost * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06003FC7 RID: 16327 RVA: 0x0021A3BC File Offset: 0x002187BC
		public float ProgressReal
		{
			get
			{
				return Find.ResearchManager.GetProgress(this);
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x06003FC8 RID: 16328 RVA: 0x0021A3DC File Offset: 0x002187DC
		public float ProgressApparent
		{
			get
			{
				return this.ProgressReal * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x06003FC9 RID: 16329 RVA: 0x0021A410 File Offset: 0x00218810
		public float ProgressPercent
		{
			get
			{
				return Find.ResearchManager.GetProgress(this) / this.baseCost;
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06003FCA RID: 16330 RVA: 0x0021A438 File Offset: 0x00218838
		public bool IsFinished
		{
			get
			{
				return this.ProgressReal >= this.baseCost;
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06003FCB RID: 16331 RVA: 0x0021A460 File Offset: 0x00218860
		public bool CanStartNow
		{
			get
			{
				return !this.IsFinished && this.PrerequisitesCompleted && (this.requiredResearchBuilding == null || this.PlayerHasAnyAppropriateResearchBench);
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06003FCC RID: 16332 RVA: 0x0021A4A4 File Offset: 0x002188A4
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

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06003FCD RID: 16333 RVA: 0x0021A504 File Offset: 0x00218904
		public string DescriptionDiscovered
		{
			get
			{
				string description;
				if (this.descriptionDiscovered != null)
				{
					description = this.descriptionDiscovered;
				}
				else
				{
					description = this.description;
				}
				return description;
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06003FCE RID: 16334 RVA: 0x0021A538 File Offset: 0x00218938
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

		// Token: 0x06003FCF RID: 16335 RVA: 0x0021A5C5 File Offset: 0x002189C5
		public override void ResolveReferences()
		{
			if (this.tab == null)
			{
				this.tab = ResearchTabDefOf.Main;
			}
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x0021A5E0 File Offset: 0x002189E0
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

		// Token: 0x06003FD1 RID: 16337 RVA: 0x0021A60C File Offset: 0x00218A0C
		public float CostFactor(TechLevel researcherTechLevel)
		{
			float result;
			if (researcherTechLevel >= this.techLevel)
			{
				result = 1f;
			}
			else
			{
				int num = (int)(this.techLevel - researcherTechLevel);
				result = 1f + (float)num;
			}
			return result;
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x0021A64C File Offset: 0x00218A4C
		public bool HasTag(ResearchProjectTagDef tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x0021A680 File Offset: 0x00218A80
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

		// Token: 0x06003FD4 RID: 16340 RVA: 0x0021A790 File Offset: 0x00218B90
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

		// Token: 0x06003FD5 RID: 16341 RVA: 0x0021A828 File Offset: 0x00218C28
		public static ResearchProjectDef Named(string defName)
		{
			return DefDatabase<ResearchProjectDef>.GetNamed(defName, true);
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x0021A844 File Offset: 0x00218C44
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

		// Token: 0x06003FD7 RID: 16343 RVA: 0x0021AB4C File Offset: 0x00218F4C
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

		// Token: 0x06003FD8 RID: 16344 RVA: 0x0021ABAB File Offset: 0x00218FAB
		public void Debug_ApplyPositionDelta(Vector2 delta)
		{
			this.x += delta.x;
			this.y += delta.y;
			this.positionModified = true;
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x0021ABE0 File Offset: 0x00218FE0
		public bool Debug_IsPositionModified()
		{
			return this.positionModified;
		}
	}
}
