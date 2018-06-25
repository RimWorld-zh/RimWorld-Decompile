using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000EF4 RID: 3828
	public class LookTargets : IExposable
	{
		// Token: 0x04003CAE RID: 15534
		public List<GlobalTargetInfo> targets;

		// Token: 0x06005B4F RID: 23375 RVA: 0x002EA1A3 File Offset: 0x002E85A3
		public LookTargets()
		{
			this.targets = new List<GlobalTargetInfo>();
		}

		// Token: 0x06005B50 RID: 23376 RVA: 0x002EA1B7 File Offset: 0x002E85B7
		public LookTargets(Thing t)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(t);
		}

		// Token: 0x06005B51 RID: 23377 RVA: 0x002EA1DC File Offset: 0x002E85DC
		public LookTargets(WorldObject o)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(o);
		}

		// Token: 0x06005B52 RID: 23378 RVA: 0x002EA201 File Offset: 0x002E8601
		public LookTargets(IntVec3 c, Map map)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(c, map, false));
		}

		// Token: 0x06005B53 RID: 23379 RVA: 0x002EA228 File Offset: 0x002E8628
		public LookTargets(int tile)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(tile));
		}

		// Token: 0x06005B54 RID: 23380 RVA: 0x002EA24D File Offset: 0x002E864D
		public LookTargets(IEnumerable<GlobalTargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				this.targets.AddRange(targets);
			}
		}

		// Token: 0x06005B55 RID: 23381 RVA: 0x002EA274 File Offset: 0x002E8674
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

		// Token: 0x06005B56 RID: 23382 RVA: 0x002EA2C8 File Offset: 0x002E86C8
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

		// Token: 0x06005B57 RID: 23383 RVA: 0x002EA38C File Offset: 0x002E878C
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

		// Token: 0x06005B58 RID: 23384 RVA: 0x002EA3E5 File Offset: 0x002E87E5
		public LookTargets(IEnumerable<Thing> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Thing>(targets);
		}

		// Token: 0x06005B59 RID: 23385 RVA: 0x002EA400 File Offset: 0x002E8800
		public LookTargets(IEnumerable<ThingWithComps> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<ThingWithComps>(targets);
		}

		// Token: 0x06005B5A RID: 23386 RVA: 0x002EA41B File Offset: 0x002E881B
		public LookTargets(IEnumerable<Pawn> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Pawn>(targets);
		}

		// Token: 0x06005B5B RID: 23387 RVA: 0x002EA436 File Offset: 0x002E8836
		public LookTargets(IEnumerable<Building> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Building>(targets);
		}

		// Token: 0x06005B5C RID: 23388 RVA: 0x002EA451 File Offset: 0x002E8851
		public LookTargets(IEnumerable<Plant> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Plant>(targets);
		}

		// Token: 0x06005B5D RID: 23389 RVA: 0x002EA46C File Offset: 0x002E886C
		public LookTargets(IEnumerable<WorldObject> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<WorldObject>(targets);
		}

		// Token: 0x06005B5E RID: 23390 RVA: 0x002EA487 File Offset: 0x002E8887
		public LookTargets(IEnumerable<Caravan> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<Caravan>(targets);
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06005B5F RID: 23391 RVA: 0x002EA4A4 File Offset: 0x002E88A4
		public static LookTargets Invalid
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06005B60 RID: 23392 RVA: 0x002EA4BC File Offset: 0x002E88BC
		public bool IsValid
		{
			get
			{
				return this.PrimaryTarget.IsValid;
			}
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06005B61 RID: 23393 RVA: 0x002EA4E0 File Offset: 0x002E88E0
		public bool Any
		{
			get
			{
				return this.targets.Count != 0;
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06005B62 RID: 23394 RVA: 0x002EA508 File Offset: 0x002E8908
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

		// Token: 0x06005B63 RID: 23395 RVA: 0x002EA58D File Offset: 0x002E898D
		public void ExposeData()
		{
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, new object[0]);
		}

		// Token: 0x06005B64 RID: 23396 RVA: 0x002EA5A8 File Offset: 0x002E89A8
		public static implicit operator LookTargets(Thing t)
		{
			return new LookTargets(t);
		}

		// Token: 0x06005B65 RID: 23397 RVA: 0x002EA5C4 File Offset: 0x002E89C4
		public static implicit operator LookTargets(WorldObject o)
		{
			return new LookTargets(o);
		}

		// Token: 0x06005B66 RID: 23398 RVA: 0x002EA5E0 File Offset: 0x002E89E0
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

		// Token: 0x06005B67 RID: 23399 RVA: 0x002EA618 File Offset: 0x002E8A18
		public static implicit operator LookTargets(List<TargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B68 RID: 23400 RVA: 0x002EA634 File Offset: 0x002E8A34
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

		// Token: 0x06005B69 RID: 23401 RVA: 0x002EA668 File Offset: 0x002E8A68
		public static implicit operator LookTargets(List<GlobalTargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6A RID: 23402 RVA: 0x002EA684 File Offset: 0x002E8A84
		public static implicit operator LookTargets(List<Thing> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6B RID: 23403 RVA: 0x002EA6A0 File Offset: 0x002E8AA0
		public static implicit operator LookTargets(List<ThingWithComps> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6C RID: 23404 RVA: 0x002EA6BC File Offset: 0x002E8ABC
		public static implicit operator LookTargets(List<Pawn> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6D RID: 23405 RVA: 0x002EA6D8 File Offset: 0x002E8AD8
		public static implicit operator LookTargets(List<Building> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6E RID: 23406 RVA: 0x002EA6F4 File Offset: 0x002E8AF4
		public static implicit operator LookTargets(List<Plant> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B6F RID: 23407 RVA: 0x002EA710 File Offset: 0x002E8B10
		public static implicit operator LookTargets(List<WorldObject> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B70 RID: 23408 RVA: 0x002EA72C File Offset: 0x002E8B2C
		public static implicit operator LookTargets(List<Caravan> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06005B71 RID: 23409 RVA: 0x002EA748 File Offset: 0x002E8B48
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

		// Token: 0x06005B72 RID: 23410 RVA: 0x002EA804 File Offset: 0x002E8C04
		public void Highlight(bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				TargetHighlighter.Highlight(this.targets[i], arrow, colonistBar, circleOverlay);
			}
		}

		// Token: 0x06005B73 RID: 23411 RVA: 0x002EA844 File Offset: 0x002E8C44
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

		// Token: 0x06005B74 RID: 23412 RVA: 0x002EA904 File Offset: 0x002E8D04
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

		// Token: 0x06005B75 RID: 23413 RVA: 0x002EA9C4 File Offset: 0x002E8DC4
		public void Notify_MapRemoved(Map map)
		{
			this.targets.RemoveAll((GlobalTargetInfo t) => t.Map == map);
		}
	}
}
