using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "ShopWaypointData", menuName = "Scriptable Objects/ShopWaypointData")]
public class ShopWaypointData : WaypointData
{
    public override void DoEffect()
    {
        //SceneManager.LoadScene("BattleSetup", LoadSceneMode.Single);
    }
}