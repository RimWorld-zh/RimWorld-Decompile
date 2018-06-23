using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000BAA RID: 2986
	public class ThingDef : BuildableDef
	{
		// Token: 0x04002BA5 RID: 11173
		public Type thingClass;

		// Token: 0x04002BA6 RID: 11174
		public ThingCategory category;

		// Token: 0x04002BA7 RID: 11175
		public TickerType tickerType = TickerType.Never;

		// Token: 0x04002BA8 RID: 11176
		public int stackLimit = 1;

		// Token: 0x04002BA9 RID: 11177
		public IntVec2 size = new IntVec2(1, 1);

		// Token: 0x04002BAA RID: 11178
		public bool destroyable = true;

		// Token: 0x04002BAB RID: 11179
		public bool rotatable = true;

		// Token: 0x04002BAC RID: 11180
		public bool smallVolume;

		// Token: 0x04002BAD RID: 11181
		public bool useHitPoints = true;

		// Token: 0x04002BAE RID: 11182
		public bool receivesSignals;

		// Token: 0x04002BAF RID: 11183
		public List<CompProperties> comps = new List<CompProperties>();

		// Token: 0x04002BB0 RID: 11184
		public List<ThingDefCountClass> killedLeavings;

		// Token: 0x04002BB1 RID: 11185
		public List<ThingDefCountClass> butcherProducts;

		// Token: 0x04002BB2 RID: 11186
		public List<ThingDefCountClass> smeltProducts;

		// Token: 0x04002BB3 RID: 11187
		public bool smeltable;

		// Token: 0x04002BB4 RID: 11188
		public bool randomizeRotationOnSpawn;

		// Token: 0x04002BB5 RID: 11189
		public List<DamageMultiplier> damageMultipliers;

		// Token: 0x04002BB6 RID: 11190
		public bool isTechHediff;

		// Token: 0x04002BB7 RID: 11191
		public RecipeMakerProperties recipeMaker;

		// Token: 0x04002BB8 RID: 11192
		public ThingDef minifiedDef;

		// Token: 0x04002BB9 RID: 11193
		public bool isUnfinishedThing;

		// Token: 0x04002BBA RID: 11194
		public bool leaveResourcesWhenKilled;

		// Token: 0x04002BBB RID: 11195
		public ThingDef slagDef;

		// Token: 0x04002BBC RID: 11196
		public bool isFrame;

		// Token: 0x04002BBD RID: 11197
		public IntVec3 interactionCellOffset = IntVec3.Zero;

		// Token: 0x04002BBE RID: 11198
		public bool hasInteractionCell;

		// Token: 0x04002BBF RID: 11199
		public ThingDef interactionCellIcon;

		// Token: 0x04002BC0 RID: 11200
		public ThingDef filthLeaving;

		// Token: 0x04002BC1 RID: 11201
		public bool forceDebugSpawnable;

		// Token: 0x04002BC2 RID: 11202
		public bool intricate;

		// Token: 0x04002BC3 RID: 11203
		public bool scatterableOnMapGen = true;

		// Token: 0x04002BC4 RID: 11204
		public float deepCommonality;

		// Token: 0x04002BC5 RID: 11205
		public int deepCountPerCell = 300;

		// Token: 0x04002BC6 RID: 11206
		public IntRange deepLumpSizeRange = IntRange.zero;

		// Token: 0x04002BC7 RID: 11207
		public float generateCommonality = 1f;

		// Token: 0x04002BC8 RID: 11208
		public float generateAllowChance = 1f;

		// Token: 0x04002BC9 RID: 11209
		private bool canOverlapZones = true;

		// Token: 0x04002BCA RID: 11210
		public FloatRange startingHpRange = FloatRange.One;

		// Token: 0x04002BCB RID: 11211
		[NoTranslate]
		public List<string> thingSetMakerTags;

		// Token: 0x04002BCC RID: 11212
		public bool alwaysFlee;

		// Token: 0x04002BCD RID: 11213
		public List<Tool> tools;

		// Token: 0x04002BCE RID: 11214
		public List<RecipeDef> recipes;

		// Token: 0x04002BCF RID: 11215
		public GraphicData graphicData;

		// Token: 0x04002BD0 RID: 11216
		public DrawerType drawerType = DrawerType.RealtimeOnly;

		// Token: 0x04002BD1 RID: 11217
		public bool drawOffscreen;

		// Token: 0x04002BD2 RID: 11218
		public ColorGenerator colorGenerator;

		// Token: 0x04002BD3 RID: 11219
		public float hideAtSnowDepth = 99999f;

		// Token: 0x04002BD4 RID: 11220
		public bool drawDamagedOverlay = true;

		// Token: 0x04002BD5 RID: 11221
		public bool castEdgeShadows;

		// Token: 0x04002BD6 RID: 11222
		public float staticSunShadowHeight;

		// Token: 0x04002BD7 RID: 11223
		public bool selectable;

		// Token: 0x04002BD8 RID: 11224
		public bool neverMultiSelect;

		// Token: 0x04002BD9 RID: 11225
		public bool isAutoAttackableMapObject;

		// Token: 0x04002BDA RID: 11226
		public bool hasTooltip;

		// Token: 0x04002BDB RID: 11227
		public List<Type> inspectorTabs;

		// Token: 0x04002BDC RID: 11228
		[Unsaved]
		public List<InspectTabBase> inspectorTabsResolved;

		// Token: 0x04002BDD RID: 11229
		public bool seeThroughFog;

		// Token: 0x04002BDE RID: 11230
		public bool drawGUIOverlay;

		// Token: 0x04002BDF RID: 11231
		public ResourceCountPriority resourceReadoutPriority = ResourceCountPriority.Uncounted;

		// Token: 0x04002BE0 RID: 11232
		public bool resourceReadoutAlwaysShow;

		// Token: 0x04002BE1 RID: 11233
		public bool drawPlaceWorkersWhileSelected;

		// Token: 0x04002BE2 RID: 11234
		public ConceptDef storedConceptLearnOpportunity;

		// Token: 0x04002BE3 RID: 11235
		public float uiIconScale = 1f;

		// Token: 0x04002BE4 RID: 11236
		public bool alwaysHaulable;

		// Token: 0x04002BE5 RID: 11237
		public bool designateHaulable;

		// Token: 0x04002BE6 RID: 11238
		public List<ThingCategoryDef> thingCategories;

		// Token: 0x04002BE7 RID: 11239
		public bool mineable;

		// Token: 0x04002BE8 RID: 11240
		public bool socialPropernessMatters;

		// Token: 0x04002BE9 RID: 11241
		public bool stealable = true;

		// Token: 0x04002BEA RID: 11242
		public SoundDef soundDrop;

		// Token: 0x04002BEB RID: 11243
		public SoundDef soundPickup;

		// Token: 0x04002BEC RID: 11244
		public SoundDef soundInteract;

		// Token: 0x04002BED RID: 11245
		public SoundDef soundImpactDefault;

		// Token: 0x04002BEE RID: 11246
		public bool saveCompressible;

		// Token: 0x04002BEF RID: 11247
		public bool isSaveable = true;

		// Token: 0x04002BF0 RID: 11248
		public bool holdsRoof;

		// Token: 0x04002BF1 RID: 11249
		public float fillPercent;

		// Token: 0x04002BF2 RID: 11250
		public bool coversFloor;

		// Token: 0x04002BF3 RID: 11251
		public bool neverOverlapFloors;

		// Token: 0x04002BF4 RID: 11252
		public SurfaceType surfaceType = SurfaceType.None;

		// Token: 0x04002BF5 RID: 11253
		public bool blockPlants;

		// Token: 0x04002BF6 RID: 11254
		public bool blockLight;

		// Token: 0x04002BF7 RID: 11255
		public bool blockWind;

		// Token: 0x04002BF8 RID: 11256
		public Tradeability tradeability = Tradeability.All;

		// Token: 0x04002BF9 RID: 11257
		[NoTranslate]
		public List<string> tradeTags;

		// Token: 0x04002BFA RID: 11258
		public bool tradeNeverStack;

		// Token: 0x04002BFB RID: 11259
		public ColorGenerator colorGeneratorInTraderStock;

		// Token: 0x04002BFC RID: 11260
		private List<VerbProperties> verbs = null;

		// Token: 0x04002BFD RID: 11261
		public float equippedAngleOffset;

		// Token: 0x04002BFE RID: 11262
		public EquipmentType equipmentType = EquipmentType.None;

		// Token: 0x04002BFF RID: 11263
		public TechLevel techLevel = TechLevel.Undefined;

		// Token: 0x04002C00 RID: 11264
		[NoTranslate]
		public List<string> weaponTags;

		// Token: 0x04002C01 RID: 11265
		[NoTranslate]
		public List<string> techHediffsTags;

		// Token: 0x04002C02 RID: 11266
		public bool destroyOnDrop;

		// Token: 0x04002C03 RID: 11267
		public List<StatModifier> equippedStatOffsets;

		// Token: 0x04002C04 RID: 11268
		public BuildableDef entityDefToBuild;

		// Token: 0x04002C05 RID: 11269
		public ThingDef projectileWhenLoaded;

		// Token: 0x04002C06 RID: 11270
		public IngestibleProperties ingestible;

		// Token: 0x04002C07 RID: 11271
		public FilthProperties filth;

		// Token: 0x04002C08 RID: 11272
		public GasProperties gas;

		// Token: 0x04002C09 RID: 11273
		public BuildingProperties building;

		// Token: 0x04002C0A RID: 11274
		public RaceProperties race;

		// Token: 0x04002C0B RID: 11275
		public ApparelProperties apparel;

		// Token: 0x04002C0C RID: 11276
		public MoteProperties mote;

		// Token: 0x04002C0D RID: 11277
		public PlantProperties plant;

		// Token: 0x04002C0E RID: 11278
		public ProjectileProperties projectile;

		// Token: 0x04002C0F RID: 11279
		public StuffProperties stuffProps;

		// Token: 0x04002C10 RID: 11280
		public SkyfallerProperties skyfaller;

		// Token: 0x04002C11 RID: 11281
		[Unsaved]
		private string descriptionDetailedCached;

		// Token: 0x04002C12 RID: 11282
		[Unsaved]
		public Graphic interactionCellGraphic;

		// Token: 0x04002C13 RID: 11283
		public const int SmallUnitPerVolume = 10;

		// Token: 0x04002C14 RID: 11284
		public const float SmallVolumePerUnit = 0.1f;

		// Token: 0x04002C15 RID: 11285
		private List<RecipeDef> allRecipesCached = null;

		// Token: 0x04002C16 RID: 11286
		private static List<VerbProperties> EmptyVerbPropertiesList = new List<VerbProperties>();

		// Token: 0x04002C17 RID: 11287
		private Dictionary<ThingDef, Thing> concreteExamplesInt;

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x0600408A RID: 16522 RVA: 0x0021F7F4 File Offset: 0x0021DBF4
		public bool EverHaulable
		{
			get
			{
				return this.alwaysHaulable || this.designateHaulable;
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x0600408B RID: 16523 RVA: 0x0021F820 File Offset: 0x0021DC20
		public float VolumePerUnit
		{
			get
			{
				return this.smallVolume ? 0.1f : 1f;
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x0600408C RID: 16524 RVA: 0x0021F850 File Offset: 0x0021DC50
		public override IntVec2 Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x0600408D RID: 16525 RVA: 0x0021F86C File Offset: 0x0021DC6C
		public bool DiscardOnDestroyed
		{
			get
			{
				return this.race == null;
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x0600408E RID: 16526 RVA: 0x0021F88C File Offset: 0x0021DC8C
		public int BaseMaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValueAbstract(StatDefOf.MaxHitPoints, null));
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x0600408F RID: 16527 RVA: 0x0021F8B4 File Offset: 0x0021DCB4
		public float BaseFlammability
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Flammability, null);
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06004090 RID: 16528 RVA: 0x0021F8D8 File Offset: 0x0021DCD8
		// (set) Token: 0x06004091 RID: 16529 RVA: 0x0021F8F9 File Offset: 0x0021DCF9
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

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06004092 RID: 16530 RVA: 0x0021F908 File Offset: 0x0021DD08
		public float BaseMass
		{
			get
			{
				return this.GetStatValueAbstract(StatDefOf.Mass, null);
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06004093 RID: 16531 RVA: 0x0021F92C File Offset: 0x0021DD2C
		public bool PlayerAcquirable
		{
			get
			{
				return !this.destroyOnDrop;
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06004094 RID: 16532 RVA: 0x0021F94C File Offset: 0x0021DD4C
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

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06004095 RID: 16533 RVA: 0x0021F9AC File Offset: 0x0021DDAC
		public bool Minifiable
		{
			get
			{
				return this.minifiedDef != null;
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06004096 RID: 16534 RVA: 0x0021F9D0 File Offset: 0x0021DDD0
		public bool HasThingIDNumber
		{
			get
			{
				return this.category != ThingCategory.Mote;
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06004097 RID: 16535 RVA: 0x0021F9F4 File Offset: 0x0021DDF4
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
						if (allDefsListForReading[j].recipeUsers != null)
						{
							if (allDefsListForReading[j].recipeUsers.Contains(this))
							{
								this.allRecipesCached.Add(allDefsListForReading[j]);
							}
						}
					}
				}
				return this.allRecipesCached;
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06004098 RID: 16536 RVA: 0x0021FAC4 File Offset: 0x0021DEC4
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
						{
							return true;
						}
						if (this.comps[i].compClass == typeof(CompPowerTrader))
						{
							return true;
						}
					}
					result = false;
				}
				return result;
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06004099 RID: 16537 RVA: 0x0021FB58 File Offset: 0x0021DF58
		public bool CoexistsWithFloors
		{
			get
			{
				return !this.neverOverlapFloors && !this.coversFloor;
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x0600409A RID: 16538 RVA: 0x0021FB84 File Offset: 0x0021DF84
		public FillCategory Fillage
		{
			get
			{
				FillCategory result;
				if (this.fillPercent < 0.01f)
				{
					result = FillCategory.None;
				}
				else if (this.fillPercent > 0.99f)
				{
					result = FillCategory.Full;
				}
				else
				{
					result = FillCategory.Partial;
				}
				return result;
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x0600409B RID: 16539 RVA: 0x0021FBC8 File Offset: 0x0021DFC8
		public bool MakeFog
		{
			get
			{
				return this.Fillage == FillCategory.Full;
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x0600409C RID: 16540 RVA: 0x0021FBE8 File Offset: 0x0021DFE8
		public bool CanOverlapZones
		{
			get
			{
				bool result;
				if (this.building != null && this.building.SupportsPlants)
				{
					result = false;
				}
				else if (this.passability == Traversability.Impassable && this.category != ThingCategory.Plant)
				{
					result = false;
				}
				else if (this.surfaceType >= SurfaceType.Item)
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
							return thingDef.CanOverlapZones;
						}
					}
					result = true;
				}
				return result;
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x0600409D RID: 16541 RVA: 0x0021FCBC File Offset: 0x0021E0BC
		public bool CountAsResource
		{
			get
			{
				return this.resourceReadoutPriority != ResourceCountPriority.Uncounted;
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x0600409E RID: 16542 RVA: 0x0021FCE0 File Offset: 0x0021E0E0
		public bool BlockPlanting
		{
			get
			{
				return (this.building == null || !this.building.SupportsPlants) && (this.blockPlants || this.category == ThingCategory.Plant || this.Fillage > FillCategory.None || this.IsEdifice());
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x0600409F RID: 16543 RVA: 0x0021FD64 File Offset: 0x0021E164
		public List<VerbProperties> Verbs
		{
			get
			{
				List<VerbProperties> emptyVerbPropertiesList;
				if (this.verbs != null)
				{
					emptyVerbPropertiesList = this.verbs;
				}
				else
				{
					emptyVerbPropertiesList = ThingDef.EmptyVerbPropertiesList;
				}
				return emptyVerbPropertiesList;
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x060040A0 RID: 16544 RVA: 0x0021FD98 File Offset: 0x0021E198
		public bool CanHaveFaction
		{
			get
			{
				bool result;
				if (this.IsBlueprint || this.IsFrame)
				{
					result = true;
				}
				else
				{
					ThingCategory thingCategory = this.category;
					result = (thingCategory == ThingCategory.Pawn || thingCategory == ThingCategory.Building);
				}
				return result;
			}
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x060040A1 RID: 16545 RVA: 0x0021FDF4 File Offset: 0x0021E1F4
		public bool Claimable
		{
			get
			{
				return this.building != null && this.building.claimable && !this.building.isNaturalRock;
			}
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x060040A2 RID: 16546 RVA: 0x0021FE38 File Offset: 0x0021E238
		public ThingCategoryDef FirstThingCategory
		{
			get
			{
				ThingCategoryDef result;
				if (this.thingCategories.NullOrEmpty<ThingCategoryDef>())
				{
					result = null;
				}
				else
				{
					result = this.thingCategories[0];
				}
				return result;
			}
		}

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x060040A3 RID: 16547 RVA: 0x0021FE70 File Offset: 0x0021E270
		public float MedicineTendXpGainFactor
		{
			get
			{
				return Mathf.Clamp(this.GetStatValueAbstract(StatDefOf.MedicalPotency, null) * 0.7f, 0.5f, 1f);
			}
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x060040A4 RID: 16548 RVA: 0x0021FEA8 File Offset: 0x0021E2A8
		public bool CanEverDeteriorate
		{
			get
			{
				return this.useHitPoints && (this.category == ThingCategory.Item || this == ThingDefOf.BurnedTree);
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x060040A5 RID: 16549 RVA: 0x0021FEE8 File Offset: 0x0021E2E8
		public bool CanInteractThroughCorners
		{
			get
			{
				return this.category == ThingCategory.Building && this.holdsRoof && (this.building == null || !this.building.isNaturalRock) && !this.IsSmoothed;
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x060040A6 RID: 16550 RVA: 0x0021FF58 File Offset: 0x0021E358
		public bool AffectsRegions
		{
			get
			{
				return this.passability == Traversability.Impassable || this.IsDoor;
			}
		}

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x060040A7 RID: 16551 RVA: 0x0021FF84 File Offset: 0x0021E384
		public bool AffectsReachability
		{
			get
			{
				return this.AffectsRegions || (this.passability == Traversability.Impassable || this.IsDoor) || TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(this);
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x060040A8 RID: 16552 RVA: 0x0021FFDC File Offset: 0x0021E3DC
		public string DescriptionDetailed
		{
			get
			{
				if (this.descriptionDetailedCached == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(this.description);
					if (this.IsApparel)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine(string.Format("{0}: {1}", "Layer".Translate(), this.apparel.GetLayersString()));
						stringBuilder.AppendLine(string.Format("{0}: {1}", "Covers".Translate(), this.apparel.GetCoveredOuterPartsString(BodyDefOf.Human)));
						if (this.equippedStatOffsets != null && this.equippedStatOffsets.Count > 0)
						{
							stringBuilder.AppendLine();
							foreach (StatModifier statModifier in this.equippedStatOffsets)
							{
								stringBuilder.AppendLine(string.Format("{0}: {1}", statModifier.stat.LabelCap, statModifier.ValueToStringAsOffset));
							}
						}
					}
					this.descriptionDetailedCached = stringBuilder.ToString();
				}
				return this.descriptionDetailedCached;
			}
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x060040A9 RID: 16553 RVA: 0x0022011C File Offset: 0x0021E51C
		public bool IsApparel
		{
			get
			{
				return this.apparel != null;
			}
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x060040AA RID: 16554 RVA: 0x00220140 File Offset: 0x0021E540
		public bool IsBed
		{
			get
			{
				return typeof(Building_Bed).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x060040AB RID: 16555 RVA: 0x0022016C File Offset: 0x0021E56C
		public bool IsCorpse
		{
			get
			{
				return typeof(Corpse).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x060040AC RID: 16556 RVA: 0x00220198 File Offset: 0x0021E598
		public bool IsFrame
		{
			get
			{
				return this.isFrame;
			}
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060040AD RID: 16557 RVA: 0x002201B4 File Offset: 0x0021E5B4
		public bool IsBlueprint
		{
			get
			{
				return this.entityDefToBuild != null && this.category == ThingCategory.Ethereal;
			}
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060040AE RID: 16558 RVA: 0x002201E4 File Offset: 0x0021E5E4
		public bool IsStuff
		{
			get
			{
				return this.stuffProps != null;
			}
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060040AF RID: 16559 RVA: 0x00220208 File Offset: 0x0021E608
		public bool IsMedicine
		{
			get
			{
				return this.statBases.StatListContains(StatDefOf.MedicalPotency);
			}
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060040B0 RID: 16560 RVA: 0x00220230 File Offset: 0x0021E630
		public bool IsDoor
		{
			get
			{
				return typeof(Building_Door).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060040B1 RID: 16561 RVA: 0x0022025C File Offset: 0x0021E65C
		public bool IsFilth
		{
			get
			{
				return this.filth != null;
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060040B2 RID: 16562 RVA: 0x00220280 File Offset: 0x0021E680
		public bool IsIngestible
		{
			get
			{
				return this.ingestible != null;
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060040B3 RID: 16563 RVA: 0x002202A4 File Offset: 0x0021E6A4
		public bool IsNutritionGivingIngestible
		{
			get
			{
				return this.IsIngestible && this.ingestible.CachedNutrition > 0f;
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060040B4 RID: 16564 RVA: 0x002202DC File Offset: 0x0021E6DC
		public bool IsWeapon
		{
			get
			{
				return this.category == ThingCategory.Item && (!this.verbs.NullOrEmpty<VerbProperties>() || !this.tools.NullOrEmpty<Tool>());
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x060040B5 RID: 16565 RVA: 0x00220324 File Offset: 0x0021E724
		public bool IsCommsConsole
		{
			get
			{
				return typeof(Building_CommsConsole).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x060040B6 RID: 16566 RVA: 0x00220350 File Offset: 0x0021E750
		public bool IsOrbitalTradeBeacon
		{
			get
			{
				return typeof(Building_OrbitalTradeBeacon).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x060040B7 RID: 16567 RVA: 0x0022037C File Offset: 0x0021E77C
		public bool IsFoodDispenser
		{
			get
			{
				return typeof(Building_NutrientPasteDispenser).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x060040B8 RID: 16568 RVA: 0x002203A8 File Offset: 0x0021E7A8
		public bool IsDrug
		{
			get
			{
				return this.ingestible != null && this.ingestible.drugCategory != DrugCategory.None;
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x060040B9 RID: 16569 RVA: 0x002203DC File Offset: 0x0021E7DC
		public bool IsPleasureDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.joy > 0f;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x060040BA RID: 16570 RVA: 0x00220414 File Offset: 0x0021E814
		public bool IsNonMedicalDrug
		{
			get
			{
				return this.IsDrug && this.ingestible.drugCategory != DrugCategory.Medical;
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x060040BB RID: 16571 RVA: 0x00220448 File Offset: 0x0021E848
		public bool IsTable
		{
			get
			{
				return this.surfaceType == SurfaceType.Eat && this.HasComp(typeof(CompGatherSpot));
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x060040BC RID: 16572 RVA: 0x0022047C File Offset: 0x0021E87C
		public bool IsWorkTable
		{
			get
			{
				return typeof(Building_WorkTable).IsAssignableFrom(this.thingClass);
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x060040BD RID: 16573 RVA: 0x002204A8 File Offset: 0x0021E8A8
		public bool IsShell
		{
			get
			{
				return this.projectileWhenLoaded != null;
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x060040BE RID: 16574 RVA: 0x002204CC File Offset: 0x0021E8CC
		public bool IsArt
		{
			get
			{
				return this.IsWithinCategory(ThingCategoryDefOf.BuildingsArt);
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x060040BF RID: 16575 RVA: 0x002204EC File Offset: 0x0021E8EC
		public bool IsSmoothable
		{
			get
			{
				return this.building != null && this.building.smoothedThing != null;
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060040C0 RID: 16576 RVA: 0x00220520 File Offset: 0x0021E920
		public bool IsSmoothed
		{
			get
			{
				return this.building != null && this.building.unsmoothedThing != null;
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x060040C1 RID: 16577 RVA: 0x00220554 File Offset: 0x0021E954
		public bool IsMetal
		{
			get
			{
				return this.stuffProps != null && this.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic);
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x060040C2 RID: 16578 RVA: 0x0022058C File Offset: 0x0021E98C
		public bool IsAddictiveDrug
		{
			get
			{
				CompProperties_Drug compProperties = this.GetCompProperties<CompProperties_Drug>();
				return compProperties != null && compProperties.addictiveness > 0f;
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x060040C3 RID: 16579 RVA: 0x002205C0 File Offset: 0x0021E9C0
		public bool IsMeat
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.MeatRaw);
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x060040C4 RID: 16580 RVA: 0x00220600 File Offset: 0x0021EA00
		public bool IsLeather
		{
			get
			{
				return this.category == ThingCategory.Item && this.thingCategories != null && this.thingCategories.Contains(ThingCategoryDefOf.Leathers);
			}
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x060040C5 RID: 16581 RVA: 0x00220640 File Offset: 0x0021EA40
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
					if (!this.verbs.NullOrEmpty<VerbProperties>())
					{
						for (int i = 0; i < this.verbs.Count; i++)
						{
							if (!this.verbs[i].IsMeleeAttack)
							{
								return true;
							}
						}
					}
					result = false;
				}
				return result;
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x060040C6 RID: 16582 RVA: 0x002206B8 File Offset: 0x0021EAB8
		public bool IsMeleeWeapon
		{
			get
			{
				return this.IsWeapon && !this.IsRangedWeapon;
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x060040C7 RID: 16583 RVA: 0x002206E4 File Offset: 0x0021EAE4
		public bool IsWeaponUsingProjectiles
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
					if (!this.verbs.NullOrEmpty<VerbProperties>())
					{
						for (int i = 0; i < this.verbs.Count; i++)
						{
							if (this.verbs[i].LaunchesProjectile)
							{
								return true;
							}
						}
					}
					result = false;
				}
				return result;
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x060040C8 RID: 16584 RVA: 0x0022075C File Offset: 0x0021EB5C
		public bool IsBuildingArtificial
		{
			get
			{
				return (this.category == ThingCategory.Building || this.IsFrame) && (this.building == null || (!this.building.isNaturalRock && !this.building.isResourceRock));
			}
		}

		// Token: 0x060040C9 RID: 16585 RVA: 0x002207BC File Offset: 0x0021EBBC
		public bool EverStorable(bool willMinifyIfPossible)
		{
			bool result;
			if (typeof(MinifiedThing).IsAssignableFrom(this.thingClass))
			{
				result = true;
			}
			else
			{
				if (!this.thingCategories.NullOrEmpty<ThingCategoryDef>())
				{
					if (this.category == ThingCategory.Item)
					{
						return true;
					}
					if (willMinifyIfPossible && this.Minifiable)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x00220830 File Offset: 0x0021EC30
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
					select pkd).FirstOrDefault<PawnKindDef>(), null);
				}
			}
			return this.concreteExamplesInt[stuff];
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x002208E8 File Offset: 0x0021ECE8
		public CompProperties CompDefFor<T>() where T : ThingComp
		{
			return this.comps.FirstOrDefault((CompProperties c) => c.compClass == typeof(T));
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x00220914 File Offset: 0x0021ED14
		public CompProperties CompDefForAssignableFrom<T>() where T : ThingComp
		{
			return this.comps.FirstOrDefault((CompProperties c) => typeof(T).IsAssignableFrom(c.compClass));
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x00220940 File Offset: 0x0021ED40
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

		// Token: 0x060040CE RID: 16590 RVA: 0x00220994 File Offset: 0x0021ED94
		public T GetCompProperties<T>() where T : CompProperties
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				T t = this.comps[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return (T)((object)null);
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x002209F8 File Offset: 0x0021EDF8
		public override void PostLoad()
		{
			if (this.graphicData != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					if (this.graphicData.shaderType == null)
					{
						this.graphicData.shaderType = ShaderTypeDefOf.Cutout;
					}
					this.graphic = this.graphicData.Graphic;
				});
			}
			if (this.verbs != null && this.verbs.Count == 1)
			{
				this.verbs[0].label = this.label;
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

		// Token: 0x060040D0 RID: 16592 RVA: 0x00220AB0 File Offset: 0x0021EEB0
		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			if (this.category == ThingCategory.Pawn)
			{
				if (!this.race.Humanlike)
				{
					PawnKindDef anyPawnKind = this.race.AnyPawnKind;
					if (anyPawnKind != null)
					{
						Material material = anyPawnKind.lifeStages.Last<PawnKindLifeStage>().bodyGraphicData.Graphic.MatAt(Rot4.East, null);
						this.uiIcon = (Texture2D)material.mainTexture;
						this.uiIconColor = material.color;
					}
				}
			}
			else
			{
				ThingDef thingDef = GenStuff.DefaultStuffFor(this);
				if (this.colorGenerator != null && (thingDef == null || thingDef.stuffProps.allowColorGenerators))
				{
					this.uiIconColor = this.colorGenerator.ExemplaryColor;
				}
				else if (thingDef != null)
				{
					this.uiIconColor = thingDef.stuffProps.color;
				}
				else if (this.graphicData != null)
				{
					this.uiIconColor = this.graphicData.color;
				}
				if (this.rotatable && this.graphic != null && this.graphic != BaseContent.BadGraphic && this.graphic.ShouldDrawRotated && this.defaultPlacingRot == Rot4.South)
				{
					this.uiIconAngle = 180f;
				}
			}
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x00220C14 File Offset: 0x0021F014
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.ingestible != null)
			{
				this.ingestible.parent = this;
			}
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
			if (this.stuffProps != null)
			{
				this.stuffProps.ResolveReferencesSpecial();
			}
			if (this.soundImpactDefault == null)
			{
				this.soundImpactDefault = SoundDefOf.BulletImpact_Ground;
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
				this.soundInteract = SoundDefOf.Standard_Pickup;
			}
			if (this.inspectorTabs != null && this.inspectorTabs.Any<Type>())
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
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate inspector tab of type ",
							this.inspectorTabs[i],
							": ",
							ex
						}), false);
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

		// Token: 0x060040D2 RID: 16594 RVA: 0x00220DE4 File Offset: 0x0021F1E4
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string str in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return str;
			}
			if (this.label.NullOrEmpty())
			{
				yield return "no label";
			}
			if (this.graphicData != null)
			{
				foreach (string err in this.graphicData.ConfigErrors(this))
				{
					yield return err;
				}
			}
			if (this.projectile != null)
			{
				foreach (string err2 in this.projectile.ConfigErrors(this))
				{
					yield return err2;
				}
			}
			if (this.statBases != null)
			{
				using (List<StatModifier>.Enumerator enumerator4 = this.statBases.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						StatModifier statBase = enumerator4.Current;
						if ((from st in this.statBases
						where st.stat == statBase.stat
						select st).Count<StatModifier>() > 1)
						{
							yield return "defines the stat base " + statBase.stat + " more than once.";
						}
					}
				}
			}
			if (!BeautyUtility.BeautyRelevant(this.category) && this.StatBaseDefined(StatDefOf.Beauty))
			{
				yield return "Beauty stat base is defined, but Things of category " + this.category + " cannot have beauty.";
			}
			if (char.IsNumber(this.defName[this.defName.Length - 1]))
			{
				yield return "ends with a numerical digit, which is not allowed on ThingDefs.";
			}
			if (this.thingClass == null)
			{
				yield return "has null thingClass.";
			}
			if (this.comps.Count > 0 && !typeof(ThingWithComps).IsAssignableFrom(this.thingClass))
			{
				yield return "has components but it's thingClass is not a ThingWithComps";
			}
			if (this.ConnectToPower && this.drawerType == DrawerType.RealtimeOnly && this.IsFrame)
			{
				yield return "connects to power but does not add to map mesh. Will not create wire meshes.";
			}
			if (this.costList != null)
			{
				foreach (ThingDefCountClass cost in this.costList)
				{
					if (cost.count == 0)
					{
						yield return "cost in " + cost.thingDef + " is zero.";
					}
				}
			}
			if (this.thingCategories != null)
			{
				ThingCategoryDef doubleCat = this.thingCategories.FirstOrDefault((ThingCategoryDef cat) => this.thingCategories.Count((ThingCategoryDef c) => c == cat) > 1);
				if (doubleCat != null)
				{
					yield return "has duplicate thingCategory " + doubleCat + ".";
				}
			}
			if (this.Fillage == FillCategory.Full && this.category != ThingCategory.Building)
			{
				yield return "gives full cover but is not a building.";
			}
			if (this.comps.Any((CompProperties c) => c.compClass == typeof(CompPowerTrader)) && this.drawerType == DrawerType.MapMeshOnly)
			{
				yield return "has PowerTrader comp but does not draw real time. It won't draw a needs-power overlay.";
			}
			if (this.equipmentType != EquipmentType.None)
			{
				if (this.techLevel == TechLevel.Undefined)
				{
					yield return "is equipment but has no tech level.";
				}
				if (!this.comps.Any((CompProperties c) => c.compClass == typeof(CompEquippable)))
				{
					yield return "is equipment but has no CompEquippable";
				}
			}
			if (this.thingClass == typeof(Bullet) && this.projectile.damageDef == null)
			{
				yield return " is a bullet but has no damageDef.";
			}
			if (this.destroyOnDrop)
			{
				if (!this.menuHidden)
				{
					yield return "destroyOnDrop but not menuHidden.";
				}
				if (this.tradeability != Tradeability.None)
				{
					yield return "destroyOnDrop but tradeability is " + this.tradeability;
				}
			}
			if (this.stackLimit > 1 && !this.drawGUIOverlay)
			{
				yield return "has stackLimit > 1 but also has drawGUIOverlay = false.";
			}
			if (this.damageMultipliers != null)
			{
				using (List<DamageMultiplier>.Enumerator enumerator6 = this.damageMultipliers.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						DamageMultiplier mult = enumerator6.Current;
						if ((from m in this.damageMultipliers
						where m.damageDef == mult.damageDef
						select m).Count<DamageMultiplier>() > 1)
						{
							yield return "has multiple damage multipliers for damageDef " + mult.damageDef;
							break;
						}
					}
				}
			}
			if (this.Fillage == FillCategory.Full && !this.IsEdifice())
			{
				yield return "fillPercent is 1.00 but is not edifice";
			}
			if (base.MadeFromStuff && this.constructEffect != null)
			{
				yield return "madeFromStuff but has a defined constructEffect (which will always be overridden by stuff's construct animation).";
			}
			if (base.MadeFromStuff && this.stuffCategories.NullOrEmpty<StuffCategoryDef>())
			{
				yield return "madeFromStuff but has no stuffCategories.";
			}
			if (this.costList.NullOrEmpty<ThingDefCountClass>() && this.costStuffCount <= 0 && this.recipeMaker != null)
			{
				yield return "has a recipeMaker but no costList or costStuffCount.";
			}
			if (this.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) > 1E-05f && !this.CanEverDeteriorate)
			{
				yield return "has >0 DeteriorationRate but can't deteriorate.";
			}
			if (this.drawerType == DrawerType.MapMeshOnly)
			{
				if (this.comps.Any((CompProperties c) => c.compClass == typeof(CompForbiddable)))
				{
					yield return "drawerType=MapMeshOnly but has a CompForbiddable, which must draw in real time.";
				}
			}
			if (this.smeltProducts != null && this.smeltable)
			{
				yield return "has smeltProducts but has smeltable=false";
			}
			if (this.equipmentType != EquipmentType.None && this.verbs.NullOrEmpty<VerbProperties>() && this.tools.NullOrEmpty<Tool>())
			{
				yield return "is equipment but has no verbs or tools";
			}
			if (this.Minifiable && this.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				yield return "is minifiable but not in any thing category";
			}
			if (this.category == ThingCategory.Building && !this.Minifiable && !this.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				yield return "is not minifiable yet has thing categories (could be confusing in thing filters because it can't be moved/stored anyway)";
			}
			if (this != ThingDefOf.MinifiedThing && (this.EverHaulable || this.Minifiable))
			{
				if (!this.statBases.NullOrEmpty<StatModifier>())
				{
					if (this.statBases.Any((StatModifier s) => s.stat == StatDefOf.Mass))
					{
						goto IL_D94;
					}
				}
				yield return "is haulable, but does not have an authored mass value";
			}
			IL_D94:
			if (this.ingestible == null && this.GetStatValueAbstract(StatDefOf.Nutrition, null) != 0f)
			{
				yield return "has nutrition but ingestible properties are null";
			}
			if (this.BaseFlammability != 0f && !this.useHitPoints && this.category != ThingCategory.Pawn)
			{
				yield return "flammable but has no hitpoints (will burn indefinitely)";
			}
			if (this.graphicData != null && this.graphicData.shadowData != null)
			{
				if (this.staticSunShadowHeight > 0f)
				{
					yield return "graphicData defines a shadowInfo but staticSunShadowHeight > 0";
				}
			}
			if (this.saveCompressible && this.Claimable)
			{
				yield return "claimable item is compressible; faction will be unset after load";
			}
			if (this.deepCommonality > 0f != this.deepLumpSizeRange.TrueMax > 0)
			{
				yield return "if deepCommonality or deepLumpSizeRange is set, the other also must be set";
			}
			if (this.verbs != null)
			{
				for (int k = 0; k < this.verbs.Count; k++)
				{
					foreach (string err3 in this.verbs[k].ConfigErrors(this))
					{
						yield return string.Format("verb {0}: {1}", k, err3);
					}
				}
			}
			if (this.race != null && this.tools != null)
			{
				int i;
				for (i = 0; i < this.tools.Count; i++)
				{
					if (this.tools[i].linkedBodyPartsGroup != null && !this.race.body.AllParts.Any((BodyPartRecord part) => part.groups.Contains(this.tools[i].linkedBodyPartsGroup)))
					{
						yield return string.Concat(new object[]
						{
							"has tool with linkedBodyPartsGroup ",
							this.tools[i].linkedBodyPartsGroup,
							" but body ",
							this.race.body,
							" has no parts with that group."
						});
					}
				}
			}
			if (this.building != null)
			{
				foreach (string err4 in this.building.ConfigErrors(this))
				{
					yield return err4;
				}
			}
			if (this.apparel != null)
			{
				foreach (string err5 in this.apparel.ConfigErrors(this))
				{
					yield return err5;
				}
			}
			if (this.comps != null)
			{
				for (int j = 0; j < this.comps.Count; j++)
				{
					foreach (string err6 in this.comps[j].ConfigErrors(this))
					{
						yield return err6;
					}
				}
			}
			if (this.race != null)
			{
				foreach (string e in this.race.ConfigErrors())
				{
					yield return e;
				}
			}
			if (this.ingestible != null)
			{
				foreach (string e2 in this.ingestible.ConfigErrors())
				{
					yield return e2;
				}
			}
			if (this.plant != null)
			{
				foreach (string e3 in this.plant.ConfigErrors())
				{
					yield return e3;
				}
			}
			if (this.recipes != null && this.race != null)
			{
				foreach (RecipeDef r in this.recipes)
				{
					if (r.requireBed != this.race.FleshType.requiresBedForSurgery)
					{
						yield return string.Format("surgery bed requirement mismatch; flesh-type {0} is {1}, recipe {2} is {3}", new object[]
						{
							this.race.FleshType,
							this.race.FleshType.requiresBedForSurgery,
							r,
							r.requireBed
						});
					}
				}
			}
			if (this.tools != null)
			{
				Tool dupeTool = this.tools.SelectMany((Tool lhs) => from rhs in this.tools
				where lhs != rhs && lhs.Id == rhs.Id
				select rhs).FirstOrDefault<Tool>();
				if (dupeTool != null)
				{
					yield return string.Format("duplicate thingdef tool id {0}", dupeTool.Id);
				}
			}
			yield break;
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x00220E10 File Offset: 0x0021F210
		public static ThingDef Named(string defName)
		{
			return DefDatabase<ThingDef>.GetNamed(defName, true);
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x060040D4 RID: 16596 RVA: 0x00220E2C File Offset: 0x0021F22C
		public string LabelAsStuff
		{
			get
			{
				string result;
				if (!this.stuffProps.stuffAdjective.NullOrEmpty())
				{
					result = this.stuffProps.stuffAdjective;
				}
				else
				{
					result = this.label;
				}
				return result;
			}
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x00220E70 File Offset: 0x0021F270
		public bool IsWithinCategory(ThingCategoryDef category)
		{
			bool result;
			if (this.thingCategories == null)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this.thingCategories.Count; i++)
				{
					if (this.thingCategories[i] == category || this.thingCategories[i].Parents.Contains(category))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x00220EEC File Offset: 0x0021F2EC
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry stat in this.<SpecialDisplayStats>__BaseCallProxy1())
			{
				yield return stat;
			}
			if (this.apparel != null)
			{
				string coveredParts = this.apparel.GetCoveredOuterPartsString(BodyDefOf.Human);
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Covers".Translate(), coveredParts, 100, "");
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Layer".Translate(), this.apparel.GetLayersString(), 95, "");
			}
			if (this.IsMedicine && this.MedicineTendXpGainFactor != 1f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MedicineXpGainFactor".Translate(), this.MedicineTendXpGainFactor.ToStringPercent(), 0, "");
			}
			if (this.fillPercent > 0f && this.fillPercent < 1f && (this.category == ThingCategory.Item || this.category == ThingCategory.Building || this.category == ThingCategory.Plant))
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "CoverEffectiveness".Translate(), this.BaseBlockChance().ToStringPercent(), 0, "")
				{
					overrideReportText = "CoverEffectivenessExplanation".Translate()
				};
			}
			if (this.constructionSkillPrerequisite > 0)
			{
				StatCategoryDef statCategoryDef = StatCategoryDefOf.Basics;
				string text = "ConstructionSkillRequired".Translate();
				string valueString = this.constructionSkillPrerequisite.ToString();
				string text2 = "ConstructionSkillRequiredExplanation".Translate();
				yield return new StatDrawEntry(statCategoryDef, text, valueString, 0, text2);
			}
			if (!this.verbs.NullOrEmpty<VerbProperties>())
			{
				VerbProperties verb = (from x in this.verbs
				where x.isPrimary
				select x).First<VerbProperties>();
				StatCategoryDef verbStatCategory = (this.category != ThingCategory.Pawn) ? (verbStatCategory = StatCategoryDefOf.Weapon) : (verbStatCategory = StatCategoryDefOf.PawnCombat);
				float warmup = verb.warmupTime;
				if (warmup > 0f)
				{
					string warmupLabel = (this.category != ThingCategory.Pawn) ? "WarmupTime".Translate() : "MeleeWarmupTime".Translate();
					yield return new StatDrawEntry(verbStatCategory, warmupLabel, warmup.ToString("0.##") + " s", 40, "");
				}
				if (verb.defaultProjectile != null)
				{
					float dam = (float)verb.defaultProjectile.projectile.DamageAmount;
					yield return new StatDrawEntry(verbStatCategory, "Damage".Translate(), dam.ToString(), 50, "");
				}
				if (verb.LaunchesProjectile)
				{
					int burstShotCount = verb.burstShotCount;
					float burstShotFireRate = 60f / verb.ticksBetweenBurstShots.TicksToSeconds();
					float range = verb.range;
					if (burstShotCount > 1)
					{
						yield return new StatDrawEntry(verbStatCategory, "BurstShotCount".Translate(), burstShotCount.ToString(), 20, "");
						yield return new StatDrawEntry(verbStatCategory, "BurstShotFireRate".Translate(), burstShotFireRate.ToString("0.##") + " rpm", 19, "");
					}
					yield return new StatDrawEntry(verbStatCategory, "Range".Translate(), range.ToString("F0"), 10, "");
					if (verb.defaultProjectile != null && verb.defaultProjectile.projectile != null && verb.defaultProjectile.projectile.stoppingPower != 0f)
					{
						StatCategoryDef statCategoryDef = verbStatCategory;
						string text2 = "StoppingPower".Translate();
						string valueString = verb.defaultProjectile.projectile.stoppingPower.ToString("F1");
						string text = "StoppingPowerExplanation".Translate();
						yield return new StatDrawEntry(statCategoryDef, text2, valueString, 0, text);
					}
				}
				if (verb.forcedMissRadius > 0f)
				{
					yield return new StatDrawEntry(verbStatCategory, "MissRadius".Translate(), verb.forcedMissRadius.ToString("0.#"), 30, "");
					yield return new StatDrawEntry(verbStatCategory, "DirectHitChance".Translate(), (1f / (float)GenRadial.NumCellsInRadius(verb.forcedMissRadius)).ToStringPercent(), 29, "");
				}
			}
			if (this.plant != null)
			{
				foreach (StatDrawEntry s in this.plant.SpecialDisplayStats())
				{
					yield return s;
				}
			}
			if (this.ingestible != null)
			{
				foreach (StatDrawEntry s2 in this.ingestible.SpecialDisplayStats())
				{
					yield return s2;
				}
			}
			if (this.race != null)
			{
				foreach (StatDrawEntry s3 in this.race.SpecialDisplayStats(this))
				{
					yield return s3;
				}
			}
			if (this.building != null)
			{
				foreach (StatDrawEntry s4 in this.building.SpecialDisplayStats(this))
				{
					yield return s4;
				}
			}
			if (this.isTechHediff)
			{
				foreach (RecipeDef def in from x in DefDatabase<RecipeDef>.AllDefs
				where x.IsIngredient(this)
				select x)
				{
					HediffDef diff = def.addsHediff;
					if (diff != null)
					{
						if (diff.addedPartProps != null)
						{
							yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BodyPartEfficiency".Translate(), diff.addedPartProps.partEfficiency.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute), 0, "");
						}
						foreach (StatDrawEntry s5 in diff.SpecialDisplayStats())
						{
							yield return s5;
						}
						HediffCompProperties_VerbGiver vg = diff.CompProps<HediffCompProperties_VerbGiver>();
						if (vg != null)
						{
							if (!vg.verbs.NullOrEmpty<VerbProperties>())
							{
								VerbProperties verb2 = vg.verbs[0];
								if (!verb2.IsMeleeAttack)
								{
									if (verb2.defaultProjectile != null)
									{
										int projDamage = verb2.defaultProjectile.projectile.DamageAmount;
										yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Damage".Translate(), projDamage.ToString(), 0, "");
									}
								}
								else
								{
									int meleeDamage = verb2.meleeDamageBaseAmount;
									yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Damage".Translate(), meleeDamage.ToString(), 0, "");
								}
							}
							else if (!vg.tools.NullOrEmpty<Tool>())
							{
								Tool tool = vg.tools[0];
								yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Damage".Translate(), tool.power.ToString(), 0, "");
							}
						}
						ThoughtDef thought = DefDatabase<ThoughtDef>.AllDefs.FirstOrDefault((ThoughtDef x) => x.hediff == diff);
						if (thought != null && thought.stages != null && thought.stages.Any<ThoughtStage>())
						{
							yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MoodChange".Translate(), thought.stages.First<ThoughtStage>().baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset), 0, "");
						}
					}
				}
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (StatDrawEntry s6 in this.comps[i].SpecialDisplayStats())
				{
					yield return s6;
				}
			}
			yield break;
		}
	}
}
