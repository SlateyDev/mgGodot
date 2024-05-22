using System;

namespace HexadoniaApp;

public class Ref<T>
{
    private readonly object _valRef;

    public Ref(ref T valRef)
    {
        _valRef = valRef;
    }

    public object GetValRef
    {
        get { return _valRef; }
    }
}