package md55debec2523a555e8b74559a56bd3d108;


public class ClientSocket
	extends android.os.AsyncTask
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_doInBackground:([Ljava/lang/Object;)Ljava/lang/Object;:GetDoInBackground_arrayLjava_lang_Object_Handler\n" +
			"";
		mono.android.Runtime.register ("p2p_project.Resources.ClientSocket, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ClientSocket.class, __md_methods);
	}


	public ClientSocket () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ClientSocket.class)
			mono.android.TypeManager.Activate ("p2p_project.Resources.ClientSocket, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public ClientSocket (java.net.InetAddress p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == ClientSocket.class)
			mono.android.TypeManager.Activate ("p2p_project.Resources.ClientSocket, p2p project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Java.Net.InetAddress, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public java.lang.Object doInBackground (java.lang.Object[] p0)
	{
		return n_doInBackground (p0);
	}

	private native java.lang.Object n_doInBackground (java.lang.Object[] p0);

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
