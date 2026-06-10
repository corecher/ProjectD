using UnityEngine;
using UnityEngine.UI;

public class CoreState : MonoBehaviour,IState
{
    public int hp = 100;
    public CoreManager coreManager;
    public Text hpText;
    public Transform player;
    public void GetDamage(int damage)
    {
        if(hp <= 0) return;
        hp-=damage;
        if(hp<=0)
        {   
            coreManager.GameOver(false,5);
        }
    }
    void Update()
    {
        if(hpText!=null)
        hpText.text = hp + "/100";
        if(player!=null)
        {
            if(player.position.y<=-16.5f)
            {
                GetDamage(20);
                player.position = new Vector2(17.8f,5.4f);
            }
        }
    }
}
