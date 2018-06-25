using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI.Group;

namespace Verse.AI
{
	public class Job : IExposable, ILoadReferenceable
	{
		public JobDef def;

		public LocalTargetInfo targetA = LocalTargetInfo.Invalid;

		public LocalTargetInfo targetB = LocalTargetInfo.Invalid;

		public LocalTargetInfo targetC = LocalTargetInfo.Invalid;

		public List<LocalTargetInfo> targetQueueA = null;

		public List<LocalTargetInfo> targetQueueB = null;

		public int count = -1;

		public List<int> countQueue = null;

		public int loadID;

		public int startTick = -1;

		public int expiryInterval = -1;

		public bool checkOverrideOnExpire = false;

		public bool playerForced = false;

		public List<ThingCountClass> placedThings = null;

		public int maxNumMeleeAttacks = int.MaxValue;

		public int maxNumStaticAttacks = int.MaxValue;

		public LocomotionUrgency locomotionUrgency = LocomotionUrgency.Jog;

		public HaulMode haulMode = HaulMode.Undefined;

		public Bill bill = null;

		public ICommunicable commTarget = null;

		public ThingDef plantDefToSow = null;

		public Verb verbToUse;

		public bool haulOpportunisticDuplicates = false;

		public bool exitMapOnArrival = false;

		public bool failIfCantJoinOrCreateCaravan = false;

		public bool killIncappedTarget = false;

		public bool ignoreForbidden = false;

		public bool ignoreDesignations = false;

		public bool canBash = false;

		public bool haulDroppedApparel = false;

		public bool restUntilHealed = false;

		public bool ignoreJoyTimeAssignment = false;

		public bool overeat = false;

		public bool attackDoorIfTargetLost = false;

		public int takeExtraIngestibles = 0;

		public bool expireRequiresEnemiesNearby = false;

		public Lord lord = null;

		public bool collideWithPawns;

		public bool forceSleep;

		public InteractionDef interaction;

		public bool endIfCantShootTargetFromCurPos;

		public bool checkEncumbrance;

		public float followRadius;

		public bool endAfterTendedOnce;

		private JobDriver cachedDriver;

		public Job()
		{
		}

		public Job(JobDef def) : this(def, null)
		{
		}

		public Job(JobDef def, LocalTargetInfo targetA) : this(def, targetA, null)
		{
		}

		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB, LocalTargetInfo targetC)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.targetC = targetC;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		public Job(JobDef def, LocalTargetInfo targetA, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.targetA = targetA;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		public Job(JobDef def, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		public RecipeDef RecipeDef
		{
			get
			{
				return (this.bill == null) ? null : this.bill.recipe;
			}
		}

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

		public void AddQueuedTarget(TargetIndex ind, LocalTargetInfo target)
		{
			this.GetTargetQueue(ind).Add(target);
		}

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

		public JobDriver MakeDriver(Pawn driverPawn)
		{
			JobDriver jobDriver = (JobDriver)Activator.CreateInstance(this.def.driverClass);
			jobDriver.pawn = driverPawn;
			jobDriver.job = this;
			return jobDriver;
		}

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

		public bool TryMakePreToilReservations(Pawn driverPawn)
		{
			return this.GetCachedDriver(driverPawn).TryMakePreToilReservations();
		}

		public string GetReport(Pawn driverPawn)
		{
			return this.GetCachedDriver(driverPawn).GetReport();
		}

		public LocalTargetInfo GetDestination(Pawn driverPawn)
		{
			return this.targetA;
		}

		public bool CanBeginNow(Pawn pawn, bool whileLyingDown = false)
		{
			if (pawn.Downed)
			{
				whileLyingDown = true;
			}
			return !whileLyingDown || this.GetCachedDriver(pawn).CanBeginNowWhileLyingDown();
		}

		public bool JobIsSameAs(Job other)
		{
			return other != null && (this == other || (this.def == other.def && !(this.targetA != other.targetA) && !(this.targetB != other.targetB) && this.verbToUse == other.verbToUse && !(this.targetC != other.targetC) && this.commTarget == other.commTarget && this.bill == other.bill));
		}

		public bool AnyTargetIs(LocalTargetInfo target)
		{
			return target.IsValid && (this.targetA == target || this.targetB == target || this.targetC == target || (this.targetQueueA != null && this.targetQueueA.Contains(target)) || (this.targetQueueB != null && this.targetQueueB.Contains(target)));
		}

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

		public string GetUniqueLoadID()
		{
			return "Job_" + this.loadID;
		}
	}
}
