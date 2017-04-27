package md5869785c10f580142a1973b002e482efd;


public class PeersAdapterViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("p2p_project.PeersAdapterViewHolder, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PeersAdapterViewHolder.class, __md_methods);
	}


	public PeersAdapterViewHolder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PeersAdapterViewHolder.class)
			mono.android.TypeManager.Activate ("p2p_project.PeersAdapterViewHolder, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
