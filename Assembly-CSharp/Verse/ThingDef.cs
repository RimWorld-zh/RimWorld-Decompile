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
		public Type thingClass;

		public ThingCategory category;

		public TickerType tickerType;

		public int stackLimit = 1;

		public IntVec2 size = new IntVec2(1, 1);

		public bool destroyable = true;

		public bool rotatable = true;

		public bool smallVolume;

		public bool useHitPoints = true;

		public bool receivesSignals;

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

		[NoTranslate]
		public List<string> itemGeneratorTags;

		public bool alwaysFlee;

		public List<Tool> tools;

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

		public ThingDef projectileWhenLoaded;

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

		public SkyfallerProperties skyfaller;

		public const int SmallUnitPerVolume = 10;

		public const float SmallVolumePerUnit = 0.1f;

		private List<RecipeDef> allRecipesCached;

		private static List<VerbProperties> EmptyVerbPropertiesList = new List<VerbProperties>();

		private Dictionary<ThingDef, Thing> concreteExamplesInt;

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
						return true;
					case ThingCategory.Building:
						return true;
					default:
						return false;
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
				return this.category == ThingCategory.Item && (!this.verbs.NullOrEmpty() || !this.tools.NullOrEmpty());
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

		public bool IsNonMedicalDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.drugCategory != DrugCategory.Medical;
			}
		}

		public bool IsTable
		{
			get
			{
				return this.surfaceType == SurfaceType.Eat && this.HasComp(typeof(CompGatherSpot));
			}
		}

		public bool IsWorkTable
		{
			get
			{
				return typeof(Building_WorkTable).IsAssignableFrom(this.thingClass);
			}
		}

		public bool IsShell
		{
			get
			{
				return this.projectileWhenLoaded != null;
			}
		}

		public bool IsArt
		{
			get
			{
				return this.IsWithinCategory(ThingCategoryDefOf.Art);
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

		public bool IsMeat
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.MeatRaw);
			}
		}

		public bool IsLeather
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.Leathers);
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
				if (!this.verbs.NullOrEmpty())
				{
					for (int i = 0; i < this.verbs.Count; i++)
					{
						if (!this.verbs[i].MeleeRange)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public bool IsMeleeWeapon
		{
			get
			{
				return this.IsWeapon && !this.IsRangedWeapon;
			}
		}

		public bool IsBuildingArtificial
		{
			get
			{
				return (this.category == ThingCategory.Building || this.IsFrame) && (this.building == null || (!this.building.isNaturalRock && !this.building.isResourceRock));
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

		public Thing GetConcreteExample(ThingDef stuff = null)
		{
			if (this.concreteExamplesInt == null)
			{
				this.concreteExamplesInt = new Dictionary<ThingDef, Thing>();
			}
			if (stuff == null)
			{
				stuff = ThingDefOf.Steel;
			}
			if (!this.concreteExamplesInt.ContainsKey(stuff))
			{
				if (this.race == null)
				{
					this.concreteExamplesInt[stuff] = ThingMaker.MakeThing(this, (!base.MadeFromStuff) ? null : stuff);
				}
				else
				{
					this.concreteExamplesInt[stuff] = PawnGenerator.GeneratePawn((from pkd in DefDatabase<PawnKindDef>.AllDefsListForReading
					where pkd.race == this
					select pkd).FirstOrDefault(), null);
				}
			}
			return this.concreteExamplesInt[stuff];
		}

		public List<Verb> GetConcreteExampleVerbs(Def def, ThingDef stuff = null)
		{
			List<Verb> result = null;
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				Thing concreteExample = thingDef.GetConcreteExample(stuff);
				if (concreteExample is Pawn)
				{
					result = (concreteExample as Pawn).verbTracker.AllVerbs;
				}
				else if (concreteExample is ThingWithComps)
				{
					result = (concreteExample as ThingWithComps).GetComp<CompEquippable>().AllVerbs;
				}
			}
			HediffDef hediffDef = def as HediffDef;
			if (hediffDef != null)
			{
				Hediff concreteExample2 = hediffDef.ConcreteExample;
				result = concreteExample2.TryGetComp<HediffComp_VerbGiver>().VerbTracker.AllVerbs;
			}
			return result;
		}

		public CompProperties CompDefFor<T>() where T : ThingComp
		{
			return this.comps.FirstOrDefault((CompProperties c) => c.compClass == typeof(T));
		}

		public CompProperties CompDefForAssignableFrom<T>() where T : ThingComp
		{
			return this.comps.FirstOrDefault((CompProperties c) => typeof(T).IsAssignableFrom(c.compClass));
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
				LongEventHandler.ExecuteWhenFinished(delegate
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
			if (this.inspectorTabs != null && this.inspectorTabs.Any())
			{
				this.inspectorTabsResolved = new List<InspectTabBase>();
				for (int i = 0; i < this.inspectorTabs.Count; i++)
				{
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
			if (this.comps != null)
			{
				for (int j = 0; j < this.comps.Count; j++)
				{
					this.comps[j].ResolveReferences(this);
				}
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = base.ConfigErrors().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string str = enumerator.Current;
					yield return str;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (base.label.NullOrEmpty())
			{
				yield return "no label";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.graphicData != null)
			{
				using (IEnumerator<string> enumerator2 = this.graphicData.ConfigErrors(this).GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						string err5 = enumerator2.Current;
						yield return err5;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.projectile != null)
			{
				using (IEnumerator<string> enumerator3 = this.projectile.ConfigErrors(this).GetEnumerator())
				{
					if (enumerator3.MoveNext())
					{
						string err4 = enumerator3.Current;
						yield return err4;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (base.statBases != null)
			{
				using (List<StatModifier>.Enumerator enumerator4 = base.statBases.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						_003CConfigErrors_003Ec__Iterator0 _003CConfigErrors_003Ec__Iterator = (_003CConfigErrors_003Ec__Iterator0)/*Error near IL_0317: stateMachine*/;
						StatModifier statBase = enumerator4.Current;
						if ((from st in base.statBases
						where st.stat == statBase.stat
						select st).Count() > 1)
						{
							yield return base.defName + " defines the stat base " + statBase.stat + " more than once.";
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			if (char.IsNumber(base.defName[base.defName.Length - 1]))
			{
				yield return base.defName + " ends with a numerical digit, which is not allowed on ThingDefs.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.thingClass == null)
			{
				yield return base.defName + " has null thingClass.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.comps.Count > 0 && !typeof(ThingWithComps).IsAssignableFrom(this.thingClass))
			{
				yield return base.defName + " has components but it's thingClass is not a ThingWithComps";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.ConnectToPower && this.drawerType == DrawerType.RealtimeOnly && this.IsFrame)
			{
				yield return base.defName + " connects to power but does not add to map mesh. Will not create wire meshes.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (base.costList != null)
			{
				foreach (ThingCountClass cost in base.costList)
				{
					if (cost.count == 0)
					{
						yield return base.defName + " cost in " + cost.thingDef + " is zero.";
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.thingCategories != null)
			{
				ThingCategoryDef doubleCat = this.thingCategories.FirstOrDefault(delegate(ThingCategoryDef cat)
				{
					ThingDef _0024this2 = ((_003CConfigErrors_003Ec__Iterator0)/*Error near IL_0638: stateMachine*/)._0024this;
					return ((_003CConfigErrors_003Ec__Iterator0)/*Error near IL_0638: stateMachine*/)._0024this.thingCategories.Count((ThingCategoryDef c) => c == cat) > 1;
				});
				if (doubleCat != null)
				{
					yield return base.defName + " has duplicate thingCategory " + doubleCat + ".";
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.Fillage == FillCategory.Full && this.category != ThingCategory.Building)
			{
				yield return base.defName + " gives full cover but is not a building.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.comps.Any((CompProperties c) => c.compClass == typeof(CompPowerTrader)) && this.drawerType == DrawerType.MapMeshOnly)
			{
				yield return base.defName + " has PowerTrader comp but does not draw real time. It won't draw a needs-power overlay.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.equipmentType != 0)
			{
				if (this.techLevel == TechLevel.Undefined)
				{
					yield return base.defName + " has no tech level.";
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (!this.comps.Any((CompProperties c) => c.compClass == typeof(CompEquippable)))
				{
					yield return "is equipment but has no CompEquippable";
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.thingClass == typeof(Bullet) && this.projectile.damageDef == null)
			{
				yield return base.defName + " is a bullet but has no damageDef.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.destroyOnDrop && !base.menuHidden)
			{
				yield return base.defName + " has destroyOnDrop but not menuHidden.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.stackLimit > 1 && !this.drawGUIOverlay)
			{
				yield return base.defName + " has stackLimit > 1 but also has drawGUIOverlay = false.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.damageMultipliers != null)
			{
				using (List<DamageMultiplier>.Enumerator enumerator6 = this.damageMultipliers.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						_003CConfigErrors_003Ec__Iterator0 _003CConfigErrors_003Ec__Iterator2 = (_003CConfigErrors_003Ec__Iterator0)/*Error near IL_0959: stateMachine*/;
						DamageMultiplier mult = enumerator6.Current;
						if ((from m in this.damageMultipliers
						where m.damageDef == mult.damageDef
						select m).Count() > 1)
						{
							yield return base.defName + " has multiple damage multipliers for damageDef " + mult.damageDef;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			if (this.Fillage == FillCategory.Full && !this.IsEdifice())
			{
				yield return "fillPercent is 1.00 but is not edifice";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (base.MadeFromStuff && base.constructEffect != null)
			{
				yield return base.defName + " is madeFromStuff but has a defined constructEffect (which will always be overridden by stuff's construct animation).";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (base.MadeFromStuff && base.stuffCategories.NullOrEmpty())
			{
				yield return "madeFromStuff but has no stuffCategories.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (base.costList.NullOrEmpty() && base.costStuffCount <= 0 && this.recipeMaker != null)
			{
				yield return "has a recipeMaker but no costList or costStuffCount.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) > 9.9999997473787516E-06 && !this.CanEverDeteriorate)
			{
				yield return "has >0 DeteriorationRate but can't deteriorate.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.drawerType == DrawerType.MapMeshOnly && this.comps.Any((CompProperties c) => c.compClass == typeof(CompForbiddable)))
			{
				yield return "drawerType=MapMeshOnly but has a CompForbiddable, which must draw in real time.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.smeltProducts != null && this.smeltable)
			{
				yield return "has smeltProducts but has smeltable=false";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.equipmentType != 0 && this.verbs.NullOrEmpty() && this.tools.NullOrEmpty())
			{
				yield return "is equipment but has no verbs or tools";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.graphicData != null && this.graphicData.shadowData != null)
			{
				if (this.castEdgeShadows)
				{
					yield return "graphicData defines a shadowInfo but castEdgeShadows is also true";
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (this.staticSunShadowHeight > 0.0)
				{
					yield return "graphicData defines a shadowInfo but staticSunShadowHeight > 0";
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.race != null && this.verbs != null)
			{
				_003CConfigErrors_003Ec__Iterator0 _003CConfigErrors_003Ec__Iterator3 = (_003CConfigErrors_003Ec__Iterator0)/*Error near IL_0d42: stateMachine*/;
				int i;
				for (i = 0; i < this.verbs.Count; i++)
				{
					if (this.verbs[i].linkedBodyPartsGroup != null && !this.race.body.AllParts.Any((BodyPartRecord part) => part.groups.Contains(_003CConfigErrors_003Ec__Iterator3._0024this.verbs[i].linkedBodyPartsGroup)))
					{
						yield return "has verb with linkedBodyPartsGroup " + this.verbs[i].linkedBodyPartsGroup + " but body " + this.race.body + " has no parts with that group.";
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.race != null && this.tools != null)
			{
				_003CConfigErrors_003Ec__Iterator0 _003CConfigErrors_003Ec__Iterator4 = (_003CConfigErrors_003Ec__Iterator0)/*Error near IL_0e8b: stateMachine*/;
				int j;
				for (j = 0; j < this.tools.Count; j++)
				{
					if (this.tools[j].linkedBodyPartsGroup != null && !this.race.body.AllParts.Any((BodyPartRecord part) => part.groups.Contains(_003CConfigErrors_003Ec__Iterator4._0024this.tools[j].linkedBodyPartsGroup)))
					{
						yield return "has tool with linkedBodyPartsGroup " + this.tools[j].linkedBodyPartsGroup + " but body " + this.race.body + " has no parts with that group.";
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.building != null)
			{
				using (IEnumerator<string> enumerator7 = this.building.ConfigErrors(this).GetEnumerator())
				{
					if (enumerator7.MoveNext())
					{
						string err3 = enumerator7.Current;
						yield return err3;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.apparel != null)
			{
				using (IEnumerator<string> enumerator8 = this.apparel.ConfigErrors(this).GetEnumerator())
				{
					if (enumerator8.MoveNext())
					{
						string err2 = enumerator8.Current;
						yield return err2;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.comps != null)
			{
				for (int k = 0; k < this.comps.Count; k++)
				{
					using (IEnumerator<string> enumerator9 = this.comps[k].ConfigErrors(this).GetEnumerator())
					{
						if (enumerator9.MoveNext())
						{
							string err = enumerator9.Current;
							yield return err;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			if (this.race != null)
			{
				using (IEnumerator<string> enumerator10 = this.race.ConfigErrors().GetEnumerator())
				{
					if (enumerator10.MoveNext())
					{
						string e3 = enumerator10.Current;
						yield return e3;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.ingestible != null)
			{
				using (IEnumerator<string> enumerator11 = this.ingestible.ConfigErrors(this).GetEnumerator())
				{
					if (enumerator11.MoveNext())
					{
						string e2 = enumerator11.Current;
						yield return e2;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.plant != null)
			{
				using (IEnumerator<string> enumerator12 = this.plant.ConfigErrors().GetEnumerator())
				{
					if (enumerator12.MoveNext())
					{
						string e = enumerator12.Current;
						yield return e;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.recipes != null && this.race != null)
			{
				foreach (RecipeDef recipe in this.recipes)
				{
					if (recipe.requireBed != this.race.FleshType.requiresBedForSurgery)
					{
						yield return string.Format("surgery bed requirement mismatch; flesh-type {0} is {1}, recipe {2} is {3}", this.race.FleshType, this.race.FleshType.requiresBedForSurgery, recipe, recipe.requireBed);
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.tools == null)
				yield break;
			Tool dupeTool = this.tools.SelectMany(delegate(Tool lhs)
			{
				ThingDef _0024this = ((_003CConfigErrors_003Ec__Iterator0)/*Error near IL_1514: stateMachine*/)._0024this;
				return from rhs in ((_003CConfigErrors_003Ec__Iterator0)/*Error near IL_1514: stateMachine*/)._0024this.tools
				where lhs != rhs && lhs.Id == rhs.Id
				select rhs;
			}).FirstOrDefault();
			if (dupeTool == null)
				yield break;
			yield return string.Format("duplicate thingdef tool id {0}", dupeTool.Id);
			/*Error: Unable to find new state assignment for yield return*/;
			IL_1573:
			/*Error near IL_1574: Unexpected return in MoveNext()*/;
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
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Covers".Translate(), coveredParts, 100, string.Empty);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.IsMedicine && this.MedicineTendXpGainFactor != 1.0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MedicineXpGainFactor".Translate(), this.MedicineTendXpGainFactor.ToStringPercent(), 0, string.Empty);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.fillPercent > 0.0 && this.fillPercent < 1.0 && (this.category == ThingCategory.Item || this.category == ThingCategory.Building || this.category == ThingCategory.Plant))
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "CoverEffectiveness".Translate(), this.BaseBlockChance().ToStringPercent(), 0, string.Empty)
				{
					overrideReportText = "CoverEffectivenessExplanation".Translate()
				};
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (base.constructionSkillPrerequisite > 0)
			{
				StatCategoryDef basics = StatCategoryDefOf.Basics;
				string label = "ConstructionSkillRequired".Translate();
				string valueString = base.constructionSkillPrerequisite.ToString();
				string overrideReportText = "ConstructionSkillRequiredExplanation".Translate();
				yield return new StatDrawEntry(basics, label, valueString, 0, overrideReportText);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!this.verbs.NullOrEmpty())
			{
				VerbProperties verb2 = (from x in this.verbs
				where x.isPrimary
				select x).First();
				object statCategoryDef;
				if (this.category == ThingCategory.Pawn)
				{
					StatCategoryDef basics = StatCategoryDefOf.PawnCombat;
					statCategoryDef = basics;
				}
				else
				{
					StatCategoryDef basics = StatCategoryDefOf.Weapon;
					statCategoryDef = basics;
				}
				StatCategoryDef verbStatCategory = (StatCategoryDef)statCategoryDef;
				float warmup = verb2.warmupTime;
				if (warmup > 0.0)
				{
					string warmupLabel = (this.category != ThingCategory.Pawn) ? "WarmupTime".Translate() : "MeleeWarmupTime".Translate();
					yield return new StatDrawEntry(verbStatCategory, warmupLabel, warmup.ToString("0.##") + " s", 40, string.Empty);
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (verb2.defaultProjectile != null)
				{
					float dam = (float)verb2.defaultProjectile.projectile.damageAmountBase;
					yield return new StatDrawEntry(verbStatCategory, "Damage".Translate(), dam.ToString(), 50, string.Empty);
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (verb2.LaunchesProjectile)
				{
					int burstShotCount = verb2.burstShotCount;
					double num = 60.0 / verb2.ticksBetweenBurstShots.TicksToSeconds();
					float range = verb2.range;
					if (burstShotCount > 1)
					{
						yield return new StatDrawEntry(verbStatCategory, "BurstShotCount".Translate(), burstShotCount.ToString(), 20, string.Empty);
						/*Error: Unable to find new state assignment for yield return*/;
					}
					yield return new StatDrawEntry(verbStatCategory, "Range".Translate(), range.ToString("0.##"), 10, string.Empty);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.plant != null)
			{
				using (IEnumerator<StatDrawEntry> enumerator = this.plant.SpecialDisplayStats().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						StatDrawEntry s5 = enumerator.Current;
						yield return s5;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.ingestible != null)
			{
				using (IEnumerator<StatDrawEntry> enumerator2 = this.ingestible.SpecialDisplayStats(this).GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						StatDrawEntry s4 = enumerator2.Current;
						yield return s4;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.race != null)
			{
				using (IEnumerator<StatDrawEntry> enumerator3 = this.race.SpecialDisplayStats(this).GetEnumerator())
				{
					if (enumerator3.MoveNext())
					{
						StatDrawEntry s3 = enumerator3.Current;
						yield return s3;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.isBodyPartOrImplant)
			{
				foreach (RecipeDef item in from x in DefDatabase<RecipeDef>.AllDefs
				where x.IsIngredient(((_003CSpecialDisplayStats_003Ec__Iterator1)/*Error near IL_0743: stateMachine*/)._0024this)
				select x)
				{
					HediffDef diff = item.addsHediff;
					if (diff != null)
					{
						if (diff.addedPartProps != null)
						{
							yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BodyPartEfficiency".Translate(), diff.addedPartProps.partEfficiency.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute), 0, string.Empty);
							/*Error: Unable to find new state assignment for yield return*/;
						}
						using (IEnumerator<StatDrawEntry> enumerator5 = diff.SpecialDisplayStats().GetEnumerator())
						{
							if (enumerator5.MoveNext())
							{
								StatDrawEntry s2 = enumerator5.Current;
								yield return s2;
								/*Error: Unable to find new state assignment for yield return*/;
							}
						}
						HediffCompProperties_VerbGiver vg = diff.CompProps<HediffCompProperties_VerbGiver>();
						if (vg != null)
						{
							if (!vg.verbs.NullOrEmpty())
							{
								VerbProperties verb = vg.verbs[0];
								if (!verb.MeleeRange)
								{
									if (verb.defaultProjectile != null)
									{
										int projDamage = verb.defaultProjectile.projectile.damageAmountBase;
										yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Damage".Translate(), projDamage.ToString(), 0, string.Empty);
										/*Error: Unable to find new state assignment for yield return*/;
									}
									goto IL_0a71;
								}
								int meleeDamage = verb.meleeDamageBaseAmount;
								yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Damage".Translate(), meleeDamage.ToString(), 0, string.Empty);
								/*Error: Unable to find new state assignment for yield return*/;
							}
							if (!vg.tools.NullOrEmpty())
							{
								Tool tool = vg.tools[0];
								yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Damage".Translate(), tool.power.ToString(), 0, string.Empty);
								/*Error: Unable to find new state assignment for yield return*/;
							}
						}
						goto IL_0a71;
					}
					continue;
					IL_0a71:
					ThoughtDef thought = DefDatabase<ThoughtDef>.AllDefs.FirstOrDefault((ThoughtDef x) => x.hediff == diff);
					if (thought != null && thought.stages != null && thought.stages.Any())
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MoodChange".Translate(), thought.stages.First().baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset), 0, string.Empty);
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				using (IEnumerator<StatDrawEntry> enumerator6 = this.comps[i].SpecialDisplayStats().GetEnumerator())
				{
					if (enumerator6.MoveNext())
					{
						StatDrawEntry s = enumerator6.Current;
						yield return s;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_0c22:
			/*Error near IL_0c23: Unexpected return in MoveNext()*/;
		}
	}
}
