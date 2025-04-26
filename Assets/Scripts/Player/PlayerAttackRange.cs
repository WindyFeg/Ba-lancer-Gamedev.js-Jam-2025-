using System;
using UnityEngine;

namespace Game
{
    public class PlayerAttackRange : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer attackRangeSprite;
        private float _defaultScaleTransform;

        private void Awake()
        {
            // Get the default scale of the sprite
            _defaultScaleTransform = attackRangeSprite.transform.localScale.x;
        }
        
        public void SetAttackRange(float range)
        {
            // Set the scale of the sprite to match the attack range
            attackRangeSprite.transform.localScale = new Vector3(_defaultScaleTransform * range, _defaultScaleTransform * range, 1);
        }
    }
}