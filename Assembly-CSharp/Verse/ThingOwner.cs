using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public class ThingOwner<T> : ThingOwner, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : Thing
	{
		private List<T> innerList = new List<T>();

		[CompilerGenerated]
		private static Predicate<T> <>f__am$cache0;

		public ThingOwner()
		{
		}

		public ThingOwner(IThingHolder owner) : base(owner)
		{
		}

		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep) : base(owner, oneStackOnly, contentsLookMode)
		{
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

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<T>(ref this.innerList, true, "innerList", this.contentsLookMode, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.innerList.RemoveAll((T x) => x == null);
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.innerList.Count; i++)
				{
					if (this.innerList[i] != null)
					{
						this.innerList[i].holdingOwner = this;
					}
				}
			}
		}

		public List<T>.Enumerator GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		public override int GetCountCanAccept(Thing item, bool canMergeWithExistingStacks = true)
		{
			int result;
			if (!(item is T))
			{
				result = 0;
			}
			else
			{
				result = base.GetCountCanAccept(item, canMergeWithExistingStacks);
			}
			return result;
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
				Log.Warning("Tried to add null item to ThingOwner.", false);
				result = 0;
			}
			else if (base.Contains(item))
			{
				Log.Warning("Tried to add " + item + " to ThingOwner but this item is already here.", false);
				result = 0;
			}
			else if (item.holdingOwner != null)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to add ",
					count,
					" of ",
					item.ToStringSafe<Thing>(),
					" to ThingOwner but this thing is already in another container. owner=",
					this.owner.ToStringSafe<IThingHolder>(),
					", current container owner=",
					item.holdingOwner.Owner.ToStringSafe<IThingHolder>(),
					". Use TryAddOrTransfer, TryTransferToContainer, or remove the item before adding it."
				}), false);
				result = 0;
			}
			else if (!base.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				result = 0;
			}
			else
			{
				int stackCount = item.stackCount;
				int num = Mathf.Min(stackCount, count);
				Thing thing = item.SplitOff(num);
				if (!this.TryAdd((T)((object)thing), canMergeWithExistingStacks))
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
				Log.Warning("Tried to add null item to ThingOwner.", false);
				result = false;
			}
			else
			{
				T t = item as T;
				if (t == null)
				{
					result = false;
				}
				else if (base.Contains(item))
				{
					Log.Warning("Tried to add " + item.ToStringSafe<Thing>() + " to ThingOwner but this item is already here.", false);
					result = false;
				}
				else if (item.holdingOwner != null)
				{
					Log.Warning(string.Concat(new string[]
					{
						"Tried to add ",
						item.ToStringSafe<Thing>(),
						" to ThingOwner but this thing is already in another container. owner=",
						this.owner.ToStringSafe<IThingHolder>(),
						", current container owner=",
						item.holdingOwner.Owner.ToStringSafe<IThingHolder>(),
						". Use TryAddOrTransfer, TryTransferToContainer, or remove the item before adding it."
					}), false);
					result = false;
				}
				else if (!base.CanAcceptAnyOf(item, canMergeWithExistingStacks))
				{
					result = false;
				}
				else
				{
					if (canMergeWithExistingStacks)
					{
						for (int i = 0; i < this.innerList.Count; i++)
						{
							T t2 = this.innerList[i];
							if (t2.CanStackWith(item))
							{
								int num = Mathf.Min(item.stackCount, t2.def.stackLimit - t2.stackCount);
								if (num > 0)
								{
									Thing other = item.SplitOff(num);
									int stackCount = t2.stackCount;
									t2.TryAbsorbStack(other, true);
									if (t2.stackCount > stackCount)
									{
										base.NotifyAddedAndMergedWith(t2, t2.stackCount - stackCount);
									}
									if (item.Destroyed || item.stackCount == 0)
									{
										return true;
									}
								}
							}
						}
					}
					if (this.Count >= this.maxStacks)
					{
						result = false;
					}
					else
					{
						item.holdingOwner = this;
						this.innerList.Add(t);
						base.NotifyAdded(t);
						result = true;
					}
				}
			}
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
						for (int i = 0; i < list.Count; i++)
						{
							if (!base.TryAddOrTransfer(list[i], canMergeWithExistingStacks) && destroyLeftover)
							{
								T t = list[i];
								t.Destroy(DestroyMode.Vanish);
							}
						}
					}
					else
					{
						foreach (T t2 in things)
						{
							if (!base.TryAddOrTransfer(t2, canMergeWithExistingStacks) && destroyLeftover)
							{
								t2.Destroy(DestroyMode.Vanish);
							}
						}
					}
				}
			}
		}

		public override int IndexOf(Thing item)
		{
			T t = item as T;
			int result;
			if (t == null)
			{
				result = -1;
			}
			else
			{
				result = this.innerList.IndexOf(t);
			}
			return result;
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
				int index = this.innerList.LastIndexOf((T)((object)item));
				this.innerList.RemoveAt(index);
				base.NotifyRemoved(item);
				result = true;
			}
			return result;
		}

		public int RemoveAll(Predicate<T> predicate)
		{
			int num = 0;
			for (int i = this.innerList.Count - 1; i >= 0; i--)
			{
				if (predicate(this.innerList[i]))
				{
					this.Remove(this.innerList[i]);
					num++;
				}
			}
			return num;
		}

		protected override Thing GetAt(int index)
		{
			return this.innerList[index];
		}

		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int stackCount, out T resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			Thing thing;
			int result = base.TryTransferToContainer(item, otherContainer, stackCount, out thing, canMergeWithExistingStacks);
			resultingTransferredItem = (T)((object)thing);
			return result;
		}

		public new T Take(Thing thing, int count)
		{
			return (T)((object)base.Take(thing, count));
		}

		public new T Take(Thing thing)
		{
			return (T)((object)base.Take(thing));
		}

		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, int count, out T resultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Action<Thing, int> placedAction2 = null;
			if (placedAction != null)
			{
				placedAction2 = delegate(Thing t, int c)
				{
					placedAction((T)((object)t), c);
				};
			}
			Thing thing2;
			bool result = base.TryDrop(thing, dropLoc, map, mode, count, out thing2, placedAction2, nearPlaceValidator);
			resultingThing = (T)((object)thing2);
			return result;
		}

		public bool TryDrop(Thing thing, ThingPlaceMode mode, out T lastResultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Action<Thing, int> placedAction2 = null;
			if (placedAction != null)
			{
				placedAction2 = delegate(Thing t, int c)
				{
					placedAction((T)((object)t), c);
				};
			}
			Thing thing2;
			bool result = base.TryDrop(thing, mode, out thing2, placedAction2, nearPlaceValidator);
			lastResultingThing = (T)((object)thing2);
			return result;
		}

		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out T lastResultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Action<Thing, int> placedAction2 = null;
			if (placedAction != null)
			{
				placedAction2 = delegate(Thing t, int c)
				{
					placedAction((T)((object)t), c);
				};
			}
			Thing thing2;
			bool result = base.TryDrop(thing, dropLoc, map, mode, out thing2, placedAction2, nearPlaceValidator);
			lastResultingThing = (T)((object)thing2);
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
			this.TryAdd(item, true);
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
			return this.Remove(item);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		[CompilerGenerated]
		private static bool <ExposeData>m__0(T x)
		{
			return x == null;
		}

		[CompilerGenerated]
		private sealed class <TryDrop>c__AnonStorey0
		{
			internal Action<T, int> placedAction;

			public <TryDrop>c__AnonStorey0()
			{
			}

			internal void <>m__0(Thing t, int c)
			{
				this.placedAction((T)((object)t), c);
			}
		}

		[CompilerGenerated]
		private sealed class <TryDrop>c__AnonStorey1
		{
			internal Action<T, int> placedAction;

			public <TryDrop>c__AnonStorey1()
			{
			}

			internal void <>m__0(Thing t, int c)
			{
				this.placedAction((T)((object)t), c);
			}
		}

		[CompilerGenerated]
		private sealed class <TryDrop>c__AnonStorey2
		{
			internal Action<T, int> placedAction;

			public <TryDrop>c__AnonStorey2()
			{
			}

			internal void <>m__0(Thing t, int c)
			{
				this.placedAction((T)((object)t), c);
			}
		}
	}
}
