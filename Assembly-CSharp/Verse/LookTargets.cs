using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000EF2 RID: 3826
	public class LookTargets : IExposable
	{
		// Token: 0x06005B26 RID: 23334 RVA: 0x002E7D57 File Offset: 0x002E6157
		public LookTargets()
		{
			this.targets = new List<GlobalTargetInfo>();
		}

		// Token: 0x06005B27 RID: 23335 RVA: 0x002E7D6B File Offset: 0x002E616B
		public LookTargets(Thing t)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(t);
		}

		// Token: 0x06005B28 RID: 23336 RVA: 0x002E7D90 File Offset: 0x002E6190
		public LookTargets(WorldObject o)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(o);
		}

		// Token: 0x06005B29 RID: 23337 RVA: 0x002E7DB5 File Offset: 0x002E61B5
		public LookTargets(IntVec3 c, Map map)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(c, map, false));
		}

		// Token: 0x06005B2A RID: 23338 RVA: 0x002E7DDC File Offset: 0x002E61DC
		public LookTargets(int tile)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(tile));
		}

		// Token: 0x06005B2B RID: 23339 RVA: 0x002E7E01 File Offset: 0x002E6201
		public LookTargets(IEnumerable<GlobalTargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				this.targets.AddRange(targets);
			}
		}

		// Token: 0x06005B2C RID: 23340 RVA: 0x002E7E28 File Offset: 0x002E6228
		public LookTargets(params GlobalTargetInfo[] targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					this.targets.Add(targets[i]);
				}
			}
		}

		// Token: 0x06005B2D RID: 23341 RVA: 0x002E7E7C File Offset: 0x002E627C
		public LookTargets(IEnumerable<TargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				IList<TargetInfo> list = targets as IList<TargetInfo>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						this.targets.Add(list[i]);
					}
				}
				else
				{
					foreach (TargetInfo target in targets)
					{
						this.targets.Add(target);
					}
				}
			}
		}

		// Token: 0x06005B2E RID: 23342 RVA: 0x002E7F40 File Offset: 0x002E6340
		public LookTargets(params TargetInfo[] targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					this.targets.Add(targets[i]);
				}
			}
		}

		// Token: 0x06005B2F RID: 23343 RVA: 0x002E7F99 File Offset: 0x002E6399
		public LookTargets(IEnumerable<Thing> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Thing>(targets);
		}

		// Token: 0x06005B30 RID: 23344 RVA: 0x002E7FB4 File Offset: 0x002E63B4
		public LookTargets(IEnumerable<ThingWithComps> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<ThingWithComps>(targets);
		}

		// Token: 0x06005B31 RID: 23345 RVA: 0x002E7FCF File Offset: 0x002E63CF
		public LookTargets(IEnumerable<Pawn> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Pawn>(targets);
		}

		// Token: 0x06005B32 RID: 23346 RVA: 0x002E7FEA File Offset: 0x002E63EA
		public LookTargets(IEnumerable<Building> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Building>(targets);
		}

		// Token: 0x06005B33 RID: 23347 RVA: 0x002E8005 File Offset: 0x002E6405
		public LookTargets(IEnumerable<Plant> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Plant>(targets);
		}

		// Token: 0x06005B34 RID: 23348 RVA: 0x002E8020 File Offset: 0x002E6420
		public LookTargets(IEnumerable<WorldObject> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<WorldObject>(targets);
		}

		// Token: 0x06005B35 RID: 23349 RVA: 0x002E803B File Offset: 0x002E643B
		public LookTargets(IEnumerable<Caravan> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<Caravan>(targets);
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x06005B36 RID: 23350 RVA: 0x002E8058 File Offset: 0x002E6458
		public static LookTargets Invalid
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06005B37 RID: 23351 RVA: 0x002E8070 File Offset: 0x002E6470
		public bool IsValid
		{
			get
			{
				return this.PrimaryTarget.IsValid;
			}
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06005B38 RID: 23352 RVA: 0x002E8094 File Offset: 0x002E6494
		public bool Any
		{
			get
			{
				return this.targets.Count != 0;
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06005B39 RID: 23353 RVA: 0x002E80BC File Offset: 0x002E64BC
		public GlobalTargetInfo PrimaryTarget
		{
			get
			{
				for (int i = 0; i < this.targets.Count; i++)
				{
					if (this.targets[i].IsValid)
					{
						return this.targets[i];
					}
				}
				if (this.targets.Count != 0)
				{
					return this.targets[0];
				}
				return GlobalTargetInfo.Invalid;
			}
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x002E8141 File Offset: 0x002E6541
		public void ExposeData()
		{
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, new object[0]);
		}

		// Token: 0x06005B3B RID: 23355 RVA: 0x002E815C File Offset: 0x002E655C
		public static implicit operator LookTargets(Thing t)
		{
			return new LookTargets(t);
		}

		// Token: 0x06005B3C RID: 23356 RVA: 0x002E8178 File Offset: 0x002E6578
		public static implicit operator LookTargets(WorldObject o)
		{
			return new LookTargets(o);
		}

		// Token: 0x06005B3D RID: 23357 RVA: 0x002E8194 File Offset: 0x002E6594
		public static implicit operator LookTargets(TargetInfo target)
		{
			return new LookTargets
			{
				targets = new List<GlobalTargetInfo>(),
				targets = 
				{
					target
				}
			};
		}

		// Token: 0x06005B3E RID: 23358 RVA: 0x002E81CC File Offset: 0x002E65CC
		public static implicit operator LookTargets(List<TargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B3F RID: 23359 RVA: 0x002E81E8 File Offset: 0x002E65E8
		public static implicit operator LookTargets(GlobalTargetInfo target)
		{
			return new LookTargets
			{
				targets = new List<GlobalTargetInfo>(),
				targets = 
				{
					target
				}
			};
		}

		// Token: 0x06005B40 RID: 23360 RVA: 0x002E821C File Offset: 0x002E661C
		public static implicit operator LookTargets(List<GlobalTargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B41 RID: 23361 RVA: 0x002E8238 File Offset: 0x002E6638
		public static implicit operator LookTargets(List<Thing> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B42 RID: 23362 RVA: 0x002E8254 File Offset: 0x002E6654
		public static implicit operator LookTargets(List<ThingWithComps> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B43 RID: 23363 RVA: 0x002E8270 File Offset: 0x002E6670
		public static implicit operator LookTargets(List<Pawn> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B44 RID: 23364 RVA: 0x002E828C File Offset: 0x002E668C
		public static implicit operator LookTargets(List<Building> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B45 RID: 23365 RVA: 0x002E82A8 File Offset: 0x002E66A8
		public static implicit operator LookTargets(List<Plant> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B46 RID: 23366 RVA: 0x002E82C4 File Offset: 0x002E66C4
		public static implicit operator LookTargets(List<WorldObject> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B47 RID: 23367 RVA: 0x002E82E0 File Offset: 0x002E66E0
		public static implicit operator LookTargets(List<Caravan> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B48 RID: 23368 RVA: 0x002E82FC File Offset: 0x002E66FC
		public static bool SameTargets(LookTargets a, LookTargets b)
		{
			bool result;
			if (a == null)
			{
				result = (b == null || !b.Any);
			}
			else if (b == null)
			{
				result = (a == null || !a.Any);
			}
			else if (a.targets.Count != b.targets.Count)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < a.targets.Count; i++)
				{
					if (a.targets[i] != b.targets[i])
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06005B49 RID: 23369 RVA: 0x002E83B8 File Offset: 0x002E67B8
		public void Highlight(bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				TargetHighlighter.Highlight(this.targets[i], arrow, colonistBar, circleOverlay);
			}
		}

		// Token: 0x06005B4A RID: 23370 RVA: 0x002E83F8 File Offset: 0x002E67F8
		private void AppendThingTargets<T>(IEnumerable<T> things) where T : Thing
		{
			if (things != null)
			{
				IList<T> list = things as IList<T>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						this.targets.Add(list[i]);
					}
				}
				else
				{
					foreach (T t in things)
					{
						this.targets.Add(t);
					}
				}
			}
		}

		// Token: 0x06005B4B RID: 23371 RVA: 0x002E84B8 File Offset: 0x002E68B8
		private void AppendWorldObjectTargets<T>(IEnumerable<T> worldObjects) where T : WorldObject
		{
			if (worldObjects != null)
			{
				IList<T> list = worldObjects as IList<T>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						this.targets.Add(list[i]);
					}
				}
				else
				{
					foreach (T t in worldObjects)
					{
						this.targets.Add(t);
					}
				}
			}
		}

		// Token: 0x06005B4C RID: 23372 RVA: 0x002E8578 File Offset: 0x002E6978
		public void Notify_MapRemoved(Map map)
		{
			this.targets.RemoveAll((GlobalTargetInfo t) => t.Map == map);
		}

		// Token: 0x04003C94 RID: 15508
		public List<GlobalTargetInfo> targets;
	}
}
