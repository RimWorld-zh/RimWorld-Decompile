using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	public class ThingOwner<T> : ThingOwner, IEnumerable, IList<T>, ICollection<T>, IEnumerable<T> where T : Thing
	{
		private List<T> innerList = new List<T>();

		T IList<T>.this[int index]
		{
			get
			{
				return this.innerList[index];
			}
			set
			{
				throw new InvalidOperationException("ThingOwner doesn't allow setting individual elements.");
			}
		}

		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public List<T> InnerListForReading
		{
			get
			{
				return this.innerList;
			}
		}

		public new T this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		public override int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		public ThingOwner()
		{
		}

		public ThingOwner(IThingHolder owner) : base(owner)
		{
		}

		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep) : base(owner, oneStackOnly, contentsLookMode)
		{
		}

		int IList<T>.IndexOf(T item)
		{
			return this.innerList.IndexOf(item);
		}

		void IList<T>.Insert(int index, T item)
		{
			throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
		}

		void ICollection<T>.Add(T item)
		{
			this.TryAdd((Thing)(object)item, true);
		}

		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		bool ICollection<T>.Contains(T item)
		{
			return this.innerList.Contains(item);
		}

		bool ICollection<T>.Remove(T item)
		{
			return this.Remove((Thing)(object)item);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return (IEnumerator<T>)(object)this.innerList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator)(object)this.innerList.GetEnumerator();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<T>(ref this.innerList, true, "innerList", base.contentsLookMode, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.innerList.RemoveAll((Predicate<T>)((T x) => x == null));
			}
			if (Scribe.mode != LoadSaveMode.LoadingVars && Scribe.mode != LoadSaveMode.PostLoadInit)
				return;
			for (int i = 0; i < this.innerList.Count; i++)
			{
				if (this.innerList[i] != null)
				{
					((Thing)(object)this.innerList[i]).holdingOwner = this;
				}
			}
		}

		public List<T>.Enumerator GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		public override bool CanAcceptAnyOf(Thing item, bool canMergeWithExistingStacks = true)
		{
			return item is T && base.CanAcceptAnyOf(item, canMergeWithExistingStacks);
		}

		public override int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add null item to ThingOwner.");
				return 0;
			}
			if (this.Contains(item))
			{
				Log.Warning("Tried to add " + item + " to ThingOwner but this item is already here.");
				return 0;
			}
			if (count <= 0)
			{
				return 0;
			}
			if (!this.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				return 0;
			}
			int stackCount = item.stackCount;
			int num = Mathf.Min(stackCount, count);
			Thing thing = item.SplitOff(num);
			if (this.Contains(thing))
			{
				this.Remove(thing);
			}
			if (!this.TryAdd((Thing)(object)(T)thing, canMergeWithExistingStacks))
			{
				if (thing != item)
				{
					int result = stackCount - item.stackCount - thing.stackCount;
					item.TryAbsorbStack(thing, false);
					return result;
				}
				this.TryAdd(thing, false);
				return stackCount - item.stackCount;
			}
			return num;
		}

		public override bool TryAdd(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add null item to ThingOwner.");
				return false;
			}
			T val = (T)(item as T);
			if (val == null)
			{
				return false;
			}
			if (this.Contains(item))
			{
				Log.Warning("Tried to add " + item.ToStringSafe<Thing>() + " to ThingOwner but this item is already here.");
				return false;
			}
			if (item.holdingOwner != null)
			{
				Log.Warning("Tried to add " + item.ToStringSafe<Thing>() + " to ThingOwner but this thing is already in another container. owner=" + base.owner.ToStringSafe<IThingHolder>() + ", other container owner=" + item.holdingOwner.Owner.ToStringSafe<IThingHolder>());
				return false;
			}
			if (!this.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				return false;
			}
			if (canMergeWithExistingStacks)
			{
				for (int i = 0; i < this.innerList.Count; i++)
				{
					T val2 = this.innerList[i];
					if (val2.CanStackWith(item))
					{
						int num = Mathf.Min(item.stackCount, ((Thing)(object)val2).def.stackLimit - ((Thing)(object)val2).stackCount);
						if (num > 0)
						{
							Thing other = item.SplitOff(num);
							int stackCount = ((Thing)(object)val2).stackCount;
							val2.TryAbsorbStack(other, true);
							if (((Thing)(object)val2).stackCount > stackCount)
							{
								base.NotifyAddedAndMergedWith((Thing)(object)val2, ((Thing)(object)val2).stackCount - stackCount);
							}
							if (((!item.Destroyed) ? item.stackCount : 0) != 0)
							{
								continue;
							}
							return true;
						}
					}
				}
			}
			if (this.Count >= base.maxStacks)
			{
				return false;
			}
			item.holdingOwner = this;
			this.innerList.Add(val);
			base.NotifyAdded((Thing)(object)val);
			return true;
		}

		public void TryAddRange(IEnumerable<T> things, bool canMergeWithExistingStacks = true)
		{
			if (things != this)
			{
				ThingOwner thingOwner = things as ThingOwner;
				if (thingOwner != null)
				{
					thingOwner.TryTransferAllToContainer(this, canMergeWithExistingStacks);
				}
				else
				{
					IList<T> list = things as IList<T>;
					if (list != null)
					{
						for (int i = 0; i < ((ICollection<T>)list).Count; i++)
						{
							this.TryAdd((Thing)(object)list[i], canMergeWithExistingStacks);
						}
					}
					else
					{
						foreach (T item in things)
						{
							this.TryAdd((Thing)(object)item, canMergeWithExistingStacks);
						}
					}
				}
			}
		}

		public override int IndexOf(Thing item)
		{
			T val = (T)(item as T);
			if (val == null)
			{
				return -1;
			}
			return this.innerList.IndexOf(val);
		}

		public override bool Remove(Thing item)
		{
			if (!this.Contains(item))
			{
				return false;
			}
			if (item.holdingOwner == this)
			{
				item.holdingOwner = null;
			}
			int index = this.innerList.LastIndexOf((T)item);
			this.innerList.RemoveAt(index);
			base.NotifyRemoved(item);
			return true;
		}

		public int RemoveAll(Predicate<T> predicate)
		{
			int num = 0;
			for (int num2 = this.innerList.Count - 1; num2 >= 0; num2--)
			{
				if (predicate(this.innerList[num2]))
				{
					this.Remove((Thing)(object)this.innerList[num2]);
					num++;
				}
			}
			return num;
		}

		protected override Thing GetAt(int index)
		{
			return (Thing)(object)this.innerList[index];
		}

		public bool TryTransferToContainer(Thing item, ThingOwner otherContainer, int stackCount, out T resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			Thing thing = default(Thing);
			bool result = base.TryTransferToContainer(item, otherContainer, stackCount, out thing, canMergeWithExistingStacks);
			resultingTransferredItem = (T)thing;
			return result;
		}

		public new T Take(Thing thing, int count)
		{
			return (T)base.Take(thing, count);
		}

		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, int count, out T resultingThing, Action<T, int> placedAction = null)
		{
			Action<Thing, int> placedAction2 = null;
			if ((object)placedAction != null)
			{
				placedAction2 = (Action<Thing, int>)delegate(Thing t, int c)
				{
					placedAction((T)t, c);
				};
			}
			Thing thing2 = default(Thing);
			bool result = base.TryDrop(thing, dropLoc, map, mode, count, out thing2, placedAction2);
			resultingThing = (T)thing2;
			return result;
		}

		public bool TryDrop(Thing thing, ThingPlaceMode mode, out T lastResultingThing, Action<T, int> placedAction = null)
		{
			Action<Thing, int> placedAction2 = null;
			if ((object)placedAction != null)
			{
				placedAction2 = (Action<Thing, int>)delegate(Thing t, int c)
				{
					placedAction((T)t, c);
				};
			}
			Thing thing2 = default(Thing);
			bool result = base.TryDrop(thing, mode, out thing2, placedAction2);
			lastResultingThing = (T)thing2;
			return result;
		}

		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out T lastResultingThing, Action<T, int> placedAction = null)
		{
			Action<Thing, int> placedAction2 = null;
			if ((object)placedAction != null)
			{
				placedAction2 = (Action<Thing, int>)delegate(Thing t, int c)
				{
					placedAction((T)t, c);
				};
			}
			Thing thing2 = default(Thing);
			bool result = base.TryDrop(thing, dropLoc, map, mode, out thing2, placedAction2);
			lastResultingThing = (T)thing2;
			return result;
		}
	}
	public abstract class ThingOwner : IEnumerable<Thing>, IEnumerable, ICollection<Thing>, IList<Thing>, IExposable
	{
		protected IThingHolder owner;

		protected int maxStacks = 999999;

		protected LookMode contentsLookMode = LookMode.Deep;

		Thing IList<Thing>.this[int index]
		{
			get
			{
				return this.GetAt(index);
			}
			set
			{
				throw new InvalidOperationException("ThingOwner doesn't allow setting individual elements.");
			}
		}

		bool ICollection<Thing>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public IThingHolder Owner
		{
			get
			{
				return this.owner;
			}
		}

		public abstract int Count
		{
			get;
		}

		public Thing this[int index]
		{
			get
			{
				return this.GetAt(index);
			}
		}

		public bool Any
		{
			get
			{
				return this.Count > 0;
			}
		}

		public int TotalStackCount
		{
			get
			{
				int num = 0;
				int count = this.Count;
				for (int num2 = 0; num2 < count; num2++)
				{
					num += this.GetAt(num2).stackCount;
				}
				return num;
			}
		}

		public string ContentsString
		{
			get
			{
				int count = this.Count;
				if (count == 0)
				{
					return "NothingLower".Translate();
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (int num = 0; num < count; num++)
				{
					if (num != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.GetAt(num).Label);
				}
				return stringBuilder.ToString();
			}
		}

		public ThingOwner()
		{
		}

		public ThingOwner(IThingHolder owner)
		{
			this.owner = owner;
		}

		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep) : this(owner)
		{
			this.maxStacks = (oneStackOnly ? 1 : 999999);
			this.contentsLookMode = contentsLookMode;
		}

		void IList<Thing>.Insert(int index, Thing item)
		{
			throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
		}

		void ICollection<Thing>.Add(Thing item)
		{
			this.TryAdd(item, true);
		}

		void ICollection<Thing>.CopyTo(Thing[] array, int arrayIndex)
		{
			for (int i = 0; i < this.Count; i++)
			{
				array[i + arrayIndex] = this.GetAt(i);
			}
		}

		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			for (int i = 0; i < this.Count; i++)
			{
				yield return this.GetAt(i);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			for (int i = 0; i < this.Count; i++)
			{
				yield return (object)this.GetAt(i);
			}
		}

		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.maxStacks, "maxStacks", 999999, false);
			Scribe_Values.Look<LookMode>(ref this.contentsLookMode, "contentsLookMode", LookMode.Deep, false);
		}

		public void ThingOwnerTick(bool removeIfDestroyed = true)
		{
			for (int num = this.Count - 1; num >= 0; num--)
			{
				Thing at = this.GetAt(num);
				if (at.def.tickerType == TickerType.Normal)
				{
					at.Tick();
					if (at.Destroyed && removeIfDestroyed)
					{
						this.Remove(at);
					}
				}
			}
		}

		public void ThingOwnerTickRare(bool removeIfDestroyed = true)
		{
			for (int num = this.Count - 1; num >= 0; num--)
			{
				Thing at = this.GetAt(num);
				if (at.def.tickerType == TickerType.Rare)
				{
					at.TickRare();
					if (at.Destroyed && removeIfDestroyed)
					{
						this.Remove(at);
					}
				}
			}
		}

		public void ThingOwnerTickLong(bool removeIfDestroyed = true)
		{
			for (int num = this.Count - 1; num >= 0; num--)
			{
				Thing at = this.GetAt(num);
				if (at.def.tickerType == TickerType.Long)
				{
					at.TickRare();
					if (at.Destroyed && removeIfDestroyed)
					{
						this.Remove(at);
					}
				}
			}
		}

		public void Clear()
		{
			for (int num = this.Count - 1; num >= 0; num--)
			{
				this.Remove(this.GetAt(num));
			}
		}

		public void ClearAndDestroyContents(DestroyMode mode = DestroyMode.Vanish)
		{
			while (this.Any)
			{
				for (int num = this.Count - 1; num >= 0; num--)
				{
					Thing at = this.GetAt(num);
					at.Destroy(mode);
					this.Remove(at);
				}
			}
		}

		public void ClearAndDestroyContentsOrPassToWorld(DestroyMode mode = DestroyMode.Vanish)
		{
			while (this.Any)
			{
				for (int num = this.Count - 1; num >= 0; num--)
				{
					Thing at = this.GetAt(num);
					at.DestroyOrPassToWorld(mode);
					this.Remove(at);
				}
			}
		}

		public virtual bool CanAcceptAnyOf(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (item != null && item.stackCount > 0)
			{
				int count = this.Count;
				if (count >= this.maxStacks)
				{
					if (canMergeWithExistingStacks)
					{
						for (int num = 0; num < count; num++)
						{
							Thing at = this.GetAt(num);
							if (at.stackCount < at.def.stackLimit && at.CanStackWith(item))
							{
								return true;
							}
						}
					}
					return false;
				}
				return true;
			}
			return false;
		}

		public abstract int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true);

		public abstract bool TryAdd(Thing item, bool canMergeWithExistingStacks = true);

		public abstract int IndexOf(Thing item);

		public abstract bool Remove(Thing item);

		protected abstract Thing GetAt(int index);

		public bool Contains(Thing item)
		{
			if (item == null)
			{
				return false;
			}
			return item.holdingOwner == this;
		}

		public void RemoveAt(int index)
		{
			if (index >= 0 && index < this.Count)
			{
				this.Remove(this.GetAt(index));
				return;
			}
			throw new ArgumentOutOfRangeException("index");
		}

		public void TryAddRange(IEnumerable<Thing> things, bool canMergeWithExistingStacks = true)
		{
			if (things != this)
			{
				ThingOwner thingOwner = things as ThingOwner;
				if (thingOwner != null)
				{
					thingOwner.TryTransferAllToContainer(this, canMergeWithExistingStacks);
				}
				else
				{
					IList<Thing> list = things as IList<Thing>;
					if (list != null)
					{
						for (int i = 0; i < list.Count; i++)
						{
							this.TryAdd(list[i], canMergeWithExistingStacks);
						}
					}
					else
					{
						foreach (Thing item in things)
						{
							this.TryAdd(item, canMergeWithExistingStacks);
						}
					}
				}
			}
		}

		public int RemoveAll(Predicate<Thing> predicate)
		{
			int num = 0;
			for (int num2 = this.Count - 1; num2 >= 0; num2--)
			{
				if (predicate(this.GetAt(num2)))
				{
					this.Remove(this.GetAt(num2));
					num++;
				}
			}
			return num;
		}

		public bool TryTransferToContainer(Thing item, ThingOwner otherContainer, bool canMergeWithExistingStacks = true)
		{
			return this.TryTransferToContainer(item, otherContainer, item.stackCount, canMergeWithExistingStacks);
		}

		public bool TryTransferToContainer(Thing item, ThingOwner otherContainer, int count, bool canMergeWithExistingStacks = true)
		{
			Thing thing = default(Thing);
			return this.TryTransferToContainer(item, otherContainer, count, out thing, canMergeWithExistingStacks);
		}

		public bool TryTransferToContainer(Thing item, ThingOwner otherContainer, int count, out Thing resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			if (!this.Contains(item))
			{
				Log.Error("Can't transfer item " + item + " because it's not here.");
				resultingTransferredItem = null;
				return false;
			}
			if (otherContainer == this)
			{
				resultingTransferredItem = item;
				return true;
			}
			if (!otherContainer.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				resultingTransferredItem = null;
				return false;
			}
			if (count <= 0)
			{
				resultingTransferredItem = null;
				return false;
			}
			Thing thing = item.SplitOff(count);
			if (this.Contains(thing))
			{
				this.Remove(thing);
			}
			bool flag = otherContainer.TryAdd(thing, canMergeWithExistingStacks);
			if (flag)
			{
				resultingTransferredItem = thing;
			}
			else
			{
				if (!otherContainer.Contains(thing) && thing.stackCount > 0)
				{
					if (item != thing)
					{
						item.TryAbsorbStack(thing, false);
					}
					else
					{
						this.TryAdd(thing, false);
					}
				}
				resultingTransferredItem = null;
			}
			return flag;
		}

		public void TryTransferAllToContainer(ThingOwner other, bool canMergeWithExistingStacks = true)
		{
			for (int num = this.Count - 1; num >= 0; num--)
			{
				this.TryTransferToContainer(this.GetAt(num), other, canMergeWithExistingStacks);
			}
		}

		public Thing Take(Thing thing, int count)
		{
			if (!this.Contains(thing))
			{
				Log.Error("Tried to take " + thing.ToStringSafe() + " but it's not here.");
				return null;
			}
			if (count > thing.stackCount)
			{
				Log.Error("Tried to get " + count + " of " + thing.ToStringSafe() + " while only having " + thing.stackCount);
				count = thing.stackCount;
			}
			if (count == thing.stackCount)
			{
				this.Remove(thing);
				return thing;
			}
			Thing thing2 = thing.SplitOff(count);
			thing2.holdingOwner = null;
			return thing2;
		}

		public void Take(Thing thing)
		{
			this.Take(thing, thing.stackCount);
		}

		public bool TryDrop(Thing thing, ThingPlaceMode mode, int count, out Thing lastResultingThing, Action<Thing, int> placedAction = null)
		{
			Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
			IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
			if (rootMap != null && rootPosition.IsValid)
			{
				return this.TryDrop(thing, rootPosition, rootMap, mode, count, out lastResultingThing, placedAction);
			}
			Log.Error("Cannot drop " + thing + " without a dropLoc and with an owner whose map is null.");
			lastResultingThing = null;
			return false;
		}

		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, int count, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			if (!this.Contains(thing))
			{
				Log.Error("Tried to drop " + thing.ToStringSafe() + " but it's not here.");
				resultingThing = null;
				return false;
			}
			if (thing.stackCount < count)
			{
				Log.Error("Tried to drop " + count + " of " + thing + " while only having " + thing.stackCount);
				count = thing.stackCount;
			}
			if (count == thing.stackCount)
			{
				if (GenDrop.TryDropSpawn(thing, dropLoc, map, mode, out resultingThing, placedAction))
				{
					this.Remove(thing);
					return true;
				}
				return false;
			}
			Thing thing2 = thing.SplitOff(count);
			if (GenDrop.TryDropSpawn(thing2, dropLoc, map, mode, out resultingThing, placedAction))
			{
				return true;
			}
			thing.TryAbsorbStack(thing2, false);
			return false;
		}

		public bool TryDrop(Thing thing, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null)
		{
			Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
			IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
			if (rootMap != null && rootPosition.IsValid)
			{
				return this.TryDrop(thing, rootPosition, rootMap, mode, out lastResultingThing, placedAction);
			}
			Log.Error("Cannot drop " + thing + " without a dropLoc and with an owner whose map is null.");
			lastResultingThing = null;
			return false;
		}

		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null)
		{
			if (!this.Contains(thing))
			{
				Log.Error(this.owner.ToStringSafe() + " container tried to drop  " + thing.ToStringSafe() + " which it didn't contain.");
				lastResultingThing = null;
				return false;
			}
			if (GenDrop.TryDropSpawn(thing, dropLoc, map, mode, out lastResultingThing, placedAction))
			{
				this.Remove(thing);
				return true;
			}
			return false;
		}

		public bool TryDropAll(IntVec3 dropLoc, Map map, ThingPlaceMode mode)
		{
			bool result = true;
			for (int num = this.Count - 1; num >= 0; num--)
			{
				Thing thing = default(Thing);
				if (!this.TryDrop(this.GetAt(num), dropLoc, map, mode, out thing, (Action<Thing, int>)null))
				{
					result = false;
				}
			}
			return result;
		}

		public bool Contains(ThingDef def)
		{
			return this.Contains(def, 1);
		}

		public bool Contains(ThingDef def, int minCount)
		{
			if (minCount <= 0)
			{
				return true;
			}
			int num = 0;
			int count = this.Count;
			for (int num2 = 0; num2 < count; num2++)
			{
				if (this.GetAt(num2).def == def)
				{
					num += this.GetAt(num2).stackCount;
				}
				if (num >= minCount)
				{
					return true;
				}
			}
			return false;
		}

		public int TotalStackCountOfDef(ThingDef def)
		{
			int num = 0;
			int count = this.Count;
			for (int num2 = 0; num2 < count; num2++)
			{
				if (this.GetAt(num2).def == def)
				{
					num += this.GetAt(num2).stackCount;
				}
			}
			return num;
		}

		public void Notify_ContainedItemDestroyed(Thing t)
		{
			if (ThingOwnerUtility.ShouldAutoRemoveDestroyedThings(this.owner))
			{
				this.Remove(t);
			}
		}

		protected void NotifyAdded(Thing item)
		{
			if (ThingOwnerUtility.ShouldAutoExtinguishInnerThings(this.owner) && item.HasAttachment(ThingDefOf.Fire))
			{
				item.GetAttachment(ThingDefOf.Fire).Destroy(DestroyMode.Vanish);
			}
			if (ThingOwnerUtility.ShouldRemoveDesignationsOnAddedThings(this.owner))
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					maps[i].designationManager.RemoveAllDesignationsOn(item, false);
				}
			}
			CompTransporter compTransporter = this.owner as CompTransporter;
			if (compTransporter != null)
			{
				compTransporter.Notify_ThingAdded(item);
			}
			Caravan caravan = this.owner as Caravan;
			if (caravan != null)
			{
				caravan.Notify_PawnAdded((Pawn)item);
			}
			Pawn_ApparelTracker pawn_ApparelTracker = this.owner as Pawn_ApparelTracker;
			if (pawn_ApparelTracker != null)
			{
				pawn_ApparelTracker.Notify_ApparelAdded((Apparel)item);
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.owner as Pawn_EquipmentTracker;
			if (pawn_EquipmentTracker != null)
			{
				pawn_EquipmentTracker.Notify_EquipmentAdded((ThingWithComps)item);
			}
			this.NotifyColonistBarIfColonistCorpse(item);
		}

		protected void NotifyAddedAndMergedWith(Thing item, int mergedCount)
		{
			CompTransporter compTransporter = this.owner as CompTransporter;
			if (compTransporter != null)
			{
				compTransporter.Notify_ThingAddedAndMergedWith(item, mergedCount);
			}
		}

		protected void NotifyRemoved(Thing item)
		{
			Pawn_InventoryTracker pawn_InventoryTracker = this.owner as Pawn_InventoryTracker;
			if (pawn_InventoryTracker != null)
			{
				pawn_InventoryTracker.Notify_ItemRemoved(item);
			}
			Pawn_ApparelTracker pawn_ApparelTracker = this.owner as Pawn_ApparelTracker;
			if (pawn_ApparelTracker != null)
			{
				pawn_ApparelTracker.Notify_ApparelRemoved((Apparel)item);
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.owner as Pawn_EquipmentTracker;
			if (pawn_EquipmentTracker != null)
			{
				pawn_EquipmentTracker.Notify_EquipmentRemoved((ThingWithComps)item);
			}
			Caravan caravan = this.owner as Caravan;
			if (caravan != null)
			{
				caravan.Notify_PawnRemoved((Pawn)item);
			}
			this.NotifyColonistBarIfColonistCorpse(item);
		}

		private void NotifyColonistBarIfColonistCorpse(Thing thing)
		{
			Corpse corpse = thing as Corpse;
			if (corpse != null && !corpse.Bugged && corpse.InnerPawn.Faction != null && corpse.InnerPawn.Faction.IsPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}
	}
}
