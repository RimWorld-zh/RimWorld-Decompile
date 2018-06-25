using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	public class LookTargets : IExposable
	{
		public List<GlobalTargetInfo> targets;

		public LookTargets()
		{
			this.targets = new List<GlobalTargetInfo>();
		}

		public LookTargets(Thing t)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(t);
		}

		public LookTargets(WorldObject o)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(o);
		}

		public LookTargets(IntVec3 c, Map map)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(c, map, false));
		}

		public LookTargets(int tile)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(tile));
		}

		public LookTargets(IEnumerable<GlobalTargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				this.targets.AddRange(targets);
			}
		}

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

		public LookTargets(IEnumerable<Thing> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Thing>(targets);
		}

		public LookTargets(IEnumerable<ThingWithComps> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<ThingWithComps>(targets);
		}

		public LookTargets(IEnumerable<Pawn> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Pawn>(targets);
		}

		public LookTargets(IEnumerable<Building> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Building>(targets);
		}

		public LookTargets(IEnumerable<Plant> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Plant>(targets);
		}

		public LookTargets(IEnumerable<WorldObject> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<WorldObject>(targets);
		}

		public LookTargets(IEnumerable<Caravan> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<Caravan>(targets);
		}

		public static LookTargets Invalid
		{
			get
			{
				return null;
			}
		}

		public bool IsValid
		{
			get
			{
				return this.PrimaryTarget.IsValid;
			}
		}

		public bool Any
		{
			get
			{
				return this.targets.Count != 0;
			}
		}

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

		public void ExposeData()
		{
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, new object[0]);
		}

		public static implicit operator LookTargets(Thing t)
		{
			return new LookTargets(t);
		}

		public static implicit operator LookTargets(WorldObject o)
		{
			return new LookTargets(o);
		}

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

		public static implicit operator LookTargets(List<TargetInfo> targets)
		{
			return new LookTargets(targets);
		}

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

		public static implicit operator LookTargets(List<GlobalTargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		public static implicit operator LookTargets(List<Thing> targets)
		{
			return new LookTargets(targets);
		}

		public static implicit operator LookTargets(List<ThingWithComps> targets)
		{
			return new LookTargets(targets);
		}

		public static implicit operator LookTargets(List<Pawn> targets)
		{
			return new LookTargets(targets);
		}

		public static implicit operator LookTargets(List<Building> targets)
		{
			return new LookTargets(targets);
		}

		public static implicit operator LookTargets(List<Plant> targets)
		{
			return new LookTargets(targets);
		}

		public static implicit operator LookTargets(List<WorldObject> targets)
		{
			return new LookTargets(targets);
		}

		public static implicit operator LookTargets(List<Caravan> targets)
		{
			return new LookTargets(targets);
		}

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

		public void Highlight(bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				TargetHighlighter.Highlight(this.targets[i], arrow, colonistBar, circleOverlay);
			}
		}

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

		public void Notify_MapRemoved(Map map)
		{
			this.targets.RemoveAll((GlobalTargetInfo t) => t.Map == map);
		}

		[CompilerGenerated]
		private sealed class <Notify_MapRemoved>c__AnonStorey0
		{
			internal Map map;

			public <Notify_MapRemoved>c__AnonStorey0()
			{
			}

			internal bool <>m__0(GlobalTargetInfo t)
			{
				return t.Map == this.map;
			}
		}
	}
}
