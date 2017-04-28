package md55debec2523a555e8b74559a56bd3d108;


public class ConnectionInfoListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.net.wifi.p2p.WifiP2pManager.ConnectionInfoListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onConnectionInfoAvailable:(Landroid/net/wifi/p2p/WifiP2pInfo;)V:GetOnConnectionInfoAvailable_Landroid_net_wifi_p2p_WifiP2pInfo_Handler:Android.Net.Wifi.P2p.WifiP2pManager/IConnectionInfoListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("p2p_project.Resources.ConnectionInfoListener, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ConnectionInfoListener.class, __md_methods);
	}


	public ConnectionInfoListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ConnectionInfoListener.class)
			mono.android.TypeManager.Activate ("p2p_project.Resources.ConnectionInfoListener, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public ConnectionInfoListener (md587ad3ac64fbc991d5ecd8dfeb9429049.MainActivity p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == ConnectionInfoListener.class)
			mono.android.TypeManager.Activate ("p2p_project.Resources.ConnectionInfoListener, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "ProvaAndroidLoginSystem1.MainActivity, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onConnectionInfoAvailable (android.net.wifi.p2p.WifiP2pInfo p0)
	{
		n_onConnectionInfoAvailable (p0);
	}

	private native void n_onConnectionInfoAvailable (android.net.wifi.p2p.WifiP2pInfo p0);

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
