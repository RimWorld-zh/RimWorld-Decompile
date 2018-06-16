using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DF9 RID: 3577
	public class ThingOwner<T> : ThingOwner, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : Thing
	{
		// Token: 0x06005065 RID: 20581 RVA: 0x00297F61 File Offset: 0x00296361
		public ThingOwner()
		{
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x00297F75 File Offset: 0x00296375
		public ThingOwner(IThingHolder owner) : base(owner)
		{
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x00297F8A File Offset: 0x0029638A
		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep) : base(owner, oneStackOnly, contentsLookMode)
		{
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06005068 RID: 20584 RVA: 0x00297FA4 File Offset: 0x002963A4
		public List<T> InnerListForReading
		{
			get
			{
				return this.innerList;
			}
		}

		// Token: 0x17000D31 RID: 3377
		public new T this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x0600506A RID: 20586 RVA: 0x00297FE4 File Offset: 0x002963E4
		public override int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x17000D2E RID: 3374
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

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x0600506D RID: 20589 RVA: 0x00298034 File Offset: 0x00296434
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600506E RID: 20590 RVA: 0x0029804C File Offset: 0x0029644C
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

		// Token: 0x0600506F RID: 20591 RVA: 0x00298114 File Offset: 0x00296514
		public List<T>.Enumerator GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x06005070 RID: 20592 RVA: 0x00298134 File Offset: 0x00296534
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

		// Token: 0x06005071 RID: 20593 RVA: 0x00298164 File Offset: 0x00296564
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

		// Token: 0x06005072 RID: 20594 RVA: 0x002982C8 File Offset: 0x002966C8
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

		// Token: 0x06005073 RID: 20595 RVA: 0x002984F0 File Offset: 0x002968F0
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

		// Token: 0x06005074 RID: 20596 RVA: 0x00298608 File Offset: 0x00296A08
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

		// Token: 0x06005075 RID: 20597 RVA: 0x00298648 File Offset: 0x00296A48
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

		// Token: 0x06005076 RID: 20598 RVA: 0x002986AC File Offset: 0x00296AAC
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

		// Token: 0x06005077 RID: 20599 RVA: 0x0029871C File Offset: 0x00296B1C
		protected override Thing GetAt(int index)
		{
			return this.innerList[index];
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x00298744 File Offset: 0x00296B44
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int stackCount, out T resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			Thing thing;
			int result = base.TryTransferToContainer(item, otherContainer, stackCount, out thing, canMergeWithExistingStacks);
			resultingTransferredItem = (T)((object)thing);
			return result;
		}

		// Token: 0x06005079 RID: 20601 RVA: 0x00298778 File Offset: 0x00296B78
		public new T Take(Thing thing, int count)
		{
			return (T)((object)base.Take(thing, count));
		}

		// Token: 0x0600507A RID: 20602 RVA: 0x0029879C File Offset: 0x00296B9C
		public new T Take(Thing thing)
		{
			return (T)((object)base.Take(thing));
		}

		// Token: 0x0600507B RID: 20603 RVA: 0x002987C0 File Offset: 0x00296BC0
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

		// Token: 0x0600507C RID: 20604 RVA: 0x00298820 File Offset: 0x00296C20
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

		// Token: 0x0600507D RID: 20605 RVA: 0x0029887C File Offset: 0x00296C7C
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

		// Token: 0x0600507E RID: 20606 RVA: 0x002988DC File Offset: 0x00296CDC
		int IList<T>.IndexOf(T item)
		{
			return this.innerList.IndexOf(item);
		}

		// Token: 0x0600507F RID: 20607 RVA: 0x002988FD File Offset: 0x00296CFD
		void IList<T>.Insert(int index, T item)
		{
			throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x0029890A File Offset: 0x00296D0A
		void ICollection<T>.Add(T item)
		{
			this.TryAdd(item, true);
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x0029891B File Offset: 0x00296D1B
		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x0029892C File Offset: 0x00296D2C
		bool ICollection<T>.Contains(T item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x00298950 File Offset: 0x00296D50
		bool ICollection<T>.Remove(T item)
		{
			return this.Remove(item);
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x00298974 File Offset: 0x00296D74
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x0029899C File Offset: 0x00296D9C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x0400352D RID: 13613
		private List<T> innerList = new List<T>();
	}
}
