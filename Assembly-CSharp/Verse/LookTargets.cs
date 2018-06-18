using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000EF1 RID: 3825
	public class LookTargets : IExposable
	{
		// Token: 0x06005B24 RID: 23332 RVA: 0x002E7E2F File Offset: 0x002E622F
		public LookTargets()
		{
			this.targets = new List<GlobalTargetInfo>();
		}

		// Token: 0x06005B25 RID: 23333 RVA: 0x002E7E43 File Offset: 0x002E6243
		public LookTargets(Thing t)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(t);
		}

		// Token: 0x06005B26 RID: 23334 RVA: 0x002E7E68 File Offset: 0x002E6268
		public LookTargets(WorldObject o)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(o);
		}

		// Token: 0x06005B27 RID: 23335 RVA: 0x002E7E8D File Offset: 0x002E628D
		public LookTargets(IntVec3 c, Map map)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(c, map, false));
		}

		// Token: 0x06005B28 RID: 23336 RVA: 0x002E7EB4 File Offset: 0x002E62B4
		public LookTargets(int tile)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(tile));
		}

		// Token: 0x06005B29 RID: 23337 RVA: 0x002E7ED9 File Offset: 0x002E62D9
		public LookTargets(IEnumerable<GlobalTargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				this.targets.AddRange(targets);
			}
		}

		// Token: 0x06005B2A RID: 23338 RVA: 0x002E7F00 File Offset: 0x002E6300
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

		// Token: 0x06005B2B RID: 23339 RVA: 0x002E7F54 File Offset: 0x002E6354
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

		// Token: 0x06005B2C RID: 23340 RVA: 0x002E8018 File Offset: 0x002E6418
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

		// Token: 0x06005B2D RID: 23341 RVA: 0x002E8071 File Offset: 0x002E6471
		public LookTargets(IEnumerable<Thing> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Thing>(targets);
		}

		// Token: 0x06005B2E RID: 23342 RVA: 0x002E808C File Offset: 0x002E648C
		public LookTargets(IEnumerable<ThingWithComps> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<ThingWithComps>(targets);
		}

		// Token: 0x06005B2F RID: 23343 RVA: 0x002E80A7 File Offset: 0x002E64A7
		public LookTargets(IEnumerable<Pawn> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Pawn>(targets);
		}

		// Token: 0x06005B30 RID: 23344 RVA: 0x002E80C2 File Offset: 0x002E64C2
		public LookTargets(IEnumerable<Building> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Building>(targets);
		}

		// Token: 0x06005B31 RID: 23345 RVA: 0x002E80DD File Offset: 0x002E64DD
		public LookTargets(IEnumerable<Plant> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Plant>(targets);
		}

		// Token: 0x06005B32 RID: 23346 RVA: 0x002E80F8 File Offset: 0x002E64F8
		public LookTargets(IEnumerable<WorldObject> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<WorldObject>(targets);
		}

		// Token: 0x06005B33 RID: 23347 RVA: 0x002E8113 File Offset: 0x002E6513
		public LookTargets(IEnumerable<Caravan> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<Caravan>(targets);
		}

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06005B34 RID: 23348 RVA: 0x002E8130 File Offset: 0x002E6530
		public static LookTargets Invalid
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x06005B35 RID: 23349 RVA: 0x002E8148 File Offset: 0x002E6548
		public bool IsValid
		{
			get
			{
				return this.PrimaryTarget.IsValid;
			}
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06005B36 RID: 23350 RVA: 0x002E816C File Offset: 0x002E656C
		public bool Any
		{
			get
			{
				return this.targets.Count != 0;
			}
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06005B37 RID: 23351 RVA: 0x002E8194 File Offset: 0x002E6594
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

		// Token: 0x06005B38 RID: 23352 RVA: 0x002E8219 File Offset: 0x002E6619
		public void ExposeData()
		{
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, new object[0]);
		}

		// Token: 0x06005B39 RID: 23353 RVA: 0x002E8234 File Offset: 0x002E6634
		public static implicit operator LookTargets(Thing t)
		{
			return new LookTargets(t);
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x002E8250 File Offset: 0x002E6650
		public static implicit operator LookTargets(WorldObject o)
		{
			return new LookTargets(o);
		}

		// Token: 0x06005B3B RID: 23355 RVA: 0x002E826C File Offset: 0x002E666C
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

		// Token: 0x06005B3C RID: 23356 RVA: 0x002E82A4 File Offset: 0x002E66A4
		public static implicit operator LookTargets(List<TargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B3D RID: 23357 RVA: 0x002E82C0 File Offset: 0x002E66C0
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

		// Token: 0x06005B3E RID: 23358 RVA: 0x002E82F4 File Offset: 0x002E66F4
		public static implicit operator LookTargets(List<GlobalTargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B3F RID: 23359 RVA: 0x002E8310 File Offset: 0x002E6710
		public static implicit operator LookTargets(List<Thing> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B40 RID: 23360 RVA: 0x002E832C File Offset: 0x002E672C
		public static implicit operator LookTargets(List<ThingWithComps> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B41 RID: 23361 RVA: 0x002E8348 File Offset: 0x002E6748
		public static implicit operator LookTargets(List<Pawn> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B42 RID: 23362 RVA: 0x002E8364 File Offset: 0x002E6764
		public static implicit operator LookTargets(List<Building> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B43 RID: 23363 RVA: 0x002E8380 File Offset: 0x002E6780
		public static implicit operator LookTargets(List<Plant> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B44 RID: 23364 RVA: 0x002E839C File Offset: 0x002E679C
		public static implicit operator LookTargets(List<WorldObject> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B45 RID: 23365 RVA: 0x002E83B8 File Offset: 0x002E67B8
		public static implicit operator LookTargets(List<Caravan> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B46 RID: 23366 RVA: 0x002E83D4 File Offset: 0x002E67D4
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

		// Token: 0x06005B47 RID: 23367 RVA: 0x002E8490 File Offset: 0x002E6890
		public void Highlight(bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				TargetHighlighter.Highlight(this.targets[i], arrow, colonistBar, circleOverlay);
			}
		}

		// Token: 0x06005B48 RID: 23368 RVA: 0x002E84D0 File Offset: 0x002E68D0
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

		// Token: 0x06005B49 RID: 23369 RVA: 0x002E8590 File Offset: 0x002E6990
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

		// Token: 0x06005B4A RID: 23370 RVA: 0x002E8650 File Offset: 0x002E6A50
		public void Notify_MapRemoved(Map map)
		{
			this.targets.RemoveAll((GlobalTargetInfo t) => t.Map == map);
		}

		// Token: 0x04003C93 RID: 15507
		public List<GlobalTargetInfo> targets;
	}
}
