﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLTest.Enemies;
using VLTest.Player.Weapons;
using VLTest.Utils;

namespace VLTest.Player
{
    public class PlayerAttack : PlayerComponent
    {
        #region MEMBERS
        private const float MAX_RAYCAST_DISTANCE = 100f;
        private const float ANTIOVERLAP_SPACE = 0.05f;

        public string enemyLayerName = "Enemy";
        public string scenarioLayerName = "Scenario";

        public ShotEffect shotEffect;
        public ObjectPool bulletDecalPool;
            
        private Transform currentCameraTransform
        {
            get { return player.cameraController.currentCamera.transform; }
        }
        private Weapon currentWeapon;
        private float remainingCooldown;
        private int shotLayerMask;
        private int enemyLayer;
        private bool weaponReady
        {
            get { return remainingCooldown <= 0; }
        }
        #endregion

        #region PRIVATE METHODS
        private void Awake()
        {
            shotLayerMask = LayerMask.GetMask(enemyLayerName, scenarioLayerName);
            enemyLayer = LayerMask.NameToLayer(enemyLayerName);
            bulletDecalPool.Populate();
        }

        private void Update()
        {
            if (player.input.fire && weaponReady)
            {
                for (int i = 0; i < currentWeapon.bulletsPerShot; i++)
                {
                    Fire();
                }
            }
            if (!weaponReady)
            {
                remainingCooldown -= Time.deltaTime;
            }
        }

        private void Fire()
        {
            Transform cameraTransform = currentCameraTransform;
            RaycastHit hit;
            Vector3 shotDirection = CreateDispersionShot(cameraTransform, currentWeapon.dispersion);
            if (Physics.Raycast(cameraTransform.position, shotDirection, out hit, MAX_RAYCAST_DISTANCE, shotLayerMask))
            {
                bool isEnemy = hit.collider.gameObject.layer == enemyLayer;
                if (isEnemy)
                {
                    EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                    enemyHealth.SetDamage(currentWeapon.damage);
                }
                else
                {
                    CreateBulletDecal(hit);
                }
            }
            remainingCooldown = currentWeapon.firingRate;
            if (shotEffect != null)
            {
                shotEffect.PlayEffect();
            }
        }

        private Vector3 CreateDispersionShot(Transform cameraTransform, float dispersionRate)
        {
            Vector2 randomDispersion = Random.insideUnitCircle * dispersionRate;
            Vector3 dispersion = cameraTransform.forward + (cameraTransform.right * randomDispersion.x) + (cameraTransform.up * randomDispersion.y);
            Vector3 dispersionPoint = cameraTransform.position + dispersion;
            Vector3 dispersionDirection = (dispersionPoint - cameraTransform.position).normalized;
#if UNITY_EDITOR
            Debug.DrawRay(cameraTransform.position, dispersionDirection * MAX_RAYCAST_DISTANCE, Color.red, 0.5f);
#endif
            return dispersionDirection;
        }

        private void CreateBulletDecal(RaycastHit hit)
        {
            ObjectPoolItem decal = bulletDecalPool.Spawn(hit.point, Quaternion.identity);
            decal.transform.forward = hit.normal;
            decal.transform.position = decal.transform.position + (decal.transform.forward * ANTIOVERLAP_SPACE); // Separates a litte amount of space to avoid overlap
        }


        private void OnEnable()
        {
            player.stats.OnWeaponChanged += OnWeapongChanged;
            currentWeapon = player.stats.currentWeapon;
        }

        private void OnDisable()
        {
            player.stats.OnWeaponChanged -= OnWeapongChanged;
        }

        private void OnWeapongChanged(Weapon weapon)
        {
            currentWeapon = weapon;
            remainingCooldown = 0;
        }
        #endregion
    }
}