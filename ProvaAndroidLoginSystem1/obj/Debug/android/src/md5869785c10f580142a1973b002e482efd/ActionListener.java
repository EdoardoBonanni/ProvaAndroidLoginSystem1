package md5869785c10f580142a1973b002e482efd;


public class ActionListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.net.wifi.p2p.WifiP2pManager.ActionListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onFailure:(I)V:GetOnFailure_IHandler:Android.Net.Wifi.P2p.WifiP2pManager/IActionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onSuccess:()V:GetOnSuccessHandler:Android.Net.Wifi.P2p.WifiP2pManager/IActionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("p2p_project.ActionListener, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ActionListener.class, __md_methods);
	}


	public ActionListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ActionListener.class)
			mono.android.TypeManager.Activate ("p2p_project.ActionListener, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public ActionListener (java.lang.String p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == ActionListener.class)
			mono.android.TypeManager.Activate ("p2p_project.ActionListener, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.String, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public void onFailure (int p0)
	{
		n_onFailure (p0);
	}

	private native void n_onFailure (int p0);


	public void onSuccess ()
	{
		n_onSuccess ();
	}

	private native void n_onSuccess ();

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
