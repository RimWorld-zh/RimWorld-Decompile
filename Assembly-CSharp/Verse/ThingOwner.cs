using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	public class ThingOwner<T> : ThingOwner, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : Thing
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
			int result;
			if (count <= 0)
			{
				result = 0;
			}
			else if (item == null)
			{
				Log.Warning("Tried to add null item to ThingOwner.");
				result = 0;
			}
			else if (base.Contains(item))
			{
				Log.Warning("Tried to add " + item + " to ThingOwner but this item is already here.");
				result = 0;
			}
			else if (item.holdingOwner != null)
			{
				Log.Warning("Tried to add " + count + " of " + item.ToStringSafe<Thing>() + " to ThingOwner but this thing is already in another container. owner=" + base.owner.ToStringSafe<IThingHolder>() + ", current container owner=" + item.holdingOwner.Owner.ToStringSafe<IThingHolder>() + ". Use TryAddOrTransfer, TryTransferToContainer, or remove the item before adding it.");
				result = 0;
			}
			else if (!this.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				result = 0;
			}
			else
			{
				int stackCount = item.stackCount;
				int num = Mathf.Min(stackCount, count);
				Thing thing = item.SplitOff(num);
				if (!this.TryAdd((Thing)(object)(T)thing, canMergeWithExistingStacks))
				{
					if (thing != item)
					{
						int num2 = stackCount - item.stackCount - thing.stackCount;
						item.TryAbsorbStack(thing, false);
						result = num2;
					}
					else
					{
						result = stackCount - item.stackCount;
					}
				}
				else
				{
					result = num;
				}
			}
			return result;
		}

		public override bool TryAdd(Thing item, bool canMergeWithExistingStacks = true)
		{
			bool result;
			if (item == null)
			{
				Log.Warning("Tried to add null item to ThingOwner.");
				result = false;
			}
			else
			{
				T val = (T)(item as T);
				if (val == null)
				{
					result = false;
				}
				else if (base.Contains(item))
				{
					Log.Warning("Tried to add " + item.ToStringSafe<Thing>() + " to ThingOwner but this item is already here.");
					result = false;
				}
				else if (item.holdingOwner != null)
				{
					Log.Warning("Tried to add " + item.ToStringSafe<Thing>() + " to ThingOwner but this thing is already in another container. owner=" + base.owner.ToStringSafe<IThingHolder>() + ", current container owner=" + item.holdingOwner.Owner.ToStringSafe<IThingHolder>() + ". Use TryAddOrTransfer, TryTransferToContainer, or remove the item before adding it.");
					result = false;
				}
				else if (!this.CanAcceptAnyOf(item, canMergeWithExistingStacks))
				{
					result = false;
				}
				else
				{
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
									goto IL_01ba;
								}
							}
						}
					}
					if (this.Count >= base.maxStacks)
					{
						result = false;
					}
					else
					{
						item.holdingOwner = this;
						this.innerList.Add(val);
						base.NotifyAdded((Thing)(object)val);
						result = true;
					}
				}
			}
			goto IL_0216;
			IL_01ba:
			result = true;
			goto IL_0216;
			IL_0216:
			return result;
		}

		public void TryAddRangeOrTransfer(IEnumerable<T> things, bool canMergeWithExistingStacks = true, bool destroyLeftover = false)
		{
			if (things != this)
			{
				ThingOwner thingOwner = things as ThingOwner;
				if (thingOwner != null)
				{
					thingOwner.TryTransferAllToContainer(this, canMergeWithExistingStacks);
					if (destroyLeftover)
					{
						thingOwner.ClearAndDestroyContents(DestroyMode.Vanish);
					}
				}
				else
				{
					IList<T> list = things as IList<T>;
					if (list != null)
					{
						for (int i = 0; i < ((ICollection<T>)list).Count; i++)
						{
							if (!base.TryAddOrTransfer((Thing)(object)list[i], canMergeWithExistingStacks) && destroyLeftover)
							{
								T val = list[i];
								val.Destroy(DestroyMode.Vanish);
							}
						}
					}
					else
					{
						foreach (T item in things)
						{
							T current = item;
							if (!base.TryAddOrTransfer((Thing)(object)current, canMergeWithExistingStacks) && destroyLeftover)
							{
								current.Destroy(DestroyMode.Vanish);
							}
						}
					}
				}
			}
		}

		public override int IndexOf(Thing item)
		{
			T val = (T)(item as T);
			return (val != null) ? this.innerList.IndexOf(val) : (-1);
		}

		public override bool Remove(Thing item)
		{
			bool result;
			if (!base.Contains(item))
			{
				result = false;
			}
			else
			{
				if (item.holdingOwner == this)
				{
					item.holdingOwner = null;
				}
				int index = this.innerList.LastIndexOf((T)item);
				this.innerList.RemoveAt(index);
				base.NotifyRemoved(item);
				result = true;
			}
			return result;
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

		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int stackCount, out T resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			Thing thing = default(Thing);
			int result = base.TryTransferToContainer(item, otherContainer, stackCount, out thing, canMergeWithExistingStacks);
			resultingTransferredItem = (T)thing;
			return result;
		}

		public new T Take(Thing thing, int count)
		{
			return (T)base.Take(thing, count);
		}

		public new T Take(Thing thing)
		{
			return (T)base.Take(thing);
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
	}
	public abstract class ThingOwner : IExposable, IList<Thing>, ICollection<Thing>, IEnumerable<Thing>, IEnumerable
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
				string result;
				if (count == 0)
				{
					result = "NothingLower".Translate();
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int num = 0; num < count; num++)
					{
						if (num != 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(this.GetAt(num).Label);
					}
					result = stringBuilder.ToString();
				}
				return result;
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
			bool result;
			if (item == null || item.stackCount <= 0)
			{
				result = false;
			}
			else
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
								goto IL_0067;
						}
					}
					result = false;
				}
				else
				{
					result = true;
				}
			}
			goto IL_0089;
			IL_0089:
			return result;
			IL_0067:
			result = true;
			goto IL_0089;
		}

		public abstract int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true);

		public abstract bool TryAdd(Thing item, bool canMergeWithExistingStacks = true);

		public abstract int IndexOf(Thing item);

		public abstract bool Remove(Thing item);

		protected abstract Thing GetAt(int index);

		public bool Contains(Thing item)
		{
			return item != null && item.holdingOwner == this;
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

		public int TryAddOrTransfer(Thing item, int count, bool canMergeWithExistingStacks = true)
		{
			int result;
			if (item == null)
			{
				Log.Warning("Tried to add or transfer null item to ThingOwner.");
				result = 0;
			}
			else
			{
				result = ((item.holdingOwner == null) ? this.TryAdd(item, count, canMergeWithExistingStacks) : item.holdingOwner.TryTransferToContainer(item, this, count, canMergeWithExistingStacks));
			}
			return result;
		}

		public bool TryAddOrTransfer(Thing item, bool canMergeWithExistingStacks = true)
		{
			bool result;
			if (item == null)
			{
				Log.Warning("Tried to add or transfer null item to ThingOwner.");
				result = false;
			}
			else
			{
				result = ((item.holdingOwner == null) ? this.TryAdd(item, canMergeWithExistingStacks) : item.holdingOwner.TryTransferToContainer(item, this, canMergeWithExistingStacks));
			}
			return result;
		}

		public void TryAddRangeOrTransfer(IEnumerable<Thing> things, bool canMergeWithExistingStacks = true, bool destroyLeftover = false)
		{
			if (things != this)
			{
				ThingOwner thingOwner = things as ThingOwner;
				if (thingOwner != null)
				{
					thingOwner.TryTransferAllToContainer(this, canMergeWithExistingStacks);
					if (destroyLeftover)
					{
						thingOwner.ClearAndDestroyContents(DestroyMode.Vanish);
					}
				}
				else
				{
					IList<Thing> list = things as IList<Thing>;
					if (list != null)
					{
						for (int i = 0; i < list.Count; i++)
						{
							if (!this.TryAddOrTransfer(list[i], canMergeWithExistingStacks) && destroyLeftover)
							{
								list[i].Destroy(DestroyMode.Vanish);
							}
						}
					}
					else
					{
						foreach (Thing item in things)
						{
							if (!this.TryAddOrTransfer(item, canMergeWithExistingStacks) && destroyLeftover)
							{
								item.Destroy(DestroyMode.Vanish);
							}
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
			return this.TryTransferToContainer(item, otherContainer, item.stackCount, canMergeWithExistingStacks) == item.stackCount;
		}

		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int count, bool canMergeWithExistingStacks = true)
		{
			Thing thing = default(Thing);
			return this.TryTransferToContainer(item, otherContainer, count, out thing, canMergeWithExistingStacks);
		}

		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int count, out Thing resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			int result;
			if (!this.Contains(item))
			{
				Log.Error("Can't transfer item " + item + " because it's not here. owner=" + this.owner.ToStringSafe());
				resultingTransferredItem = null;
				result = 0;
			}
			else if (otherContainer == this && count > 0)
			{
				resultingTransferredItem = item;
				result = item.stackCount;
			}
			else if (!otherContainer.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				resultingTransferredItem = null;
				result = 0;
			}
			else if (count <= 0)
			{
				resultingTransferredItem = null;
				result = 0;
			}
			else if (this.owner is Map || otherContainer.owner is Map)
			{
				Log.Warning("Can't transfer items to or from Maps directly. They must be spawned or despawned manually. Use TryAdd(item.SplitOff(count))");
				resultingTransferredItem = null;
				result = 0;
			}
			else
			{
				int num = Mathf.Min(item.stackCount, count);
				Thing thing = item.SplitOff(num);
				if (this.Contains(thing))
				{
					this.Remove(thing);
				}
				if (otherContainer.TryAdd(thing, canMergeWithExistingStacks))
				{
					resultingTransferredItem = thing;
					result = thing.stackCount;
				}
				else
				{
					resultingTransferredItem = null;
					if (!otherContainer.Contains(thing) && thing.stackCount > 0 && !thing.Destroyed)
					{
						int num2 = num - thing.stackCount;
						if (item != thing)
						{
							item.TryAbsorbStack(thing, false);
						}
						else
						{
							this.TryAdd(thing, false);
						}
						result = num2;
					}
					else
					{
						result = thing.stackCount;
					}
				}
			}
			return result;
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
			Thing result;
			if (!this.Contains(thing))
			{
				Log.Error("Tried to take " + thing.ToStringSafe() + " but it's not here.");
				result = null;
			}
			else
			{
				if (count > thing.stackCount)
				{
					Log.Error("Tried to get " + count + " of " + thing.ToStringSafe() + " while only having " + thing.stackCount);
					count = thing.stackCount;
				}
				if (count == thing.stackCount)
				{
					this.Remove(thing);
					result = thing;
				}
				else
				{
					Thing thing2 = thing.SplitOff(count);
					thing2.holdingOwner = null;
					result = thing2;
				}
			}
			return result;
		}

		public Thing Take(Thing thing)
		{
			return this.Take(thing, thing.stackCount);
		}

		public bool TryDrop(Thing thing, ThingPlaceMode mode, int count, out Thing lastResultingThing, Action<Thing, int> placedAction = null)
		{
			Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
			IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
			bool result;
			if (rootMap == null || !rootPosition.IsValid)
			{
				Log.Error("Cannot drop " + thing + " without a dropLoc and with an owner whose map is null.");
				lastResultingThing = null;
				result = false;
			}
			else
			{
				result = this.TryDrop(thing, rootPosition, rootMap, mode, count, out lastResultingThing, placedAction);
			}
			return result;
		}

		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, int count, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			bool result;
			if (!this.Contains(thing))
			{
				Log.Error("Tried to drop " + thing.ToStringSafe() + " but it's not here.");
				resultingThing = null;
				result = false;
			}
			else
			{
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
						result = true;
					}
					else
					{
						result = false;
					}
				}
				else
				{
					Thing thing2 = thing.SplitOff(count);
					if (GenDrop.TryDropSpawn(thing2, dropLoc, map, mode, out resultingThing, placedAction))
					{
						result = true;
					}
					else
					{
						thing.TryAbsorbStack(thing2, false);
						result = false;
					}
				}
			}
			return result;
		}

		public bool TryDrop(Thing thing, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null)
		{
			Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
			IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
			bool result;
			if (rootMap == null || !rootPosition.IsValid)
			{
				Log.Error("Cannot drop " + thing + " without a dropLoc and with an owner whose map is null.");
				lastResultingThing = null;
				result = false;
			}
			else
			{
				result = this.TryDrop(thing, rootPosition, rootMap, mode, out lastResultingThing, placedAction);
			}
			return result;
		}

		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null)
		{
			bool result;
			if (!this.Contains(thing))
			{
				Log.Error(this.owner.ToStringSafe() + " container tried to drop  " + thing.ToStringSafe() + " which it didn't contain.");
				lastResultingThing = null;
				result = false;
			}
			else if (GenDrop.TryDropSpawn(thing, dropLoc, map, mode, out lastResultingThing, placedAction))
			{
				this.Remove(thing);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
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
			bool result;
			if (minCount <= 0)
			{
				result = true;
			}
			else
			{
				int num = 0;
				int count = this.Count;
				for (int num2 = 0; num2 < count; num2++)
				{
					if (this.GetAt(num2).def == def)
					{
						num += this.GetAt(num2).stackCount;
					}
					if (num >= minCount)
						goto IL_0048;
				}
				result = false;
			}
			goto IL_0062;
			IL_0062:
			return result;
			IL_0048:
			result = true;
			goto IL_0062;
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
			int i = 0;
			if (i < this.Count)
			{
				yield return this.GetAt(i);
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			int i = 0;
			if (i < this.Count)
			{
				yield return (object)this.GetAt(i);
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
