using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.FogOfWar
{
    public interface IFOWShowable
    {
        public bool CanShow();
        public bool CanHide();

        public void RequestSetCanShow(bool value);
        public void SyncSetCanShowRPC(bool value);
        public void SetCanShowRPC(bool value);
        public event GlobalDelegates.BoolDelegate OnSetCanShow;
        public event GlobalDelegates.BoolDelegate OnSetCanShowFeedback;
        public void RequestSetCanHide(bool value);
        public void SyncSetCanHideRPC(bool value);
        public void SetCanHideRPC(bool value);
        public event GlobalDelegates.BoolDelegate OnSetCanHide;
        public event GlobalDelegates.BoolDelegate OnSetCanHideFeedback;
        public void TryAddFOWViewable(int viewableIndex);
        public void TryAddFOWViewable(Entity viewable);
        public void SyncTryAddViewableRPC(int viewableIndex,bool show);
        public void ShowElements();
        public event GlobalDelegates.NoParameterDelegate OnShowElement;
        public event GlobalDelegates.NoParameterDelegate OnShowElementFeedback;

        public void TryRemoveFOWViewable(int viewableIndex);
        public void TryRemoveFOWViewable(IFOWViewable viewable);
        public void SyncTryRemoveViewableRPC(int viewableIndex,bool hide);
        public void HideElements();
        public event GlobalDelegates.NoParameterDelegate OnHideElement;
        public event GlobalDelegates.NoParameterDelegate OnHideElementFeedback;

    }
}