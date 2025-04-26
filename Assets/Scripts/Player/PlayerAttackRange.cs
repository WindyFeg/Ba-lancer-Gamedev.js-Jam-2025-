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
            _defaultScaleTransform = 2f;
        }

        public void SetAttackRange(float range)
        {
            attackRangeSprite.transform.localScale = new Vector3(_defaultScaleTransform * range, _defaultScaleTransform * range, 1);
        }
    }
}