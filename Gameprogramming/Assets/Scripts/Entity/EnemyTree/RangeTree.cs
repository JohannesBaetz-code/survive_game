using System;
using System.Collections.Generic;
using Entity.BehaviourTree;
using Entity.EnemyTree.EngagePlayer;
using Entity.EnemyTree.FollowPlayer;
using Entity.EnemyTree.TreeFOV;
using Entity.EnemyTree.Walk;
using UnityEngine;
using Tree = Entity.BehaviourTree.Tree;

namespace Entity.EnemyTree
{
    /// <summary>
    /// The class from which the instances are attached to gameobjects. These become rangeenemys afterwords.
    /// </summary>
    public class RangeTree : Tree
    {
        private void OnEnable()
        {
            EnemyShoot.SpawnParticle += SpawnParticle;
        }

        /// <summary>
        /// setup the behaviour tree. In lists you set the child nodes.
        /// </summary>
        /// <returns></returns>
        protected override Node SetupTree()
        {
            return new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new AttackPlayer(ref _info),
                    new StopGoing(ref _info)
                }),
                new Selector(new List<Node>
                {
                    new DetectBulletRange(_info),
                    new DetectPlayer(ref _info,
                        new Selector(new List<Node>
                        {
                            new DefinePlayerAttackRange(ref _info,
                                new DetectPlayerAttackRange(ref _info)),
                            //new DetectObstacle(ref _info),
                            new DetectPlayerFollow(ref _info)
                        }))
                }),
                new Sequence(new List<Node>
                {
                    new CanFollowPlayer(_info),
                    new EngagePlayer.FollowPlayer(_info)
                }),
                new WalkBetweenPoints(ref _info)
            });
        }

        /// <summary>
        /// method to spawn the particle effects in the direction where the enemies are shooting.
        /// </summary>
        /// <param name="direction"></param>
        public void SpawnParticle(Vector3 direction)
        {
            if (direction == Vector3.zero)
                direction = (_info.Player.transform.position - gameObject.transform.position).normalized;
            GameObject particle = Instantiate(_info.EnemyWeapon.WeaponParticle);
            particle.transform.position = _info.EnemyWeapon.WeaponObject.transform.position;
            particle.GetComponent<Rigidbody>()
                .AddForce(direction.normalized * _info.EnemyWeapon.ParticleSpeed, ForceMode.Impulse);
        }

        private void OnDestroy()
        {
            _info.EnemyWeapon = null;
        }
    }
}
