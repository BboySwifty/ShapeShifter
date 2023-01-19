using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu]
public class ShapeShiftSO : ScriptableObject
{
    public float speedModifier;
    public float jumpModifier;
    public float gravityScale;
    public bool canWallJump;
    public float wallSlidingSpeed = -1;
    public bool isInvisible = false;
    public bool spritesAreReversed;
    public SpriteLibraryAsset spriteLibraryAsset;
}
