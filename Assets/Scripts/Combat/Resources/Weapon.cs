using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float weaponReach =3f;
    

    internal float WeaponReach()
    {
        return weaponReach;
    }
}
