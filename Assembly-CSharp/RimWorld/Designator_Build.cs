using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E0 RID: 2016
	public class Designator_Build : Designator_Place
	{
		// Token: 0x040017AA RID: 6058
		protected BuildableDef entDef;

		// Token: 0x040017AB RID: 6059
		private ThingDef stuffDef = null;

		// Token: 0x040017AC RID: 6060
		private bool writeStuff = false;

		// Token: 0x040017AD RID: 6061
		private static readonly Vector2 TerrainTextureCroppedSize = new Vector2(64f, 64f);

		// Token: 0x040017AE RID: 6062
		private static readonly Vector2 DragPriceDrawOffset = new Vector2(19f, 17f);

		// Token: 0x040017AF RID: 6063
		private const float DragPriceDrawNumberX = 29f;

		// Token: 0x06002CC0 RID: 11456 RVA: 0x001790BC File Offset: 0x001774BC
		public Designator_Build(BuildableDef entDef)
		{
			this.entDef = entDef;
			this.icon = entDef.uiIcon;
			this.iconAngle = entDef.uiIconAngle;
			this.iconOffset = entDef.uiIconOffset;
			this.hotKey = entDef.designationHotKey;
			this.tutorTag = entDef.defName;
			this.order = 20f;
			ThingDef thingDef = entDef as ThingDef;
			if (thingDef != null)
			{
				this.iconProportions = thingDef.graphicData.drawSize;
				this.iconDrawScale = GenUI.IconDrawScale(thingDef);
			}
			else
			{
				this.iconProportions = new Vector2(1f, 1f);
				this.iconDrawScale = 1f;
			}
			TerrainDef terrainDef = entDef as TerrainDef;
			if (terrainDef != null)
			{
				this.iconTexCoords = new Rect(0f, 0f, Designator_Build.TerrainTextureCroppedSize.x / (float)this.icon.width, Designator_Build.TerrainTextureCroppedSize.y / (float)this.icon.height);
			}
			this.ResetStuffToDefault();
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06002CC1 RID: 11457 RVA: 0x001791DC File Offset: 0x001775DC
		public override BuildableDef PlacingDef
		{
			get
			{
				return this.entDef;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002CC2 RID: 11458 RVA: 0x001791F8 File Offset: 0x001775F8
		public override string Label
		{
			get
			{
				ThingDef thingDef = this.entDef as ThingDef;
				string result;
				if (thingDef != null && this.writeStuff)
				{
					result = GenLabel.ThingLabel(thingDef, this.stuffDef, 1);
				}
				else if (thingDef != null && thingDef.MadeFromStuff)
				{
					result = this.entDef.label + "...";
				}
				else
				{
					result = this.entDef.label;
				}
				return result;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06002CC3 RID: 11459 RVA: 0x00179274 File Offset: 0x00177674
		public override string Desc
		{
			get
			{
				return this.entDef.description;
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002CC4 RID: 11460 RVA: 0x00179294 File Offset: 0x00177694
		public override Color IconDrawColor
		{
			get
			{
				Color result;
				if (this.stuffDef != null)
				{
					result = this.stuffDef.stuffProps.color;
				}
				else
				{
					result = this.entDef.uiIconColor;
				}
				return result;
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06002CC5 RID: 11461 RVA: 0x001792D8 File Offset: 0x001776D8
		public override bool Visible
		{
			get
			{
				bool result;
				if (DebugSettings.godMode)
				{
					result = true;
				}
				else if (this.entDef.minTechLevelToBuild != TechLevel.Undefined && Faction.OfPlayer.def.techLevel < this.entDef.minTechLevelToBuild)
				{
					result = false;
				}
				else if (this.entDef.maxTechLevelToBuild != TechLevel.Undefined && Faction.OfPlayer.def.techLevel > this.entDef.maxTechLevelToBuild)
				{
					result = false;
				}
				else if (!this.entDef.IsResearchFinished)
				{
					result = false;
				}
				else
				{
					if (this.entDef.buildingPrerequisites != null)
					{
						for (int i = 0; i < this.entDef.buildingPrerequisites.Count; i++)
						{
							if (!base.Map.listerBuildings.ColonistsHaveBuilding(this.entDef.buildingPrerequisites[i]))
							{
								return false;
							}
						}
					}
					result = true;
				}
				return result;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06002CC6 RID: 11462 RVA: 0x001793E8 File Offset: 0x001777E8
		public override int DraggableDimensions
		{
			get
			{
				return this.entDef.placingDraggableDimensions;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06002CC7 RID: 11463 RVA: 0x00179408 File Offset: 0x00177808
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06002CC8 RID: 11464 RVA: 0x00179420 File Offset: 0x00177820
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06002CC9 RID: 11465 RVA: 0x0017943C File Offset: 0x0017783C
		public override string HighlightTag
		{
			get
			{
				if (this.cachedHighlightTag == null && this.tutorTag != null)
				{
					this.cachedHighlightTag = "Designator-Build-" + this.tutorTag;
				}
				return this.cachedHighlightTag;
			}
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x00179484 File Offset: 0x00177884
		public void ResetStuffToDefault()
		{
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null && thingDef.MadeFromStuff)
			{
				this.stuffDef = GenStuff.DefaultStuffFor(thingDef);
			}
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x001794BC File Offset: 0x001778BC
		public override void DrawMouseAttachments()
		{
			base.DrawMouseAttachments();
			if (!ArchitectCategoryTab.InfoRect.Contains(UI.MousePositionOnUIInverted))
			{
				DesignationDragger dragger = Find.DesignatorManager.Dragger;
				int num;
				if (dragger.Dragging)
				{
					num = dragger.DragCells.Count<IntVec3>();
				}
				else
				{
					num = 1;
				}
				float num2 = 0f;
				Vector2 vector = Event.current.mousePosition + Designator_Build.DragPriceDrawOffset;
				List<ThingDefCountClass> list = this.entDef.CostListAdjusted(this.stuffDef, true);
				for (int i = 0; i < list.Count; i++)
				{
					ThingDefCountClass thingDefCountClass = list[i];
					float y = vector.y + num2;
					Rect rect = new Rect(vector.x, y, 27f, 27f);
					Widgets.ThingIcon(rect, thingDefCountClass.thingDef);
					Rect rect2 = new Rect(vector.x + 29f, y, 999f, 29f);
					int num3 = num * thingDefCountClass.count;
					string text = num3.ToString();
					if (base.Map.resourceCounter.GetCount(thingDefCountClass.thingDef) < num3)
					{
						GUI.color = Color.red;
						text = text + " (" + "NotEnoughStoredLower".Translate() + ")";
					}
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect2, text);
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
					num2 += 29f;
				}
			}
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x0017964C File Offset: 0x00177A4C
		public override void ProcessInput(Event ev)
		{
			if (base.CheckCanInteract())
			{
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef == null || !thingDef.MadeFromStuff)
				{
					base.ProcessInput(ev);
				}
				else
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (ThingDef thingDef2 in base.Map.resourceCounter.AllCountedAmounts.Keys)
					{
						if (thingDef2.IsStuff && thingDef2.stuffProps.CanMake(thingDef) && (DebugSettings.godMode || base.Map.listerThings.ThingsOfDef(thingDef2).Count > 0))
						{
							ThingDef localStuffDef = thingDef2;
							string label = GenLabel.ThingLabel(this.entDef, localStuffDef, 1).CapitalizeFirst();
							list.Add(new FloatMenuOption(label, delegate()
							{
								this.<ProcessInput>__BaseCallProxy0(ev);
								Find.DesignatorManager.Select(this);
								this.stuffDef = localStuffDef;
								this.writeStuff = true;
							}, MenuOptionPriority.Default, null, null, 0f, null, null)
							{
								tutorTag = "SelectStuff-" + thingDef.defName + "-" + localStuffDef.defName
							});
						}
					}
					if (list.Count == 0)
					{
						Messages.Message("NoStuffsToBuildWith".Translate(), MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						FloatMenu floatMenu = new FloatMenu(list);
						floatMenu.vanishIfMouseDistant = true;
						floatMenu.onCloseCallback = delegate()
						{
							this.writeStuff = true;
						};
						Find.WindowStack.Add(floatMenu);
						Find.DesignatorManager.Select(this);
					}
				}
			}
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x00179838 File Offset: 0x00177C38
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return GenConstruct.CanPlaceBlueprintAt(this.entDef, c, this.placingRot, base.Map, DebugSettings.godMode, null);
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x0017986C File Offset: 0x00177C6C
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (!TutorSystem.TutorialMode || TutorSystem.AllowAction(new EventPack(base.TutorTagDesignate, c)))
			{
				if (DebugSettings.godMode || this.entDef.GetStatValueAbstract(StatDefOf.WorkToBuild, this.stuffDef) == 0f)
				{
					if (this.entDef is TerrainDef)
					{
						base.Map.terrainGrid.SetTerrain(c, (TerrainDef)this.entDef);
					}
					else
					{
						Thing thing = ThingMaker.MakeThing((ThingDef)this.entDef, this.stuffDef);
						thing.SetFactionDirect(Faction.OfPlayer);
						GenSpawn.Spawn(thing, c, base.Map, this.placingRot, WipeMode.Vanish, false);
					}
				}
				else
				{
					GenSpawn.WipeExistingThings(c, this.placingRot, this.entDef.blueprintDef, base.Map, DestroyMode.Deconstruct);
					GenConstruct.PlaceBlueprintForBuild(this.entDef, c, base.Map, this.placingRot, Faction.OfPlayer, this.stuffDef);
				}
				MoteMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, this.placingRot, this.entDef.Size), base.Map);
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef != null && thingDef.IsOrbitalTradeBeacon)
				{
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.BuildOrbitalTradeBeacon, KnowledgeAmount.Total);
				}
				if (TutorSystem.TutorialMode)
				{
					TutorSystem.Notify_Event(new EventPack(base.TutorTagDesignate, c));
				}
				if (this.entDef.PlaceWorkers != null)
				{
					for (int i = 0; i < this.entDef.PlaceWorkers.Count; i++)
					{
						this.entDef.PlaceWorkers[i].PostPlace(base.Map, this.entDef, c, this.placingRot);
					}
				}
			}
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x00179A44 File Offset: 0x00177E44
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.entDef, this.placingRot);
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x00179A60 File Offset: 0x00177E60
		public override void DrawPanelReadout(ref float curY, float width)
		{
			if (this.entDef.costStuffCount <= 0 && this.stuffDef != null)
			{
				this.stuffDef = null;
			}
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null)
			{
				Widgets.InfoCardButton(width - 24f - 2f, 6f, thingDef, this.stuffDef);
			}
			else
			{
				Widgets.InfoCardButton(width - 24f - 2f, 6f, this.entDef);
			}
			Text.Font = GameFont.Small;
			List<ThingDefCountClass> list = this.entDef.CostListAdjusted(this.stuffDef, false);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = list[i];
				Color color = GUI.color;
				Widgets.ThingIcon(new Rect(0f, curY, 20f, 20f), thingDefCountClass.thingDef);
				GUI.color = color;
				if (thingDefCountClass.thingDef != null && thingDefCountClass.thingDef.resourceReadoutPriority != ResourceCountPriority.Uncounted && base.Map.resourceCounter.GetCount(thingDefCountClass.thingDef) < thingDefCountClass.count)
				{
					GUI.color = Color.red;
				}
				Widgets.Label(new Rect(26f, curY + 2f, 50f, 100f), thingDefCountClass.count.ToString());
				GUI.color = Color.white;
				string text;
				if (thingDefCountClass.thingDef == null)
				{
					text = "(" + "UnchosenStuff".Translate() + ")";
				}
				else
				{
					text = thingDefCountClass.thingDef.LabelCap;
				}
				float width2 = width - 60f;
				float num = Text.CalcHeight(text, width2) - 5f;
				Widgets.Label(new Rect(60f, curY + 2f, width2, num + 5f), text);
				curY += num;
			}
			if (this.entDef.constructionSkillPrerequisite > 0)
			{
				Rect rect = new Rect(0f, curY + 2f, width, 24f);
				if (!this.AnyColonistWithConstructionSkill(this.entDef.constructionSkillPrerequisite, false))
				{
					GUI.color = Color.red;
					TooltipHandler.TipRegion(rect, "NoColonistWithConstructionSkillTip".Translate(new object[]
					{
						Faction.OfPlayer.def.pawnsPlural
					}));
				}
				else if (!this.AnyColonistWithConstructionSkill(this.entDef.constructionSkillPrerequisite, true))
				{
					GUI.color = Color.yellow;
					TooltipHandler.TipRegion(rect, "AllColonistsWithConstructionSkillHaveDisaledConstructingTip".Translate(new object[]
					{
						Faction.OfPlayer.def.pawnsPlural,
						WorkTypeDefOf.Construction.gerundLabel
					}));
				}
				else
				{
					GUI.color = new Color(0.72f, 0.87f, 0.72f);
				}
				Widgets.Label(rect, string.Format("{0}: {1}", "ConstructionNeeded".Translate(), this.entDef.constructionSkillPrerequisite));
				GUI.color = Color.white;
				curY += 18f;
			}
			curY += 4f;
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x00179D94 File Offset: 0x00178194
		private bool AnyColonistWithConstructionSkill(int skill, bool careIfDisabled)
		{
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonists)
			{
				if (pawn.skills.GetSkill(SkillDefOf.Construction).Level >= skill && (!careIfDisabled || pawn.workSettings.WorkIsActive(WorkTypeDefOf.Construction)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x00179E3C File Offset: 0x0017823C
		public void SetStuffDef(ThingDef stuffDef)
		{
			this.stuffDef = stuffDef;
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x00179E46 File Offset: 0x00178246
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
