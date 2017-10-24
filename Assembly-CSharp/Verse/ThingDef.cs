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

		public TickerType tickerType = TickerType.Never;

		public int stackLimit = 1;

		public IntVec2 size = new IntVec2(1, 1);

		public bool destroyable = true;

		public bool rotatable = true;

		public bool smallVolume = false;

		public bool useHitPoints = true;

		public bool receivesSignals = false;

		public List<CompProperties> comps = new List<CompProperties>();

		public List<ThingCountClass> killedLeavings = null;

		public List<ThingCountClass> butcherProducts = null;

		public List<ThingCountClass> smeltProducts = null;

		public bool smeltable = false;

		public bool randomizeRotationOnSpawn = false;

		public List<DamageMultiplier> damageMultipliers = null;

		public bool isBodyPartOrImplant = false;

		public RecipeMakerProperties recipeMaker = null;

		public ThingDef minifiedDef = null;

		public bool isUnfinishedThing = false;

		public bool leaveResourcesWhenKilled = false;

		public ThingDef slagDef = null;

		public bool isFrame = false;

		public IntVec3 interactionCellOffset = IntVec3.Zero;

		public bool hasInteractionCell = false;

		public ThingDef filthLeaving = null;

		public bool forceDebugSpawnable = false;

		public bool intricate = false;

		public bool scatterableOnMapGen = true;

		public float deepCommonality = 0f;

		public int deepCountPerCell = 150;

		public float generateCommonality = 1f;

		public float generateAllowChance = 1f;

		private bool canOverlapZones = true;

		public FloatRange startingHpRange = FloatRange.One;

		[NoTranslate]
		public List<string> itemGeneratorTags = null;

		public bool alwaysFlee;

		public List<Tool> tools;

		public GraphicData graphicData = null;

		public DrawerType drawerType = DrawerType.RealtimeOnly;

		public bool drawOffscreen = false;

		public ColorGenerator colorGenerator = null;

		public float hideAtSnowDepth = 99999f;

		public bool drawDamagedOverlay = true;

		public bool castEdgeShadows = false;

		public float staticSunShadowHeight = 0f;

		public bool selectable = false;

		public bool neverMultiSelect = false;

		public bool isAutoAttackableMapObject = false;

		public bool hasTooltip = false;

		public List<Type> inspectorTabs = null;

		[Unsaved]
		public List<InspectTabBase> inspectorTabsResolved = null;

		public bool seeThroughFog = false;

		public bool drawGUIOverlay = false;

		public ResourceCountPriority resourceReadoutPriority = ResourceCountPriority.Uncounted;

		public bool resourceReadoutAlwaysShow = false;

		public bool drawPlaceWorkersWhileSelected = false;

		public ConceptDef storedConceptLearnOpportunity = null;

		public float iconDrawScale = -1f;

		public bool alwaysHaulable = false;

		public bool designateHaulable = false;

		public List<ThingCategoryDef> thingCategories = null;

		public bool mineable = false;

		public bool socialPropernessMatters = false;

		public bool stealable = true;

		public SoundDef soundDrop;

		public SoundDef soundPickup;

		public SoundDef soundInteract;

		public SoundDef soundImpactDefault;

		public bool saveCompressible = false;

		public bool isSaveable = true;

		public bool holdsRoof = false;

		public float fillPercent = 0f;

		public bool coversFloor = false;

		public bool neverOverlapFloors = false;

		public SurfaceType surfaceType = SurfaceType.None;

		public bool blockPlants = false;

		public bool blockLight = false;

		public bool blockWind = false;

		[Unsaved]
		public bool affectsRegions = false;

		public Tradeability tradeability = Tradeability.Stockable;

		[NoTranslate]
		public List<string> tradeTags = null;

		public bool tradeNeverStack = false;

		public ColorGenerator colorGeneratorInTraderStock = null;

		public Type blueprintClass = typeof(Blueprint_Build);

		public GraphicData blueprintGraphicData = null;

		public TerrainDef naturalTerrain = null;

		public TerrainDef leaveTerrain = null;

		public List<RecipeDef> recipes = null;

		private List<VerbProperties> verbs = null;

		public float equippedAngleOffset = 0f;

		public EquipmentType equipmentType = EquipmentType.None;

		public TechLevel techLevel = TechLevel.Undefined;

		[NoTranslate]
		public List<string> weaponTags = null;

		[NoTranslate]
		public List<string> techHediffsTags = null;

		public bool canBeSpawningInventory = true;

		public bool destroyOnDrop = false;

		public List<StatModifier> equippedStatOffsets = null;

		public BuildableDef entityDefToBuild = null;

		public ThingDef projectileWhenLoaded;

		public IngestibleProperties ingestible = null;

		public FilthProperties filth = null;

		public GasProperties gas = null;

		public BuildingProperties building = null;

		public RaceProperties race = null;

		public ApparelProperties apparel = null;

		public MoteProperties mote = null;

		public PlantProperties plant = null;

		public ProjectileProperties projectile = null;

		public StuffProperties stuffProps = null;

		public SkyfallerProperties skyfaller = null;

		public const int SmallUnitPerVolume = 10;

		public const float SmallVolumePerUnit = 0.1f;

		private List<RecipeDef> allRecipesCached = null;

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
				int num = 0;
				bool result;
				while (true)
				{
					if (num < this.comps.Count)
					{
						CompProperties_Power compProperties_Power = this.comps[num] as CompProperties_Power;
						if (compProperties_Power != null && compProperties_Power.transmitsPower)
						{
							result = true;
							break;
						}
						num++;
						continue;
					}
					result = false;
					break;
				}
				return result;
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
				bool result;
				if (this.EverTransmitsPower)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].compClass == typeof(CompPowerBattery))
							goto IL_003b;
						if (this.comps[i].compClass == typeof(CompPowerTrader))
							goto IL_0062;
					}
					result = false;
				}
				goto IL_0086;
				IL_0062:
				result = true;
				goto IL_0086;
				IL_0086:
				return result;
				IL_003b:
				result = true;
				goto IL_0086;
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
				return (FillCategory)((!(this.fillPercent < 0.0099999997764825821)) ? ((!(this.fillPercent > 0.99000000953674316)) ? 1 : 2) : 0);
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
				bool result;
				if (this.building != null && this.building.SupportsPlants)
				{
					result = false;
				}
				else if (base.passability == Traversability.Impassable && this.category != ThingCategory.Plant)
				{
					result = false;
				}
				else if ((int)this.surfaceType >= 1)
				{
					result = false;
				}
				else if (typeof(ISlotGroupParent).IsAssignableFrom(this.thingClass))
				{
					result = false;
				}
				else if (!this.canOverlapZones)
				{
					result = false;
				}
				else
				{
					if (this.IsBlueprint || this.IsFrame)
					{
						ThingDef thingDef = this.entityDefToBuild as ThingDef;
						if (thingDef != null)
						{
							result = thingDef.CanOverlapZones;
							goto IL_00c5;
						}
					}
					result = true;
				}
				goto IL_00c5;
				IL_00c5:
				return result;
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
				return (byte)((this.building == null || !this.building.SupportsPlants) ? (this.blockPlants ? 1 : ((this.category == ThingCategory.Plant) ? 1 : (((int)this.Fillage > 0) ? 1 : (this.IsEdifice() ? 1 : 0)))) : 0) != 0;
			}
		}

		public List<VerbProperties> Verbs
		{
			get
			{
				return (this.verbs == null) ? ThingDef.EmptyVerbPropertiesList : this.verbs;
			}
		}

		public bool CanHaveFaction
		{
			get
			{
				bool result;
				if (!this.IsBlueprint && !this.IsFrame)
				{
					switch (this.category)
					{
					case ThingCategory.Pawn:
					{
						result = true;
						break;
					}
					case ThingCategory.Building:
					{
						result = true;
						break;
					}
					default:
					{
						result = false;
						break;
					}
					}
				}
				else
				{
					result = true;
				}
				return result;
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
				return (!this.thingCategories.NullOrEmpty()) ? this.thingCategories[0] : null;
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
				return this.useHitPoints && (this.category == ThingCategory.Item || this == ThingDefOf.BurnedTree);
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
				return (byte)(this.AffectsRegions ? 1 : ((base.passability == Traversability.Impassable || this.IsDoor) ? 1 : (TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(this) ? 1 : 0))) != 0;
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
				bool result;
				if (!this.IsWeapon)
				{
					result = false;
				}
				else
				{
					if (!this.verbs.NullOrEmpty())
					{
						for (int i = 0; i < this.verbs.Count; i++)
						{
							if (!this.verbs[i].MeleeRange)
								goto IL_0042;
						}
					}
					result = false;
				}
				goto IL_0067;
				IL_0067:
				return result;
				IL_0042:
				result = true;
				goto IL_0067;
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
				return this.stuffProps.stuffAdjective.NullOrEmpty() ? base.label : this.stuffProps.stuffAdjective;
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
			return ((IEnumerable<CompProperties>)this.comps).FirstOrDefault<CompProperties>((Func<CompProperties, bool>)((CompProperties c) => c.compClass == typeof(T)));
		}

		public CompProperties CompDefForAssignableFrom<T>() where T : ThingComp
		{
			return ((IEnumerable<CompProperties>)this.comps).FirstOrDefault<CompProperties>((Func<CompProperties, bool>)((CompProperties c) => typeof(T).IsAssignableFrom(c.compClass)));
		}

		public bool HasComp(Type compType)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.comps.Count)
				{
					if (this.comps[num].compClass == compType)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public T GetCompProperties<T>() where T : CompProperties
		{
			int num = 0;
			T result;
			while (true)
			{
				if (num < this.comps.Count)
				{
					T val = (T)(this.comps[num] as T);
					if (val != null)
					{
						result = val;
						break;
					}
					num++;
					continue;
				}
				result = (T)null;
				break;
			}
			return result;
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
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
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
						_003CConfigErrors_003Ec__Iterator0 _003CConfigErrors_003Ec__Iterator = (_003CConfigErrors_003Ec__Iterator0)/*Error near IL_0327: stateMachine*/;
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
				ThingCategoryDef doubleCat = this.thingCategories.FirstOrDefault((Func<ThingCategoryDef, bool>)delegate(ThingCategoryDef cat)
				{
					ThingDef _0024this2 = ((_003CConfigErrors_003Ec__Iterator0)/*Error near IL_0651: stateMachine*/)._0024this;
					return ((_003CConfigErrors_003Ec__Iterator0)/*Error near IL_0651: stateMachine*/)._0024this.thingCategories.Count((Func<ThingCategoryDef, bool>)((ThingCategoryDef c) => c == cat)) > 1;
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
			if (this.comps.Any((Predicate<CompProperties>)((CompProperties c) => c.compClass == typeof(CompPowerTrader))) && this.drawerType == DrawerType.MapMeshOnly)
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
				if (!this.comps.Any((Predicate<CompProperties>)((CompProperties c) => c.compClass == typeof(CompEquippable))))
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
						_003CConfigErrors_003Ec__Iterator0 _003CConfigErrors_003Ec__Iterator2 = (_003CConfigErrors_003Ec__Iterator0)/*Error near IL_0977: stateMachine*/;
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
			if (this.drawerType == DrawerType.MapMeshOnly && this.comps.Any((Predicate<CompProperties>)((CompProperties c) => c.compClass == typeof(CompForbiddable))))
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
				_003CConfigErrors_003Ec__Iterator0 _003CConfigErrors_003Ec__Iterator3 = (_003CConfigErrors_003Ec__Iterator0)/*Error near IL_0d69: stateMachine*/;
				int j;
				for (j = 0; j < this.verbs.Count; j++)
				{
					if (this.verbs[j].linkedBodyPartsGroup != null && !this.race.body.AllParts.Any((Predicate<BodyPartRecord>)((BodyPartRecord part) => part.groups.Contains(_003CConfigErrors_003Ec__Iterator3._0024this.verbs[j].linkedBodyPartsGroup))))
					{
						yield return "has verb with linkedBodyPartsGroup " + this.verbs[j].linkedBodyPartsGroup + " but body " + this.race.body + " has no parts with that group.";
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.race != null && this.tools != null)
			{
				_003CConfigErrors_003Ec__Iterator0 _003CConfigErrors_003Ec__Iterator4 = (_003CConfigErrors_003Ec__Iterator0)/*Error near IL_0eb8: stateMachine*/;
				int i;
				for (i = 0; i < this.tools.Count; i++)
				{
					if (this.tools[i].linkedBodyPartsGroup != null && !this.race.body.AllParts.Any((Predicate<BodyPartRecord>)((BodyPartRecord part) => part.groups.Contains(_003CConfigErrors_003Ec__Iterator4._0024this.tools[i].linkedBodyPartsGroup))))
					{
						yield return "has tool with linkedBodyPartsGroup " + this.tools[i].linkedBodyPartsGroup + " but body " + this.race.body + " has no parts with that group.";
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
			Tool dupeTool = this.tools.SelectMany((Func<Tool, IEnumerable<Tool>>)delegate(Tool lhs)
			{
				ThingDef _0024this = ((_003CConfigErrors_003Ec__Iterator0)/*Error near IL_156c: stateMachine*/)._0024this;
				return from rhs in ((_003CConfigErrors_003Ec__Iterator0)/*Error near IL_156c: stateMachine*/)._0024this.tools
				where lhs != rhs && lhs.Id == rhs.Id
				select rhs;
			}).FirstOrDefault();
			if (dupeTool == null)
				yield break;
			yield return string.Format("duplicate thingdef tool id {0}", dupeTool.Id);
			/*Error: Unable to find new state assignment for yield return*/;
			IL_15cc:
			/*Error near IL_15cd: Unexpected return in MoveNext()*/;
		}

		public static ThingDef Named(string defName)
		{
			return DefDatabase<ThingDef>.GetNamed(defName, true);
		}

		public bool IsWithinCategory(ThingCategoryDef category)
		{
			bool result;
			if (this.thingCategories == null)
			{
				result = false;
			}
			else
			{
				int num = 0;
				while (num < this.thingCategories.Count)
				{
					if (this.thingCategories[num] != category && !this.thingCategories[num].Parents.Contains(category))
					{
						num++;
						continue;
					}
					goto IL_004a;
				}
				result = false;
			}
			goto IL_006e;
			IL_004a:
			result = true;
			goto IL_006e;
			IL_006e:
			return result;
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.apparel != null)
			{
				string coveredParts = this.apparel.GetCoveredOuterPartsString(BodyDefOf.Human);
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Covers".Translate(), coveredParts, 100, "");
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.IsMedicine && this.MedicineTendXpGainFactor != 1.0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MedicineXpGainFactor".Translate(), this.MedicineTendXpGainFactor.ToStringPercent(), 0, "");
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.fillPercent > 0.0 && this.fillPercent < 1.0 && (this.category == ThingCategory.Item || this.category == ThingCategory.Building || this.category == ThingCategory.Plant))
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "CoverEffectiveness".Translate(), this.BaseBlockChance().ToStringPercent(), 0, "")
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
				object obj;
				StatCategoryDef verbStatCategory3;
				if (this.category == ThingCategory.Pawn)
				{
					StatCategoryDef basics;
					verbStatCategory3 = (basics = StatCategoryDefOf.PawnCombat);
					obj = basics;
				}
				else
				{
					StatCategoryDef basics;
					verbStatCategory3 = (basics = StatCategoryDefOf.Weapon);
					obj = basics;
				}
				verbStatCategory3 = (StatCategoryDef)obj;
				float warmup = verb2.warmupTime;
				if (warmup > 0.0)
				{
					string warmupLabel = (this.category != ThingCategory.Pawn) ? "WarmupTime".Translate() : "MeleeWarmupTime".Translate();
					yield return new StatDrawEntry(verbStatCategory3, warmupLabel, warmup.ToString("0.##") + " s", 40, "");
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (verb2.defaultProjectile != null)
				{
					float dam = (float)verb2.defaultProjectile.projectile.damageAmountBase;
					yield return new StatDrawEntry(verbStatCategory3, "Damage".Translate(), dam.ToString(), 50, "");
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (verb2.LaunchesProjectile)
				{
					int burstShotCount = verb2.burstShotCount;
					float burstShotFireRate = (float)(60.0 / verb2.ticksBetweenBurstShots.TicksToSeconds());
					float range = verb2.range;
					if (burstShotCount > 1)
					{
						yield return new StatDrawEntry(verbStatCategory3, "BurstShotCount".Translate(), burstShotCount.ToString(), 20, "");
						/*Error: Unable to find new state assignment for yield return*/;
					}
					yield return new StatDrawEntry(verbStatCategory3, "Range".Translate(), range.ToString("0.##"), 10, "");
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
				where x.IsIngredient(((_003CSpecialDisplayStats_003Ec__Iterator1)/*Error near IL_0763: stateMachine*/)._0024this)
				select x)
				{
					HediffDef diff = item.addsHediff;
					if (diff != null)
					{
						if (diff.addedPartProps != null)
						{
							yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BodyPartEfficiency".Translate(), diff.addedPartProps.partEfficiency.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute), 0, "");
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
										yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Damage".Translate(), projDamage.ToString(), 0, "");
										/*Error: Unable to find new state assignment for yield return*/;
									}
									goto IL_0aa2;
								}
								int meleeDamage = verb.meleeDamageBaseAmount;
								yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Damage".Translate(), meleeDamage.ToString(), 0, "");
								/*Error: Unable to find new state assignment for yield return*/;
							}
							if (!vg.tools.NullOrEmpty())
							{
								Tool tool = vg.tools[0];
								yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Damage".Translate(), tool.power.ToString(), 0, "");
								/*Error: Unable to find new state assignment for yield return*/;
							}
						}
						goto IL_0aa2;
					}
					continue;
					IL_0aa2:
					ThoughtDef thought = DefDatabase<ThoughtDef>.AllDefs.FirstOrDefault((Func<ThoughtDef, bool>)((ThoughtDef x) => x.hediff == diff));
					if (thought != null && thought.stages != null && thought.stages.Any())
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MoodChange".Translate(), thought.stages.First().baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset), 0, "");
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
			IL_0c5b:
			/*Error near IL_0c5c: Unexpected return in MoveNext()*/;
		}
	}
}
