using System;
using Entity.BehaviourTree;
using Playercontroller.Shooting;
using UnityEngine;
using Weapon;

namespace Entity.EnemyTree.EngagePlayer
{
    /// <summary>
    /// Class which handles the interaction with the enemy weapon and shoots at the player
    /// </summary>
    public class EnemyShoot
    {
        public static Action<Vector3> SpawnParticle;

        private EnemyWeapon _weapon;
        private WeaponInformation _weaponInformation;
        private TreeInfo _info;

        public EnemyShoot(ref TreeInfo info)
        {
            _weapon = info.EnemyWeapon;
            _weaponInformation = new WeaponInformation(LayerMask.NameToLayer("Player"), info.ThisEnemy.transform, info.HealthManager);
            _weapon.Awake(ref _weaponInformation);
            _info = info;
        }

        public void OnShoot()
        {
            _weapon.OnShoot();
        }

        public void Update()
        {
            _weapon.DirectionToPlayer = _info.Player.transform.position;
            if (_weapon.TryUpdateAndShoot(out Vector3 rayDirection))
            {
                SpawnParticle?.Invoke(rayDirection);
            }
        }

    }
}
