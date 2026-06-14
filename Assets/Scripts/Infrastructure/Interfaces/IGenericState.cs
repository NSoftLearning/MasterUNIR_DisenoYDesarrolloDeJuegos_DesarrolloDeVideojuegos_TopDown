using System;
using UnityEngine;

public interface IGenericState<TStateId> where TStateId : Enum
{
    TStateId StateId { get; }

    void Enter();
    void Exit();
    void Tick();
}
