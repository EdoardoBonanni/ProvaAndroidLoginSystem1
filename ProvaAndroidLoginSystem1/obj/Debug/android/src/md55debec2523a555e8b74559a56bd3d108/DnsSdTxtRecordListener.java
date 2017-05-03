package md55debec2523a555e8b74559a56bd3d108;


public class DnsSdTxtRecordListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.net.wifi.p2p.WifiP2pManager.DnsSdTxtRecordListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDnsSdTxtRecordAvailable:(Ljava/lang/String;Ljava/util/Map;Landroid/net/wifi/p2p/WifiP2pDevice;)V:GetOnDnsSdTxtRecordAvailable_Ljava_lang_String_Ljava_util_Map_Landroid_net_wifi_p2p_WifiP2pDevice_Handler:Android.Net.Wifi.P2p.WifiP2pManager/IDnsSdTxtRecordListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("p2p_project.Resources.DnsSdTxtRecordListener, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DnsSdTxtRecordListener.class, __md_methods);
	}


	public DnsSdTxtRecordListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DnsSdTxtRecordListener.class)
			mono.android.TypeManager.Activate ("p2p_project.Resources.DnsSdTxtRecordListener, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onDnsSdTxtRecordAvailable (java.lang.String p0, java.util.Map p1, android.net.wifi.p2p.WifiP2pDevice p2)
	{
		n_onDnsSdTxtRecordAvailable (p0, p1, p2);
	}

	private native void n_onDnsSdTxtRecordAvailable (java.lang.String p0, java.util.Map p1, android.net.wifi.p2p.WifiP2pDevice p2);

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
