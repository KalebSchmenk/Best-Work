using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyHealthInterface
{
    int health { get; set; }

    void TakeDamage(int damage);

    Transform hitLocation { get; set; }
}
