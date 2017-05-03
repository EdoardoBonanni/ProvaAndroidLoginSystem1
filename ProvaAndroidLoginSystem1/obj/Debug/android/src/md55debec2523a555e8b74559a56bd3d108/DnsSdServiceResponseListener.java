package md55debec2523a555e8b74559a56bd3d108;


public class DnsSdServiceResponseListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.net.wifi.p2p.WifiP2pManager.DnsSdServiceResponseListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDnsSdServiceAvailable:(Ljava/lang/String;Ljava/lang/String;Landroid/net/wifi/p2p/WifiP2pDevice;)V:GetOnDnsSdServiceAvailable_Ljava_lang_String_Ljava_lang_String_Landroid_net_wifi_p2p_WifiP2pDevice_Handler:Android.Net.Wifi.P2p.WifiP2pManager/IDnsSdServiceResponseListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("p2p_project.Resources.DnsSdServiceResponseListener, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DnsSdServiceResponseListener.class, __md_methods);
	}


	public DnsSdServiceResponseListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DnsSdServiceResponseListener.class)
			mono.android.TypeManager.Activate ("p2p_project.Resources.DnsSdServiceResponseListener, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDnsSdServiceAvailable (java.lang.String p0, java.lang.String p1, android.net.wifi.p2p.WifiP2pDevice p2)
	{
		n_onDnsSdServiceAvailable (p0, p1, p2);
	}

	private native void n_onDnsSdServiceAvailable (java.lang.String p0, java.lang.String p1, android.net.wifi.p2p.WifiP2pDevice p2);

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
