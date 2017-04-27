package md55debec2523a555e8b74559a56bd3d108;


public class PeerListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.net.wifi.p2p.WifiP2pManager.PeerListListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPeersAvailable:(Landroid/net/wifi/p2p/WifiP2pDeviceList;)V:GetOnPeersAvailable_Landroid_net_wifi_p2p_WifiP2pDeviceList_Handler:Android.Net.Wifi.P2p.WifiP2pManager/IPeerListListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("p2p_project.Resources.PeerListener, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PeerListener.class, __md_methods);
	}


	public PeerListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PeerListener.class)
			mono.android.TypeManager.Activate ("p2p_project.Resources.PeerListener, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onPeersAvailable (android.net.wifi.p2p.WifiP2pDeviceList p0)
	{
		n_onPeersAvailable (p0);
	}

	private native void n_onPeersAvailable (android.net.wifi.p2p.WifiP2pDeviceList p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
