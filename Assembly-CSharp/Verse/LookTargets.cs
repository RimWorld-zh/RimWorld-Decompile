using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000EF3 RID: 3827
	public class LookTargets : IExposable
	{
		// Token: 0x04003CA6 RID: 15526
		public List<GlobalTargetInfo> targets;

		// Token: 0x06005B4F RID: 23375 RVA: 0x002E9F83 File Offset: 0x002E8383
		public LookTargets()
		{
			this.targets = new List<GlobalTargetInfo>();
		}

		// Token: 0x06005B50 RID: 23376 RVA: 0x002E9F97 File Offset: 0x002E8397
		public LookTargets(Thing t)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(t);
		}

		// Token: 0x06005B51 RID: 23377 RVA: 0x002E9FBC File Offset: 0x002E83BC
		public LookTargets(WorldObject o)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(o);
		}

		// Token: 0x06005B52 RID: 23378 RVA: 0x002E9FE1 File Offset: 0x002E83E1
		public LookTargets(IntVec3 c, Map map)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(c, map, false));
		}

		// Token: 0x06005B53 RID: 23379 RVA: 0x002EA008 File Offset: 0x002E8408
		public LookTargets(int tile)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(tile));
		}

		// Token: 0x06005B54 RID: 23380 RVA: 0x002EA02D File Offset: 0x002E842D
		public LookTargets(IEnumerable<GlobalTargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				this.targets.AddRange(targets);
			}
		}

		// Token: 0x06005B55 RID: 23381 RVA: 0x002EA054 File Offset: 0x002E8454
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

		// Token: 0x06005B56 RID: 23382 RVA: 0x002EA0A8 File Offset: 0x002E84A8
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

		// Token: 0x06005B57 RID: 23383 RVA: 0x002EA16C File Offset: 0x002E856C
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

		// Token: 0x06005B58 RID: 23384 RVA: 0x002EA1C5 File Offset: 0x002E85C5
		public LookTargets(IEnumerable<Thing> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Thing>(targets);
		}

		// Token: 0x06005B59 RID: 23385 RVA: 0x002EA1E0 File Offset: 0x002E85E0
		public LookTargets(IEnumerable<ThingWithComps> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<ThingWithComps>(targets);
		}

		// Token: 0x06005B5A RID: 23386 RVA: 0x002EA1FB File Offset: 0x002E85FB
		public LookTargets(IEnumerable<Pawn> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Pawn>(targets);
		}

		// Token: 0x06005B5B RID: 23387 RVA: 0x002EA216 File Offset: 0x002E8616
		public LookTargets(IEnumerable<Building> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Building>(targets);
		}

		// Token: 0x06005B5C RID: 23388 RVA: 0x002EA231 File Offset: 0x002E8631
		public LookTargets(IEnumerable<Plant> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Plant>(targets);
		}

		// Token: 0x06005B5D RID: 23389 RVA: 0x002EA24C File Offset: 0x002E864C
		public LookTargets(IEnumerable<WorldObject> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<WorldObject>(targets);
		}

		// Token: 0x06005B5E RID: 23390 RVA: 0x002EA267 File Offset: 0x002E8667
		public LookTargets(IEnumerable<Caravan> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<Caravan>(targets);
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06005B5F RID: 23391 RVA: 0x002EA284 File Offset: 0x002E8684
		public static LookTargets Invalid
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06005B60 RID: 23392 RVA: 0x002EA29C File Offset: 0x002E869C
		public bool IsValid
		{
			get
			{
				return this.PrimaryTarget.IsValid;
			}
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06005B61 RID: 23393 RVA: 0x002EA2C0 File Offset: 0x002E86C0
		public bool Any
		{
			get
			{
				return this.targets.Count != 0;
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06005B62 RID: 23394 RVA: 0x002EA2E8 File Offset: 0x002E86E8
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

		// Token: 0x06005B63 RID: 23395 RVA: 0x002EA36D File Offset: 0x002E876D
		public void ExposeData()
		{
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, new object[0]);
		}

		// Token: 0x06005B64 RID: 23396 RVA: 0x002EA388 File Offset: 0x002E8788
		public static implicit operator LookTargets(Thing t)
		{
			return new LookTargets(t);
		}

		// Token: 0x06005B65 RID: 23397 RVA: 0x002EA3A4 File Offset: 0x002E87A4
		public static implicit operator LookTargets(WorldObject o)
		{
			return new LookTargets(o);
		}

		// Token: 0x06005B66 RID: 23398 RVA: 0x002EA3C0 File Offset: 0x002E87C0
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

		// Token: 0x06005B67 RID: 23399 RVA: 0x002EA3F8 File Offset: 0x002E87F8
		public static implicit operator LookTargets(List<TargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B68 RID: 23400 RVA: 0x002EA414 File Offset: 0x002E8814
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

		// Token: 0x06005B69 RID: 23401 RVA: 0x002EA448 File Offset: 0x002E8848
		public static implicit operator LookTargets(List<GlobalTargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6A RID: 23402 RVA: 0x002EA464 File Offset: 0x002E8864
		public static implicit operator LookTargets(List<Thing> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6B RID: 23403 RVA: 0x002EA480 File Offset: 0x002E8880
		public static implicit operator LookTargets(List<ThingWithComps> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6C RID: 23404 RVA: 0x002EA49C File Offset: 0x002E889C
		public static implicit operator LookTargets(List<Pawn> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6D RID: 23405 RVA: 0x002EA4B8 File Offset: 0x002E88B8
		public static implicit operator LookTargets(List<Building> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6E RID: 23406 RVA: 0x002EA4D4 File Offset: 0x002E88D4
		public static implicit operator LookTargets(List<Plant> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6F RID: 23407 RVA: 0x002EA4F0 File Offset: 0x002E88F0
		public static implicit operator LookTargets(List<WorldObject> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B70 RID: 23408 RVA: 0x002EA50C File Offset: 0x002E890C
		public static implicit operator LookTargets(List<Caravan> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B71 RID: 23409 RVA: 0x002EA528 File Offset: 0x002E8928
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

		// Token: 0x06005B72 RID: 23410 RVA: 0x002EA5E4 File Offset: 0x002E89E4
		public void Highlight(bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				TargetHighlighter.Highlight(this.targets[i], arrow, colonistBar, circleOverlay);
			}
		}

		// Token: 0x06005B73 RID: 23411 RVA: 0x002EA624 File Offset: 0x002E8A24
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

		// Token: 0x06005B74 RID: 23412 RVA: 0x002EA6E4 File Offset: 0x002E8AE4
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

		// Token: 0x06005B75 RID: 23413 RVA: 0x002EA7A4 File Offset: 0x002E8BA4
		public void Notify_MapRemoved(Map map)
		{
			this.targets.RemoveAll((GlobalTargetInfo t) => t.Map == map);
		}
	}
}
