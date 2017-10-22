using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public sealed class PawnDestinationManager
	{
		private Dictionary<Faction, Dictionary<Pawn, IntVec3>> reservedDestinations = new Dictionary<Faction, Dictionary<Pawn, IntVec3>>();

		private static readonly Material DestinationMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestination");

		private static readonly Material DestinationSelectionMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestinationSelection");

		public PawnDestinationManager()
		{
			foreach (Faction allFaction in Find.FactionManager.AllFactions)
			{
				this.RegisterFaction(allFaction);
			}
		}

		public void RegisterFaction(Faction faction)
		{
			this.reservedDestinations.Add(faction, new Dictionary<Pawn, IntVec3>());
		}

		public void ReserveDestinationFor(Pawn p, IntVec3 loc)
		{
			if (p.Faction != null)
			{
				Pawn pawn = this.ReserverOfDestinationForFaction(loc, p.Faction);
				if (pawn != null && pawn != p)
					return;
				this.reservedDestinations[p.Faction][p] = loc;
			}
		}

		public IntVec3 DestinationReservedFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return IntVec3.Invalid;
			}
			if (this.reservedDestinations[p.Faction].ContainsKey(p))
			{
				return this.reservedDestinations[p.Faction][p];
			}
			return IntVec3.Invalid;
		}

		public bool DestinationIsReserved(IntVec3 loc)
		{
			Dictionary<Faction, Dictionary<Pawn, IntVec3>>.Enumerator enumerator = this.reservedDestinations.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Dictionary<Pawn, IntVec3>.Enumerator enumerator2 = enumerator.Current.Value.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.Value == loc)
							{
								return true;
							}
						}
					}
					finally
					{
						((IDisposable)(object)enumerator2).Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return false;
		}

		public bool DestinationIsReserved(IntVec3 c, Pawn searcher)
		{
			if (searcher.Faction == null)
			{
				return false;
			}
			Dictionary<Pawn, IntVec3>.Enumerator enumerator = this.reservedDestinations[searcher.Faction].GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Pawn, IntVec3> current = enumerator.Current;
					if (current.Value == c && current.Key != searcher)
					{
						return true;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return false;
		}

		private Pawn ReserverOfDestinationForFaction(IntVec3 c, Faction faction)
		{
			if (faction == null)
			{
				return null;
			}
			Dictionary<Pawn, IntVec3>.Enumerator enumerator = this.reservedDestinations[faction].GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Pawn, IntVec3> current = enumerator.Current;
					if (current.Value == c)
					{
						return current.Key;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return null;
		}

		public void UnreserveAllFor(Pawn p)
		{
			if (p.Faction != null)
			{
				this.reservedDestinations[p.Faction].Remove(p);
			}
		}

		public void DebugDrawDestinations()
		{
			Dictionary<Pawn, IntVec3>.Enumerator enumerator = this.reservedDestinations[Faction.OfPlayer].GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Pawn, IntVec3> current = enumerator.Current;
					if (!(current.Key.Position == current.Value))
					{
						IntVec3 value = current.Value;
						Vector3 s = new Vector3(1f, 1f, 1f);
						Matrix4x4 matrix = default(Matrix4x4);
						matrix.SetTRS(value.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, s);
						Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationManager.DestinationMat, 0);
						if (Find.Selector.IsSelected(current.Key))
						{
							Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationManager.DestinationSelectionMat, 0);
						}
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}
	}
}
