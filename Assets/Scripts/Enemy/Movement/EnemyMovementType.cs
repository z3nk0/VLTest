﻿using UnityEngine;
using System;

namespace VLTest.Enemies.Movement
{
    public abstract class EnemyMovementType : ScriptableObject
    {
        public abstract EnemyMovementLogic CreateMovement(GameObject enemy);

    }

    public abstract class EnemyMovementLogic
    {
        protected Transform transform;

        public abstract void Move(float speed, Action callback);
        public abstract void Cancel();

    }
}