using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Build : Designator_Place
	{
		protected BuildableDef entDef;

		private ThingDef stuffDef;

		private bool writeStuff;

		private static readonly Vector2 TerrainTextureCroppedSize = new Vector2(64f, 64f);

		private static readonly Vector2 DragPriceDrawOffset = new Vector2(19f, 17f);

		private const float DragPriceDrawNumberX = 29f;

		public override BuildableDef PlacingDef
		{
			get
			{
				return this.entDef;
			}
		}

		public override string Label
		{
			get
			{
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef != null && this.writeStuff)
				{
					return GenLabel.ThingLabel(thingDef, this.stuffDef, 1);
				}
				return this.entDef.label;
			}
		}

		public override string Desc
		{
			get
			{
				return this.entDef.description;
			}
		}

		public override Color IconDrawColor
		{
			get
			{
				if (this.stuffDef != null)
				{
					return this.stuffDef.stuffProps.color;
				}
				return this.entDef.IconDrawColor;
			}
		}

		public override bool Visible
		{
			get
			{
				if (DebugSettings.godMode)
				{
					return true;
				}
				if (this.entDef.minTechLevelToBuild != 0 && (int)Faction.OfPlayer.def.techLevel < (int)this.entDef.minTechLevelToBuild)
				{
					return false;
				}
				if (this.entDef.maxTechLevelToBuild != 0 && (int)Faction.OfPlayer.def.techLevel > (int)this.entDef.maxTechLevelToBuild)
				{
					return false;
				}
				if (this.entDef.researchPrerequisites != null)
				{
					for (int i = 0; i < this.entDef.researchPrerequisites.Count; i++)
					{
						if (!this.entDef.researchPrerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				if (this.entDef.buildingPrerequisites != null)
				{
					for (int j = 0; j < this.entDef.buildingPrerequisites.Count; j++)
					{
						if (!base.Map.listerBuildings.ColonistsHaveBuilding(this.entDef.buildingPrerequisites[j]))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		public override int DraggableDimensions
		{
			get
			{
				return this.entDef.placingDraggableDimensions;
			}
		}

		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		public override string HighlightTag
		{
			get
			{
				if (base.cachedHighlightTag == null && base.tutorTag != null)
				{
					base.cachedHighlightTag = "Designator-Build-" + base.tutorTag;
				}
				return base.cachedHighlightTag;
			}
		}

		public Designator_Build(BuildableDef entDef)
		{
			this.entDef = entDef;
			base.icon = entDef.uiIcon;
			base.iconAngle = entDef.uiIconAngle;
			base.hotKey = entDef.designationHotKey;
			base.tutorTag = entDef.defName;
			ThingDef thingDef = entDef as ThingDef;
			if (thingDef != null && thingDef.uiIconPath.NullOrEmpty())
			{
				base.iconProportions = thingDef.graphicData.drawSize;
				base.iconDrawScale = GenUI.IconDrawScale(thingDef);
			}
			else
			{
				base.iconProportions = new Vector2(1f, 1f);
				base.iconDrawScale = 1f;
			}
			TerrainDef terrainDef = entDef as TerrainDef;
			if (terrainDef != null)
			{
				Vector2 terrainTextureCroppedSize = Designator_Build.TerrainTextureCroppedSize;
				float width = terrainTextureCroppedSize.x / (float)base.icon.width;
				Vector2 terrainTextureCroppedSize2 = Designator_Build.TerrainTextureCroppedSize;
				base.iconTexCoords = new Rect(0f, 0f, width, terrainTextureCroppedSize2.y / (float)base.icon.height);
			}
			this.ResetStuffToDefault();
		}

		public void ResetStuffToDefault()
		{
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null && thingDef.MadeFromStuff)
			{
				this.stuffDef = GenStuff.DefaultStuffFor(thingDef);
			}
		}

		public override void DrawMouseAttachments()
		{
			base.DrawMouseAttachments();
			if (!ArchitectCategoryTab.InfoRect.Contains(UI.MousePositionOnUIInverted))
			{
				DesignationDragger dragger = Find.DesignatorManager.Dragger;
				int num = (!dragger.Dragging) ? 1 : dragger.DragCells.Count();
				float num2 = 0f;
				Vector2 vector = Event.current.mousePosition + Designator_Build.DragPriceDrawOffset;
				List<ThingCountClass> list = this.entDef.CostListAdjusted(this.stuffDef, true);
				for (int i = 0; i < list.Count; i++)
				{
					ThingCountClass thingCountClass = list[i];
					float y = vector.y + num2;
					Rect position = new Rect(vector.x, y, 27f, 27f);
					GUI.DrawTexture(position, thingCountClass.thingDef.uiIcon);
					Rect rect = new Rect((float)(vector.x + 29.0), y, 999f, 29f);
					int num3 = num * thingCountClass.count;
					string text = num3.ToString();
					if (base.Map.resourceCounter.GetCount(thingCountClass.thingDef) < num3)
					{
						GUI.color = Color.red;
						text = text + " (" + "NotEnoughStoredLower".Translate() + ")";
					}
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect, text);
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
					num2 = (float)(num2 + 29.0);
				}
			}
		}

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
					foreach (ThingDef key in base.Map.resourceCounter.AllCountedAmounts.Keys)
					{
						if (key.IsStuff && key.stuffProps.CanMake(thingDef) && (DebugSettings.godMode || base.Map.listerThings.ThingsOfDef(key).Count > 0))
						{
							ThingDef localStuffDef = key;
							string labelCap = localStuffDef.LabelCap;
							FloatMenuOption floatMenuOption = new FloatMenuOption(labelCap, delegate
							{
								base.ProcessInput(ev);
								Find.DesignatorManager.Select(this);
								this.stuffDef = localStuffDef;
								this.writeStuff = true;
							}, MenuOptionPriority.Default, null, null, 0f, null, null);
							floatMenuOption.tutorTag = "SelectStuff-" + thingDef.defName + "-" + localStuffDef.defName;
							list.Add(floatMenuOption);
						}
					}
					if (list.Count == 0)
					{
						Messages.Message("NoStuffsToBuildWith".Translate(), MessageTypeDefOf.RejectInput);
					}
					else
					{
						FloatMenu floatMenu = new FloatMenu(list);
						floatMenu.vanishIfMouseDistant = true;
						Find.WindowStack.Add(floatMenu);
						Find.DesignatorManager.Select(this);
					}
				}
			}
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return GenConstruct.CanPlaceBlueprintAt(this.entDef, c, base.placingRot, base.Map, DebugSettings.godMode, null);
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(base.TutorTagDesignate, c)))
				return;
			if (DebugSettings.godMode || this.entDef.GetStatValueAbstract(StatDefOf.WorkToBuild, this.stuffDef) == 0.0)
			{
				if (this.entDef is TerrainDef)
				{
					base.Map.terrainGrid.SetTerrain(c, (TerrainDef)this.entDef);
				}
				else
				{
					Thing thing = ThingMaker.MakeThing((ThingDef)this.entDef, this.stuffDef);
					thing.SetFactionDirect(Faction.OfPlayer);
					GenSpawn.Spawn(thing, c, base.Map, base.placingRot, false);
				}
			}
			else
			{
				GenSpawn.WipeExistingThings(c, base.placingRot, this.entDef.blueprintDef, base.Map, DestroyMode.Deconstruct);
				GenConstruct.PlaceBlueprintForBuild(this.entDef, c, base.Map, base.placingRot, Faction.OfPlayer, this.stuffDef);
			}
			MoteMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, base.placingRot, this.entDef.Size), base.Map);
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
					this.entDef.PlaceWorkers[i].PostPlace(base.Map, this.entDef, c, base.placingRot);
				}
			}
		}

		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.entDef);
		}

		public override void DrawPanelReadout(ref float curY, float width)
		{
			if (this.entDef.costStuffCount <= 0 && this.stuffDef != null)
			{
				this.stuffDef = null;
			}
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null)
			{
				Widgets.InfoCardButton((float)(width - 24.0 - 6.0), 6f, thingDef, this.stuffDef);
			}
			else
			{
				Widgets.InfoCardButton((float)(width - 24.0 - 6.0), 6f, this.entDef);
			}
			Text.Font = GameFont.Tiny;
			List<ThingCountClass> list = this.entDef.CostListAdjusted(this.stuffDef, false);
			for (int i = 0; i < list.Count; i++)
			{
				ThingCountClass thingCountClass = list[i];
				Color color = GUI.color;
				Texture2D image;
				if (thingCountClass.thingDef == null)
				{
					image = BaseContent.BadTex;
				}
				else
				{
					image = thingCountClass.thingDef.uiIcon;
					GUI.color = thingCountClass.thingDef.graphicData.color;
				}
				GUI.DrawTexture(new Rect(0f, curY, 20f, 20f), image);
				GUI.color = color;
				if (thingCountClass.thingDef != null && thingCountClass.thingDef.resourceReadoutPriority != 0 && base.Map.resourceCounter.GetCount(thingCountClass.thingDef) < thingCountClass.count)
				{
					GUI.color = Color.red;
				}
				Widgets.Label(new Rect(26f, (float)(curY + 2.0), 50f, 100f), thingCountClass.count.ToString());
				GUI.color = Color.white;
				string text = (thingCountClass.thingDef != null) ? thingCountClass.thingDef.LabelCap : ("(" + "UnchosenStuff".Translate() + ")");
				float width2 = (float)(width - 60.0);
				float num = (float)(Text.CalcHeight(text, width2) - 2.0);
				Widgets.Label(new Rect(60f, (float)(curY + 2.0), width2, num), text);
				curY += num;
			}
			if (this.entDef.constructionSkillPrerequisite > 0)
			{
				Text.Font = GameFont.Tiny;
				Rect rect = new Rect(0f, (float)(curY + 2.0), width, 24f);
				Widgets.Label(rect, string.Format("{0}: {1}", "ConstructionNeeded".Translate(), this.entDef.constructionSkillPrerequisite));
				curY += 18f;
			}
		}

		public void SetStuffDef(ThingDef stuffDef)
		{
			this.stuffDef = stuffDef;
		}
	}
}
