package md50267bfff25029ca0e0a85c6bf7f96414;


public class Registro
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("p2p_project.Resources.Model.Registro, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Registro.class, __md_methods);
	}


	public Registro () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Registro.class)
			mono.android.TypeManager.Activate ("p2p_project.Resources.Model.Registro, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
