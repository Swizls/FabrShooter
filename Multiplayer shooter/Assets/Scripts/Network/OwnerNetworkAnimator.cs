using Unity.Netcode.Components;
using UnityEngine;

public class OwnerNetwork : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
