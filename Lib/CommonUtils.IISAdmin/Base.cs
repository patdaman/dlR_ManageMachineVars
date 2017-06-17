using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public abstract class Base : IDisposable
{
    private bool disposed = false;
    private string instanceName;
    private List<object> trackingList;

    public Base(string instanceName, List<object> tracking)
    {
        this.instanceName = instanceName;
        trackingList = tracking;
        trackingList.Add(this);
    }

    public string InstanceName
    {
        get
        {
            return instanceName;
        }
    }

    //Implement IDisposable.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // Free other state (managed objects).
                trackingList.Remove(this);
            }
            disposed = true;
        }
    }

    // Use C# destructor syntax for finalization code.
    ~Base()
    {
        Dispose(false);
    }
}
