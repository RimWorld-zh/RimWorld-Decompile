using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F36 RID: 3894
	public static class GenDebug
	{
		// Token: 0x06005DE7 RID: 24039 RVA: 0x002FC1BC File Offset: 0x002FA5BC
		public static void DebugPlaceSphere(Vector3 Loc, float Scale)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.transform.position = Loc;
			gameObject.transform.localScale = new Vector3(Scale, Scale, Scale);
		}

		// Token: 0x06005DE8 RID: 24040 RVA: 0x002FC1F0 File Offset: 0x002FA5F0
		public static void LogList<T>(IEnumerable<T> list)
		{
			foreach (T t in list)
			{
				Log.Message("    " + t.ToString(), false);
			}
		}

		// Token: 0x06005DE9 RID: 24041 RVA: 0x002FC260 File Offset: 0x002FA660
		public static void ClearArea(CellRect r, Map map)
		{
			r.ClipInsideMap(map);
			foreach (IntVec3 c in r)
			{
				map.roofGrid.SetRoof(c, null);
			}
			foreach (IntVec3 c2 in r)
			{
				foreach (Thing thing in c2.GetThingList(map).ToList<Thing>())
				{
					if (thing.def.destroyable)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}
	}
}
