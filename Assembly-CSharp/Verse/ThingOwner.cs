using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DF8 RID: 3576
	public class ThingOwner<T> : ThingOwner, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : Thing
	{
		// Token: 0x04003539 RID: 13625
		private List<T> innerList = new List<T>();

		// Token: 0x0600507B RID: 20603 RVA: 0x00299929 File Offset: 0x00297D29
		public ThingOwner()
		{
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x0029993D File Offset: 0x00297D3D
		public ThingOwner(IThingHolder owner) : base(owner)
		{
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x00299952 File Offset: 0x00297D52
		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep) : base(owner, oneStackOnly, contentsLookMode)
		{
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x0600507E RID: 20606 RVA: 0x0029996C File Offset: 0x00297D6C
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
		// (get) Token: 0x06005080 RID: 20608 RVA: 0x002999AC File Offset: 0x00297DAC
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
		// (get) Token: 0x06005083 RID: 20611 RVA: 0x002999FC File Offset: 0x00297DFC
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x00299A14 File Offset: 0x00297E14
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

		// Token: 0x06005085 RID: 20613 RVA: 0x00299ADC File Offset: 0x00297EDC
		public List<T>.Enumerator GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x00299AFC File Offset: 0x00297EFC
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

		// Token: 0x06005087 RID: 20615 RVA: 0x00299B2C File Offset: 0x00297F2C
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

		// Token: 0x06005088 RID: 20616 RVA: 0x00299C90 File Offset: 0x00298090
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

		// Token: 0x06005089 RID: 20617 RVA: 0x00299EB8 File Offset: 0x002982B8
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

		// Token: 0x0600508A RID: 20618 RVA: 0x00299FD0 File Offset: 0x002983D0
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

		// Token: 0x0600508B RID: 20619 RVA: 0x0029A010 File Offset: 0x00298410
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

		// Token: 0x0600508C RID: 20620 RVA: 0x0029A074 File Offset: 0x00298474
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

		// Token: 0x0600508D RID: 20621 RVA: 0x0029A0E4 File Offset: 0x002984E4
		protected override Thing GetAt(int index)
		{
			return this.innerList[index];
		}

		// Token: 0x0600508E RID: 20622 RVA: 0x0029A10C File Offset: 0x0029850C
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int stackCount, out T resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			Thing thing;
			int result = base.TryTransferToContainer(item, otherContainer, stackCount, out thing, canMergeWithExistingStacks);
			resultingTransferredItem = (T)((object)thing);
			return result;
		}

		// Token: 0x0600508F RID: 20623 RVA: 0x0029A140 File Offset: 0x00298540
		public new T Take(Thing thing, int count)
		{
			return (T)((object)base.Take(thing, count));
		}

		// Token: 0x06005090 RID: 20624 RVA: 0x0029A164 File Offset: 0x00298564
		public new T Take(Thing thing)
		{
			return (T)((object)base.Take(thing));
		}

		// Token: 0x06005091 RID: 20625 RVA: 0x0029A188 File Offset: 0x00298588
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

		// Token: 0x06005092 RID: 20626 RVA: 0x0029A1E8 File Offset: 0x002985E8
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

		// Token: 0x06005093 RID: 20627 RVA: 0x0029A244 File Offset: 0x00298644
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

		// Token: 0x06005094 RID: 20628 RVA: 0x0029A2A4 File Offset: 0x002986A4
		int IList<T>.IndexOf(T item)
		{
			return this.innerList.IndexOf(item);
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x0029A2C5 File Offset: 0x002986C5
		void IList<T>.Insert(int index, T item)
		{
			throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
		}

		// Token: 0x06005096 RID: 20630 RVA: 0x0029A2D2 File Offset: 0x002986D2
		void ICollection<T>.Add(T item)
		{
			this.TryAdd(item, true);
		}

		// Token: 0x06005097 RID: 20631 RVA: 0x0029A2E3 File Offset: 0x002986E3
		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		// Token: 0x06005098 RID: 20632 RVA: 0x0029A2F4 File Offset: 0x002986F4
		bool ICollection<T>.Contains(T item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x06005099 RID: 20633 RVA: 0x0029A318 File Offset: 0x00298718
		bool ICollection<T>.Remove(T item)
		{
			return this.Remove(item);
		}

		// Token: 0x0600509A RID: 20634 RVA: 0x0029A33C File Offset: 0x0029873C
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x0600509B RID: 20635 RVA: 0x0029A364 File Offset: 0x00298764
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}
	}
}
