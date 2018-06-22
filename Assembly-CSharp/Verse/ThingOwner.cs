using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DF5 RID: 3573
	public class ThingOwner<T> : ThingOwner, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : Thing
	{
		// Token: 0x06005077 RID: 20599 RVA: 0x0029951D File Offset: 0x0029791D
		public ThingOwner()
		{
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x00299531 File Offset: 0x00297931
		public ThingOwner(IThingHolder owner) : base(owner)
		{
		}

		// Token: 0x06005079 RID: 20601 RVA: 0x00299546 File Offset: 0x00297946
		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep) : base(owner, oneStackOnly, contentsLookMode)
		{
		}

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x0600507A RID: 20602 RVA: 0x00299560 File Offset: 0x00297960
		public List<T> InnerListForReading
		{
			get
			{
				return this.innerList;
			}
		}

		// Token: 0x17000D32 RID: 3378
		public new T this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x0600507C RID: 20604 RVA: 0x002995A0 File Offset: 0x002979A0
		public override int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x17000D2F RID: 3375
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

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x0600507F RID: 20607 RVA: 0x002995F0 File Offset: 0x002979F0
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x00299608 File Offset: 0x00297A08
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

		// Token: 0x06005081 RID: 20609 RVA: 0x002996D0 File Offset: 0x00297AD0
		public List<T>.Enumerator GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x002996F0 File Offset: 0x00297AF0
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

		// Token: 0x06005083 RID: 20611 RVA: 0x00299720 File Offset: 0x00297B20
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

		// Token: 0x06005084 RID: 20612 RVA: 0x00299884 File Offset: 0x00297C84
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

		// Token: 0x06005085 RID: 20613 RVA: 0x00299AAC File Offset: 0x00297EAC
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

		// Token: 0x06005086 RID: 20614 RVA: 0x00299BC4 File Offset: 0x00297FC4
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

		// Token: 0x06005087 RID: 20615 RVA: 0x00299C04 File Offset: 0x00298004
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

		// Token: 0x06005088 RID: 20616 RVA: 0x00299C68 File Offset: 0x00298068
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

		// Token: 0x06005089 RID: 20617 RVA: 0x00299CD8 File Offset: 0x002980D8
		protected override Thing GetAt(int index)
		{
			return this.innerList[index];
		}

		// Token: 0x0600508A RID: 20618 RVA: 0x00299D00 File Offset: 0x00298100
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int stackCount, out T resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			Thing thing;
			int result = base.TryTransferToContainer(item, otherContainer, stackCount, out thing, canMergeWithExistingStacks);
			resultingTransferredItem = (T)((object)thing);
			return result;
		}

		// Token: 0x0600508B RID: 20619 RVA: 0x00299D34 File Offset: 0x00298134
		public new T Take(Thing thing, int count)
		{
			return (T)((object)base.Take(thing, count));
		}

		// Token: 0x0600508C RID: 20620 RVA: 0x00299D58 File Offset: 0x00298158
		public new T Take(Thing thing)
		{
			return (T)((object)base.Take(thing));
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x00299D7C File Offset: 0x0029817C
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

		// Token: 0x0600508E RID: 20622 RVA: 0x00299DDC File Offset: 0x002981DC
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

		// Token: 0x0600508F RID: 20623 RVA: 0x00299E38 File Offset: 0x00298238
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

		// Token: 0x06005090 RID: 20624 RVA: 0x00299E98 File Offset: 0x00298298
		int IList<T>.IndexOf(T item)
		{
			return this.innerList.IndexOf(item);
		}

		// Token: 0x06005091 RID: 20625 RVA: 0x00299EB9 File Offset: 0x002982B9
		void IList<T>.Insert(int index, T item)
		{
			throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x00299EC6 File Offset: 0x002982C6
		void ICollection<T>.Add(T item)
		{
			this.TryAdd(item, true);
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x00299ED7 File Offset: 0x002982D7
		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x00299EE8 File Offset: 0x002982E8
		bool ICollection<T>.Contains(T item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x00299F0C File Offset: 0x0029830C
		bool ICollection<T>.Remove(T item)
		{
			return this.Remove(item);
		}

		// Token: 0x06005096 RID: 20630 RVA: 0x00299F30 File Offset: 0x00298330
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x06005097 RID: 20631 RVA: 0x00299F58 File Offset: 0x00298358
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x04003532 RID: 13618
		private List<T> innerList = new List<T>();
	}
}
