using UnityEngine;
using UnityEngine.UI;

public class CoreState : MonoBehaviour,IState
{
    public int hp = 100;
    public CoreManager coreManager;
    public Text hpText;
    public void GetDamage(int damage)
    {
        hp-=damage;
        if(hp<=0)
        {   
            coreManager.GameOver(false);
        }
    }
    void Update()
    {
        hpText.text = hp + "/100";
    }
}
