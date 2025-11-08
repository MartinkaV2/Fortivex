using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("Towers")]
    [SerializeField] private GameObject pistolTower;
    [SerializeField] private GameObject sniperTower;
    [SerializeField] private GameObject bombaCannon;

    private GameObject placingTower;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            setTower(pistolTower);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            setTower(bombaCannon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            setTower(sniperTower);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearSelected();
        }

        if (placingTower)
        {
            if (!placingTower.GetComponent <TowerPlacement>().isPlacing)
            {
                placingTower = null;
            }
        }
    }

    private void ClearSelected ()
    {
        if (placingTower)
        {
            Destroy(placingTower);
            placingTower = null;
        }
    }

    private void setTower(GameObject tower)
    { 
    ClearSelected();
    placingTower = Instantiate(tower);
    }
}
