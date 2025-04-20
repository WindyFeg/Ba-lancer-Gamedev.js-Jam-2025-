using UnityEngine;

namespace Enemy
{
    public class MeleeOrc : IMonster
    {
        
        public override void OnIdle()
        {
            throw new System.NotImplementedException();
        }
        
        public override void OnAttack()
        {
            throw new System.NotImplementedException();
        }

        public override void OnTrigger()
        {
            throw new System.NotImplementedException();
        }
        
        public override void OnDead()
        {
            Destroy(gameObject);
        }
    }
}