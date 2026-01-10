using System;
using System.Collections.Generic;

namespace TosCore.TapBlocker
{
/// <summary>
/// ブロック要求の一覧とレイヤー状態を集約したテーブル。
/// </summary>
internal sealed class TapBlockRegistry : IDisposable
{
    private readonly Dictionary<TapBlockLayer, TapBlockLayerState> _layerStates = new();
    private readonly Dictionary<int, TapBlockLayerState> _requestIndex = new();
    private readonly List<TapBlockRequestSnapshot> _snapshots = new();
    private readonly Func<TapBlockLayer, TapBlockLayerState> _stateFactory;

    public TapBlockRegistry(Func<TapBlockLayer, TapBlockLayerState> stateFactory)
    {
        _stateFactory = stateFactory;
    }

    public IReadOnlyList<TapBlockRequestSnapshot> Snapshots => _snapshots;

    public TapBlockLayerState Acquire(TapBlockLayer layer, int requestId, string reason)
    {
        var state = GetOrCreate(layer);
        state.AddRequest(requestId);
        _requestIndex[requestId] = state;
        _snapshots.Add(new TapBlockRequestSnapshot(requestId, layer, reason ?? string.Empty));
        return state;
    }

    public void Release(int requestId)
    {
        if (!_requestIndex.TryGetValue(requestId, out var state)) return;

        _requestIndex.Remove(requestId);
        state.RemoveRequest(requestId);
        RemoveSnapshot(requestId);

        if (!state.HasRequests)
        {
            _layerStates.Remove(state.Layer);
            state.Dispose();
        }
    }

    public bool IsLayerBlocked(TapBlockLayer layer) =>
        _layerStates.TryGetValue(layer, out var state) && state.HasRequests;

    public void Dispose()
    {
        foreach (var state in _layerStates.Values)
        {
            state.Dispose();
        }

        _layerStates.Clear();
        _requestIndex.Clear();
        _snapshots.Clear();
    }

    private TapBlockLayerState GetOrCreate(TapBlockLayer layer)
    {
        if (_layerStates.TryGetValue(layer, out var existing)) return existing;

        var state = _stateFactory.Invoke(layer);
        _layerStates.Add(layer, state);
        return state;
    }

    private void RemoveSnapshot(int requestId)
    {
        for (var i = 0; i < _snapshots.Count; i++)
        {
            if (_snapshots[i].Id != requestId) continue;
            _snapshots.RemoveAt(i);
            return;
        }
    }
}
}