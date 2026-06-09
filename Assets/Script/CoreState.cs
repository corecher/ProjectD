using UnityEngine;
using UnityEngine.UI;

public class CoreState : MonoBehaviour,IState
{
    public int hp = 100;
    public CoreManager coreManager;
    public Text hpText;
    public void GetDamage(int damage)
    {
        if(hp <= 0) return;
        hp-=damage;
        if(hp<=0)
        {   
            coreManager.GameOver(false,4);
        }
    }
    void Update()
    {
        hpText.text = hp + "/100";
    }
}
