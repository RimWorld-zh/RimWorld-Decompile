using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000D4B RID: 3403
	public class Pawn : ThingWithComps, IStrippable, IBillGiver, IVerbOwner, ITrader, IAttackTarget, IAttackTargetSearcher, IThingHolder, ILoadReferenceable
	{
		// Token: 0x04003284 RID: 12932
		public PawnKindDef kindDef;

		// Token: 0x04003285 RID: 12933
		private Name nameInt;

		// Token: 0x04003286 RID: 12934
		public Gender gender = Gender.None;

		// Token: 0x04003287 RID: 12935
		public Pawn_AgeTracker ageTracker;

		// Token: 0x04003288 RID: 12936
		public Pawn_HealthTracker health;

		// Token: 0x04003289 RID: 12937
		public Pawn_RecordsTracker records;

		// Token: 0x0400328A RID: 12938
		public Pawn_InventoryTracker inventory;

		// Token: 0x0400328B RID: 12939
		public Pawn_MeleeVerbs meleeVerbs;

		// Token: 0x0400328C RID: 12940
		public VerbTracker verbTracker;

		// Token: 0x0400328D RID: 12941
		public Pawn_CarryTracker carryTracker;

		// Token: 0x0400328E RID: 12942
		public Pawn_NeedsTracker needs;

		// Token: 0x0400328F RID: 12943
		public Pawn_MindState mindState;

		// Token: 0x04003290 RID: 12944
		public Pawn_RotationTracker rotationTracker;

		// Token: 0x04003291 RID: 12945
		public Pawn_PathFollower pather;

		// Token: 0x04003292 RID: 12946
		public Pawn_Thinker thinker;

		// Token: 0x04003293 RID: 12947
		public Pawn_JobTracker jobs;

		// Token: 0x04003294 RID: 12948
		public Pawn_StanceTracker stances;

		// Token: 0x04003295 RID: 12949
		public Pawn_NativeVerbs natives;

		// Token: 0x04003296 RID: 12950
		public Pawn_FilthTracker filth;

		// Token: 0x04003297 RID: 12951
		public Pawn_EquipmentTracker equipment;

		// Token: 0x04003298 RID: 12952
		public Pawn_ApparelTracker apparel;

		// Token: 0x04003299 RID: 12953
		public Pawn_Ownership ownership;

		// Token: 0x0400329A RID: 12954
		public Pawn_SkillTracker skills;

		// Token: 0x0400329B RID: 12955
		public Pawn_StoryTracker story;

		// Token: 0x0400329C RID: 12956
		public Pawn_GuestTracker guest;

		// Token: 0x0400329D RID: 12957
		public Pawn_GuiltTracker guilt;

		// Token: 0x0400329E RID: 12958
		public Pawn_WorkSettings workSettings;

		// Token: 0x0400329F RID: 12959
		public Pawn_TraderTracker trader;

		// Token: 0x040032A0 RID: 12960
		public Pawn_TrainingTracker training;

		// Token: 0x040032A1 RID: 12961
		public Pawn_CallTracker caller;

		// Token: 0x040032A2 RID: 12962
		public Pawn_RelationsTracker relations;

		// Token: 0x040032A3 RID: 12963
		public Pawn_InteractionsTracker interactions;

		// Token: 0x040032A4 RID: 12964
		public Pawn_PlayerSettings playerSettings;

		// Token: 0x040032A5 RID: 12965
		public Pawn_OutfitTracker outfits;

		// Token: 0x040032A6 RID: 12966
		public Pawn_DrugPolicyTracker drugs;

		// Token: 0x040032A7 RID: 12967
		public Pawn_TimetableTracker timetable;

		// Token: 0x040032A8 RID: 12968
		public Pawn_DraftController drafter;

		// Token: 0x040032A9 RID: 12969
		private Pawn_DrawTracker drawer;

		// Token: 0x040032AA RID: 12970
		private const float HumanSizedHeatOutput = 0.3f;

		// Token: 0x040032AB RID: 12971
		private const float AnimalHeatOutputFactor = 0.6f;

		// Token: 0x040032AC RID: 12972
		private static string NotSurgeryReadyTrans;

		// Token: 0x040032AD RID: 12973
		private static string CannotReachTrans;

		// Token: 0x040032AE RID: 12974
		public const int MaxMoveTicks = 450;

		// Token: 0x040032AF RID: 12975
		private static List<string> states = new List<string>();

		// Token: 0x040032B0 RID: 12976
		private int lastSleepDisturbedTick = 0;

		// Token: 0x040032B1 RID: 12977
		private const int SleepDisturbanceMinInterval = 300;

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x06004B1C RID: 19228 RVA: 0x002727A4 File Offset: 0x00270BA4
		// (set) Token: 0x06004B1D RID: 19229 RVA: 0x002727BF File Offset: 0x00270BBF
		public Name Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x06004B1E RID: 19230 RVA: 0x002727CC File Offset: 0x00270BCC
		public RaceProperties RaceProps
		{
			get
			{
				return this.def.race;
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06004B1F RID: 19231 RVA: 0x002727EC File Offset: 0x00270BEC
		public Job CurJob
		{
			get
			{
				return (this.jobs == null) ? null : this.jobs.curJob;
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x06004B20 RID: 19232 RVA: 0x00272820 File Offset: 0x00270C20
		public JobDef CurJobDef
		{
			get
			{
				return (this.CurJob == null) ? null : this.CurJob.def;
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x06004B21 RID: 19233 RVA: 0x00272854 File Offset: 0x00270C54
		public bool Downed
		{
			get
			{
				return this.health.Downed;
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06004B22 RID: 19234 RVA: 0x00272874 File Offset: 0x00270C74
		public bool Dead
		{
			get
			{
				return this.health.Dead;
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x06004B23 RID: 19235 RVA: 0x00272894 File Offset: 0x00270C94
		public string KindLabel
		{
			get
			{
				return GenLabel.BestKindLabel(this, false, false, false, -1);
			}
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06004B24 RID: 19236 RVA: 0x002728B4 File Offset: 0x00270CB4
		public bool InMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState;
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06004B25 RID: 19237 RVA: 0x002728EC File Offset: 0x00270CEC
		public MentalState MentalState
		{
			get
			{
				MentalState result;
				if (this.Dead)
				{
					result = null;
				}
				else
				{
					result = this.mindState.mentalStateHandler.CurState;
				}
				return result;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06004B26 RID: 19238 RVA: 0x00272924 File Offset: 0x00270D24
		public MentalStateDef MentalStateDef
		{
			get
			{
				MentalStateDef result;
				if (this.Dead)
				{
					result = null;
				}
				else
				{
					result = this.mindState.mentalStateHandler.CurStateDef;
				}
				return result;
			}
		}

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06004B27 RID: 19239 RVA: 0x0027295C File Offset: 0x00270D5C
		public bool InAggroMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState && this.mindState.mentalStateHandler.CurStateDef.IsAggro;
			}
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06004B28 RID: 19240 RVA: 0x002729B0 File Offset: 0x00270DB0
		public bool Inspired
		{
			get
			{
				return !this.Dead && this.mindState.inspirationHandler.Inspired;
			}
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06004B29 RID: 19241 RVA: 0x002729E8 File Offset: 0x00270DE8
		public Inspiration Inspiration
		{
			get
			{
				Inspiration result;
				if (this.Dead)
				{
					result = null;
				}
				else
				{
					result = this.mindState.inspirationHandler.CurState;
				}
				return result;
			}
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06004B2A RID: 19242 RVA: 0x00272A20 File Offset: 0x00270E20
		public InspirationDef InspirationDef
		{
			get
			{
				InspirationDef result;
				if (this.Dead)
				{
					result = null;
				}
				else
				{
					result = this.mindState.inspirationHandler.CurStateDef;
				}
				return result;
			}
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06004B2B RID: 19243 RVA: 0x00272A58 File Offset: 0x00270E58
		public override Vector3 DrawPos
		{
			get
			{
				return this.Drawer.DrawPos;
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06004B2C RID: 19244 RVA: 0x00272A78 File Offset: 0x00270E78
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x06004B2D RID: 19245 RVA: 0x00272A94 File Offset: 0x00270E94
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.def.Verbs;
			}
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06004B2E RID: 19246 RVA: 0x00272AB4 File Offset: 0x00270EB4
		public List<Tool> Tools
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x06004B2F RID: 19247 RVA: 0x00272ACC File Offset: 0x00270ECC
		public bool IsColonist
		{
			get
			{
				return base.Faction != null && base.Faction.IsPlayer && this.RaceProps.Humanlike;
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06004B30 RID: 19248 RVA: 0x00272B0C File Offset: 0x00270F0C
		public bool IsFreeColonist
		{
			get
			{
				return this.IsColonist && this.HostFaction == null;
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x06004B31 RID: 19249 RVA: 0x00272B38 File Offset: 0x00270F38
		public Faction HostFaction
		{
			get
			{
				Faction result;
				if (this.guest == null)
				{
					result = null;
				}
				else
				{
					result = this.guest.HostFaction;
				}
				return result;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06004B32 RID: 19250 RVA: 0x00272B6C File Offset: 0x00270F6C
		public bool Drafted
		{
			get
			{
				return this.drafter != null && this.drafter.Drafted;
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x06004B33 RID: 19251 RVA: 0x00272B9C File Offset: 0x00270F9C
		public bool IsPrisoner
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner;
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x06004B34 RID: 19252 RVA: 0x00272BCC File Offset: 0x00270FCC
		public bool IsPrisonerOfColony
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner && this.guest.HostFaction.IsPlayer;
			}
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x06004B35 RID: 19253 RVA: 0x00272C10 File Offset: 0x00271010
		public bool IsColonistPlayerControlled
		{
			get
			{
				return base.Spawned && this.IsColonist && this.MentalStateDef == null && this.HostFaction == null;
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06004B36 RID: 19254 RVA: 0x00272C54 File Offset: 0x00271054
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06004B37 RID: 19255 RVA: 0x00272C80 File Offset: 0x00271080
		public bool InContainerEnclosed
		{
			get
			{
				return base.ParentHolder.IsEnclosingContainer();
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06004B38 RID: 19256 RVA: 0x00272CA0 File Offset: 0x002710A0
		public Corpse Corpse
		{
			get
			{
				return base.ParentHolder as Corpse;
			}
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06004B39 RID: 19257 RVA: 0x00272CC0 File Offset: 0x002710C0
		public Pawn CarriedBy
		{
			get
			{
				Pawn result;
				if (base.ParentHolder == null)
				{
					result = null;
				}
				else
				{
					Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
					if (pawn_CarryTracker != null)
					{
						result = pawn_CarryTracker.pawn;
					}
					else
					{
						result = null;
					}
				}
				return result;
			}
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06004B3A RID: 19258 RVA: 0x00272D08 File Offset: 0x00271108
		public override string LabelNoCount
		{
			get
			{
				string result;
				if (this.Name != null)
				{
					if (this.story == null || this.story.TitleShortCap.NullOrEmpty())
					{
						result = this.Name.ToStringShort;
					}
					else
					{
						result = this.Name.ToStringShort + ", " + this.story.TitleShortCap;
					}
				}
				else
				{
					result = this.KindLabel;
				}
				return result;
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06004B3B RID: 19259 RVA: 0x00272D88 File Offset: 0x00271188
		public override string LabelShort
		{
			get
			{
				string result;
				if (this.Name != null)
				{
					result = this.Name.ToStringShort;
				}
				else
				{
					result = this.LabelNoCount;
				}
				return result;
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x06004B3C RID: 19260 RVA: 0x00272DC0 File Offset: 0x002711C0
		public Pawn_DrawTracker Drawer
		{
			get
			{
				if (this.drawer == null)
				{
					this.drawer = new Pawn_DrawTracker(this);
				}
				return this.drawer;
			}
		}

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x06004B3D RID: 19261 RVA: 0x00272DF4 File Offset: 0x002711F4
		public BillStack BillStack
		{
			get
			{
				return this.health.surgeryBills;
			}
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06004B3E RID: 19262 RVA: 0x00272E14 File Offset: 0x00271214
		public override IntVec3 InteractionCell
		{
			get
			{
				Building_Bed building_Bed = this.CurrentBed();
				if (building_Bed != null)
				{
					IntVec3 position = base.Position;
					IntVec3 position2 = base.Position;
					IntVec3 position3 = base.Position;
					IntVec3 position4 = base.Position;
					if (building_Bed.Rotation.IsHorizontal)
					{
						position.z++;
						position2.z--;
						position3.x--;
						position4.x++;
					}
					else
					{
						position.x--;
						position2.x++;
						position3.z++;
						position4.z--;
					}
					if (position.Standable(base.Map))
					{
						if (position.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position.GetDoor(base.Map) == null)
						{
							return position;
						}
					}
					if (position2.Standable(base.Map))
					{
						if (position2.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position2.GetDoor(base.Map) == null)
						{
							return position2;
						}
					}
					if (position3.Standable(base.Map))
					{
						if (position3.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position3.GetDoor(base.Map) == null)
						{
							return position3;
						}
					}
					if (position4.Standable(base.Map))
					{
						if (position4.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position4.GetDoor(base.Map) == null)
						{
							return position4;
						}
					}
					if (position.Standable(base.Map))
					{
						if (position.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position;
						}
					}
					if (position2.Standable(base.Map))
					{
						if (position2.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position2;
						}
					}
					if (position3.Standable(base.Map))
					{
						if (position3.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position3;
						}
					}
					if (position4.Standable(base.Map))
					{
						if (position4.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position4;
						}
					}
					if (position.Standable(base.Map))
					{
						return position;
					}
					if (position2.Standable(base.Map))
					{
						return position2;
					}
					if (position3.Standable(base.Map))
					{
						return position3;
					}
					if (position4.Standable(base.Map))
					{
						return position4;
					}
				}
				return base.InteractionCell;
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06004B3F RID: 19263 RVA: 0x00273204 File Offset: 0x00271604
		public TraderKindDef TraderKind
		{
			get
			{
				return (this.trader == null) ? null : this.trader.traderKind;
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06004B40 RID: 19264 RVA: 0x00273238 File Offset: 0x00271638
		public IEnumerable<Thing> Goods
		{
			get
			{
				return this.trader.Goods;
			}
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x06004B41 RID: 19265 RVA: 0x00273258 File Offset: 0x00271658
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.trader.RandomPriceFactorSeed;
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x06004B42 RID: 19266 RVA: 0x00273278 File Offset: 0x00271678
		public string TraderName
		{
			get
			{
				return this.trader.TraderName;
			}
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x06004B43 RID: 19267 RVA: 0x00273298 File Offset: 0x00271698
		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06004B44 RID: 19268 RVA: 0x002732C8 File Offset: 0x002716C8
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06004B45 RID: 19269 RVA: 0x002732E4 File Offset: 0x002716E4
		public float BodySize
		{
			get
			{
				return this.ageTracker.CurLifeStage.bodySizeFactor * this.RaceProps.baseBodySize;
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06004B46 RID: 19270 RVA: 0x00273318 File Offset: 0x00271718
		public float HealthScale
		{
			get
			{
				return this.ageTracker.CurLifeStage.healthScaleFactor * this.RaceProps.baseHealthScale;
			}
		}

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06004B47 RID: 19271 RVA: 0x0027334C File Offset: 0x0027174C
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x06004B48 RID: 19272 RVA: 0x00273364 File Offset: 0x00271764
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				LocalTargetInfo result;
				if (!base.Spawned)
				{
					result = LocalTargetInfo.Invalid;
				}
				else
				{
					Stance curStance = this.stances.curStance;
					if (curStance is Stance_Warmup || curStance is Stance_Cooldown)
					{
						result = ((Stance_Busy)curStance).focusTarg;
					}
					else
					{
						result = LocalTargetInfo.Invalid;
					}
				}
				return result;
			}
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06004B49 RID: 19273 RVA: 0x002733C8 File Offset: 0x002717C8
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06004B4A RID: 19274 RVA: 0x002733E0 File Offset: 0x002717E0
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.mindState.lastAttackedTarget;
			}
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06004B4B RID: 19275 RVA: 0x00273400 File Offset: 0x00271800
		public int LastAttackTargetTick
		{
			get
			{
				return this.mindState.lastAttackTargetTick;
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x06004B4C RID: 19276 RVA: 0x00273420 File Offset: 0x00271820
		public Verb CurrentEffectiveVerb
		{
			get
			{
				Building_Turret building_Turret = this.MannedThing() as Building_Turret;
				Verb result;
				if (building_Turret != null)
				{
					result = building_Turret.AttackVerb;
				}
				else
				{
					result = this.TryGetAttackVerb(null, !this.IsColonist);
				}
				return result;
			}
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x00273464 File Offset: 0x00271864
		public int GetRootTile()
		{
			return base.Tile;
		}

		// Token: 0x06004B4E RID: 19278 RVA: 0x00273480 File Offset: 0x00271880
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06004B4F RID: 19279 RVA: 0x00273498 File Offset: 0x00271898
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.inventory != null)
			{
				outChildren.Add(this.inventory);
			}
			if (this.carryTracker != null)
			{
				outChildren.Add(this.carryTracker);
			}
			if (this.equipment != null)
			{
				outChildren.Add(this.equipment);
			}
			if (this.apparel != null)
			{
				outChildren.Add(this.apparel);
			}
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x00273510 File Offset: 0x00271910
		public string GetKindLabelPlural(int count = -1)
		{
			return GenLabel.BestKindLabel(this, false, false, true, count);
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x00273533 File Offset: 0x00271933
		public static void ResetStaticData()
		{
			Pawn.NotSurgeryReadyTrans = "NotSurgeryReady".Translate();
			Pawn.CannotReachTrans = "CannotReach".Translate();
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x00273554 File Offset: 0x00271954
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
			Scribe_Deep.Look<Name>(ref this.nameInt, "name", new object[0]);
			Scribe_Deep.Look<Pawn_MindState>(ref this.mindState, "mindState", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_JobTracker>(ref this.jobs, "jobs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StanceTracker>(ref this.stances, "stances", new object[]
			{
				this
			});
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NativeVerbs>(ref this.natives, "natives", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_MeleeVerbs>(ref this.meleeVerbs, "meleeVerbs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RotationTracker>(ref this.rotationTracker, "rotationTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PathFollower>(ref this.pather, "pather", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_CarryTracker>(ref this.carryTracker, "carryTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ApparelTracker>(ref this.apparel, "apparel", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StoryTracker>(ref this.story, "story", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_EquipmentTracker>(ref this.equipment, "equipment", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DraftController>(ref this.drafter, "drafter", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_AgeTracker>(ref this.ageTracker, "ageTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_HealthTracker>(ref this.health, "healthTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RecordsTracker>(ref this.records, "records", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InventoryTracker>(ref this.inventory, "inventory", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FilthTracker>(ref this.filth, "filth", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NeedsTracker>(ref this.needs, "needs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuestTracker>(ref this.guest, "guest", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuiltTracker>(ref this.guilt, "guilt", new object[0]);
			Scribe_Deep.Look<Pawn_RelationsTracker>(ref this.relations, "social", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_Ownership>(ref this.ownership, "ownership", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InteractionsTracker>(ref this.interactions, "interactions", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_SkillTracker>(ref this.skills, "skills", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_WorkSettings>(ref this.workSettings, "workSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_OutfitTracker>(ref this.outfits, "outfits", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DrugPolicyTracker>(ref this.drugs, "drugs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TimetableTracker>(ref this.timetable, "timetable", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PlayerSettings>(ref this.playerSettings, "playerSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TrainingTracker>(ref this.training, "training", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.PawnPostLoadInit(this);
			}
		}

		// Token: 0x06004B53 RID: 19283 RVA: 0x002738F0 File Offset: 0x00271CF0
		public override string ToString()
		{
			string result;
			if (this.story != null)
			{
				result = this.LabelShort;
			}
			else if (this.thingIDNumber > 0)
			{
				result = base.ThingID;
			}
			else if (this.kindDef != null)
			{
				result = this.KindLabel + "_" + base.ThingID;
			}
			else if (this.def != null)
			{
				result = base.ThingID;
			}
			else
			{
				result = base.GetType().ToString();
			}
			return result;
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x00273980 File Offset: 0x00271D80
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Dead)
			{
				Log.Warning("Tried to spawn Dead Pawn " + this.ToStringSafe<Pawn>() + ". Replacing with corpse.", false);
				Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
				corpse.InnerPawn = this;
				GenSpawn.Spawn(corpse, base.Position, map, WipeMode.Vanish);
			}
			else if (this.def == null || this.kindDef == null)
			{
				Log.Warning("Tried to spawn pawn without def " + this.ToStringSafe<Pawn>() + ".", false);
			}
			else
			{
				base.SpawnSetup(map, respawningAfterLoad);
				if (Find.WorldPawns.Contains(this))
				{
					Find.WorldPawns.RemovePawn(this);
				}
				PawnComponentsUtility.AddComponentsForSpawn(this);
				if (!PawnUtility.InValidState(this))
				{
					Log.Error("Pawn " + this.ToStringSafe<Pawn>() + " spawned in invalid state. Destroying...", false);
					try
					{
						this.DeSpawn(DestroyMode.Vanish);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Tried to despawn ",
							this.ToStringSafe<Pawn>(),
							" because of the previous error but couldn't: ",
							ex
						}), false);
					}
					Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Discard);
				}
				else
				{
					this.Drawer.Notify_Spawned();
					this.rotationTracker.Notify_Spawned();
					this.pather.ResetToCurrentPosition();
					base.Map.mapPawns.RegisterPawn(this);
					if (this.RaceProps.IsFlesh)
					{
						this.relations.everSeenByPlayer = true;
					}
					AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
					if (this.needs != null && this.needs.mood != null && this.needs.mood.recentMemory != null)
					{
						this.needs.mood.recentMemory.Notify_Spawned(respawningAfterLoad);
					}
					if (!respawningAfterLoad)
					{
						this.records.AccumulateStoryEvent(StoryEventDefOf.Seen);
						Find.GameEnder.CheckOrUpdateGameOver();
						PawnDiedOrDownedThoughtsUtility.RemoveDiedThoughts(this);
					}
				}
			}
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x00273B98 File Offset: 0x00271F98
		public override void PostMapInit()
		{
			base.PostMapInit();
			this.pather.TryResumePathingAfterLoading();
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x00273BAC File Offset: 0x00271FAC
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.Drawer.DrawAt(drawLoc);
		}

		// Token: 0x06004B57 RID: 19287 RVA: 0x00273BBB File Offset: 0x00271FBB
		public override void DrawGUIOverlay()
		{
			this.Drawer.ui.DrawPawnGUIOverlay();
		}

		// Token: 0x06004B58 RID: 19288 RVA: 0x00273BD0 File Offset: 0x00271FD0
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.IsColonistPlayerControlled)
			{
				if (this.pather.curPath != null)
				{
					this.pather.curPath.DrawPath(this);
				}
				this.jobs.DrawLinesBetweenTargets();
			}
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x00273C20 File Offset: 0x00272020
		public override void TickRare()
		{
			base.TickRare();
			if (!base.Suspended)
			{
				if (this.apparel != null)
				{
					this.apparel.ApparelTrackerTickRare();
				}
				this.inventory.InventoryTrackerTickRare();
			}
			if (this.training != null)
			{
				this.training.TrainingTrackerTickRare();
			}
			if (base.Spawned && this.RaceProps.IsFlesh)
			{
				GenTemperature.PushHeat(this, 0.3f * this.BodySize * 4.16666651f * ((!this.def.race.Humanlike) ? 0.6f : 1f));
			}
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x00273CD0 File Offset: 0x002720D0
		public override void Tick()
		{
			if (DebugSettings.noAnimals && base.Spawned && this.RaceProps.Animal)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			else
			{
				base.Tick();
				if (Find.TickManager.TicksGame % 250 == 0)
				{
					this.TickRare();
				}
				bool suspended = base.Suspended;
				if (!suspended)
				{
					if (base.Spawned)
					{
						this.pather.PatherTick();
					}
					if (base.Spawned)
					{
						this.stances.StanceTrackerTick();
						this.verbTracker.VerbsTick();
						this.natives.NativeVerbsTick();
					}
					if (base.Spawned)
					{
						Profiler.BeginSample("jobs");
						this.jobs.JobTrackerTick();
						Profiler.EndSample();
					}
					if (base.Spawned)
					{
						Profiler.BeginSample("Drawer");
						this.Drawer.DrawTrackerTick();
						Profiler.EndSample();
						Profiler.BeginSample("rotationTracker");
						this.rotationTracker.RotationTrackerTick();
						Profiler.EndSample();
					}
					Profiler.BeginSample("health");
					this.health.HealthTick();
					Profiler.EndSample();
					if (!this.Dead)
					{
						Profiler.BeginSample("mindState");
						this.mindState.MindStateTick();
						Profiler.EndSample();
						this.carryTracker.CarryHandsTick();
					}
				}
				if (!this.Dead)
				{
					this.needs.NeedsTrackerTick();
				}
				if (!suspended)
				{
					if (this.equipment != null)
					{
						Profiler.BeginSample("equipment");
						this.equipment.EquipmentTrackerTick();
						Profiler.EndSample();
					}
					if (this.apparel != null)
					{
						this.apparel.ApparelTrackerTick();
					}
					if (this.interactions != null && base.Spawned)
					{
						Profiler.BeginSample("interactions");
						this.interactions.InteractionsTrackerTick();
						Profiler.EndSample();
					}
					if (this.caller != null)
					{
						this.caller.CallTrackerTick();
					}
					if (this.skills != null)
					{
						this.skills.SkillsTick();
					}
					if (this.inventory != null)
					{
						this.inventory.InventoryTrackerTick();
					}
					if (this.drafter != null)
					{
						this.drafter.DraftControllerTick();
					}
					if (this.relations != null)
					{
						this.relations.RelationsTrackerTick();
					}
					if (this.RaceProps.Humanlike)
					{
						this.guest.GuestTrackerTick();
					}
					this.ageTracker.AgeTick();
					this.records.RecordsTick();
				}
			}
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x00273F64 File Offset: 0x00272364
		public void TickMothballed(int interval)
		{
			if (!base.Suspended)
			{
				this.ageTracker.AgeTickMothballed(interval);
				this.records.RecordsTickMothballed(interval);
			}
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x00273F8C File Offset: 0x0027238C
		public void Notify_Teleported(bool endCurrentJob = true)
		{
			this.Drawer.tweener.ResetTweenedPosToRoot();
			this.pather.Notify_Teleported_Int();
			if (endCurrentJob && this.jobs != null && this.jobs.curJob != null)
			{
				this.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
			}
		}

		// Token: 0x06004B5D RID: 19293 RVA: 0x00273FE4 File Offset: 0x002723E4
		public void Notify_PassedToWorld()
		{
			if (((base.Faction == null && this.RaceProps.Humanlike) || (base.Faction != null && base.Faction.IsPlayer) || base.Faction == Faction.OfAncients || base.Faction == Faction.OfAncientsHostile) && !this.Dead && Find.WorldPawns.GetSituation(this) == WorldPawnSituation.Free)
			{
				bool tryMedievalOrBetter = base.Faction != null && base.Faction.def.techLevel >= TechLevel.Medieval;
				Faction faction;
				if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, false, TechLevel.Undefined))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, true, TechLevel.Undefined))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
			}
			if (!this.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(this))
			{
				this.ClearMind(false);
			}
			if (this.relations != null)
			{
				this.relations.Notify_PassedToWorld();
			}
		}

		// Token: 0x06004B5E RID: 19294 RVA: 0x00274130 File Offset: 0x00272530
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (!absorbed)
			{
				if (this.story != null && this.story.traits.HasTrait(TraitDefOf.Tough) && dinfo.Def.externalViolence)
				{
					dinfo.SetAmount(dinfo.Amount * 0.6f);
				}
				this.health.PreApplyDamage(dinfo, out absorbed);
			}
		}

		// Token: 0x06004B5F RID: 19295 RVA: 0x002741AC File Offset: 0x002725AC
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (dinfo.Def.externalViolence)
			{
				this.records.AddTo(RecordDefOf.DamageTaken, totalDamageDealt);
			}
			if (dinfo.Def.makesBlood && !dinfo.InstantPermanentInjury && totalDamageDealt > 0f && Rand.Chance(0.5f))
			{
				this.health.DropBloodFilth();
			}
			this.records.AccumulateStoryEvent(StoryEventDefOf.DamageTaken);
			this.health.PostApplyDamage(dinfo, totalDamageDealt);
			if (!this.Dead)
			{
				this.mindState.Notify_DamageTaken(dinfo);
			}
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x0027425C File Offset: 0x0027265C
		public override Thing SplitOff(int count)
		{
			if (count <= 0 || count >= this.stackCount)
			{
				return base.SplitOff(count);
			}
			throw new NotImplementedException("Split off on Pawns is not supported (unless we're taking a full stack).");
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x06004B61 RID: 19297 RVA: 0x00274298 File Offset: 0x00272698
		public int TicksPerMoveCardinal
		{
			get
			{
				return this.TicksPerMove(false);
			}
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x06004B62 RID: 19298 RVA: 0x002742B4 File Offset: 0x002726B4
		public int TicksPerMoveDiagonal
		{
			get
			{
				return this.TicksPerMove(true);
			}
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x002742D0 File Offset: 0x002726D0
		private int TicksPerMove(bool diagonal)
		{
			float num = this.GetStatValue(StatDefOf.MoveSpeed, true);
			if (RestraintsUtility.InRestraints(this))
			{
				num *= 0.35f;
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null && this.carryTracker.CarriedThing.def.category == ThingCategory.Pawn)
			{
				num *= 0.6f;
			}
			float num2 = num / 60f;
			float num3;
			if (num2 == 0f)
			{
				num3 = 450f;
			}
			else
			{
				num3 = 1f / num2;
				if (base.Spawned && !base.Map.roofGrid.Roofed(base.Position))
				{
					num3 /= base.Map.weatherManager.CurMoveSpeedMultiplier;
				}
				if (diagonal)
				{
					num3 *= 1.41421f;
				}
			}
			int value = Mathf.RoundToInt(num3);
			return Mathf.Clamp(value, 1, 450);
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x002743CC File Offset: 0x002727CC
		public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
		{
			IntVec3 positionHeld = base.PositionHeld;
			Map map = base.Map;
			Map mapHeld = base.MapHeld;
			bool flag = base.Spawned;
			bool spawnedOrAnyParentSpawned = base.SpawnedOrAnyParentSpawned;
			bool wasWorldPawn = this.IsWorldPawn();
			Caravan caravan = this.GetCaravan();
			Building_Grave assignedGrave = null;
			if (this.ownership != null)
			{
				assignedGrave = this.ownership.AssignedGrave;
			}
			bool flag2 = this.InBed();
			float bedRotation = 0f;
			if (flag2)
			{
				bedRotation = this.CurrentBed().Rotation.AsAngle;
			}
			ThingOwner thingOwner = null;
			bool inContainerEnclosed = this.InContainerEnclosed;
			if (inContainerEnclosed)
			{
				thingOwner = this.holdingOwner;
				thingOwner.Remove(this);
			}
			bool flag3 = false;
			bool flag4 = false;
			if (Current.ProgramState == ProgramState.Playing && map != null)
			{
				flag3 = (map.designationManager.DesignationOn(this, DesignationDefOf.Hunt) != null);
				flag4 = (map.designationManager.DesignationOn(this, DesignationDefOf.Slaughter) != null);
			}
			bool flag5 = PawnUtility.ShouldSendNotificationAbout(this) && (!flag4 || dinfo == null || dinfo.Value.Def != DamageDefOf.ExecutionCut);
			float num = 0f;
			Thing attachment = this.GetAttachment(ThingDefOf.Fire);
			if (attachment != null)
			{
				num = ((Fire)attachment).CurrentSize();
			}
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this, dinfo, PawnDiedOrDownedThoughtsKind.Died);
			if (Current.ProgramState == ProgramState.Playing && this.IsColonist)
			{
				Find.StoryWatcher.watcherRampUp.Notify_ColonistViolentlyDownedOrKilled(this);
			}
			if (this.IsColonist)
			{
				Find.StoryWatcher.statsRecord.colonistsKilled++;
			}
			if (flag && dinfo != null && dinfo.Value.Def.externalViolence)
			{
				LifeStageUtility.PlayNearestLifestageSound(this, (LifeStageAge ls) => ls.soundDeath, 1f);
			}
			if (dinfo != null && dinfo.Value.Instigator != null)
			{
				Pawn pawn = dinfo.Value.Instigator as Pawn;
				if (pawn != null)
				{
					RecordsUtility.Notify_PawnKilled(this, pawn);
					if (this.IsColonist)
					{
						pawn.records.AccumulateStoryEvent(StoryEventDefOf.KilledPlayer);
					}
				}
			}
			TaleUtility.Notify_PawnDied(this, dinfo);
			if (flag)
			{
				Find.BattleLog.Add(new BattleLogEntry_StateTransition(this, this.RaceProps.DeathActionWorker.DeathRules, (dinfo == null) ? null : (dinfo.Value.Instigator as Pawn), exactCulprit, (dinfo == null) ? null : dinfo.Value.HitPart));
			}
			this.health.surgeryBills.Clear();
			if (this.apparel != null)
			{
				this.apparel.Notify_PawnKilled(dinfo);
			}
			if (this.RaceProps.IsFlesh)
			{
				this.relations.Notify_PawnKilled(dinfo, map);
			}
			this.meleeVerbs.Notify_PawnKilled();
			Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
			if (pawn_CarryTracker != null)
			{
				Thing thing;
				if (this.holdingOwner.TryDrop(this, pawn_CarryTracker.pawn.Position, pawn_CarryTracker.pawn.Map, ThingPlaceMode.Near, out thing, null, null))
				{
					map = pawn_CarryTracker.pawn.Map;
					flag = true;
				}
			}
			this.health.SetDead();
			if (this.health.deflectionEffecter != null)
			{
				this.health.deflectionEffecter.Cleanup();
				this.health.deflectionEffecter = null;
			}
			if (caravan != null)
			{
				caravan.Notify_MemberDied(this);
			}
			if (flag)
			{
				this.DropAndForbidEverything(false);
			}
			if (flag)
			{
				this.DeSpawn(DestroyMode.Vanish);
			}
			Corpse corpse = null;
			if (!PawnGenerator.IsBeingGenerated(this))
			{
				if (inContainerEnclosed)
				{
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
					if (!thingOwner.TryAdd(corpse, true))
					{
						corpse.Destroy(DestroyMode.Vanish);
						corpse = null;
					}
				}
				else if (spawnedOrAnyParentSpawned)
				{
					if (this.holdingOwner != null)
					{
						this.holdingOwner.Remove(this);
					}
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
					if (GenPlace.TryPlaceThing(corpse, positionHeld, mapHeld, ThingPlaceMode.Direct, null, null))
					{
						corpse.Rotation = base.Rotation;
						if (HuntJobUtility.WasKilledByHunter(this, dinfo))
						{
							((Pawn)dinfo.Value.Instigator).Reserve(corpse, ((Pawn)dinfo.Value.Instigator).CurJob, 1, -1, null);
						}
						else if (!flag3 && !flag4)
						{
							corpse.SetForbiddenIfOutsideHomeArea();
						}
						if (num > 0f)
						{
							FireUtility.TryStartFireIn(corpse.Position, corpse.Map, num);
						}
					}
					else
					{
						corpse.Destroy(DestroyMode.Vanish);
						corpse = null;
					}
				}
				else if (caravan != null && caravan.Spawned)
				{
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
					caravan.AddPawnOrItem(corpse, true);
				}
				else if (this.holdingOwner != null || this.IsWorldPawn())
				{
					Corpse.PostCorpseDestroy(this);
				}
				else
				{
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
				}
			}
			if (corpse != null)
			{
				Hediff firstHediffOfDef = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
				CompRottable comp = corpse.GetComp<CompRottable>();
				if (firstHediffOfDef != null && Rand.Value < firstHediffOfDef.Severity && comp != null)
				{
					comp.RotImmediately();
				}
			}
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.KillFinalize);
			}
			PawnComponentsUtility.RemoveComponentsOnKilled(this);
			this.health.hediffSet.DirtyCache();
			PortraitsCache.SetDirty(this);
			for (int i = 0; i < this.health.hediffSet.hediffs.Count; i++)
			{
				this.health.hediffSet.hediffs[i].Notify_PawnDied();
			}
			if (base.Faction != null)
			{
				base.Faction.Notify_MemberDied(this, dinfo, wasWorldPawn, mapHeld);
			}
			if (corpse != null)
			{
				if (this.RaceProps.DeathActionWorker != null && flag)
				{
					this.RaceProps.DeathActionWorker.PawnDied(corpse);
				}
				if (Find.Scenario != null)
				{
					Find.Scenario.Notify_PawnDied(corpse);
				}
			}
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				BillUtility.Notify_ColonistUnavailable(this);
			}
			if (spawnedOrAnyParentSpawned)
			{
				GenHostility.Notify_PawnLostForTutor(this, mapHeld);
			}
			if (base.Faction != null && base.Faction.IsPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			if (flag5)
			{
				this.health.NotifyPlayerOfKilled(dinfo, exactCulprit, caravan);
			}
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x00274AF0 File Offset: 0x00272EF0
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.Vanish && mode != DestroyMode.KillFinalize)
			{
				Log.Error(string.Concat(new object[]
				{
					"Destroyed pawn ",
					this,
					" with unsupported mode ",
					mode,
					"."
				}), false);
			}
			base.Destroy(mode);
			Find.WorldPawns.Notify_PawnDestroyed(this);
			if (this.ownership != null)
			{
				this.ownership.UnclaimAll();
			}
			this.ClearMind(false);
			if (Current.ProgramState == ProgramState.Playing)
			{
				Lord lord = this.GetLord();
				if (lord != null)
				{
					PawnLostCondition cond = (mode != DestroyMode.KillFinalize) ? PawnLostCondition.Vanished : PawnLostCondition.IncappedOrKilled;
					lord.Notify_PawnLost(this, cond);
				}
				Find.GameEnder.CheckOrUpdateGameOver();
				Find.TaleManager.Notify_PawnDestroyed(this);
			}
			foreach (Pawn pawn in from p in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where p.playerSettings != null && p.playerSettings.Master == this
			select p)
			{
				pawn.playerSettings.Master = null;
			}
			if (mode != DestroyMode.KillFinalize)
			{
				if (this.equipment != null)
				{
					this.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				}
				this.inventory.DestroyAll(DestroyMode.Vanish);
				if (this.apparel != null)
				{
					this.apparel.DestroyAll(DestroyMode.Vanish);
				}
			}
			WorldPawns worldPawns = Find.WorldPawns;
			if (!worldPawns.IsBeingDiscarded(this) && !worldPawns.Contains(this))
			{
				worldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			}
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x00274C8C File Offset: 0x0027308C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			if (this.jobs != null && this.jobs.curJob != null)
			{
				this.jobs.StopAll(false);
			}
			base.DeSpawn(mode);
			if (this.pather != null)
			{
				this.pather.StopDead();
			}
			if (this.needs != null && this.needs.mood != null)
			{
				this.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (this.meleeVerbs != null)
			{
				this.meleeVerbs.Notify_PawnDespawned();
			}
			this.ClearAllReservations(false);
			if (map != null)
			{
				map.mapPawns.DeRegisterPawn(this);
			}
			PawnComponentsUtility.RemoveComponentsOnDespawned(this);
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x00274D50 File Offset: 0x00273150
		public override void Discard(bool silentlyRemoveReferences = false)
		{
			if (Find.WorldPawns.Contains(this))
			{
				Log.Warning("Tried to discard a world pawn " + this + ".", false);
			}
			else
			{
				base.Discard(silentlyRemoveReferences);
				if (this.relations != null)
				{
					this.relations.ClearAllRelations();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.PlayLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
					Find.BattleLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
					Find.TaleManager.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				}
				foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive)
				{
					if (pawn.needs.mood != null)
					{
						pawn.needs.mood.thoughts.memories.Notify_PawnDiscarded(this);
					}
				}
				Corpse.PostCorpseDestroy(this);
			}
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x00274E50 File Offset: 0x00273250
		private Corpse MakeCorpse(Building_Grave assignedGrave, bool inBed, float bedRotation)
		{
			Corpse result;
			if (this.holdingOwner != null)
			{
				Log.Warning("We can't make corpse because the pawn is in a ThingOwner. Remove him from the container first. This should have been already handled before calling this method. holder=" + base.ParentHolder, false);
				result = null;
			}
			else
			{
				Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
				corpse.InnerPawn = this;
				if (assignedGrave != null)
				{
					corpse.InnerPawn.ownership.ClaimGrave(assignedGrave);
				}
				if (inBed)
				{
					corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(bedRotation + 180f);
				}
				result = corpse;
			}
			return result;
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x00274EEC File Offset: 0x002732EC
		public void ExitMap(bool allowedToJoinOrCreateCaravan, Rot4 exitDir)
		{
			if (this.IsWorldPawn())
			{
				Log.Warning("Called ExitMap() on world pawn " + this, false);
			}
			else if (allowedToJoinOrCreateCaravan && CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this))
			{
				CaravanExitMapUtility.ExitMapAndJoinOrCreateCaravan(this, exitDir);
			}
			else
			{
				Lord lord = this.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this, PawnLostCondition.ExitedMap);
				}
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					Pawn pawn = this.carryTracker.CarriedThing as Pawn;
					if (pawn != null)
					{
						if (base.Faction != null && base.Faction != pawn.Faction)
						{
							base.Faction.kidnapped.KidnapPawn(pawn, this);
						}
						else
						{
							this.carryTracker.innerContainer.Remove(pawn);
							pawn.ExitMap(false, exitDir);
						}
					}
					else
					{
						this.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
					}
					this.carryTracker.innerContainer.Clear();
				}
				bool flag = !this.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(this);
				if (base.Faction != null)
				{
					base.Faction.Notify_MemberExitedMap(this, flag);
				}
				if (this.ownership != null)
				{
					this.ownership.UnclaimAll();
				}
				if (this.guest != null)
				{
					if (flag)
					{
						this.guest.SetGuestStatus(null, false);
					}
					this.guest.Released = false;
				}
				if (base.Spawned)
				{
					this.DeSpawn(DestroyMode.Vanish);
				}
				this.inventory.UnloadEverything = false;
				if (flag)
				{
					this.ClearMind(false);
				}
				if (this.relations != null)
				{
					this.relations.Notify_ExitedMap();
				}
				Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			}
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x002750BC File Offset: 0x002734BC
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PreTraded(action, playerNegotiator, trader);
			if (base.SpawnedOrAnyParentSpawned)
			{
				this.DropAndForbidEverything(false);
			}
			if (this.ownership != null)
			{
				this.ownership.UnclaimAll();
			}
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, false);
			}
			if (action == TradeAction.PlayerBuys)
			{
				this.SetFaction(Faction.OfPlayer, null);
			}
			else if (action == TradeAction.PlayerSells)
			{
				if (this.RaceProps.Humanlike)
				{
					TaleRecorder.RecordTale(TaleDefOf.SoldPrisoner, new object[]
					{
						playerNegotiator,
						this,
						trader
					});
				}
				if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
				if (this.RaceProps.IsFlesh)
				{
					this.relations.Notify_PawnSold(playerNegotiator);
				}
				if (this.RaceProps.Humanlike)
				{
					foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowPrisonerSold, null);
					}
				}
			}
			this.ClearMind(false);
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x00275214 File Offset: 0x00273614
		public void PreKidnapped(Pawn kidnapper)
		{
			if (this.IsColonist && kidnapper != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.KidnappedColonist, new object[]
				{
					kidnapper,
					this
				});
			}
			if (this.ownership != null)
			{
				this.ownership.UnclaimAll();
			}
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, false);
			}
			if (this.RaceProps.IsFlesh)
			{
				this.relations.Notify_PawnKidnapped();
			}
			this.ClearMind(false);
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x0027529C File Offset: 0x0027369C
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (newFaction == base.Faction)
			{
				Log.Warning("Used SetFaction to change " + this.ToStringSafe<Pawn>() + " to same faction " + newFaction.ToStringSafe<Faction>(), false);
			}
			else
			{
				Faction faction = base.Faction;
				if (this.guest != null)
				{
					this.guest.SetGuestStatus(null, false);
				}
				if (base.Spawned)
				{
					base.Map.mapPawns.DeRegisterPawn(this);
					base.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
					base.Map.designationManager.RemoveAllDesignationsOn(this, false);
				}
				if ((newFaction == Faction.OfPlayer || base.Faction == Faction.OfPlayer) && Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
				Lord lord = this.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this, PawnLostCondition.ChangedFaction);
				}
				if (base.Faction != null && base.Faction.leader == this)
				{
					base.Faction.Notify_LeaderLost();
				}
				if (newFaction == Faction.OfPlayer && this.RaceProps.Humanlike)
				{
					this.ChangeKind(newFaction.def.basicMemberKind);
				}
				base.SetFaction(newFaction, null);
				PawnComponentsUtility.AddAndRemoveDynamicComponents(this, false);
				if (base.Faction != null && base.Faction.IsPlayer)
				{
					if (this.workSettings != null)
					{
						this.workSettings.EnableAndInitialize();
					}
					Find.Storyteller.intenderPopulation.Notify_PopulationGained();
				}
				if (this.Drafted)
				{
					this.drafter.Drafted = false;
				}
				ReachabilityUtility.ClearCache();
				this.health.surgeryBills.Clear();
				if (base.Spawned)
				{
					base.Map.mapPawns.RegisterPawn(this);
				}
				this.GenerateNecessaryName();
				if (this.playerSettings != null)
				{
					this.playerSettings.ResetMedicalCare();
				}
				this.ClearMind(true);
				if (!this.Dead && this.needs.mood != null)
				{
					this.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
				if (base.Spawned)
				{
					base.Map.attackTargetsCache.UpdateTarget(this);
				}
				Find.GameEnder.CheckOrUpdateGameOver();
				AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
				if (this.needs != null)
				{
					this.needs.AddOrRemoveNeedsAsAppropriate();
				}
				if (this.playerSettings != null)
				{
					this.playerSettings.Notify_FactionChanged();
				}
				if (this.relations != null)
				{
					this.relations.Notify_ChangedFaction();
				}
				if (this.RaceProps.Animal && newFaction == Faction.OfPlayer)
				{
					this.training.SetWantedRecursive(TrainableDefOf.Tameness, true);
					this.training.Train(TrainableDefOf.Tameness, recruiter, true);
				}
				if (faction == Faction.OfPlayer)
				{
					BillUtility.Notify_ColonistUnavailable(this);
				}
			}
		}

		// Token: 0x06004B6D RID: 19309 RVA: 0x00275588 File Offset: 0x00273988
		public void ClearMind(bool ifLayingKeepLaying = false)
		{
			if (this.pather != null)
			{
				this.pather.StopDead();
			}
			if (this.mindState != null)
			{
				this.mindState.Reset();
			}
			if (this.jobs != null)
			{
				this.jobs.StopAll(ifLayingKeepLaying);
			}
			this.VerifyReservations();
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x002755E0 File Offset: 0x002739E0
		public void ClearAllReservations(bool releaseDestinationsOnlyIfObsolete = true)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (releaseDestinationsOnlyIfObsolete)
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllObsoleteClaimedBy(this);
				}
				else
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
				}
				maps[i].reservationManager.ReleaseAllClaimedBy(this);
				maps[i].physicalInteractionReservationManager.ReleaseAllClaimedBy(this);
				maps[i].attackTargetReservationManager.ReleaseAllClaimedBy(this);
			}
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x00275674 File Offset: 0x00273A74
		public void ClearReservationsForJob(Job job)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].pawnDestinationReservationManager.ReleaseClaimedBy(this, job);
				maps[i].reservationManager.ReleaseClaimedBy(this, job);
				maps[i].physicalInteractionReservationManager.ReleaseClaimedBy(this, job);
				maps[i].attackTargetReservationManager.ReleaseClaimedBy(this, job);
			}
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x002756F0 File Offset: 0x00273AF0
		public void VerifyReservations()
		{
			if (this.jobs != null)
			{
				if (this.CurJob == null && this.jobs.jobQueue.Count <= 0 && !this.jobs.startingNewJob)
				{
					bool flag = false;
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						LocalTargetInfo obj = maps[i].reservationManager.FirstReservationFor(this);
						if (obj.IsValid)
						{
							Log.ErrorOnce(string.Format("Reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), obj.ToStringSafe<LocalTargetInfo>()), 97771429 ^ this.thingIDNumber, false);
							flag = true;
						}
						LocalTargetInfo obj2 = maps[i].physicalInteractionReservationManager.FirstReservationFor(this);
						if (obj2.IsValid)
						{
							Log.ErrorOnce(string.Format("Physical interaction reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), obj2.ToStringSafe<LocalTargetInfo>()), 19586765 ^ this.thingIDNumber, false);
							flag = true;
						}
						IAttackTarget attackTarget = maps[i].attackTargetReservationManager.FirstReservationFor(this);
						if (attackTarget != null)
						{
							Log.ErrorOnce(string.Format("Attack target reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), attackTarget.ToStringSafe<IAttackTarget>()), 100495878 ^ this.thingIDNumber, false);
							flag = true;
						}
						IntVec3 obj3 = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationFor(this);
						if (obj3.IsValid)
						{
							Job job = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationJobFor(this);
							Log.ErrorOnce(string.Format("Pawn destination reservation manager failed to clean up properly; {0}/{1}/{2} still reserving {3}", new object[]
							{
								this.ToStringSafe<Pawn>(),
								job.ToStringSafe<Job>(),
								job.def.ToStringSafe<JobDef>(),
								obj3.ToStringSafe<IntVec3>()
							}), 1958674 ^ this.thingIDNumber, false);
							flag = true;
						}
					}
					if (flag)
					{
						this.ClearAllReservations(true);
					}
				}
			}
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x002758D8 File Offset: 0x00273CD8
		public void DropAndForbidEverything(bool keepInventoryAndEquipmentIfInBed = false)
		{
			if (this.kindDef.destroyGearOnDrop)
			{
				this.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				this.apparel.DestroyAll(DestroyMode.Vanish);
			}
			if (this.InContainerEnclosed)
			{
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					this.carryTracker.innerContainer.TryTransferToContainer(this.carryTracker.CarriedThing, this.holdingOwner, true);
				}
				if (this.equipment != null && this.equipment.Primary != null)
				{
					this.equipment.TryTransferEquipmentToContainer(this.equipment.Primary, this.holdingOwner);
				}
				if (this.inventory != null)
				{
					this.inventory.innerContainer.TryTransferAllToContainer(this.holdingOwner, true);
				}
			}
			else if (base.SpawnedOrAnyParentSpawned)
			{
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					Thing thing;
					this.carryTracker.TryDropCarriedThing(base.PositionHeld, ThingPlaceMode.Near, out thing, null);
				}
				if (!keepInventoryAndEquipmentIfInBed || !this.InBed())
				{
					if (this.equipment != null)
					{
						this.equipment.DropAllEquipment(base.PositionHeld, true);
					}
					if (this.inventory != null && this.inventory.innerContainer.TotalStackCount > 0)
					{
						this.inventory.DropAllNearPawn(base.PositionHeld, true, false);
					}
				}
			}
		}

		// Token: 0x06004B72 RID: 19314 RVA: 0x00275A60 File Offset: 0x00273E60
		public void GenerateNecessaryName()
		{
			if (base.Faction == Faction.OfPlayer && this.RaceProps.Animal)
			{
				if (this.Name == null || this.Name.Numerical)
				{
					if (Rand.Value < this.RaceProps.nameOnTameChance)
					{
						this.Name = PawnBioAndNameGenerator.GeneratePawnName(this, NameStyle.Full, null);
					}
					else
					{
						this.Name = PawnBioAndNameGenerator.GeneratePawnName(this, NameStyle.Numeric, null);
					}
				}
			}
		}

		// Token: 0x06004B73 RID: 19315 RVA: 0x00275AE8 File Offset: 0x00273EE8
		public Verb TryGetAttackVerb(Thing target, bool allowManualCastWeapons = false)
		{
			Verb result;
			if (this.equipment != null && this.equipment.Primary != null && this.equipment.PrimaryEq.PrimaryVerb.Available() && (!this.equipment.PrimaryEq.PrimaryVerb.verbProps.onlyManualCast || (this.CurJob != null && this.CurJob.def != JobDefOf.Wait_Combat) || allowManualCastWeapons))
			{
				result = this.equipment.PrimaryEq.PrimaryVerb;
			}
			else
			{
				result = this.meleeVerbs.TryGetMeleeVerb(target);
			}
			return result;
		}

		// Token: 0x06004B74 RID: 19316 RVA: 0x00275B9C File Offset: 0x00273F9C
		public bool TryStartAttack(LocalTargetInfo targ)
		{
			bool result;
			if (this.stances.FullBodyBusy)
			{
				result = false;
			}
			else if (this.story != null && this.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				result = false;
			}
			else
			{
				bool allowManualCastWeapons = !this.IsColonist;
				Verb verb = this.TryGetAttackVerb(targ.Thing, allowManualCastWeapons);
				result = (verb != null && verb.TryStartCastOn(targ, false, true));
			}
			return result;
		}

		// Token: 0x06004B75 RID: 19317 RVA: 0x00275C1C File Offset: 0x0027401C
		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			if (this.RaceProps.meatDef != null)
			{
				int meatCount = GenMath.RoundRandom(this.GetStatValue(StatDefOf.MeatAmount, true) * efficiency);
				if (meatCount > 0)
				{
					Thing meat = ThingMaker.MakeThing(this.RaceProps.meatDef, null);
					meat.stackCount = meatCount;
					yield return meat;
				}
			}
			foreach (Thing t in this.<ButcherProducts>__BaseCallProxy0(butcher, efficiency))
			{
				yield return t;
			}
			if (this.RaceProps.leatherDef != null)
			{
				int leatherCount = GenMath.RoundRandom(this.GetStatValue(StatDefOf.LeatherAmount, true) * efficiency);
				if (leatherCount > 0)
				{
					Thing leather = ThingMaker.MakeThing(this.RaceProps.leatherDef, null);
					leather.stackCount = leatherCount;
					yield return leather;
				}
			}
			if (!this.RaceProps.Humanlike)
			{
				PawnKindLifeStage lifeStage = this.ageTracker.CurKindLifeStage;
				if (lifeStage.butcherBodyPart != null)
				{
					if (this.gender == Gender.None || (this.gender == Gender.Male && lifeStage.butcherBodyPart.allowMale) || (this.gender == Gender.Female && lifeStage.butcherBodyPart.allowFemale))
					{
						for (;;)
						{
							BodyPartRecord record = (from x in this.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
							where x.IsInGroup(lifeStage.butcherBodyPart.bodyPartGroup)
							select x).FirstOrDefault<BodyPartRecord>();
							if (record == null)
							{
								break;
							}
							this.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this, record), null, null, null);
							Thing thing;
							if (lifeStage.butcherBodyPart.thing != null)
							{
								thing = ThingMaker.MakeThing(lifeStage.butcherBodyPart.thing, null);
							}
							else
							{
								thing = ThingMaker.MakeThing(record.def.spawnThingOnRemoved, null);
							}
							yield return thing;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06004B76 RID: 19318 RVA: 0x00275C54 File Offset: 0x00274054
		public string MainDesc(bool writeAge)
		{
			string text = GenLabel.BestKindLabel(this, true, true, false, -1);
			if (base.Faction != null && !base.Faction.def.hidden)
			{
				text = "PawnMainDescFactionedWrap".Translate(new object[]
				{
					text,
					base.Faction.Name
				});
			}
			if (writeAge && this.ageTracker != null)
			{
				text = text + ", " + "AgeIndicator".Translate(new object[]
				{
					this.ageTracker.AgeNumberString
				});
			}
			return text.CapitalizeFirst();
		}

		// Token: 0x06004B77 RID: 19319 RVA: 0x00275CF8 File Offset: 0x002740F8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.MainDesc(true));
			if (this.TraderKind != null)
			{
				stringBuilder.AppendLine(this.TraderKind.LabelCap);
			}
			if (this.InMentalState)
			{
				stringBuilder.AppendLine(this.MentalState.InspectLine);
			}
			Pawn.states.Clear();
			if (this.stances != null && this.stances.stunner != null && this.stances.stunner.Stunned)
			{
				Pawn.states.AddDistinct("StunLower".Translate());
			}
			if (this.health != null && this.health.hediffSet != null)
			{
				List<Hediff> hediffs = this.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff hediff = hediffs[i];
					if (!hediff.def.battleStateLabel.NullOrEmpty())
					{
						Pawn.states.AddDistinct(hediff.def.battleStateLabel);
					}
				}
			}
			if (Pawn.states.Count > 0)
			{
				Pawn.states.Sort();
				stringBuilder.AppendLine(string.Format("{0}: {1}", "State".Translate(), Pawn.states.ToCommaList(false).CapitalizeFirst()));
				Pawn.states.Clear();
			}
			if (this.Inspired)
			{
				stringBuilder.AppendLine(this.Inspiration.InspectLine);
			}
			if (this.equipment != null && this.equipment.Primary != null)
			{
				stringBuilder.AppendLine("Equipped".Translate() + ": " + ((this.equipment.Primary == null) ? "EquippedNothing".Translate() : this.equipment.Primary.Label).CapitalizeFirst());
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
			{
				stringBuilder.Append("Carrying".Translate() + ": ");
				stringBuilder.AppendLine(this.carryTracker.CarriedThing.LabelCap);
			}
			string text = null;
			Lord lord = this.GetLord();
			if (lord != null && lord.LordJob != null)
			{
				text = lord.LordJob.GetReport();
			}
			if (this.jobs.curJob != null)
			{
				try
				{
					string text2 = this.jobs.curDriver.GetReport().CapitalizeFirst();
					if (!text.NullOrEmpty())
					{
						text = text + ": " + text2;
					}
					else
					{
						text = text2;
					}
				}
				catch (Exception arg)
				{
					Log.Error("JobDriver.GetReport() exception: " + arg, false);
				}
			}
			if (!text.NullOrEmpty())
			{
				stringBuilder.AppendLine(text);
			}
			if (this.jobs.curJob != null && this.jobs.jobQueue.Count > 0)
			{
				try
				{
					string text3 = this.jobs.jobQueue[0].job.GetReport(this).CapitalizeFirst();
					if (this.jobs.jobQueue.Count > 1)
					{
						string text4 = text3;
						text3 = string.Concat(new object[]
						{
							text4,
							" (+",
							this.jobs.jobQueue.Count - 1,
							")"
						});
					}
					stringBuilder.AppendLine("Queued".Translate() + ": " + text3);
				}
				catch (Exception arg2)
				{
					Log.Error("JobDriver.GetReport() exception: " + arg2, false);
				}
			}
			if (RestraintsUtility.ShouldShowRestraintsInfo(this))
			{
				stringBuilder.AppendLine("InRestraints".Translate());
			}
			stringBuilder.Append(base.InspectStringPartsFromComps());
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06004B78 RID: 19320 RVA: 0x00276130 File Offset: 0x00274530
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (this.IsColonistPlayerControlled)
			{
				foreach (Gizmo c in this.<GetGizmos>__BaseCallProxy1())
				{
					yield return c;
				}
				if (this.drafter != null)
				{
					foreach (Gizmo c2 in this.drafter.GetGizmos())
					{
						yield return c2;
					}
				}
				foreach (Gizmo attack in PawnAttackGizmoUtility.GetAttackGizmos(this))
				{
					yield return attack;
				}
			}
			if (this.equipment != null)
			{
				foreach (Gizmo g in this.equipment.GetGizmos())
				{
					yield return g;
				}
			}
			if (this.IsColonistPlayerControlled)
			{
				if (this.apparel != null)
				{
					foreach (Gizmo g2 in this.apparel.GetGizmos())
					{
						yield return g2;
					}
				}
				if (this.playerSettings != null)
				{
					foreach (Gizmo g3 in this.playerSettings.GetGizmos())
					{
						yield return g3;
					}
				}
			}
			foreach (Gizmo g4 in this.mindState.GetGizmos())
			{
				yield return g4;
			}
			yield break;
		}

		// Token: 0x06004B79 RID: 19321 RVA: 0x0027615C File Offset: 0x0027455C
		public virtual IEnumerable<FloatMenuOption> GetExtraFloatMenuOptionsFor(IntVec3 sq)
		{
			yield break;
		}

		// Token: 0x06004B7A RID: 19322 RVA: 0x00276180 File Offset: 0x00274580
		public override TipSignal GetTooltip()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.LabelCap);
			string text = "";
			if (this.gender != Gender.None)
			{
				text = this.gender.GetLabel();
			}
			if (!this.LabelCap.EqualsIgnoreCase(this.KindLabel))
			{
				if (text != "")
				{
					text += " ";
				}
				text += this.KindLabel;
			}
			if (text != "")
			{
				stringBuilder.Append(" (" + text + ")");
			}
			stringBuilder.AppendLine();
			if (this.equipment != null && this.equipment.Primary != null)
			{
				stringBuilder.AppendLine(this.equipment.Primary.LabelCap);
			}
			stringBuilder.AppendLine(HealthUtility.GetGeneralConditionLabel(this, false));
			return new TipSignal(stringBuilder.ToString().TrimEndNewlines(), this.thingIDNumber * 152317, TooltipPriority.Pawn);
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x06004B7B RID: 19323 RVA: 0x00276294 File Offset: 0x00274694
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				foreach (StatDrawEntry s in this.<get_SpecialDisplayStats>__BaseCallProxy2())
				{
					yield return s;
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "BodySize".Translate(), this.BodySize.ToString("F2"), 0, "");
				yield break;
			}
		}

		// Token: 0x06004B7C RID: 19324 RVA: 0x002762C0 File Offset: 0x002746C0
		public bool CurrentlyUsableForBills()
		{
			bool result;
			if (!this.InBed() && (this.RaceProps.FleshType.requiresBedForSurgery || !this.Downed))
			{
				JobFailReason.Is(Pawn.NotSurgeryReadyTrans, null);
				result = false;
			}
			else if (!this.InteractionCell.IsValid)
			{
				JobFailReason.Is(Pawn.CannotReachTrans, null);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06004B7D RID: 19325 RVA: 0x0027633C File Offset: 0x0027473C
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x00276358 File Offset: 0x00274758
		public bool AnythingToStrip()
		{
			return (this.equipment != null && this.equipment.HasAnything()) || (this.apparel != null && this.apparel.WornApparelCount > 0) || (this.inventory != null && this.inventory.innerContainer.Count > 0);
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x002763C8 File Offset: 0x002747C8
		public void Strip()
		{
			Caravan caravan = this.GetCaravan();
			if (caravan != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(this, caravan.PawnsListForReading, null);
				if (this.apparel != null)
				{
					CaravanInventoryUtility.MoveAllApparelToSomeonesInventory(this, caravan.PawnsListForReading);
				}
				if (this.equipment != null)
				{
					CaravanInventoryUtility.MoveAllEquipmentToSomeonesInventory(this, caravan.PawnsListForReading);
				}
			}
			else
			{
				IntVec3 pos = (this.Corpse == null) ? base.PositionHeld : this.Corpse.PositionHeld;
				if (this.equipment != null)
				{
					this.equipment.DropAllEquipment(pos, false);
				}
				if (this.apparel != null)
				{
					this.apparel.DropAll(pos, false);
				}
				if (this.inventory != null)
				{
					this.inventory.DropAllNearPawn(pos, false, false);
				}
			}
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x00276494 File Offset: 0x00274894
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x002764B5 File Offset: 0x002748B5
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x002764C6 File Offset: 0x002748C6
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x002764D8 File Offset: 0x002748D8
		public void HearClamor(Thing source, ClamorDef type)
		{
			if (!this.Dead && !this.Downed)
			{
				if (type == ClamorDefOf.Movement)
				{
					Pawn pawn = source as Pawn;
					if (pawn != null)
					{
						this.CheckForDisturbedSleep(pawn);
					}
				}
				if (type == ClamorDefOf.Harm)
				{
					if (base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction == source.Faction && this.HostFaction == null)
					{
						this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
						if (this.CurJob != null)
						{
							this.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
						}
					}
				}
				if (type == ClamorDefOf.Construction)
				{
					if (base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction != source.Faction && this.HostFaction == null)
					{
						this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
						if (this.CurJob != null)
						{
							this.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
						}
					}
				}
				if (type == ClamorDefOf.Impact)
				{
					this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
					if (this.CurJob != null && !this.Awake())
					{
						this.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
					}
				}
			}
		}

		// Token: 0x06004B84 RID: 19332 RVA: 0x00276660 File Offset: 0x00274A60
		private void CheckForDisturbedSleep(Pawn source)
		{
			if (this.needs.mood != null)
			{
				if (!this.Awake())
				{
					if (base.Faction == Faction.OfPlayer)
					{
						if (Find.TickManager.TicksGame >= this.lastSleepDisturbedTick + 300)
						{
							if (source != null)
							{
								if (LovePartnerRelationUtility.LovePartnerRelationExists(this, source))
								{
									return;
								}
								if (source.RaceProps.petness > 0f)
								{
									return;
								}
								if (source.relations != null)
								{
									if (source.relations.DirectRelations.Any((DirectPawnRelation dr) => dr.def == PawnRelationDefOf.Bond))
									{
										return;
									}
								}
							}
							this.lastSleepDisturbedTick = Find.TickManager.TicksGame;
							this.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleepDisturbed, null);
						}
					}
				}
			}
		}

		// Token: 0x06004B85 RID: 19333 RVA: 0x00276770 File Offset: 0x00274B70
		public bool CheckAcceptArrest(Pawn arrester)
		{
			bool result;
			if (this.health.Downed)
			{
				result = true;
			}
			else if (this.story != null && this.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				result = true;
			}
			else
			{
				if (base.Faction != null && base.Faction != arrester.factionInt)
				{
					base.Faction.Notify_MemberCaptured(this, arrester.Faction);
				}
				float num = (!this.IsWildMan()) ? 0.5f : 0.2f;
				if (Rand.Value < num)
				{
					result = true;
				}
				else
				{
					Messages.Message("MessageRefusedArrest".Translate(new object[]
					{
						this.LabelShort
					}), this, MessageTypeDefOf.ThreatSmall, true);
					if (base.Faction == null || !arrester.HostileTo(this))
					{
						this.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, false, false, null, false);
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x00276878 File Offset: 0x00274C78
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			bool result;
			if (!base.Spawned)
			{
				result = true;
			}
			else if (!this.InMentalState && this.GetTraderCaravanRole() == TraderCaravanRole.Carrier && !(this.jobs.curDriver is JobDriver_AttackMelee))
			{
				result = true;
			}
			else if (this.mindState.duty != null && this.mindState.duty.def.threatDisabled)
			{
				result = true;
			}
			else if (!this.mindState.Active)
			{
				result = true;
			}
			else
			{
				if (this.Downed)
				{
					if (disabledFor == null)
					{
						return true;
					}
					Pawn pawn = disabledFor.Thing as Pawn;
					if (pawn == null || pawn.mindState == null || pawn.mindState.duty == null || !pawn.mindState.duty.attackDownedIfStarving || !pawn.Starving())
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06004B87 RID: 19335 RVA: 0x00276990 File Offset: 0x00274D90
		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			bool result;
			if (this.InAggroMentalState || (base.Faction.HostileTo(Faction.OfPlayer) && this.HostFaction == null && !this.Downed && !this.InMentalState))
			{
				reason = "Enemies".Translate();
				result = true;
			}
			else
			{
				reason = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06004B88 RID: 19336 RVA: 0x00276A00 File Offset: 0x00274E00
		public void ChangeKind(PawnKindDef newKindDef)
		{
			if (this.kindDef != newKindDef)
			{
				if (this.kindDef == PawnKindDefOf.WildMan && base.Spawned)
				{
					base.Map.reachability.ClearCache();
				}
				this.kindDef = newKindDef;
				if (this.kindDef == PawnKindDefOf.WildMan)
				{
					this.mindState.wildManEverReachedOutside = false;
					if (base.Spawned)
					{
						base.Map.reachability.ClearCache();
					}
				}
			}
		}
	}
}
