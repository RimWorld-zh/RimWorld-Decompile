using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Steamworks
{
	public sealed class CallResult<T>
	{
		private CCallbackBaseVTable VTable;

		private IntPtr m_pVTable = IntPtr.Zero;

		private CCallbackBase m_CCallbackBase;

		private GCHandle m_pCCallbackBase;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private CallResult<T>.APIDispatchDelegate m_Func;

		private SteamAPICall_t m_hAPICall = SteamAPICall_t.Invalid;

		private readonly int m_size = Marshal.SizeOf(typeof(T));

		public CallResult(CallResult<T>.APIDispatchDelegate func = null)
		{
			this.m_Func = func;
			this.BuildCCallbackBase();
		}

		private event CallResult<T>.APIDispatchDelegate m_Func
		{
			add
			{
				CallResult<T>.APIDispatchDelegate apidispatchDelegate = this.m_Func;
				CallResult<T>.APIDispatchDelegate apidispatchDelegate2;
				do
				{
					apidispatchDelegate2 = apidispatchDelegate;
					apidispatchDelegate = Interlocked.CompareExchange<CallResult<T>.APIDispatchDelegate>(ref this.m_Func, (CallResult<T>.APIDispatchDelegate)Delegate.Combine(apidispatchDelegate2, value), apidispatchDelegate);
				}
				while (apidispatchDelegate != apidispatchDelegate2);
			}
			remove
			{
				CallResult<T>.APIDispatchDelegate apidispatchDelegate = this.m_Func;
				CallResult<T>.APIDispatchDelegate apidispatchDelegate2;
				do
				{
					apidispatchDelegate2 = apidispatchDelegate;
					apidispatchDelegate = Interlocked.CompareExchange<CallResult<T>.APIDispatchDelegate>(ref this.m_Func, (CallResult<T>.APIDispatchDelegate)Delegate.Remove(apidispatchDelegate2, value), apidispatchDelegate);
				}
				while (apidispatchDelegate != apidispatchDelegate2);
			}
		}

		public SteamAPICall_t Handle
		{
			get
			{
				return this.m_hAPICall;
			}
		}

		public static CallResult<T> Create(CallResult<T>.APIDispatchDelegate func = null)
		{
			return new CallResult<T>(func);
		}

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

		public bool IsActive()
		{
			return this.m_hAPICall != SteamAPICall_t.Invalid;
		}

		public void Cancel()
		{
			if (this.m_hAPICall != SteamAPICall_t.Invalid)
			{
				NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong)this.m_hAPICall);
				this.m_hAPICall = SteamAPICall_t.Invalid;
			}
		}

		public void SetGameserverFlag()
		{
			CCallbackBase ccallbackBase = this.m_CCallbackBase;
			ccallbackBase.m_nCallbackFlags |= 2;
		}

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

		private int OnGetCallbackSizeBytes(IntPtr thisptr)
		{
			return this.m_size;
		}

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

		public delegate void APIDispatchDelegate(T param, bool bIOFailure);
	}
}
