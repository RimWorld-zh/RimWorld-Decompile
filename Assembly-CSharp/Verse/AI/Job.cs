using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000A31 RID: 2609
	public class Job : IExposable, ILoadReferenceable
	{
		// Token: 0x040024D3 RID: 9427
		public JobDef def;

		// Token: 0x040024D4 RID: 9428
		public LocalTargetInfo targetA = LocalTargetInfo.Invalid;

		// Token: 0x040024D5 RID: 9429
		public LocalTargetInfo targetB = LocalTargetInfo.Invalid;

		// Token: 0x040024D6 RID: 9430
		public LocalTargetInfo targetC = LocalTargetInfo.Invalid;

		// Token: 0x040024D7 RID: 9431
		public List<LocalTargetInfo> targetQueueA = null;

		// Token: 0x040024D8 RID: 9432
		public List<LocalTargetInfo> targetQueueB = null;

		// Token: 0x040024D9 RID: 9433
		public int count = -1;

		// Token: 0x040024DA RID: 9434
		public List<int> countQueue = null;

		// Token: 0x040024DB RID: 9435
		public int loadID;

		// Token: 0x040024DC RID: 9436
		public int startTick = -1;

		// Token: 0x040024DD RID: 9437
		public int expiryInterval = -1;

		// Token: 0x040024DE RID: 9438
		public bool checkOverrideOnExpire = false;

		// Token: 0x040024DF RID: 9439
		public bool playerForced = false;

		// Token: 0x040024E0 RID: 9440
		public List<ThingCountClass> placedThings = null;

		// Token: 0x040024E1 RID: 9441
		public int maxNumMeleeAttacks = int.MaxValue;

		// Token: 0x040024E2 RID: 9442
		public int maxNumStaticAttacks = int.MaxValue;

		// Token: 0x040024E3 RID: 9443
		public LocomotionUrgency locomotionUrgency = LocomotionUrgency.Jog;

		// Token: 0x040024E4 RID: 9444
		public HaulMode haulMode = HaulMode.Undefined;

		// Token: 0x040024E5 RID: 9445
		public Bill bill = null;

		// Token: 0x040024E6 RID: 9446
		public ICommunicable commTarget = null;

		// Token: 0x040024E7 RID: 9447
		public ThingDef plantDefToSow = null;

		// Token: 0x040024E8 RID: 9448
		public Verb verbToUse;

		// Token: 0x040024E9 RID: 9449
		public bool haulOpportunisticDuplicates = false;

		// Token: 0x040024EA RID: 9450
		public bool exitMapOnArrival = false;

		// Token: 0x040024EB RID: 9451
		public bool failIfCantJoinOrCreateCaravan = false;

		// Token: 0x040024EC RID: 9452
		public bool killIncappedTarget = false;

		// Token: 0x040024ED RID: 9453
		public bool ignoreForbidden = false;

		// Token: 0x040024EE RID: 9454
		public bool ignoreDesignations = false;

		// Token: 0x040024EF RID: 9455
		public bool canBash = false;

		// Token: 0x040024F0 RID: 9456
		public bool haulDroppedApparel = false;

		// Token: 0x040024F1 RID: 9457
		public bool restUntilHealed = false;

		// Token: 0x040024F2 RID: 9458
		public bool ignoreJoyTimeAssignment = false;

		// Token: 0x040024F3 RID: 9459
		public bool overeat = false;

		// Token: 0x040024F4 RID: 9460
		public bool attackDoorIfTargetLost = false;

		// Token: 0x040024F5 RID: 9461
		public int takeExtraIngestibles = 0;

		// Token: 0x040024F6 RID: 9462
		public bool expireRequiresEnemiesNearby = false;

		// Token: 0x040024F7 RID: 9463
		public Lord lord = null;

		// Token: 0x040024F8 RID: 9464
		public bool collideWithPawns;

		// Token: 0x040024F9 RID: 9465
		public bool forceSleep;

		// Token: 0x040024FA RID: 9466
		public InteractionDef interaction;

		// Token: 0x040024FB RID: 9467
		public bool endIfCantShootTargetFromCurPos;

		// Token: 0x040024FC RID: 9468
		public bool checkEncumbrance;

		// Token: 0x040024FD RID: 9469
		public float followRadius;

		// Token: 0x040024FE RID: 9470
		public bool endAfterTendedOnce;

		// Token: 0x040024FF RID: 9471
		private JobDriver cachedDriver;

		// Token: 0x060039DC RID: 14812 RVA: 0x001E93C8 File Offset: 0x001E77C8
		public Job()
		{
		}

		// Token: 0x060039DD RID: 14813 RVA: 0x001E94DE File Offset: 0x001E78DE
		public Job(JobDef def) : this(def, null)
		{
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x001E94EE File Offset: 0x001E78EE
		public Job(JobDef def, LocalTargetInfo targetA) : this(def, targetA, null)
		{
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x001E9500 File Offset: 0x001E7900
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x001E963C File Offset: 0x001E7A3C
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB, LocalTargetInfo targetC)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.targetC = targetC;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x001E9780 File Offset: 0x001E7B80
		public Job(JobDef def, LocalTargetInfo targetA, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.targetA = targetA;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x001E98C4 File Offset: 0x001E7CC4
		public Job(JobDef def, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x060039E3 RID: 14819 RVA: 0x001E9A00 File Offset: 0x001E7E00
		public RecipeDef RecipeDef
		{
			get
			{
				return (this.bill == null) ? null : this.bill.recipe;
			}
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x001E9A34 File Offset: 0x001E7E34
		public LocalTargetInfo GetTarget(TargetIndex ind)
		{
			LocalTargetInfo result;
			switch (ind)
			{
			case TargetIndex.A:
				result = this.targetA;
				break;
			case TargetIndex.B:
				result = this.targetB;
				break;
			case TargetIndex.C:
				result = this.targetC;
				break;
			default:
				throw new ArgumentException();
			}
			return result;
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x001E9A88 File Offset: 0x001E7E88
		public List<LocalTargetInfo> GetTargetQueue(TargetIndex ind)
		{
			List<LocalTargetInfo> result;
			if (ind != TargetIndex.A)
			{
				if (ind != TargetIndex.B)
				{
					throw new ArgumentException();
				}
				if (this.targetQueueB == null)
				{
					this.targetQueueB = new List<LocalTargetInfo>();
				}
				result = this.targetQueueB;
			}
			else
			{
				if (this.targetQueueA == null)
				{
					this.targetQueueA = new List<LocalTargetInfo>();
				}
				result = this.targetQueueA;
			}
			return result;
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x001E9AF4 File Offset: 0x001E7EF4
		public void SetTarget(TargetIndex ind, LocalTargetInfo pack)
		{
			if (ind != TargetIndex.A)
			{
				if (ind != TargetIndex.B)
				{
					if (ind != TargetIndex.C)
					{
						throw new ArgumentException();
					}
					this.targetC = pack;
				}
				else
				{
					this.targetB = pack;
				}
			}
			else
			{
				this.targetA = pack;
			}
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x001E9B46 File Offset: 0x001E7F46
		public void AddQueuedTarget(TargetIndex ind, LocalTargetInfo target)
		{
			this.GetTargetQueue(ind).Add(target);
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x001E9B58 File Offset: 0x001E7F58
		public void ExposeData()
		{
			ILoadReferenceable loadReferenceable = (ILoadReferenceable)this.commTarget;
			Scribe_References.Look<ILoadReferenceable>(ref loadReferenceable, "commTarget", false);
			this.commTarget = (ICommunicable)loadReferenceable;
			Scribe_References.Look<Verb>(ref this.verbToUse, "verbToUse", false);
			Scribe_References.Look<Bill>(ref this.bill, "bill", false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_Defs.Look<JobDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_TargetInfo.Look(ref this.targetA, "targetA");
			Scribe_TargetInfo.Look(ref this.targetB, "targetB");
			Scribe_TargetInfo.Look(ref this.targetC, "targetC");
			Scribe_Collections.Look<LocalTargetInfo>(ref this.targetQueueA, "targetQueueA", LookMode.Undefined, new object[0]);
			Scribe_Collections.Look<LocalTargetInfo>(ref this.targetQueueB, "targetQueueB", LookMode.Undefined, new object[0]);
			Scribe_Values.Look<int>(ref this.count, "count", -1, false);
			Scribe_Collections.Look<int>(ref this.countQueue, "countQueue", LookMode.Undefined, new object[0]);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", -1, false);
			Scribe_Values.Look<int>(ref this.expiryInterval, "expiryInterval", -1, false);
			Scribe_Values.Look<bool>(ref this.checkOverrideOnExpire, "checkOverrideOnExpire", false, false);
			Scribe_Values.Look<bool>(ref this.playerForced, "playerForced", false, false);
			Scribe_Collections.Look<ThingCountClass>(ref this.placedThings, "placedThings", LookMode.Undefined, new object[0]);
			Scribe_Values.Look<int>(ref this.maxNumMeleeAttacks, "maxNumMeleeAttacks", int.MaxValue, false);
			Scribe_Values.Look<int>(ref this.maxNumStaticAttacks, "maxNumStaticAttacks", int.MaxValue, false);
			Scribe_Values.Look<bool>(ref this.exitMapOnArrival, "exitMapOnArrival", false, false);
			Scribe_Values.Look<bool>(ref this.failIfCantJoinOrCreateCaravan, "failIfCantJoinOrCreateCaravan", false, false);
			Scribe_Values.Look<bool>(ref this.killIncappedTarget, "killIncappedTarget", false, false);
			Scribe_Values.Look<bool>(ref this.haulOpportunisticDuplicates, "haulOpportunisticDuplicates", false, false);
			Scribe_Values.Look<HaulMode>(ref this.haulMode, "haulMode", HaulMode.Undefined, false);
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToSow, "plantDefToSow");
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotionUrgency, "locomotionUrgency", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.ignoreDesignations, "ignoreDesignations", false, false);
			Scribe_Values.Look<bool>(ref this.canBash, "canBash", false, false);
			Scribe_Values.Look<bool>(ref this.haulDroppedApparel, "haulDroppedApparel", false, false);
			Scribe_Values.Look<bool>(ref this.restUntilHealed, "restUntilHealed", false, false);
			Scribe_Values.Look<bool>(ref this.ignoreJoyTimeAssignment, "ignoreJoyTimeAssignment", false, false);
			Scribe_Values.Look<bool>(ref this.overeat, "overeat", false, false);
			Scribe_Values.Look<bool>(ref this.attackDoorIfTargetLost, "attackDoorIfTargetLost", false, false);
			Scribe_Values.Look<int>(ref this.takeExtraIngestibles, "takeExtraIngestibles", 0, false);
			Scribe_Values.Look<bool>(ref this.expireRequiresEnemiesNearby, "expireRequiresEnemiesNearby", false, false);
			Scribe_Values.Look<bool>(ref this.collideWithPawns, "collideWithPawns", false, false);
			Scribe_Values.Look<bool>(ref this.forceSleep, "forceSleep", false, false);
			Scribe_Defs.Look<InteractionDef>(ref this.interaction, "interaction");
			Scribe_Values.Look<bool>(ref this.endIfCantShootTargetFromCurPos, "endIfCantShootTargetFromCurPos", false, false);
			Scribe_Values.Look<bool>(ref this.checkEncumbrance, "checkEncumbrance", false, false);
			Scribe_Values.Look<float>(ref this.followRadius, "followRadius", 0f, false);
			Scribe_Values.Look<bool>(ref this.endAfterTendedOnce, "endAfterTendedOnce", false, false);
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x001E9E90 File Offset: 0x001E8290
		public JobDriver MakeDriver(Pawn driverPawn)
		{
			JobDriver jobDriver = (JobDriver)Activator.CreateInstance(this.def.driverClass);
			jobDriver.pawn = driverPawn;
			jobDriver.job = this;
			return jobDriver;
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x001E9ECC File Offset: 0x001E82CC
		public JobDriver GetCachedDriver(Pawn driverPawn)
		{
			if (this.cachedDriver == null)
			{
				this.cachedDriver = this.MakeDriver(driverPawn);
			}
			if (this.cachedDriver.pawn != driverPawn)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to use the same driver for 2 pawns: ",
					this.cachedDriver.ToStringSafe<JobDriver>(),
					", first pawn= ",
					this.cachedDriver.pawn.ToStringSafe<Pawn>(),
					", second pawn=",
					driverPawn.ToStringSafe<Pawn>()
				}), false);
			}
			return this.cachedDriver;
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x001E9F68 File Offset: 0x001E8368
		public bool TryMakePreToilReservations(Pawn driverPawn)
		{
			return this.GetCachedDriver(driverPawn).TryMakePreToilReservations();
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x001E9F8C File Offset: 0x001E838C
		public string GetReport(Pawn driverPawn)
		{
			return this.GetCachedDriver(driverPawn).GetReport();
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x001E9FB0 File Offset: 0x001E83B0
		public LocalTargetInfo GetDestination(Pawn driverPawn)
		{
			return this.targetA;
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x001E9FCC File Offset: 0x001E83CC
		public bool CanBeginNow(Pawn pawn, bool whileLyingDown = false)
		{
			if (pawn.Downed)
			{
				whileLyingDown = true;
			}
			return !whileLyingDown || this.GetCachedDriver(pawn).CanBeginNowWhileLyingDown();
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x001EA008 File Offset: 0x001E8408
		public bool JobIsSameAs(Job other)
		{
			return other != null && (this == other || (this.def == other.def && !(this.targetA != other.targetA) && !(this.targetB != other.targetB) && this.verbToUse == other.verbToUse && !(this.targetC != other.targetC) && this.commTarget == other.commTarget && this.bill == other.bill));
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x001EA0C8 File Offset: 0x001E84C8
		public bool AnyTargetIs(LocalTargetInfo target)
		{
			return target.IsValid && (this.targetA == target || this.targetB == target || this.targetC == target || (this.targetQueueA != null && this.targetQueueA.Contains(target)) || (this.targetQueueB != null && this.targetQueueB.Contains(target)));
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x001EA15C File Offset: 0x001E855C
		public override string ToString()
		{
			string text = this.def.ToString() + " (" + this.GetUniqueLoadID() + ")";
			if (this.targetA.IsValid)
			{
				text = text + " A=" + this.targetA.ToString();
			}
			if (this.targetB.IsValid)
			{
				text = text + " B=" + this.targetB.ToString();
			}
			if (this.targetC.IsValid)
			{
				text = text + " C=" + this.targetC.ToString();
			}
			return text;
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x001EA21C File Offset: 0x001E861C
		public string GetUniqueLoadID()
		{
			return "Job_" + this.loadID;
		}
	}
}
