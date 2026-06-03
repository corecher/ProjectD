using System.Collections;
using UnityEngine;

public class EnemyState : MonoBehaviour,IState
{
    public int hp=10;
    public int damage=10;
    public GameObject explosionEffect;
    public CoreManager coreManager;
    public Sprite bossSprite;
    public void GetDamage(int damage)
    {
        hp-=damage;
        if(hp<=0)
        {
            Instantiate(explosionEffect,transform.position,Quaternion.identity);
            if(gameObject.name == "BossEnemy") coreManager.GameOver(true);
            Destroy(gameObject);
        }
    }
    

}
