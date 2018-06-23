using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000005 RID: 5
	public sealed class CallResult<T>
	{
		// Token: 0x04000008 RID: 8
		private CCallbackBaseVTable VTable;

		// Token: 0x04000009 RID: 9
		private IntPtr m_pVTable = IntPtr.Zero;

		// Token: 0x0400000A RID: 10
		private CCallbackBase m_CCallbackBase;

		// Token: 0x0400000B RID: 11
		private GCHandle m_pCCallbackBase;

		// Token: 0x0400000D RID: 13
		private SteamAPICall_t m_hAPICall = SteamAPICall_t.Invalid;

		// Token: 0x0400000E RID: 14
		private readonly int m_size = Marshal.SizeOf(typeof(T));

		// Token: 0x06000013 RID: 19 RVA: 0x000023DC File Offset: 0x000005DC
		public CallResult(CallResult<T>.APIDispatchDelegate func = null)
		{
			this.m_Func = func;
			this.BuildCCallbackBase();
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000014 RID: 20 RVA: 0x00002428 File Offset: 0x00000628
		// (remove) Token: 0x06000015 RID: 21 RVA: 0x00002460 File Offset: 0x00000660
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event CallResult<T>.APIDispatchDelegate m_Func;

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002498 File Offset: 0x00000698
		public SteamAPICall_t Handle
		{
			get
			{
				return this.m_hAPICall;
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000024B4 File Offset: 0x000006B4
		public static CallResult<T> Create(CallResult<T>.APIDispatchDelegate func = null)
		{
			return new CallResult<T>(func);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000024D0 File Offset: 0x000006D0
		~CallResult()
		{
			this.Cancel();
			if (this.m_pVTable != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pVTable);
			}
			if (this.m_pCCallbackBase.IsAllocated)
			{
				this.m_pCCallbackBase.Free();
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002540 File Offset: 0x00000740
		public void Set(SteamAPICall_t hAPICall, CallResult<T>.APIDispatchDelegate func = null)
		{
			if (func != null)
			{
				this.m_Func = func;
			}
			if (this.m_Func == null)
			{
				throw new Exception("CallResult function was null, you must either set it in the CallResult Constructor or in Set()");
			}
			if (this.m_hAPICall != SteamAPICall_t.Invalid)
			{
				NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong)this.m_hAPICall);
			}
			this.m_hAPICall = hAPICall;
			if (hAPICall != SteamAPICall_t.Invalid)
			{
				NativeMethods.SteamAPI_RegisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong)hAPICall);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000025D8 File Offset: 0x000007D8
		public bool IsActive()
		{
			return this.m_hAPICall != SteamAPICall_t.Invalid;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000025FD File Offset: 0x000007FD
		public void Cancel()
		{
			if (this.m_hAPICall != SteamAPICall_t.Invalid)
			{
				NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong)this.m_hAPICall);
				this.m_hAPICall = SteamAPICall_t.Invalid;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000263D File Offset: 0x0000083D
		public void SetGameserverFlag()
		{
			CCallbackBase ccallbackBase = this.m_CCallbackBase;
			ccallbackBase.m_nCallbackFlags |= 2;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002654 File Offset: 0x00000854
		private void OnRunCallback(IntPtr thisptr, IntPtr pvParam)
		{
			this.m_hAPICall = SteamAPICall_t.Invalid;
			try
			{
				this.m_Func((T)((object)Marshal.PtrToStructure(pvParam, typeof(T))), false);
			}
			catch (Exception e)
			{
				CallbackDispatcher.ExceptionHandler(e);
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000026B4 File Offset: 0x000008B4
		private void OnRunCallResult(IntPtr thisptr, IntPtr pvParam, bool bFailed, ulong hSteamAPICall)
		{
			SteamAPICall_t x = (SteamAPICall_t)hSteamAPICall;
			if (x == this.m_hAPICall)
			{
				try
				{
					this.m_Func((T)((object)Marshal.PtrToStructure(pvParam, typeof(T))), bFailed);
				}
				catch (Exception e)
				{
					CallbackDispatcher.ExceptionHandler(e);
				}
				if (x == this.m_hAPICall)
				{
					this.m_hAPICall = SteamAPICall_t.Invalid;
				}
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002744 File Offset: 0x00000944
		private int OnGetCallbackSizeBytes(IntPtr thisptr)
		{
			return this.m_size;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002760 File Offset: 0x00000960
		private void BuildCCallbackBase()
		{
			this.VTable = new CCallbackBaseVTable
			{
				m_RunCallback = new CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
				m_RunCallResult = new CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
				m_GetCallbackSizeBytes = new CCallbackBaseVTable.GetCallbackSizeBytesDel(this.OnGetCallbackSizeBytes)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CCallbackBaseVTable)));
			Marshal.StructureToPtr(this.VTable, this.m_pVTable, false);
			this.m_CCallbackBase = new CCallbackBase
			{
				m_vfptr = this.m_pVTable,
				m_nCallbackFlags = 0,
				m_iCallback = CallbackIdentities.GetCallbackIdentity(typeof(T))
			};
			this.m_pCCallbackBase = GCHandle.Alloc(this.m_CCallbackBase, GCHandleType.Pinned);
		}

		// Token: 0x02000006 RID: 6
		// (Invoke) Token: 0x06000022 RID: 34
		public delegate void APIDispatchDelegate(T param, bool bIOFailure);
	}
}
