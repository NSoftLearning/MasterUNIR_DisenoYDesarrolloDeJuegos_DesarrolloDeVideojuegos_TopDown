using System;
using UnityEngine;

public interface ITrap
{
    public void Activate();
    public void Deactivate();
    public event Action OnActivate;
    public event Action OnDeactivate;
}
