using UnityEngine;
using UnityEngine.UI;

public class HpManager : MonoBehaviour
{
    public Text hp;
    public TextMesh playerHp;
    public PlayerMovement playerMovement;
    public EnemySpawner enemySpawner;
    public CoreState coreState;
    void Update()
    {
        if(playerMovement != null)
        playerHp.text = playerMovement.hp+"/20";
        if(enemySpawner != null)
        hp.text = enemySpawner.hp+"/2000";
        if(coreState != null)
        playerHp.text = coreState.hp + "/100";
    }
}
