using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	public class ThingDef : BuildableDef
	{
		public const int SmallUnitPerVolume = 10;

		public const float SmallVolumePerUnit = 0.1f;

		public Type thingClass;

		public ThingCategory category;

		public TickerType tickerType;

		public int stackLimit = 1;

		public IntVec2 size = new IntVec2(1, 1);

		public bool destroyable = true;

		public bool rotatable = true;

		public bool smallVolume;

		public bool useHitPoints = true;

		public List<CompProperties> comps = new List<CompProperties>();

		public List<ThingCountClass> killedLeavings;

		public List<ThingCountClass> butcherProducts;

		public List<ThingCountClass> smeltProducts;

		public bool smeltable;

		public bool randomizeRotationOnSpawn;

		public List<DamageMultiplier> damageMultipliers;

		public bool isBodyPartOrImplant;

		public RecipeMakerProperties recipeMaker;

		public ThingDef minifiedDef;

		public bool isUnfinishedThing;

		public bool leaveResourcesWhenKilled;

		public ThingDef slagDef;

		public bool isFrame;

		public IntVec3 interactionCellOffset = IntVec3.Zero;

		public bool hasInteractionCell;

		public ThingDef filthLeaving;

		public bool forceDebugSpawnable;

		public bool intricate;

		public bool scatterableOnMapGen = true;

		public float deepCommonality;

		public int deepCountPerCell = 150;

		public float generateCommonality = 1f;

		public float generateAllowChance = 1f;

		private bool canOverlapZones = true;

		public FloatRange startingHpRange = FloatRange.One;

		public GraphicData graphicData;

		public DrawerType drawerType = DrawerType.RealtimeOnly;

		public bool drawOffscreen;

		public ColorGenerator colorGenerator;

		public float hideAtSnowDepth = 99999f;

		public bool drawDamagedOverlay = true;

		public bool castEdgeShadows;

		public float staticSunShadowHeight;

		public bool selectable;

		public bool neverMultiSelect;

		public bool isAutoAttackableMapObject;

		public bool hasTooltip;

		public List<Type> inspectorTabs;

		[Unsaved]
		public List<InspectTabBase> inspectorTabsResolved;

		public bool seeThroughFog;

		public bool drawGUIOverlay;

		public ResourceCountPriority resourceReadoutPriority;

		public bool resourceReadoutAlwaysShow;

		public bool drawPlaceWorkersWhileSelected;

		public ConceptDef storedConceptLearnOpportunity;

		public float iconDrawScale = -1f;

		public bool alwaysHaulable;

		public bool designateHaulable;

		public List<ThingCategoryDef> thingCategories;

		public bool mineable;

		public bool socialPropernessMatters;

		public bool stealable = true;

		public SoundDef soundDrop;

		public SoundDef soundPickup;

		public SoundDef soundInteract;

		public SoundDef soundImpactDefault;

		public bool saveCompressible;

		public bool isSaveable = true;

		public bool holdsRoof;

		public float fillPercent;

		public bool coversFloor;

		public bool neverOverlapFloors;

		public SurfaceType surfaceType;

		public bool blockPlants;

		public bool blockLight;

		public bool blockWind;

		[Unsaved]
		public bool affectsRegions;

		public Tradeability tradeability = Tradeability.Stockable;

		[NoTranslate]
		public List<string> tradeTags;

		public bool tradeNeverStack;

		public ColorGenerator colorGeneratorInTraderStock;

		public Type blueprintClass = typeof(Blueprint_Build);

		public GraphicData blueprintGraphicData;

		public TerrainDef naturalTerrain;

		public TerrainDef leaveTerrain;

		public List<RecipeDef> recipes;

		private List<VerbProperties> verbs;

		public float equippedAngleOffset;

		public EquipmentType equipmentType;

		public TechLevel techLevel;

		[NoTranslate]
		public List<string> weaponTags;

		[NoTranslate]
		public List<string> techHediffsTags;

		public bool canBeSpawningInventory = true;

		public bool destroyOnDrop;

		public List<StatModifier> equippedStatOffsets;

		public BuildableDef entityDefToBuild;

		public IngestibleProperties ingestible;

		public FilthProperties filth;

		public GasProperties gas;

		public BuildingProperties building;

		public RaceProperties race;

		public ApparelProperties apparel;

		public MoteProperties mote;

		public PlantProperties plant;

		public ProjectileProperties projectile;

		public StuffProperties stuffProps;

		private List<RecipeDef> allRecipesCached;

		private static List<VerbProperties> EmptyVerbPropertiesList = new List<VerbProperties>();

		public bool EverHaulable
		{
			get
			{
				return this.alwaysHaulable || this.designateHaulable;
			}
		}

		public bool EverStoreable
		{
			get
			{
				return !this.thingCategories.NullOrEmpty();
			}
		}

		public float VolumePerUnit
		{
			get
			{
				return (float)(this.smallVolume ? 0.10000000149011612 : 1.0);
			}
		}

		public override Color IconDrawColor
		{
			get
			{
				return this.graphicData.color;
			}
		}

		public override IntVec2 Size
		{
			get
			{
				return this.size;
			}
		}

		public bool DiscardOnDestroyed
		{
			get
			{
				return this.race == null;
			}
		}

		public int BaseMaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValueAbstract(StatDefOf.MaxHitPoints, null));
			}
		}

		public float BaseFlammability
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Flammability, null);
			}
		}

		public float BaseMarketValue
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
			set
			{
				this.SetStatBaseValue(StatDefOf.MarketValue, value);
			}
		}

		public float BaseMass
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Mass, null);
			}
		}

		public bool PlayerAcquirable
		{
			get
			{
				return !this.destroyOnDrop;
			}
		}

		public bool EverTransmitsPower
		{
			get
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					CompProperties_Power compProperties_Power = this.comps[i] as CompProperties_Power;
					if (compProperties_Power != null && compProperties_Power.transmitsPower)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool Minifiable
		{
			get
			{
				return this.minifiedDef != null;
			}
		}

		public bool HasThingIDNumber
		{
			get
			{
				return this.category != ThingCategory.Mote;
			}
		}

		public List<RecipeDef> AllRecipes
		{
			get
			{
				if (this.allRecipesCached == null)
				{
					this.allRecipesCached = new List<RecipeDef>();
					if (this.recipes != null)
					{
						for (int i = 0; i < this.recipes.Count; i++)
						{
							this.allRecipesCached.Add(this.recipes[i]);
						}
					}
					List<RecipeDef> allDefsListForReading = DefDatabase<RecipeDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].recipeUsers != null && allDefsListForReading[j].recipeUsers.Contains(this))
						{
							this.allRecipesCached.Add(allDefsListForReading[j]);
						}
					}
				}
				return this.allRecipesCached;
			}
		}

		public bool ConnectToPower
		{
			get
			{
				if (this.EverTransmitsPower)
				{
					return false;
				}
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == typeof(CompPowerBattery))
					{
						return true;
					}
					if (this.comps[i].compClass == typeof(CompPowerTrader))
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool CoexistsWithFloors
		{
			get
			{
				return !this.neverOverlapFloors && !this.coversFloor;
			}
		}

		public FillCategory Fillage
		{
			get
			{
				if (this.fillPercent < 0.0099999997764825821)
				{
					return FillCategory.None;
				}
				if (this.fillPercent > 0.99000000953674316)
				{
					return FillCategory.Full;
				}
				return FillCategory.Partial;
			}
		}

		public bool MakeFog
		{
			get
			{
				return this.Fillage == FillCategory.Full;
			}
		}

		public bool CanOverlapZones
		{
			get
			{
				if (this.building != null && this.building.SupportsPlants)
				{
					return false;
				}
				if (base.passability == Traversability.Impassable && this.category != ThingCategory.Plant)
				{
					return false;
				}
				if ((int)this.surfaceType >= 1)
				{
					return false;
				}
				if (typeof(ISlotGroupParent).IsAssignableFrom(this.thingClass))
				{
					return false;
				}
				if (!this.canOverlapZones)
				{
					return false;
				}
				if (this.IsBlueprint || this.IsFrame)
				{
					ThingDef thingDef = this.entityDefToBuild as ThingDef;
					if (thingDef != null)
					{
						return thingDef.CanOverlapZones;
					}
				}
				return true;
			}
		}

		public bool CountAsResource
		{
			get
			{
				return this.resourceReadoutPriority != ResourceCountPriority.Uncounted;
			}
		}

		public bool BlockPlanting
		{
			get
			{
				if (this.building != null && this.building.SupportsPlants)
				{
					return false;
				}
				if (this.blockPlants)
				{
					return true;
				}
				if (this.category == ThingCategory.Plant)
				{
					return true;
				}
				if ((int)this.Fillage > 0)
				{
					return true;
				}
				if (this.IsEdifice())
				{
					return true;
				}
				return false;
			}
		}

		public List<VerbProperties> Verbs
		{
			get
			{
				if (this.verbs != null)
				{
					return this.verbs;
				}
				return ThingDef.EmptyVerbPropertiesList;
			}
		}

		public bool CanHaveFaction
		{
			get
			{
				if (!this.IsBlueprint && !this.IsFrame)
				{
					switch (this.category)
					{
					case ThingCategory.Pawn:
					{
						return true;
					}
					case ThingCategory.Building:
					{
						return true;
					}
					default:
					{
						return false;
					}
					}
				}
				return true;
			}
		}

		public bool Claimable
		{
			get
			{
				return this.building != null && this.building.claimable;
			}
		}

		public ThingCategoryDef FirstThingCategory
		{
			get
			{
				if (this.thingCategories.NullOrEmpty())
				{
					return null;
				}
				return this.thingCategories[0];
			}
		}

		public float MedicineTendXpGainFactor
		{
			get
			{
				return Mathf.Clamp((float)(this.GetStatValueAbstract(StatDefOf.MedicalPotency, null) * 0.699999988079071), 0.5f, 1f);
			}
		}

		public bool CanEverDeteriorate
		{
			get
			{
				if (!this.useHitPoints)
				{
					return false;
				}
				return this.category == ThingCategory.Item || this == ThingDefOf.BurnedTree;
			}
		}

		public bool AffectsRegions
		{
			get
			{
				return base.passability == Traversability.Impassable || this.IsDoor;
			}
		}

		public bool AffectsReachability
		{
			get
			{
				if (this.AffectsRegions)
				{
					return true;
				}
				if (base.passability != Traversability.Impassable && !this.IsDoor)
				{
					if (TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(this))
					{
						return true;
					}
					return false;
				}
				return true;
			}
		}

		public bool IsApparel
		{
			get
			{
				return this.apparel != null;
			}
		}

		public bool IsBed
		{
			get
			{
				return typeof(Building_Bed).IsAssignableFrom(this.thingClass);
			}
		}

		public bool IsCorpse
		{
			get
			{
				return typeof(Corpse).IsAssignableFrom(this.thingClass);
			}
		}

		public bool IsFrame
		{
			get
			{
				return this.isFrame;
			}
		}

		public bool IsBlueprint
		{
			get
			{
				return this.entityDefToBuild != null && this.category == ThingCategory.Ethereal;
			}
		}

		public bool IsStuff
		{
			get
			{
				return this.stuffProps != null;
			}
		}

		public bool IsMedicine
		{
			get
			{
				return base.statBases.StatListContains(StatDefOf.MedicalPotency);
			}
		}

		public bool IsDoor
		{
			get
			{
				return typeof(Building_Door).IsAssignableFrom(this.thingClass);
			}
		}

		public bool IsFilth
		{
			get
			{
				return this.filth != null;
			}
		}

		public bool IsIngestible
		{
			get
			{
				return this.ingestible != null;
			}
		}

		public bool IsNutritionGivingIngestible
		{
			get
			{
				return this.IsIngestible && this.ingestible.nutrition > 0.0;
			}
		}

		public bool IsWeapon
		{
			get
			{
				return this.category == ThingCategory.Item && !this.verbs.NullOrEmpty();
			}
		}

		public bool IsCommsConsole
		{
			get
			{
				return typeof(Building_CommsConsole).IsAssignableFrom(this.thingClass);
			}
		}

		public bool IsOrbitalTradeBeacon
		{
			get
			{
				return typeof(Building_OrbitalTradeBeacon).IsAssignableFrom(this.thingClass);
			}
		}

		public bool IsFoodDispenser
		{
			get
			{
				return typeof(Building_NutrientPasteDispenser).IsAssignableFrom(this.thingClass);
			}
		}

		public bool IsDrug
		{
			get
			{
				return this.ingestible != null && this.ingestible.drugCategory != DrugCategory.None;
			}
		}

		public bool IsPleasureDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.joy > 0.0;
			}
		}

		public bool IsTable
		{
			get
			{
				return this.surfaceType == SurfaceType.Eat && this.HasComp(typeof(CompGatherSpot));
			}
		}

		public bool IsAddictiveDrug
		{
			get
			{
				CompProperties_Drug compProperties = this.GetCompProperties<CompProperties_Drug>();
				return compProperties != null && compProperties.addictiveness > 0.0;
			}
		}

		public bool IsRangedWeapon
		{
			get
			{
				if (!this.IsWeapon)
				{
					return false;
				}
				for (int i = 0; i < this.verbs.Count; i++)
				{
					if (!this.verbs[i].MeleeRange)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool IsMeleeWeapon
		{
			get
			{
				if (!this.IsWeapon)
				{
					return false;
				}
				for (int i = 0; i < this.verbs.Count; i++)
				{
					if (this.verbs[i].MeleeRange)
					{
						return true;
					}
				}
				return false;
			}
		}

		public string LabelAsStuff
		{
			get
			{
				if (!this.stuffProps.stuffAdjective.NullOrEmpty())
				{
					return this.stuffProps.stuffAdjective;
				}
				return base.label;
			}
		}

		public CompProperties CompDefFor<T>() where T : ThingComp
		{
			return ((IEnumerable<CompProperties>)this.comps).FirstOrDefault<CompProperties>((Func<CompProperties, bool>)((CompProperties c) => c.compClass == typeof(T)));
		}

		public CompProperties CompDefForAssignableFrom<T>() where T : ThingComp
		{
			return ((IEnumerable<CompProperties>)this.comps).FirstOrDefault<CompProperties>((Func<CompProperties, bool>)((CompProperties c) => typeof(T).IsAssignableFrom(c.compClass)));
		}

		public bool HasComp(Type compType)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i].compClass == compType)
				{
					return true;
				}
			}
			return false;
		}

		public T GetCompProperties<T>() where T : CompProperties
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				T val = (T)(this.comps[i] as T);
				if (val != null)
				{
					return val;
				}
			}
			return (T)null;
		}

		public override void PostLoad()
		{
			if (this.graphicData != null)
			{
				LongEventHandler.ExecuteWhenFinished((Action)delegate
				{
					if (this.graphicData.shaderType == ShaderType.None)
					{
						this.graphicData.shaderType = ShaderType.Cutout;
					}
					base.graphic = this.graphicData.Graphic;
				});
			}
			if (this.verbs != null && this.verbs.Count == 1)
			{
				this.verbs[0].label = base.label;
			}
			base.PostLoad();
			if (this.category == ThingCategory.Building && this.building == null)
			{
				this.building = new BuildingProperties();
			}
			if (this.inspectorTabs != null)
			{
				for (int i = 0; i < this.inspectorTabs.Count; i++)
				{
					if (this.inspectorTabsResolved == null)
					{
						this.inspectorTabsResolved = new List<InspectTabBase>();
					}
					try
					{
						this.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(this.inspectorTabs[i]));
					}
					catch (Exception ex)
					{
						Log.Error("Could not instantiate inspector tab of type " + this.inspectorTabs[i] + ": " + ex);
					}
				}
			}
			if (this.building != null)
			{
				this.building.PostLoadSpecial(this);
			}
			if (this.plant != null)
			{
				this.plant.PostLoadSpecial(this);
			}
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.building != null)
			{
				this.building.ResolveReferencesSpecial();
			}
			if (this.graphicData != null)
			{
				this.graphicData.ResolveReferencesSpecial();
			}
			if (this.race != null)
			{
				this.race.ResolveReferencesSpecial();
			}
			if (this.soundImpactDefault == null)
			{
				this.soundImpactDefault = SoundDefOf.BulletImpactGround;
			}
			if (this.soundDrop == null)
			{
				this.soundDrop = SoundDefOf.Standard_Drop;
			}
			if (this.soundPickup == null)
			{
				this.soundPickup = SoundDefOf.Standard_Pickup;
			}
			if (this.soundInteract == null)
			{
				this.soundPickup = SoundDefOf.Standard_Pickup;
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].ResolveReferences(this);
				}
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (base.label.NullOrEmpty())
			{
				yield return "no label";
			}
			if (this.graphicData != null)
			{
				foreach (string item2 in this.graphicData.ConfigErrors(this))
				{
					yield return item2;
				}
			}
			if (this.projectile != null)
			{
				foreach (string item3 in this.projectile.ConfigErrors(this))
				{
					yield return item3;
				}
			}
			if (base.statBases != null)
			{
				List<StatModifier>.Enumerator enumerator4 = base.statBases.GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						StatModifier statBase = enumerator4.Current;
						if ((from st in base.statBases
						where st.stat == ((_003CConfigErrors_003Ec__Iterator1E6)/*Error near IL_02ec: stateMachine*/)._003CstatBase_003E__7.stat
						select st).Count() > 1)
						{
							yield return base.defName + " defines the stat base " + statBase.stat + " more than once.";
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator4).Dispose();
				}
			}
			if (char.IsNumber(base.defName[base.defName.Length - 1]))
			{
				yield return base.defName + " ends with a numerical digit, which is not allowed on ThingDefs.";
			}
			if (this.thingClass == null)
			{
				yield return base.defName + " has null thingClass.";
			}
			if (this.comps.Count > 0 && !typeof(ThingWithComps).IsAssignableFrom(this.thingClass))
			{
				yield return base.defName + " has components but it's thingClass is not a ThingWithComps";
			}
			if (this.ConnectToPower && this.drawerType == DrawerType.RealtimeOnly && this.IsFrame)
			{
				yield return base.defName + " connects to power but does not add to map mesh. Will not create wire meshes.";
			}
			if (base.costList != null)
			{
				List<ThingCountClass>.Enumerator enumerator5 = base.costList.GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						ThingCountClass cost = enumerator5.Current;
						if (cost.count == 0)
						{
							yield return base.defName + " cost in " + cost.thingDef + " is zero.";
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator5).Dispose();
				}
			}
			if (this.thingCategories != null)
			{
				ThingCategoryDef doubleCat = this.thingCategories.FirstOrDefault((Func<ThingCategoryDef, bool>)delegate(ThingCategoryDef cat)
				{
					_003CConfigErrors_003Ec__Iterator1E6 _003CConfigErrors_003Ec__Iterator1E = (_003CConfigErrors_003Ec__Iterator1E6)/*Error near IL_05aa: stateMachine*/;
					return ((_003CConfigErrors_003Ec__Iterator1E6)/*Error near IL_05aa: stateMachine*/)._003C_003Ef__this.thingCategories.Count((Func<ThingCategoryDef, bool>)((ThingCategoryDef c) => c == cat)) > 1;
				});
				if (doubleCat != null)
				{
					yield return base.defName + " has duplicate thingCategory " + doubleCat + ".";
				}
			}
			if (this.Fillage == FillCategory.Full && this.category != ThingCategory.Building)
			{
				yield return base.defName + " gives full cover but is not a building.";
			}
			if (this.comps.Any((Predicate<CompProperties>)((CompProperties c) => c.compClass == typeof(CompPowerTrader))) && this.drawerType == DrawerType.MapMeshOnly)
			{
				yield return base.defName + " has PowerTrader comp but does not draw real time. It won't draw a needs-power overlay.";
			}
			if (this.equipmentType != 0)
			{
				if (this.techLevel == TechLevel.Undefined)
				{
					yield return base.defName + " has no tech level.";
				}
				if (!this.comps.Any((Predicate<CompProperties>)((CompProperties c) => c.compClass == typeof(CompEquippable))))
				{
					yield return "is equipment but has no CompEquippable";
				}
			}
			if (this.thingClass == typeof(Bullet) && this.projectile.damageDef == null)
			{
				yield return base.defName + " is a bullet but has no damageDef.";
			}
			if (this.destroyOnDrop && !base.menuHidden)
			{
				yield return base.defName + " has destroyOnDrop but not menuHidden.";
			}
			if (this.stackLimit > 1 && !this.drawGUIOverlay)
			{
				yield return base.defName + " has stackLimit > 1 but also has drawGUIOverlay = false.";
			}
			if (this.damageMultipliers != null)
			{
				List<DamageMultiplier>.Enumerator enumerator6 = this.damageMultipliers.GetEnumerator();
				try
				{
					while (enumerator6.MoveNext())
					{
						DamageMultiplier mult = enumerator6.Current;
						if ((from m in this.damageMultipliers
						where m.damageDef == ((_003CConfigErrors_003Ec__Iterator1E6)/*Error near IL_0896: stateMachine*/)._003Cmult_003E__12.damageDef
						select m).Count() > 1)
						{
							yield return base.defName + " has multiple damage multipliers for damageDef " + mult.damageDef;
							break;
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator6).Dispose();
				}
			}
			if (this.Fillage == FillCategory.Full && !this.IsEdifice())
			{
				yield return "fillPercent is 1.00 but is not edifice";
			}
			if (base.MadeFromStuff && base.constructEffect != null)
			{
				yield return base.defName + " is madeFromStuff but has a defined constructEffect (which will always be overridden by stuff's construct animation).";
			}
			if (base.MadeFromStuff && base.stuffCategories.NullOrEmpty())
			{
				yield return "madeFromStuff but has no stuffCategories.";
			}
			if (base.costList.NullOrEmpty() && base.costStuffCount <= 0 && this.recipeMaker != null)
			{
				yield return "has a recipeMaker but no costList or costStuffCount.";
			}
			if (this.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) > 9.9999997473787516E-06 && !this.CanEverDeteriorate)
			{
				yield return "has >0 DeteriorationRate but can't deteriorate.";
			}
			if (this.drawerType == DrawerType.MapMeshOnly && this.comps.Any((Predicate<CompProperties>)((CompProperties c) => c.compClass == typeof(CompForbiddable))))
			{
				yield return "drawerType=MapMeshOnly but has a CompForbiddable, which must draw in real time.";
			}
			if (this.smeltProducts != null && this.smeltable)
			{
				yield return "has smeltProducts but has smeltable=false";
			}
			if (this.graphicData != null && this.graphicData.shadowData != null)
			{
				if (this.castEdgeShadows)
				{
					yield return "graphicData defines a shadowInfo but castEdgeShadows is also true";
				}
				if (this.staticSunShadowHeight > 0.0)
				{
					yield return "graphicData defines a shadowInfo but staticSunShadowHeight > 0";
				}
			}
			if (this.race != null && this.verbs != null)
			{
				for (int j = 0; j < this.verbs.Count; j++)
				{
					if (this.verbs[j].linkedBodyPartsGroup != null && !this.race.body.AllParts.Any((Predicate<BodyPartRecord>)((BodyPartRecord part) => part.groups.Contains(((_003CConfigErrors_003Ec__Iterator1E6)/*Error near IL_0bd3: stateMachine*/)._003C_003Ef__this.verbs[((_003CConfigErrors_003Ec__Iterator1E6)/*Error near IL_0bd3: stateMachine*/)._003Ci_003E__13].linkedBodyPartsGroup))))
					{
						yield return "has verb with linkedBodyPartsGroup " + this.verbs[j].linkedBodyPartsGroup + " but body " + this.race.body + " has no parts with that group.";
					}
				}
			}
			if (this.building != null)
			{
				foreach (string item4 in this.building.ConfigErrors(this))
				{
					yield return item4;
				}
			}
			if (this.apparel != null)
			{
				foreach (string item5 in this.apparel.ConfigErrors(this))
				{
					yield return item5;
				}
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					foreach (string item6 in this.comps[i].ConfigErrors(this))
					{
						yield return item6;
					}
				}
			}
			if (this.race != null)
			{
				foreach (string item7 in this.race.ConfigErrors())
				{
					yield return item7;
				}
			}
			if (this.ingestible != null)
			{
				foreach (string item8 in this.ingestible.ConfigErrors(this))
				{
					yield return item8;
				}
			}
			if (this.plant != null)
			{
				foreach (string item9 in this.plant.ConfigErrors())
				{
					yield return item9;
				}
			}
		}

		public static ThingDef Named(string defName)
		{
			return DefDatabase<ThingDef>.GetNamed(defName, true);
		}

		public bool IsWithinCategory(ThingCategoryDef category)
		{
			if (this.thingCategories == null)
			{
				return false;
			}
			int num = 0;
			while (num < this.thingCategories.Count)
			{
				if (this.thingCategories[num] != category && !this.thingCategories[num].Parents.Contains(category))
				{
					num++;
					continue;
				}
				return true;
			}
			return false;
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.apparel != null)
			{
				string coveredParts = this.apparel.GetCoveredOuterPartsString(BodyDefOf.Human);
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Covers".Translate(), coveredParts, 100);
			}
			if (this.IsMedicine && this.MedicineTendXpGainFactor != 1.0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MedicineXpGainFactor".Translate(), this.MedicineTendXpGainFactor.ToStringPercent(), 0);
			}
			if (this.fillPercent > 0.0 && this.fillPercent < 1.0 && (this.category == ThingCategory.Item || this.category == ThingCategory.Building || this.category == ThingCategory.Plant))
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "CoverEffectiveness".Translate(), this.BaseBlockChance().ToStringPercent(), 0)
				{
					overrideReportText = "CoverEffectivenessExplanation".Translate()
				};
			}
			if (!this.verbs.NullOrEmpty())
			{
				VerbProperties verb2 = (from x in this.verbs
				where x.isPrimary
				select x).First();
				object obj;
				StatCategoryDef verbStatCategory3;
				if (this.category == ThingCategory.Pawn)
				{
					StatCategoryDef pawnCombat;
					verbStatCategory3 = (pawnCombat = StatCategoryDefOf.PawnCombat);
					obj = pawnCombat;
				}
				else
				{
					StatCategoryDef pawnCombat;
					verbStatCategory3 = (pawnCombat = StatCategoryDefOf.Weapon);
					obj = pawnCombat;
				}
				verbStatCategory3 = (StatCategoryDef)obj;
				float warmup = verb2.warmupTime;
				if (warmup > 0.0)
				{
					string warmupLabel = (this.category != ThingCategory.Pawn) ? "WarmupTime".Translate() : "MeleeWarmupTime".Translate();
					yield return new StatDrawEntry(verbStatCategory3, warmupLabel, warmup.ToString("0.##") + " s", 40);
				}
				if (verb2.projectileDef != null)
				{
					float dam = (float)verb2.projectileDef.projectile.damageAmountBase;
					yield return new StatDrawEntry(verbStatCategory3, "Damage".Translate(), dam.ToString(), 50);
				}
				if (verb2.projectileDef != null)
				{
					int burstShotCount = verb2.burstShotCount;
					float burstShotFireRate = (float)(60.0 / verb2.ticksBetweenBurstShots.TicksToSeconds());
					float range = verb2.range;
					if (burstShotCount > 1)
					{
						yield return new StatDrawEntry(verbStatCategory3, "BurstShotCount".Translate(), burstShotCount.ToString(), 20);
						yield return new StatDrawEntry(verbStatCategory3, "BurstShotFireRate".Translate(), burstShotFireRate.ToString("0.##") + " rpm", 19);
					}
					yield return new StatDrawEntry(verbStatCategory3, "Range".Translate(), range.ToString("0.##"), 10);
				}
			}
			if (this.plant != null)
			{
				foreach (StatDrawEntry item in this.plant.SpecialDisplayStats())
				{
					yield return item;
				}
			}
			if (this.ingestible != null)
			{
				foreach (StatDrawEntry item2 in this.ingestible.SpecialDisplayStats(this))
				{
					yield return item2;
				}
			}
			if (this.race != null)
			{
				foreach (StatDrawEntry item3 in this.race.SpecialDisplayStats(this))
				{
					yield return item3;
				}
			}
			if (this.isBodyPartOrImplant)
			{
				foreach (RecipeDef item4 in from x in DefDatabase<RecipeDef>.AllDefs
				where x.IsIngredient(((_003CSpecialDisplayStats_003Ec__Iterator1E7)/*Error near IL_063c: stateMachine*/)._003C_003Ef__this)
				select x)
				{
					HediffDef diff = item4.addsHediff;
					if (diff != null)
					{
						if (diff.addedPartProps != null)
						{
							yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BodyPartEfficiency".Translate(), diff.addedPartProps.partEfficiency.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute), 0);
						}
						foreach (StatDrawEntry item5 in diff.SpecialDisplayStats())
						{
							yield return item5;
						}
						HediffCompProperties_VerbGiver vg = diff.CompProps<HediffCompProperties_VerbGiver>();
						if (vg != null)
						{
							VerbProperties verb = vg.verbs.FirstOrDefault();
							if (!verb.MeleeRange)
							{
								int projDamage = verb.projectileDef.projectile.damageAmountBase;
								yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Damage".Translate(), projDamage.ToString(), 0);
							}
							else
							{
								int meleeDamage = verb.meleeDamageBaseAmount;
								yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Damage".Translate(), meleeDamage.ToString(), 0);
							}
						}
						ThoughtDef thought = DefDatabase<ThoughtDef>.AllDefs.FirstOrDefault((Func<ThoughtDef, bool>)((ThoughtDef x) => x.hediff == ((_003CSpecialDisplayStats_003Ec__Iterator1E7)/*Error near IL_0862: stateMachine*/)._003Cdiff_003E__18));
						if (thought != null && thought.stages != null && thought.stages.Any())
						{
							yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MoodChange".Translate(), thought.stages.First().baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset), 0);
						}
					}
				}
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (StatDrawEntry item6 in this.comps[i].SpecialDisplayStats())
				{
					yield return item6;
				}
			}
		}
	}
}
