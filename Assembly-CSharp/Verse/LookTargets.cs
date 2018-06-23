using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000EF1 RID: 3825
	public class LookTargets : IExposable
	{
		// Token: 0x04003CA6 RID: 15526
		public List<GlobalTargetInfo> targets;

		// Token: 0x06005B4C RID: 23372 RVA: 0x002E9E63 File Offset: 0x002E8263
		public LookTargets()
		{
			this.targets = new List<GlobalTargetInfo>();
		}

		// Token: 0x06005B4D RID: 23373 RVA: 0x002E9E77 File Offset: 0x002E8277
		public LookTargets(Thing t)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(t);
		}

		// Token: 0x06005B4E RID: 23374 RVA: 0x002E9E9C File Offset: 0x002E829C
		public LookTargets(WorldObject o)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(o);
		}

		// Token: 0x06005B4F RID: 23375 RVA: 0x002E9EC1 File Offset: 0x002E82C1
		public LookTargets(IntVec3 c, Map map)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(c, map, false));
		}

		// Token: 0x06005B50 RID: 23376 RVA: 0x002E9EE8 File Offset: 0x002E82E8
		public LookTargets(int tile)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(tile));
		}

		// Token: 0x06005B51 RID: 23377 RVA: 0x002E9F0D File Offset: 0x002E830D
		public LookTargets(IEnumerable<GlobalTargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				this.targets.AddRange(targets);
			}
		}

		// Token: 0x06005B52 RID: 23378 RVA: 0x002E9F34 File Offset: 0x002E8334
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

		// Token: 0x06005B53 RID: 23379 RVA: 0x002E9F88 File Offset: 0x002E8388
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

		// Token: 0x06005B54 RID: 23380 RVA: 0x002EA04C File Offset: 0x002E844C
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

		// Token: 0x06005B55 RID: 23381 RVA: 0x002EA0A5 File Offset: 0x002E84A5
		public LookTargets(IEnumerable<Thing> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Thing>(targets);
		}

		// Token: 0x06005B56 RID: 23382 RVA: 0x002EA0C0 File Offset: 0x002E84C0
		public LookTargets(IEnumerable<ThingWithComps> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<ThingWithComps>(targets);
		}

		// Token: 0x06005B57 RID: 23383 RVA: 0x002EA0DB File Offset: 0x002E84DB
		public LookTargets(IEnumerable<Pawn> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Pawn>(targets);
		}

		// Token: 0x06005B58 RID: 23384 RVA: 0x002EA0F6 File Offset: 0x002E84F6
		public LookTargets(IEnumerable<Building> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Building>(targets);
		}

		// Token: 0x06005B59 RID: 23385 RVA: 0x002EA111 File Offset: 0x002E8511
		public LookTargets(IEnumerable<Plant> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Plant>(targets);
		}

		// Token: 0x06005B5A RID: 23386 RVA: 0x002EA12C File Offset: 0x002E852C
		public LookTargets(IEnumerable<WorldObject> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<WorldObject>(targets);
		}

		// Token: 0x06005B5B RID: 23387 RVA: 0x002EA147 File Offset: 0x002E8547
		public LookTargets(IEnumerable<Caravan> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<Caravan>(targets);
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06005B5C RID: 23388 RVA: 0x002EA164 File Offset: 0x002E8564
		public static LookTargets Invalid
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06005B5D RID: 23389 RVA: 0x002EA17C File Offset: 0x002E857C
		public bool IsValid
		{
			get
			{
				return this.PrimaryTarget.IsValid;
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06005B5E RID: 23390 RVA: 0x002EA1A0 File Offset: 0x002E85A0
		public bool Any
		{
			get
			{
				return this.targets.Count != 0;
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06005B5F RID: 23391 RVA: 0x002EA1C8 File Offset: 0x002E85C8
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

		// Token: 0x06005B60 RID: 23392 RVA: 0x002EA24D File Offset: 0x002E864D
		public void ExposeData()
		{
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, new object[0]);
		}

		// Token: 0x06005B61 RID: 23393 RVA: 0x002EA268 File Offset: 0x002E8668
		public static implicit operator LookTargets(Thing t)
		{
			return new LookTargets(t);
		}

		// Token: 0x06005B62 RID: 23394 RVA: 0x002EA284 File Offset: 0x002E8684
		public static implicit operator LookTargets(WorldObject o)
		{
			return new LookTargets(o);
		}

		// Token: 0x06005B63 RID: 23395 RVA: 0x002EA2A0 File Offset: 0x002E86A0
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

		// Token: 0x06005B64 RID: 23396 RVA: 0x002EA2D8 File Offset: 0x002E86D8
		public static implicit operator LookTargets(List<TargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B65 RID: 23397 RVA: 0x002EA2F4 File Offset: 0x002E86F4
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

		// Token: 0x06005B66 RID: 23398 RVA: 0x002EA328 File Offset: 0x002E8728
		public static implicit operator LookTargets(List<GlobalTargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B67 RID: 23399 RVA: 0x002EA344 File Offset: 0x002E8744
		public static implicit operator LookTargets(List<Thing> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B68 RID: 23400 RVA: 0x002EA360 File Offset: 0x002E8760
		public static implicit operator LookTargets(List<ThingWithComps> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B69 RID: 23401 RVA: 0x002EA37C File Offset: 0x002E877C
		public static implicit operator LookTargets(List<Pawn> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6A RID: 23402 RVA: 0x002EA398 File Offset: 0x002E8798
		public static implicit operator LookTargets(List<Building> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6B RID: 23403 RVA: 0x002EA3B4 File Offset: 0x002E87B4
		public static implicit operator LookTargets(List<Plant> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6C RID: 23404 RVA: 0x002EA3D0 File Offset: 0x002E87D0
		public static implicit operator LookTargets(List<WorldObject> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6D RID: 23405 RVA: 0x002EA3EC File Offset: 0x002E87EC
		public static implicit operator LookTargets(List<Caravan> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6E RID: 23406 RVA: 0x002EA408 File Offset: 0x002E8808
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

		// Token: 0x06005B6F RID: 23407 RVA: 0x002EA4C4 File Offset: 0x002E88C4
		public void Highlight(bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				TargetHighlighter.Highlight(this.targets[i], arrow, colonistBar, circleOverlay);
			}
		}

		// Token: 0x06005B70 RID: 23408 RVA: 0x002EA504 File Offset: 0x002E8904
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

		// Token: 0x06005B71 RID: 23409 RVA: 0x002EA5C4 File Offset: 0x002E89C4
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

		// Token: 0x06005B72 RID: 23410 RVA: 0x002EA684 File Offset: 0x002E8A84
		public void Notify_MapRemoved(Map map)
		{
			this.targets.RemoveAll((GlobalTargetInfo t) => t.Map == map);
		}
	}
}
